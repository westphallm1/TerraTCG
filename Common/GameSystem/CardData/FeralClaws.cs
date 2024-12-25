﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using TerraTCG.Common.GameSystem.Drawing;
using TerraTCG.Common.GameSystem.GameState;
using TerraTCG.Common.GameSystem.GameState.GameActions;
using TerraTCG.Common.GameSystem.GameState.Modifiers;

namespace TerraTCG.Common.GameSystem.CardData
{
    internal class FeralClaws : ModSystem, ICardTemplate
    {
        public Card CreateCard() => new ()
        {
            Name = "FeralClaws",
            CardType = CardType.ITEM,
            SubTypes = [CardSubtype.EQUIPMENT, CardSubtype.ITEM],
            SelectInHandAction = (card, player) => new ApplyModifierAction(card, player),
            Skills = [ 
                new() { Cost = 2 }
            ],
            Modifiers = [
                new RelentlessModifier()  {
                    Texture = TextureCache.Instance.GetItemTexture(ItemID.FeralClaws),
                    Source = CardSubtype.EQUIPMENT,
                }
            ]
        };
    }
}