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
    internal class AntlionSwarmer : BaseCardTemplate, ICardTemplate
    {
        public override Card CreateCard() => new ()
        {
            Name = "AntlionSwarmer",
            MaxHealth = 7,
            CardType = CardType.CREATURE,
            NPCID = NPCID.FlyingAntlion,
            SubTypes = [CardSubtype.DESERT, CardSubtype.FIGHTER],
            Attacks = [
                new() {
                    Damage = 2,
                    Cost = 2,
                }
            ],
            Modifiers = () => [
                new EvasiveModifier(),
				new SameColumnDamageModifier(1),
            ]
        };
    }
}
