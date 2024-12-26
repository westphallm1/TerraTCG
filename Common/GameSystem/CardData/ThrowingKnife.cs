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
    internal class ThrowingKnife : ModSystem, ICardTemplate
    {
        public Card CreateCard() => new ()
        {
            Name = "ThrowingKnife",
            CardType = CardType.ITEM,
            SubTypes = [CardSubtype.CONSUMABLE, CardSubtype.ITEM],
            SelectInHandAction = (card, player) => new ApplyModifierAction(card, player),
            Skills = [ // TODO this is wonky, but item texts are drawn using the skill template
                new() { Cost = 1 }
            ],
            Modifiers = [
                new FlatDamageModifier(1, [GameEvent.AFTER_ATTACK])  {
                    Texture = TextureCache.Instance.GetItemTexture(ItemID.ThrowingKnife),
                    Source = CardSubtype.CONSUMABLE,
                }
            ]
        };
    }
}