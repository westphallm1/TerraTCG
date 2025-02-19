using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using TerraTCG.Common.GameSystem.Drawing;
using TerraTCG.Common.GameSystem.Drawing.Animations;
using TerraTCG.Common.GameSystem.GameState;
using TerraTCG.Common.GameSystem.GameState.Modifiers;

namespace TerraTCG.Common.GameSystem.CardData
{
    internal class QueenSlime : BaseCardTemplate, ICardTemplate
    {
		private class QueenSlimeHallowedModifier : ICardModifier
		{
			public bool AppliesToZone(Zone zone) => zone.Column == 1;

			public ModifierType Category => ModifierType.RELENTLESS;

			public bool ShouldRemove(GameEventInfo eventInfo) {
				if(FieldModifierHelper.ShouldRemove(eventInfo, "QueenSlime"))
				{
					return true;
				}
				if(AppliesToZone(eventInfo.Zone) && eventInfo.Event == GameEvent.AFTER_ATTACK && eventInfo.Zone?.PlacedCard is PlacedCard card && card.IsExerted)
				{
					card.IsExerted = false;
					eventInfo.Zone.QueueAnimation(new BecomeActiveAnimation(card));
					return true;
				}
				return false;
			}

		}

        public override Card CreateCard() => new ()
        {
            Name = "QueenSlime",
            MaxHealth = 9,
            CardType = CardType.CREATURE,
			Points = 2,
            NPCID = NPCID.QueenSlimeBoss,
			DrawZoneNPC = CardOverlayRenderer.Instance.DrawQueenSlimeNPC,
            SubTypes = [CardSubtype.BOSS, CardSubtype.HALLOWED, CardSubtype.FIGHTER],
            Attacks = [
                new() {
                    Damage = 3,
                    Cost = 2,
                }
            ],
			FieldModifiers = () => [
				new QueenSlimeHallowedModifier()
			],
        };
    }
}
