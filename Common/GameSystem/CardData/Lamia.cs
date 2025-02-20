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
    internal class Lamia : BaseCardTemplate, ICardTemplate
    {
		private class LamiaAttackCostDecreaseModifier : ICardModifier
		{
			public void ModifyAttack(ref Attack attack, Zone sourceZone, Zone destZone) 
			{
				if(sourceZone.ColumnAligned(destZone))
				{
					attack.Cost = Math.Max(1, attack.Cost - 1);
				}
			}

			public bool ShouldRemove(GameEventInfo eventInfo) => FieldModifierHelper.ShouldRemove(eventInfo, "Lamia");
		}

        public override Card CreateCard() => new ()
        {
            Name = "Lamia",
            MaxHealth = 7,
            CardType = CardType.CREATURE,
            NPCID = NPCID.DesertLamiaDark,
            SubTypes = [CardSubtype.DESERT, CardSubtype.CASTER],
			FieldModifiers = () => [new LamiaAttackCostDecreaseModifier()],
            Attacks = [
                new() {
                    Damage = 2,
                    Cost = 2,
                }
            ],
        };
    }
}
