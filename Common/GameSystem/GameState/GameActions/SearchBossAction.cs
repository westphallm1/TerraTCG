using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using TerraTCG.Common.GameSystem.Drawing.Animations;
using TerraTCG.Common.GameSystem.Drawing.Animations.FieldAnimations;
using static TerraTCG.Common.GameSystem.GameState.GameActions.IGameAction;

namespace TerraTCG.Common.GameSystem.GameState.GameActions
{
    internal class SearchBossAction : TownsfolkAction
    {
		public SearchBossAction() : base() { }

		public SearchBossAction(Card card, GamePlayer player) : base(card, player) { }

        public override ActionLogInfo GetLogMessage() => new(Card, $"{ActionText("Used")} {Card.CardName}");

        public override bool CanAcceptZone(Zone zone) => false;

        public override bool AcceptZone(Zone zone) => false;

        public override Zone TargetZone() => null;

        public override bool CanAcceptActionButton() => Player.Resources.SufficientResourcesFor(GetActionButtonResources()) && 
			Player.Resources.Health < Player.Opponent.Resources.Health &&
			Player.Deck.Cards.Any(c => c.SubTypes[0] == CardSubtype.BOSS);

        public override bool AcceptActionButton() => true;

        public override void Complete()
        {
            base.Complete();

			var firstBoss = Player.Deck.Cards.Where(c => c.SubTypes[0] == CardSubtype.BOSS).FirstOrDefault();
			if(firstBoss != null)
			{
				Player.Deck.Cards.Remove(firstBoss);
				Player.Hand.Add(firstBoss);
			}
            GameSounds.PlaySound(GameAction.USE_SKILL);
        }
		public override void PostSend(BinaryWriter writer)
		{
			// no-op
		}

		public override void PostReceive(BinaryReader reader, CardGame game)
		{
			// no-op
		}
    }
}
