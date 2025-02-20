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
    internal class Basilisk : BaseCardTemplate, ICardTemplate
    {
		private class BasiliskOnEnterDealDmgModifier : ICardModifier
		{
			public void ModifyCardEntrance(Zone sourceZone) 
			{
				var occupiedOpposingZone = sourceZone.Owner.Opponent.Field.Zones.Where(z => z.ColumnAligned(sourceZone) && z.HasPlacedCard())
					.FirstOrDefault();

				if(occupiedOpposingZone?.PlacedCard is PlacedCard card)
				{
					sourceZone.QueueAnimation(new ActionAnimation(sourceZone.PlacedCard));
					occupiedOpposingZone.QueueAnimation(new TakeDamageAnimation(card, card.CurrentHealth));
					card.CurrentHealth -= 2;

					sourceZone.PlacedCard.Heal(2);
				}
			}
		}

        public override Card CreateCard() => new ()
        {
            Name = "Basilisk",
            MaxHealth = 10,
            CardType = CardType.CREATURE,
            NPCID = NPCID.DesertBeast,
            SubTypes = [CardSubtype.EXPERT, CardSubtype.DESERT, CardSubtype.FIGHTER],
			FieldModifiers = () => [
				new BasiliskOnEnterDealDmgModifier(),
			],
            Attacks = [
                new() {
                    Damage = 4,
                    Cost = 3,
                }
            ],
        };
    }
}
