using System;
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
    internal class Scorpion: BaseCardTemplate, ICardTemplate
    {
        private void SandySting(GamePlayer player, Zone zone, Zone targetZone)
        {
            targetZone.PlacedCard.AddModifiers([new SameColumnDamageModifier(1, [GameEvent.END_TURN])]);
        }

        public override Card CreateCard() => new ()
        {
            Name = "Scorpion",
            MaxHealth = 5,
            MoveCost = 1,
            CardType = CardType.CREATURE,
            NPCID = NPCID.Scorpion,
            SubTypes = [CardSubtype.DESERT, CardSubtype.CRITTER],
            Role = ZoneRole.DEFENSE,
            Attacks = [
                new() {
                    Damage = 1,
                    Cost = 1,
                }
            ],
            Skills = [
                new() {
                    Cost = 0,
                    SkillType = ActionType.TARGET_ALLY,
                    DoSkill = SandySting,
                }
            ]
        };
    }
}
