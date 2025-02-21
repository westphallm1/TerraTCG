using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TerraTCG.Common.GameSystem.CardData;
using TerraTCG.Common.GameSystem.GameState;
using TerraTCG.Common.GameSystem.GameState.Modifiers;
using TerraTCG.Content.NPCs;

namespace TerraTCG.Common.GameSystem.Drawing
{
    internal class TextureCache : ModSystem
    {

        internal static TextureCache Instance => ModContent.GetInstance<TextureCache>();

        internal Asset<Texture2D> Field { get; private set; }
        internal Asset<Texture2D> Zone { get; private set; }
        internal Asset<Texture2D> ZoneHighlighted { get; private set; }

        internal Asset<Texture2D> ZoneSelectable { get; private set; }

        internal Asset<Texture2D> CardBack { get; private set; }

        internal Asset<Texture2D> OffenseIcon { get; private set; }
        internal Asset<Texture2D> DefenseIcon { get; private set; }
        internal Asset<Texture2D> HeartIcon { get; private set; }

        internal Asset<Texture2D> ManaIcon { get; private set; }
        public Asset<Texture2D> MoveIcon { get; private set; }
        public Asset<Texture2D> Button { get; private set; }
        public Asset<Texture2D> ButtonHighlighted { get; private set; }
        public Asset<Texture2D> StarIcon { get; private set; }
        public Asset<Texture2D> TownsfolkIcon { get; private set; }
        public Asset<Texture2D> PlayerStatsZone { get; private set; }
        public Asset<Texture2D> AttackIcon { get; private set; }
        public Asset<Texture2D> LightRay { get; private set; }
        public Asset<Texture2D> MapBG { get; private set; }
        public Asset<Texture2D> CancelButton { get; private set; }
        public Asset<Texture2D> CardPreviewFrame { get; private set; }
        public Asset<Texture2D> BiomeIcons { get; private set; }
		public Asset<Texture2D> OwnedIcon { get; private set; }
		public Asset<Texture2D> EmoteIcons { get; private set; }
		public Asset<Texture2D> Foiling { get; private set; }
		public Asset<Texture2D> Sparkles { get; private set; }
		public Asset<Texture2D> Sparkles2 { get; private set; }
		public Asset<Texture2D> Glint { get; private set; }
		public Asset<Texture2D> WoFBack { get; private set; }
		public Asset<Texture2D> KingSlimeCrown { get; private set; }
		public Asset<Texture2D> QueenSlimeCore { get; private set; }
		internal Dictionary<int, Asset<Texture2D>> BestiaryTextureCache { get; private set; }
        internal Dictionary<int, Asset<Texture2D>> NPCTextureCache { get; private set; }
        internal Dictionary<int, Asset<Texture2D>> ItemTextureCache { get; private set; }
		internal Dictionary<int, Asset<Texture2D>> CustomNPCOverlayTextureCache { get; private set; }

        internal Dictionary<ModifierType, Asset<Texture2D>> ModifierIconTextures { get; private set; }
		public Dictionary<CardSubtype, Asset<Texture2D>> FoilMasks { get; private set; }
		public Dictionary<string, Asset<Texture2D>> CardFoilMasks { get; private set; }
		public Dictionary<string, Asset<Texture2D>> StaticCardOverlays { get; private set; }
		public Dictionary<CardSleeve, Asset<Texture2D>> CardSleeves { get; private set; }
		public Dictionary<CardSubtype, Asset<Texture2D>> BiomeMapBackgrounds { get; private set; }

		public Dictionary<string, Asset<Texture2D>> OtherMapBackgrounds { get; private set; }

        internal Dictionary<CardSubtype, Rectangle> BiomeIconBounds { get; private set; }
        internal Dictionary<CardSubtype, Rectangle> CardTypeEmoteBounds { get; private set; }
        internal Dictionary<CardSubtype, Asset<Texture2D>> SpecialCardSubtypes { get; private set; }

        internal const int TUTORIAL_SLIDE_COUNT = 20;

        public Asset<Texture2D> TutorialFrame { get; private set; }
        internal List<Asset<Texture2D>> TutorialSlides { get; private set; }
        internal List<Asset<Texture2D>> TutorialOverlays { get; private set; }
		public Asset<Texture2D> QueenSlimeCrown { get; internal set; }

