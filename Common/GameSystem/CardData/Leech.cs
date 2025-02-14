﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using TerraTCG.Common.GameSystem.Drawing;
using TerraTCG.Common.GameSystem.GameState;
using TerraTCG.Common.GameSystem.GameState.Modifiers;

namespace TerraTCG.Common.GameSystem.CardData
{
    internal class Leech: BaseCardTemplate, ICardTemplate
    {
        public override Card CreateCard() => new ()
        {
            Name = "Leech",
            MaxHealth = 7,
            MoveCost = 2,
            CardType = CardType.CREATURE,
            NPCID = NPCID.LeechHead,
			Role = ZoneRole.DEFENSE,
			Priority = 10,
			DrawZoneNPC = CardOverlayRenderer.Instance.DrawBestiaryZoneNPC,
            SubTypes = [CardSubtype.EVIL, CardSubtype.CRITTER],
            Attacks = [
                new() {
                    Damage = 2,
                    Cost = 2,
                }
            ],
        };
    }
}
