using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using TerraTCG.Common.GameSystem.GameState;
using TerraTCG.Common.GameSystem.GameState.Modifiers;

namespace TerraTCG.Common.GameSystem.CardData
{
    internal class IlluminantBat: BaseCardTemplate, ICardTemplate
    {
		private class IlluminantBatHallowedModifier : ICardModifier
		{
			public bool AppliesToZone(Zone zone) => zone.Column == 1;

			public ModifierType Category => ModifierType.EVASIVE;

			public void ModifyZoneSelection(Zone sourceZone, Zone endZone, ref List<Zone> destZones)
			{
				if(AppliesToZone(sourceZone))
				{
					// allow the targeting of blocked enemy zones
					destZones = endZone.Siblings.Where(z => !z.IsEmpty()).ToList();
				}
			}
		}

        public override Card CreateCard() => new ()
        {
            Name = "IlluminantBat",
            MaxHealth = 7,
            CardType = CardType.CREATURE,
            NPCID = NPCID.IlluminantBat,
            SubTypes = [CardSubtype.HALLOWED, CardSubtype.SCOUT],
            Attacks = [
                new() {
                    Damage = 2,
                    Cost = 2,
                }
            ],
			FieldModifiers = () => [new IlluminantBatHallowedModifier()],
        };
    }
}
