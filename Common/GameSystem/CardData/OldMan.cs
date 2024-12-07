﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using TerraTCG.Common.GameSystem.GameState;
using TerraTCG.Common.GameSystem.GameState.GameActions;

namespace TerraTCG.Common.GameSystem.CardData
{
    internal class OldMan : ModSystem, ICardTemplate
    {
        internal static ICardTemplate Instance => ModContent.GetInstance<OldMan>();
        public Card CreateCard() => new ()
        {
            Name = "OldMan",
            CardType = CardType.TOWNSFOLK,
            SubTypes = [CardSubtype.TOWNSFOLK],
            SelectInHandAction = (card, player) => new MoveCardAction(card, player, player.Opponent),
            Modifiers = [
                new() {
                    // TODO this should probably not be empty, just used to set the flag
                    // to draw modifier text
                }
            ]
        };
    }
}