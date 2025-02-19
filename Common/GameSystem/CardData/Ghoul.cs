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
    internal class Ghoul : BaseCardTemplate, ICardTemplate
    {
        public override Card CreateCard() => new ()
        {
            Name = "Ghoul",
            MaxHealth = 7,
            CardType = CardType.CREATURE,
            NPCID = NPCID.DesertGhoul,
            SubTypes = [CardSubtype.DESERT, CardSubtype.FIGHTER],
            Attacks = [
                new() {
                    Damage = 3,
                    Cost = 2,
                }
            ],
            Modifiers = () => [
				new SameColumnDamageModifier(1),
            ]
        };
    }
}
