using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using TerraTCG.Common.GameSystem.Drawing.Animations;
using TerraTCG.Common.GameSystem.Drawing.Animations.FieldAnimations;
using static TerraTCG.Common.GameSystem.GameState.GameActions.IGameAction;

namespace TerraTCG.Common.GameSystem.GameState.GameActions
{
    internal class MoveCardAction(Card card, GamePlayer player, bool targetEnemies = true, bool allowSwap = false) : TownsfolkAction(card, player)
    {
        private Zone sourceZone;
        private Zone destZone;

        public override ActionLogInfo GetLogMessage() => new(Card, $"{ActionText("Moved")} {destZone.CardName}");

        private int Step => sourceZone == null ? 0 : 1;

		public override bool CanAcceptZone(Zone zone)
		{
			if(!base.CanAcceptZone(zone))
			{
				return false;
			}
			if (Step == 0)
			{
				return !zone.IsEmpty() && 
					(targetEnemies || zone.Owner == Player) &&
					(allowSwap || zone.Siblings.Any(z => z.IsEmpty() && z.Row == zone.Row));
			} else
			{
				return zone.Owner == sourceZone.Owner && 
					(allowSwap || zone.IsEmpty()) && 
					zone.Row == sourceZone.Row;
			}
		}


        public override bool AcceptZone(Zone zone)
        {
            if(Step == 0)
            {
                sourceZone = zone;
            } else
            {
                destZone = zone;
            }
            return sourceZone != null && destZone != null;
        }

		public override string GetZoneTooltip(Zone zone)
		{
			return base.GetZoneTooltip(Step == 0 ? zone : sourceZone);
		}
		public override Zone TargetZone() => sourceZone;

        public override void Complete()
        {
            base.Complete();
            var duration = GetAnimationStartDelay();

			var toMove = destZone.PlacedCard;
            destZone.PlacedCard = sourceZone.PlacedCard;
			sourceZone.PlacedCard = toMove;

            var movedCard = destZone.PlacedCard;

            sourceZone.QueueAnimation(new IdleAnimation(movedCard, duration));
            sourceZone.QueueAnimation(new RemoveCardAnimation(movedCard));
			if(toMove != null)
			{
				sourceZone.QueueAnimation(new PlaceCardAnimation(toMove));
			}

			if(toMove == null)
			{
				destZone.QueueAnimation(new NoOpAnimation(duration));
			} else
			{
				destZone.QueueAnimation(new IdleAnimation(toMove, duration));
				destZone.QueueAnimation(new RemoveCardAnimation(toMove));
			}

            destZone.QueueAnimation(new PlaceCardAnimation(movedCard));
            GameSounds.PlaySound(GameAction.PLACE_CARD);
        }
    }
}
