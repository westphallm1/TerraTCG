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
    internal class DrawCardAction : TownsfolkAction
    {
		private int drawCount;
		public DrawCardAction() : base() { }

		public DrawCardAction(Card card, GamePlayer player, int drawCount=1) : base(card, player) 
		{ 
			this.drawCount = drawCount; 
		}

        public override ActionLogInfo GetLogMessage() => new(Card, $"{ActionText("Drew")} {drawCount} {ActionText("Cards")} {ActionText("With")} {Card.CardName}");

        public override bool CanAcceptZone(Zone zone) => false;

        public override bool AcceptZone(Zone zone) => false;

		public override bool CanAcceptActionButton() => Player.Resources.SufficientResourcesFor(GetActionButtonResources());

		public override bool AcceptActionButton() => true;

		public override Zone TargetZone() => null;

		public override void Complete()
        {
            base.Complete();

            for(int _ = 0; _ < drawCount; _++)
            {
                Player.Hand.Add(Player.Deck.Draw());
            }
            GameSounds.PlaySound(GameAction.USE_SKILL);
        }

		public override void PostSend(BinaryWriter writer)
		{
			writer.Write((byte)drawCount);
		}

		public override void PostReceive(BinaryReader reader, CardGame game)
		{
			drawCount = reader.ReadByte();
		}
    }
}
