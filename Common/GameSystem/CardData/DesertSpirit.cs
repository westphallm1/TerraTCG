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
    internal class DesertSpirit : BaseCardTemplate, ICardTemplate
    {
		private class MoveAttackersToUnoccupiedZoneModifier : ICardModifier
		{
			public void ModifyAttackZones(ref Zone sourceZone, ref Zone destZone) 
			{
				var destIdx = destZone.Index;
				var sameColumnZone = sourceZone.Siblings.Where(z => z.Index == destIdx).First();
				if(sameColumnZone.IsEmpty())
				{
					sameColumnZone.PlacedCard = sourceZone.PlacedCard;
					sourceZone.PlacedCard = null;

					sameColumnZone.QueueAnimation(new PlaceCardAnimation(sameColumnZone.PlacedCard));
					sourceZone.QueueAnimation(new RemoveCardAnimation(sameColumnZone.PlacedCard));

					sourceZone = sameColumnZone;
				}
			}

			public bool ShouldRemove(GameEventInfo eventInfo) => FieldModifierHelper.ShouldRemove(eventInfo, "DesertSpirit");
		}

        public override Card CreateCard() => new ()
        {
            Name = "DesertSpirit",
            MaxHealth = 6,
            CardType = CardType.CREATURE,
            NPCID = NPCID.DesertDjinn,
            SubTypes = [CardSubtype.DESERT, CardSubtype.CASTER],
            Role = ZoneRole.DEFENSE,
			FieldModifiers = () => [new MoveAttackersToUnoccupiedZoneModifier()],
            Attacks = [
                new() {
                    Damage = 2,
                    Cost = 2,
                }
            ],
        };
    }
}
