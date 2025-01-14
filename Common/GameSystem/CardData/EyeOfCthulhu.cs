﻿using System;
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
    internal class EyeOfCthulhu: BaseCardTemplate, ICardTemplate
    {
        private class HalfHealthDamageBoostModifier : ICardModifier
        {
            public void ModifyAttack(ref Attack attack, Zone sourceZone, Zone destZone) 
            {
                if(sourceZone.PlacedCard is PlacedCard card && card.CurrentHealth <= (card.Template.MaxHealth + 1) / 2)
                {
                    attack.Damage *= 2;
                }
            }
        }
        public override Card CreateCard() => new ()
        {
            Name = "EyeOfCthulhu",
            MaxHealth = 10,
            MoveCost = 2,
            NPCID = NPCID.EyeofCthulhu,
            CardType = CardType.CREATURE,
            SubTypes = [CardSubtype.BOSS, CardSubtype.FOREST, CardSubtype.SCOUT],
            Modifiers = () => [
                new EvasiveModifier(),
                new HalfHealthDamageBoostModifier(),
            ],
            Attacks = [
                new() {
                    Damage = 4,
                    Cost = 3,
                }
            ]
        };
    }
}