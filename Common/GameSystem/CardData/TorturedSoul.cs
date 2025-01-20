﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using TerraTCG.Common.GameSystem.GameState;
using TerraTCG.Common.GameSystem.GameState.GameActions;
using TerraTCG.Common.GameSystem.GameState.Modifiers;

namespace TerraTCG.Common.GameSystem.CardData
{
    internal class TorturedSoul : BaseCardTemplate, ICardTemplate
    {
        public override Card CreateCard() => new ()
        {
            Name = "TorturedSoul",
            CardType = CardType.TOWNSFOLK,
            SubTypes = [CardSubtype.TOWNSFOLK],
            SelectInHandAction = (card, player) => new SearchBossAction(card, player),
        };
    }
}