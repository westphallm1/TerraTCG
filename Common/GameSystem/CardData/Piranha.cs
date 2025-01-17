﻿using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
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
    internal class Piranha : BaseCardTemplate, ICardTemplate
    {
        public override Card CreateCard() => new ()
        {
            Name = "Piranha",
            MaxHealth = 6,
            MoveCost = 2,
            CardType = CardType.CREATURE,
            NPCID = NPCID.Piranha,
            SubTypes = [CardSubtype.JUNGLE, CardSubtype.FIGHTER],
            Attacks = [
                new() {
                    Damage = 4,
                    Cost = 4,
                    TargetModifiers = z=>[new BleedModifier(1)]
                }
            ],
        };
    }
}
