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
    internal class Gastropod : BaseCardTemplate, ICardTemplate
    {
		private class GastropodHallowedModifier : ICardModifier
		{
			public bool AppliesToZone(Zone zone) => zone.Column == 1;

			public void ModifyAttack(ref Attack attack, Zone sourceZone, Zone destZone) 
			{
				if(AppliesToZone(sourceZone))
				{
					attack.Cost = Math.Max(1, attack.Cost - 1);
				}
			}

			// Field modifier, refresh at start of turn
			public bool ShouldRemove(GameEventInfo eventInfo) => FieldModifierHelper.ShouldRemove(eventInfo, "Gastropod");
		}

        public override Card CreateCard() => new ()
        {
            Name = "Gastropod",
            MaxHealth = 6,
            MoveCost = 2,
            CardType = CardType.CREATURE,
            NPCID = NPCID.Gastropod,
            SubTypes = [CardSubtype.HALLOWED, CardSubtype.SCOUT],
            Attacks = [
                new() {
                    Damage = 2,
                    Cost = 2,
                }
            ],
			FieldModifiers = () => [new GastropodHallowedModifier()],
        };
    }
}
