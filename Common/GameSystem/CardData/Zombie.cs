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
    internal class Zombie : BaseCardTemplate, ICardTemplate
    {
        public override Card CreateCard() => new ()
        {
            Name = "Zombie",
            MaxHealth = 7,
            MoveCost = 2,
            NPCID = NPCID.Zombie,
            CardType = CardType.CREATURE,
            SubTypes = [CardSubtype.FOREST, CardSubtype.FIGHTER],
            Attacks = [
                new() {
                    Name = "Diseased Bite",
                    Damage = 3,
                    Cost = 2,
                }
            ]
        };
    }
}