		public override void Load()
        {
            base.Load();
            Field = Mod.Assets.Request<Texture2D>("Assets/FieldElements/Field");
            Zone = Mod.Assets.Request<Texture2D>("Assets/FieldElements/Zone");
            ZoneHighlighted = Mod.Assets.Request<Texture2D>("Assets/FieldElements/Zone_Highlighted");
            ZoneSelectable = Mod.Assets.Request<Texture2D>("Assets/FieldElements/Zone_Selectable");
            CardBack = Mod.Assets.Request<Texture2D>("Assets/FieldElements/Card_Back");
            OffenseIcon = Main.Assets.Request<Texture2D>("Images/UI/PVP_0");
            DefenseIcon = Main.Assets.Request<Texture2D>("Images/Item_" + ItemID.CobaltShield);
            HeartIcon = Main.Assets.Request<Texture2D>("Images/Item_" + ItemID.Heart);
            ManaIcon = Main.Assets.Request<Texture2D>("Images/Item_" + ItemID.Star);
            MoveIcon = Mod.Assets.Request<Texture2D>("Assets/FieldElements/Boots_Icon");
            Button = Mod.Assets.Request<Texture2D>("Assets/FieldElements/RadialButton");
            ButtonHighlighted = Main.Assets.Request<Texture2D>("Images/UI/Wires_1");
            StarIcon = Main.Assets.Request<Texture2D>("Images/Projectile_" + ProjectileID.FallingStar);
            TownsfolkIcon = Mod.Assets.Request<Texture2D>("Assets/FieldElements/TownsfolkMana");
            PlayerStatsZone = Mod.Assets.Request<Texture2D>("Assets/FieldElements/PlayerStats");
            AttackIcon = Mod.Assets.Request<Texture2D>("Assets/FieldElements/Attack_Icon");
            LightRay = Main.Assets.Request<Texture2D>("Images/Projectile_" + ProjectileID.MedusaHeadRay);
            CancelButton = Mod.Assets.Request<Texture2D>("Assets/FieldElements/CancelGame");
            CardPreviewFrame = Mod.Assets.Request<Texture2D>("Assets/FieldElements/CardPreviewFrame");
            BiomeIcons = Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Icon_Tags_Shadow");
            EmoteIcons = Main.Assets.Request<Texture2D>("Images/Extra_"+ExtrasID.EmoteBubble);
            Foiling = Mod.Assets.Request<Texture2D>("Assets/FoilMasks/Foil");
            Sparkles = Mod.Assets.Request<Texture2D>("Assets/FoilMasks/Sparkles");
            Sparkles2 = Mod.Assets.Request<Texture2D>("Assets/FoilMasks/Sparkles2");
            Glint = Mod.Assets.Request<Texture2D>("Assets/FoilMasks/Glint");
			WoFBack = Main.Assets.Request<Texture2D>("Images/WallOfFlesh");

            KingSlimeCrown = Main.Assets.Request<Texture2D>("Images/Extra_" + ExtrasID.KingSlimeCrown);
            QueenSlimeCore = Main.Assets.Request<Texture2D>("Images/Extra_" + ExtrasID.QueenSlimeCrystalCore);
            QueenSlimeCrown = Main.Assets.Request<Texture2D>("Images/Extra_" + ExtrasID.QueenSlimeCrown);

            NPCTextureCache = [];
            BestiaryTextureCache = [];
            ItemTextureCache = [];
			CustomNPCOverlayTextureCache = [];

            ModifierIconTextures = new Dictionary<ModifierType, Asset<Texture2D>>
            {
                [ModifierType.PAUSED] = Mod.Assets.Request<Texture2D>("Assets/FieldElements/PausedIcon"),
                [ModifierType.SPIKED] = Mod.Assets.Request<Texture2D>("Assets/FieldElements/Spiked_Icon"),
                [ModifierType.DEFENSE_BOOST] = Mod.Assets.Request<Texture2D>("Assets/FieldElements/Defense_Icon"),
                [ModifierType.EVASIVE] = Mod.Assets.Request<Texture2D>("Assets/FieldElements/Evasive_Icon"),
                [ModifierType.RELENTLESS] = Mod.Assets.Request<Texture2D>("Assets/FieldElements/Relentless_Icon"),
                [ModifierType.BLEEDING] = Mod.Assets.Request<Texture2D>("Assets/FieldElements/Bleed_Icon"),
                [ModifierType.POISON] = Mod.Assets.Request<Texture2D>("Assets/FieldElements/Poison_Icon"),
                [ModifierType.MORBID] = Mod.Assets.Request<Texture2D>("Assets/FieldElements/Morbid_Icon"),
                [ModifierType.LIFESTEAL] = Mod.Assets.Request<Texture2D>("Assets/FieldElements/Lifesteal_Icon"),
                [ModifierType.CURSED] = Mod.Assets.Request<Texture2D>("Assets/FieldElements/CursedIcon"),
                [ModifierType.FREEZING] = Mod.Assets.Request<Texture2D>("Assets/FieldElements/Freezing_Icon"),
                [ModifierType.SHIFTING_SANDS] = Mod.Assets.Request<Texture2D>("Assets/FieldElements/Sandy_Icon"),
            };

            FoilMasks = new Dictionary<CardSubtype, Asset<Texture2D>>
            {
                [CardSubtype.FOREST] = Mod.Assets.Request<Texture2D>("Assets/FoilMasks/FOREST"),
                [CardSubtype.CAVERN] = Mod.Assets.Request<Texture2D>("Assets/FoilMasks/CAVERN"),
                [CardSubtype.JUNGLE] = Mod.Assets.Request<Texture2D>("Assets/FoilMasks/JUNGLE"),
                [CardSubtype.GOBLIN_ARMY] = Mod.Assets.Request<Texture2D>("Assets/FoilMasks/GOBLIN_ARMY"),
                [CardSubtype.BLOOD_MOON] = Mod.Assets.Request<Texture2D>("Assets/FoilMasks/BLOOD_MOON"),
                [CardSubtype.OCEAN] = Mod.Assets.Request<Texture2D>("Assets/FoilMasks/OCEAN"),
                [CardSubtype.MUSHROOM] = Mod.Assets.Request<Texture2D>("Assets/FoilMasks/MUSHROOM"),
                [CardSubtype.EVIL] = Mod.Assets.Request<Texture2D>("Assets/FoilMasks/CRIMSON"),
                [CardSubtype.SNOW] = Mod.Assets.Request<Texture2D>("Assets/FoilMasks/SNOW"),
                [CardSubtype.CONSUMABLE] = Mod.Assets.Request<Texture2D>("Assets/FoilMasks/ITEM"),
                [CardSubtype.EQUIPMENT] = Mod.Assets.Request<Texture2D>("Assets/FoilMasks/ITEM"),
                [CardSubtype.TOWNSFOLK] = Mod.Assets.Request<Texture2D>("Assets/FoilMasks/TOWNSFOLK"),
            };

			CardFoilMasks = new Dictionary<string, Asset<Texture2D>>
			{
				[ModContent.GetInstance<KingSlime>().Card.FullName] = Mod.Assets.Request<Texture2D>("Assets/FoilMasks/KingSlime"),
				[ModContent.GetInstance<QueenBee>().Card.FullName] = Mod.Assets.Request<Texture2D>("Assets/FoilMasks/QueenBee"),
				[ModContent.GetInstance<Skeletron>().Card.FullName] = Mod.Assets.Request<Texture2D>("Assets/FoilMasks/Skeletron"),
				[ModContent.GetInstance<EyeOfCthulhu>().Card.FullName] = Mod.Assets.Request<Texture2D>("Assets/FoilMasks/EyeOfCthulhu"),
				[ModContent.GetInstance<EaterOfWorlds>().Card.FullName] = Mod.Assets.Request<Texture2D>("Assets/FoilMasks/EaterOfWorlds"),
				[ModContent.GetInstance<BrainOfCthulhu>().Card.FullName] = Mod.Assets.Request<Texture2D>("Assets/FoilMasks/BrainOfCthulhu"),
				[ModContent.GetInstance<QueenSlime>().Card.FullName] = Mod.Assets.Request<Texture2D>("Assets/FoilMasks/QueenSlime"),
				[ModContent.GetInstance<Deerclops>().Card.FullName] = Mod.Assets.Request<Texture2D>("Assets/FoilMasks/Deerclops"),
				[ModContent.GetInstance<Destroyer>().Card.FullName] = Mod.Assets.Request<Texture2D>("Assets/FoilMasks/Destroyer"),
				[ModContent.GetInstance<Twins>().Card.FullName] = Mod.Assets.Request<Texture2D>("Assets/FoilMasks/Twins"),
				[ModContent.GetInstance<SkeletronPrime>().Card.FullName] = Mod.Assets.Request<Texture2D>("Assets/FoilMasks/SkeletronPrime"),
				// TODO this is a bit hacky, foil crimson and corrupt EVIL cards differently
				[ModContent.GetInstance<FaceMonster>().Card.FullName] = Mod.Assets.Request<Texture2D>("Assets/FoilMasks/CRIMSON"),
				[ModContent.GetInstance<Crimera>().Card.FullName] = Mod.Assets.Request<Texture2D>("Assets/FoilMasks/CRIMSON"),
				[ModContent.GetInstance<Creeper>().Card.FullName] = Mod.Assets.Request<Texture2D>("Assets/FoilMasks/CRIMSON"),
				[ModContent.GetInstance<EaterOfSouls>().Card.FullName] = Mod.Assets.Request<Texture2D>("Assets/FoilMasks/CORRUPTION"),
				[ModContent.GetInstance<Devourer>().Card.FullName] = Mod.Assets.Request<Texture2D>("Assets/FoilMasks/CORRUPTION"),
				[ModContent.GetInstance<Leech>().Card.FullName] = Mod.Assets.Request<Texture2D>("Assets/FoilMasks/BLOOD_MOON"),
			};
			StaticCardOverlays = new Dictionary<string, Asset<Texture2D>>
			{
				[ModContent.GetInstance<KingSlime>().Card.FullName] = Mod.Assets.Request<Texture2D>("Assets/FoilMasks/KingSlime"),
			};

			CardSleeves = new Dictionary<CardSleeve, Asset<Texture2D>>
			{
				[CardSleeve.FOREST] = Mod.Assets.Request<Texture2D>("Assets/FieldElements/Card_Back"),
				[CardSleeve.CORRUPT] = Mod.Assets.Request<Texture2D>("Assets/FieldElements/Card_Back_Corrupt"),
				[CardSleeve.CRIMSON] = Mod.Assets.Request<Texture2D>("Assets/FieldElements/Card_Back_Crimson"),
				[CardSleeve.DUNGEON] = Mod.Assets.Request<Texture2D>("Assets/FieldElements/Card_Back_Dungeon"),
				[CardSleeve.EOC] = Mod.Assets.Request<Texture2D>("Assets/FieldElements/Card_Back_EoC"),
				[CardSleeve.JUNGLE] = Mod.Assets.Request<Texture2D>("Assets/FieldElements/Card_Back_Jungle"),
				[CardSleeve.SLIME] = Mod.Assets.Request<Texture2D>("Assets/FieldElements/Card_Back_Slime"),
				[CardSleeve.SNOW] = Mod.Assets.Request<Texture2D>("Assets/FieldElements/Card_Back_Snow"),
				[CardSleeve.HALLOWED] = Mod.Assets.Request<Texture2D>("Assets/FieldElements/Card_Back_Hallow"),
				[CardSleeve.WOF] = Mod.Assets.Request<Texture2D>("Assets/FieldElements/Card_Back_WoF"),
				[CardSleeve.SKELETRON_PRIME] = Mod.Assets.Request<Texture2D>("Assets/FieldElements/Card_Back_Prime"),
				[CardSleeve.DESTROYER] = Mod.Assets.Request<Texture2D>("Assets/FieldElements/Card_Back_Destroyer"),
				[CardSleeve.TWINS] = Mod.Assets.Request<Texture2D>("Assets/FieldElements/Card_Back_Twins"),
			};

            BiomeMapBackgrounds = new Dictionary<CardSubtype, Asset<Texture2D>>
            {
                [CardSubtype.FOREST] = Main.Assets.Request<Texture2D>("Images/MapBG1"),
                [CardSubtype.CAVERN] = Main.Assets.Request<Texture2D>("Images/MapBG2"),
                [CardSubtype.JUNGLE] = Main.Assets.Request<Texture2D>("Images/MapBG9"),
                [CardSubtype.GOBLIN_ARMY] = Main.Assets.Request<Texture2D>("Images/MapBG1"),
                [CardSubtype.BLOOD_MOON] = Main.Assets.Request<Texture2D>("Images/MapBG26"),
                [CardSubtype.OCEAN] = Main.Assets.Request<Texture2D>("Images/MapBG11"),
                [CardSubtype.MUSHROOM] = Main.Assets.Request<Texture2D>("Images/MapBG20"),
                [CardSubtype.SNOW] = Main.Assets.Request<Texture2D>("Images/MapBG12"),
                [CardSubtype.HALLOWED] = Main.Assets.Request<Texture2D>("Images/MapBG8"),
                [CardSubtype.DESERT] = Main.Assets.Request<Texture2D>("Images/MapBG10"),
            };
			OtherMapBackgrounds = new Dictionary<string, Asset<Texture2D>>
			{
				["CRIMSON"] = Main.Assets.Request<Texture2D>("Images/MapBG7"),
				["CORRUPTION"] = Main.Assets.Request<Texture2D>("Images/MapBG6"),
				["Skeletron"] = Main.Assets.Request<Texture2D>("Images/MapBG5"),
				["SkeletronPrime"] = Main.Assets.Request<Texture2D>("Images/MapBG1"),
				["Twins"] = Main.Assets.Request<Texture2D>("Images/MapBG1"),
				["Destroyer"] = Main.Assets.Request<Texture2D>("Images/MapBG1"),
				["QueenBee"] = Main.Assets.Request<Texture2D>("Images/MapBG16"),
				["WallOfFlesh"] = Main.Assets.Request<Texture2D>("Images/MapBG24"),
			};

            BiomeIconBounds = new Dictionary<CardSubtype, Rectangle>
            {
                [CardSubtype.FOREST] = new Rectangle(0, 0, 30, 30),
                [CardSubtype.CAVERN] = new Rectangle(60, 0, 30, 30),
                [CardSubtype.JUNGLE] = new Rectangle(180, 30, 30, 30),
                [CardSubtype.GOBLIN_ARMY] = new Rectangle(30, 90, 30, 30),
                [CardSubtype.BLOOD_MOON] = new Rectangle(180, 60, 30, 30),
                [CardSubtype.OCEAN] = new Rectangle(360, 30, 30, 30),
                [CardSubtype.MUSHROOM] = new Rectangle(240, 30, 30, 30),
                [CardSubtype.EVIL] = new Rectangle(360, 0, 30, 30),
                [CardSubtype.SNOW] = new Rectangle(150, 0, 30, 30),
                [CardSubtype.HALLOWED] = new Rectangle(30, 30, 30, 30),
            };

			SpecialCardSubtypes = new Dictionary<CardSubtype, Asset<Texture2D>>
			{
				[CardSubtype.OWNED] = Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Icon_Rank_Light"),
			};

            CardTypeEmoteBounds = new Dictionary<CardSubtype, Rectangle>
            {
                [CardSubtype.EQUIPMENT] = new Rectangle(137, 557, 30, 30),
                [CardSubtype.CONSUMABLE] = new Rectangle(103, 527, 30, 30),
                [CardSubtype.TOWNSFOLK] = new Rectangle(69, 753, 30, 30)
            };
        }

