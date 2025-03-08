using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerraTCG.Common.GameSystem.Drawing.Animations.FieldAnimations;
using TerraTCG.Common.Netcode.Packets;
using static TerraTCG.Common.GameSystem.GameState.GameActions.IGameAction;

namespace TerraTCG.Common.GameSystem.GameState.GameActions
{
    internal abstract class TownsfolkAction : IGameAction
    {

        internal Card Card { get; set;  }
        internal GamePlayer Player { get; set; }

        private bool checkingValidZone = false;
		public TownsfolkAction()
		{

		}

		public TownsfolkAction(Card card, GamePlayer player)
		{
			Card = card;
			Player = player;
		}

        public virtual bool CanAcceptZone(Zone zone) => checkingValidZone || Player.Resources.SufficientResourcesFor(GetZoneResources(zone));
		public abstract bool AcceptZone(Zone zone);

		public virtual bool CanAcceptActionButton() => false;
		public virtual bool AcceptActionButton() => false;

        public abstract ActionLogInfo GetLogMessage();

        public virtual Zone TargetZone() => null;

        public TimeSpan GetAnimationStartDelay() => ShowCardAnimation.DURATION;
        // Player == TCGPlayer.LocalGamePlayer ? TimeSpan.FromSeconds(0f) : ShowCardAnimation.DURATION;

        public string GetActionButtonTooltip()
        {
			var useResourceTo = GetActionButtonResources().GetUsageTooltip();
			var useCard = $"{ActionText("Use")} {Card.CardName}";
			return string.IsNullOrEmpty(useResourceTo) ? useCard : $"{useResourceTo}\n{useCard}";
        }

        public virtual string GetZoneTooltip(Zone zone)
        {
			var useResourceTo = GetZoneResources(zone).GetUsageTooltip();
			var useCardOnCard = $"{ActionText("Use")} {Card.CardName} {ActionText("On")} {zone.CardName}";
			return string.IsNullOrEmpty(useResourceTo) ? useCardOnCard : $"{useResourceTo}\n{useCardOnCard}";
		}

        // TODO this is hacky, check whether not having a townsfolk emblem
        // is the reason that the zone can't be selected
        public string GetCantAcceptZoneTooltip(Zone zone) {
            if(Player.Resources.TownsfolkMana == 0)
            {
                // TODO this is hacky
                checkingValidZone = true;
                var couldUseIfHadMana = CanAcceptZone(zone);
                checkingValidZone = false;
                if (couldUseIfHadMana)
                {
                    return ActionText("NotEnoughTownsfolk");
                }
            }
            return null;
        }

		public virtual PlayerResources GetZoneResources(Zone zone) => new(0, 0, townsfolkMana: 1);

		public virtual PlayerResources GetActionButtonResources() => new(0, 0, townsfolkMana: 1);

		public virtual void Complete()
        {
            Player.Resources = Player.Resources.UseResource(townsfolkMana: 1);
            Player.Hand.Remove(Card);
			Player.Game.FieldAnimation = new ShowCardAnimation(TCGPlayer.TotalGameTime, Card, TargetZone(), Player == TCGPlayer.LocalGamePlayer);
        }

		public void Send(BinaryWriter writer)
		{
			writer.Write(Player.Index);
			writer.Write(CardNetworkSync.Serialize(Card));
			PostSend(writer);
		}

		public abstract void PostSend(BinaryWriter writer);

		public void Receive(BinaryReader reader, CardGame game)
		{
			Player = game.GamePlayers[reader.ReadByte()];
			Card = CardNetworkSync.Deserialize(reader.ReadUInt16());
			PostReceive(reader, game);
		}

		public abstract void PostReceive(BinaryReader reader, CardGame game);

	}
}
