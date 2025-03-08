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
    internal class RampAction : TownsfolkAction
    {
		private int amount;

		public RampAction() : base() { }

		public RampAction(Card card, GamePlayer player, int amount=1) : base(card, player) 
		{
			this.amount = amount;
		}

        public override ActionLogInfo GetLogMessage() => new(Card, $"{ActionText("AddedMana")} {ActionText("With")} {Card.CardName}");

        public override bool CanAcceptZone(Zone zone) => false;

        public override bool AcceptZone(Zone zone) => false;

        public override Zone TargetZone() => null;

		public override bool CanAcceptActionButton() => Player.Resources.SufficientResourcesFor(GetActionButtonResources());

        public override bool AcceptActionButton() => true;

        public override void Complete()
        {
            base.Complete();
            Player.ManaPerTurn += amount;
            GameSounds.PlaySound(GameAction.USE_SKILL);
        }

		public override void PostSend(BinaryWriter writer)
		{
			writer.Write((byte)amount);
		}

		public override void PostReceive(BinaryReader reader, CardGame game)
		{
			amount = reader.ReadByte();
		}
    }
}