        public Asset<Texture2D> GetNPCTexture(int npcId)
        {
            if(!NPCTextureCache.TryGetValue(npcId, out var asset))
            {
                asset = Main.Assets.Request<Texture2D>($"Images/NPC_{npcId}");
                NPCTextureCache[npcId] = asset;
            }             
            return asset;
        }
        public Asset<Texture2D> GetItemTexture(int itemId)
        {
            if(!ItemTextureCache.TryGetValue(itemId, out var asset))
            {
                asset = Main.Assets.Request<Texture2D>($"Images/Item_{itemId}");
                ItemTextureCache[itemId] = asset;
            }             
            return asset;
        }

        public Asset<Texture2D> GetBestiaryTexture(int npcId)
        {
            if(!BestiaryTextureCache.TryGetValue(npcId, out var asset))
            {
                asset = Main.Assets.Request<Texture2D>($"Images/UI/Bestiary/NPCs/NPC_{npcId}");
				BestiaryTextureCache[npcId] = asset;
            }             
            return asset;
        }

		internal Asset<Texture2D> GetStaticNPCTexture(int npcId)
		{
            if(!BestiaryTextureCache.TryGetValue(npcId, out var asset))
            {
                asset = Mod.Assets.Request<Texture2D>($"Assets/CardOverlays/NPC_{npcId}");
				CustomNPCOverlayTextureCache[npcId] = asset;
            }             
            return asset;
		}

        // Tutorial images are large, don't load them if we don't need to
        public void LoadTutorial()
        {
            TutorialFrame = Mod.Assets.Request<Texture2D>($"Assets/Tutorial/TutorialFrame");
            TutorialSlides = [];
            TutorialOverlays = [];
            for(int i = 0; i < TUTORIAL_SLIDE_COUNT; i++)
            {
                TutorialSlides.Add(
                    Mod.Assets.Request<Texture2D>($"Assets/Tutorial/Tutorial{i}"));
            }
            for(int i = 0; i < 16; i++)
            {
                TutorialOverlays.Add(
                    Mod.Assets.Request<Texture2D>($"Assets/Tutorial/TutorialOverlay{i}"));
            }
        }

	}
}
