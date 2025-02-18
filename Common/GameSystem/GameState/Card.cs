using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent;
using Terraria.Localization;
using Terraria.ModLoader;
using TerraTCG.Common.GameSystem.Drawing;
using TerraTCG.Common.GameSystem.GameState.GameActions;
using TerraTCG.Common.GameSystem.GameState.Modifiers;
using TerraTCG.Common.Util;

namespace TerraTCG.Common.GameSystem.GameState
{
    internal enum CardType
    {
        CREATURE,
        ITEM,
        TOWNSFOLK
    }

    internal enum CardSubtype
    {
        // Empty value
        NONE,
        // Creature Supertypes
        EXPERT,
        BOSS,
        // Creature Roles
        FIGHTER,
        SCOUT,
        DEFENDER,
        SLIME,
        CRITTER,
        CASTER,

        // Biomes
        FOREST,
        CAVERN,
        JUNGLE,
        BLOOD_MOON,
        OCEAN,
        GOBLIN_ARMY,
        MUSHROOM,

        // Item Subtypes
        EQUIPMENT,
        CONSUMABLE,

        // Duplicate CardTypes (for text rendering)
        TOWNSFOLK,
        ITEM,
		EVIL,
		OWNED,
		SNOW,
		HALLOWED,
		DESERT,
	}

    // Bot helper function, apply additional
    // logic for whether the bot should use
    // this item on an ally
    internal delegate bool ShouldTarget(Zone zone);

	// Delegate function type to check whether a given card can
	// be promoted onto a given zone
	internal delegate bool CanPromote(Zone zone, Card card);

    internal class Card
    {
        internal string Name { get; set; }

        // The mod that originated this card
        internal string Mod { get; set; } = ModContent.GetInstance<TerraTCG>().Name;

        internal string FullName => $"{Mod}/{Name}";

		// Automatically loaded from its texture
		internal int GoreType { get; set; }

		internal int NPCID { get; set; }

		internal string TexturePath => $"{Mod}/Assets/Cards/{Name}";

		internal Asset<Texture2D> Texture => ModContent.Request<Texture2D>(TexturePath);

        internal CardType CardType { get; set; }

        internal List<CardSubtype> SubTypes { get; set; }

        internal CardSubtype SortType => 
            (SubTypes[0] == CardSubtype.EXPERT || SubTypes[0] == CardSubtype.BOSS) ? SubTypes[1] : SubTypes[0];

		internal CardSubtype RoleSubtype => SubTypes.Last();

        internal int MaxHealth { get; set; }

        internal int MoveCost { get; set; }

        // Points awarded on defeating this card
        internal int Points { get; set; } = 1;

        // Used for cards that can only be created by other cards (eg. Nymph)
        internal bool IsCollectable { get; set; } = true;

        internal SelectInHandAction SelectInHandAction { get; set; }
            = (zone, player) => new DeployCreatureAction(zone, player);
        internal SelectOnFieldAction SelectOnFieldAction { get; set; }
            = (zone, player) => new MoveCardOrAttackAction(zone, player);

        internal List<Skill> Skills { get; set; }

        internal bool HasSkill => (Skills?.Count ?? 0) > 0;

        internal List<Attack> Attacks { get; set; }

		// List of the modifiers that the card applies to itself upon entering
		// the battlefield
        internal Func<List<ICardModifier>> Modifiers { get; set; }

		// List of the modifiers that the card continually applies to the field
		// while this card is alive
		// These are re-applied every turn, and *should* return true on
		// ShouldRemove(GameEvent.START_TURN)
        internal Func<List<ICardModifier>> FieldModifiers { get; set; }

        // Tags to help bot players decide what to do with cards

        // Where to place the card on the board
        internal ZoneRole Role { get; set; } = ZoneRole.OFFENSE;


        // Among cards of the same type, how important is it to play this one first?
        internal int Priority { get; set; } = 0;

		// Helper func for the bot player that suggests whether a zone would be a good target
		// for this item
        internal ShouldTarget ShouldTarget { get; set; } = z => !(z.PlacedCard?.IsExerted ?? true);

		// Func to apply additional restrictions to whether a zone can be targeted
		internal Func<Zone, bool> CanTargetZone { get; set; } = (zone) => true;

		// Func to check whether this card can evolve onto a given card on the field
		// By default, check whether the biome and combat role match
		internal CanPromote CanPromote { get; set; } = (zone, card) => 
			zone.PlacedCard?.Template.SubTypes[0] == card.SubTypes[1] &&
			zone.PlacedCard?.Template.SubTypes.Last() == card.SubTypes.Last();


        // Localization utils
        internal string CardName => Language.GetTextValue($"Mods.TerraTCG.Cards.{Name}.Name");

        internal string AttackName => Language.GetTextValue($"Mods.TerraTCG.Cards.{Name}.Attack.Name");
        internal string AttackDescription => Language.GetTextValue($"Mods.TerraTCG.Cards.{Name}.Attack.Description");
        internal string SkillName => Language.GetTextValue($"Mods.TerraTCG.Cards.{Name}.Skill.Name");
        internal string SkillDescription => Language.GetTextValue($"Mods.TerraTCG.Cards.{Name}.Skill.Description");

        internal string ModifierDescription => Language.GetTextValue($"Mods.TerraTCG.Cards.{Name}.Modifier.Description");

        internal bool HasAttackText => Language.Exists($"Mods.TerraTCG.Cards.{Name}.Attack.Name");
        internal bool HasAttackDescription => HasAttackText && Language.Exists($"Mods.TerraTCG.Cards.{Name}.Attack.Description");
        internal bool HasSkillText => Language.Exists($"Mods.TerraTCG.Cards.{Name}.Skill.Name");
        internal bool HasSkillDescription => HasSkillText && Language.Exists($"Mods.TerraTCG.Cards.{Name}.Skill.Description");
        internal bool HasModifierText => Language.Exists($"Mods.TerraTCG.Cards.{Name}.Modifier.Description");

        internal string TypeLine => string.Join(" ", 
            SubTypes.Select(t => Language.GetTextValue($"Mods.TerraTCG.Cards.Types.{t}")));

        internal DrawZoneNPC DrawZoneNPC { get; set; } = CardOverlayRenderer.Instance.DefaultDrawZoneNPC;

		// Do a search of whether any text on the card contains the search term
		internal bool MatchesTextFilter(string textFilter)
        {
            var allMyText = new StringBuilder();
            // All cards have a name and typeline
            allMyText.Append(CardName);
            allMyText.Append(TypeLine);

            // Cards variably have attacks, skills, and static modifiers
            if(HasAttackText)
            {
                allMyText.Append(AttackName);
            }
            if(HasAttackDescription)
            {
                allMyText.Append(AttackDescription);
            }
            if(HasSkill)
            {
                allMyText.Append(SkillName);
            }
            if (HasSkillDescription)
            {
                allMyText.Append(SkillDescription);
            }
            if(HasModifierText)
            {
                allMyText.Append(ModifierDescription);
            }

            return allMyText.ToString().Replace("\n"," ").Contains(textFilter, StringComparison.CurrentCultureIgnoreCase);
        }
    }
}
