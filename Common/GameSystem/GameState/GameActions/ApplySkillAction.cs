using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using TerraTCG.Common.GameSystem.Drawing.Animations;
using TerraTCG.Common.GameSystem.Drawing.Animations.FieldAnimations;
using TerraTCG.Common.Netcode.Packets;
using static TerraTCG.Common.GameSystem.GameState.GameActions.IGameAction;

namespace TerraTCG.Common.GameSystem.GameState.GameActions
{
    internal class ApplySkillAction() : IGameAction
    {
        private Zone zone;

		private Card card;
		private GamePlayer player;

		public ApplySkillAction(Card card, GamePlayer player) : this()
		{
			this.card = card;
			this.player = player;
		}

        public bool CanAcceptZone(Zone zone) => player.Owns(zone) && !zone.IsEmpty() && player.Resources.SufficientResourcesFor(GetZoneResources(zone));

        public bool AcceptZone(Zone zone)
        {
            this.zone = zone;
            return true;
        }

        public ActionLogInfo GetLogMessage() => new(card, $"{ActionText("Used")} {card.CardName} {ActionText("On")} {zone.CardName}");

        public string GetZoneTooltip(Zone zone)
        {
			var useResourceTo = GetZoneResources(zone).GetUsageTooltip();
			var useCardOnCard = $"{ActionText("Use")} {card.CardName} {ActionText("On")} {zone.CardName}";
			return string.IsNullOrEmpty(useResourceTo) ? useCardOnCard : $"{useResourceTo}\n{useCardOnCard}";
		}

        public string GetCantAcceptZoneTooltip(Zone zone)
		{
			if (!player.Owns(zone) || zone.IsEmpty()) return "";

			var notEnoughResourceTo = player.Resources.GetDeficencyTooltip(GetZoneResources(zone));
			return string.IsNullOrEmpty(notEnoughResourceTo) ? "" : $"{notEnoughResourceTo} {ActionText("Use")}";
		}

		public PlayerResources GetZoneResources(Zone zone) => new(
			0, 
			mana: zone.PlacedCard.ModifyIncomingSkill(card).Cost,
			0
		);

        public void Complete()
        {
            var showAnimation = new ShowCardAnimation(TCGPlayer.TotalGameTime, card, zone, player == TCGPlayer.LocalGamePlayer);
            player.Game.FieldAnimation = showAnimation;
            if(card.CardType == CardType.ITEM)
            {
                player.Game.CurrentTurn.UsedItemCount += 1;
            }
            var duration = showAnimation.Duration;

            zone.QueueAnimation(new IdleAnimation(zone.PlacedCard, duration: duration));
            zone.QueueAnimation(new ApplyModifierAnimation(zone.PlacedCard, card.Skills[0].Texture));

            card.Skills[0].DoSkill(player, null, zone);
			player.Resources -= GetZoneResources(zone);
            player.Hand.Remove(card);
            if(card.SubTypes.Contains(CardSubtype.EQUIPMENT))
            {
                GameSounds.PlaySound(GameAction.USE_EQUIPMENT);
            } else
            {
                GameSounds.PlaySound(GameAction.USE_CONSUMABLE);
            }
        }
		public void Send(BinaryWriter writer)
		{
			writer.Write(player.Index);
			writer.Write(CardNetworkSync.Serialize(card));
			writer.Write((byte)zone.Index);
		}

		public void Receive(BinaryReader reader, CardGame game)
		{
			player = game.GamePlayers[reader.ReadByte()];
			card = CardNetworkSync.Deserialize(reader.ReadUInt16());
			zone = player.Field.Zones[reader.ReadByte()];
		}
    }
}
