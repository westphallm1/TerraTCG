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
    internal class Vulture : BaseCardTemplate, ICardTemplate
    {
		private class VultureEvasiveModifier : ICardModifier
		{
			public bool AppliesToZone(Zone zone) => zone.Owner.Opponent.Field.Zones.Any(z => z.ColumnAligned(zone) && z.HasPlacedCard());

			public void ModifyZoneSelection(Zone sourceZone, Zone endZone, ref List<Zone> destZones)
			{
				// no-op
				foreach(var zone in sourceZone.Owner.Opponent.Field.Zones)
				{
					if (zone.HasPlacedCard() && zone.ColumnAligned(sourceZone) && !destZones.Contains(zone))
					{
						destZones.Add(zone);
					}
				}
			}
		}

		private class UnicornOnEnterModifier : ICardModifier
		{

			public void ModifyCardEntrance(Zone sourceZone) 
			{
				// Having cards enter unpaused on turn 1 screws up a 
				// bunch of enemy decision-making.
				var centerZone = sourceZone.Siblings.Where(z => z.Index == 1).First();
				if(sourceZone.Owner.Game.CurrentTurn.TurnCount > 1 && centerZone.HasPlacedCard())
				{
					centerZone.PlacedCard.IsExerted = false;
				}
			}

			public bool ShouldRemove(GameEventInfo eventInfo) => true;
		}

        public override Card CreateCard() => new ()
        {
            Name = "Vulture",
            MaxHealth = 7,
            CardType = CardType.CREATURE,
            NPCID = NPCID.Vulture,
            SubTypes = [CardSubtype.DESERT, CardSubtype.FIGHTER],
            Attacks = [
                new() {
                    Damage = 4,
                    Cost = 3,
                }
            ],
			Modifiers = () => [new VultureEvasiveModifier()],
        };
    }
}
