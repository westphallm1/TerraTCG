﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using TerraTCG.Common.GameSystem.GameState;

namespace TerraTCG.Common.GameSystem.CardData
{
    internal class Skeleton : BaseCardTemplate, ICardTemplate
    {
        public override Card CreateCard() => new ()
        {
            Name = "Skeleton",
            MaxHealth = 8,
            MoveCost = 2,
            NPCID = NPCID.Skeleton,
            CardType = CardType.CREATURE,
            SubTypes = [CardSubtype.CAVERN, CardSubtype.FIGHTER],
            Attacks = [
                new() {
                    Name = "Bone Break",
                    Damage = 4,
                    Cost = 3,
                }
            ]
        };
    }
}
