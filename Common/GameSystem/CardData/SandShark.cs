using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using TerraTCG.Common.GameSystem.Drawing.Animations;
using TerraTCG.Common.GameSystem.GameState;
using TerraTCG.Common.GameSystem.GameState.GameActions;
using TerraTCG.Common.GameSystem.GameState.Modifiers;

namespace TerraTCG.Common.GameSystem.CardData
{
    internal class SandShark : BaseCardTemplate, ICardTemplate
    {
		private class SandSharkUnpauseModifier : ICardModifier
		{
			private bool lastAttackWasInSameColumn;

			public void ModifyAttack(ref Attack attack, Zone sourceZone, Zone destZone) 
			{
				lastAttackWasInSameColumn = sourceZone.ColumnAligned(destZone);
			}

			public bool ShouldRemove(GameEventInfo eventInfo)
			{
				if (eventInfo.Event == GameEvent.AFTER_ATTACK && lastAttackWasInSameColumn)
				{
					var card = eventInfo.Zone.PlacedCard;
					card.IsExerted = false;
					eventInfo.Zone.QueueAnimation(new BecomeActiveAnimation(card));
				}
				return false;
			}

		}

        public override Card CreateCard() => new ()
        {
            Name = "SandShark",
            MaxHealth = 10,
            CardType = CardType.CREATURE,
            NPCID = NPCID.SandShark,
            SubTypes = [CardSubtype.EXPERT, CardSubtype.DESERT, CardSubtype.FIGHTER],
			Modifiers = () => [new SandSharkUnpauseModifier()],
            Attacks = [
                new() {
                    Damage = 3,
                    Cost = 3,
                }
            ],
        };
    }
}
