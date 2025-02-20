using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;

namespace TerraTCG.Common.GameSystem.GameState.Modifiers
{
    internal enum GameEvent
    {
        // End of the current turn
        END_TURN,

        // After doing an attack
        AFTER_ATTACK,

        // After receiving an attack
        AFTER_RECEIVE_ATTACK,
        START_TURN,
		CREATURE_DIED,
		CREATURE_ENTERED,
	}

	// Keyworded modifier types
    internal enum ModifierType
    {
        NONE,
        PAUSED,
        DEFENSE_BOOST,
        BLEEDING,
        RELENTLESS,
        EVASIVE,
        SPIKED,
        LIFESTEAL,
		POISON,
		MORBID,
		ZEALOUS,
		CURSED,
		FREEZING,
		SHIFTING_SANDS
	}

    internal struct GameEventInfo
    {
        public GameEvent Event { get; set; }
        public bool IsMyTurn { get; set; }
        public GamePlayer TurnPlayer { get; set; }
        public Zone Zone { get; set; }
    }

    internal interface ICardModifier
    {
        public Asset<Texture2D> Texture { get => null; }

        public Card SourceCard { get => null ; set { } }
        public CardSubtype Source { get => SourceCard?.SortType ?? CardSubtype.NONE; }

        public ModifierType Category { get => ModifierType.NONE; }

        public string Description { get => ""; }

        // Scale of the buff, used to generate tooltips for stacked buffs
        public int Amount { get => 0; }

        // Modify a skill as it's performed against this card
        public void ModifyIncomingSkill(ref Skill skill, Card sourceCard) 
        {
            // no-op
        }

        // Modify a skill as this card uses it
        public void ModifySkill(ref Skill skill, Zone sourceZone, Zone destZone) 
        {
            // no-op
        }

		// Switch the source or destination zone of an attack before
		// damage is dealt
        public void ModifyAttackZones(ref Zone sourceZone, ref Zone destZone, bool preCalculating) 
        {
			// no-op
        }

        // Modify an attack as this card performs it
        public void ModifyAttack(ref Attack attack, Zone sourceZone, Zone destZone) 
        {
            // no-op
        }

        // Modify an attack as it is performed against this card
        public void ModifyIncomingAttack(ref Attack attack, Zone sourceZone, Zone destZone) 
        {
            // no-op
        }

        public void ModifyZoneSelection(Zone sourceZone, Zone endZone, ref List<Zone> destZones)
        {
            // no-op
        }

        public void ModifyIncomingZoneSelection(Zone sourceZone, Zone endZone, ref List<Zone> destZones)
        {
            // no-op
        }

        public void ModifyCardEntrance(Zone sourceZone) 
        {
            // no-op
        }

        public bool ShouldRemove(GameEventInfo eventInfo) {
            return false;
        }

		public bool AppliesToZone(Zone zone)
		{
			// no-op
			return true;
		}
	}

	// TODO do we want to just make this a parent class?
	internal static class FieldModifierHelper
	{

		public static bool ShouldRemove(GameEventInfo eventInfo, string cardName)
		{
			return eventInfo.Event == GameEvent.START_TURN || 
				(eventInfo.Event == GameEvent.CREATURE_DIED && eventInfo.Zone.PlacedCard?.Template.Name == cardName);
		}
	}
}
