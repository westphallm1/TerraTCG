using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using TerraTCG.Common.GameSystem;
using TerraTCG.Common.GameSystem.BotPlayer;
using TerraTCG.Common.GameSystem.Drawing;
using TerraTCG.Common.GameSystem.GameState;
using TerraTCG.Common.UI;
using TerraTCG.Content.Items;
using static TerraTCG.Content.NPCs.NPCDuelReward;

namespace TerraTCG.Content.NPCs
{
	// TODO is this the correct location for this?
	internal enum CardSleeve
	{
		FOREST,
		CORRUPT,
		CRIMSON,
		DUNGEON,
		EOC,
		JUNGLE,
		SLIME,
		SNOW,
		WOF,
		HALLOWED,
		SKELETRON_PRIME,
		DESTROYER,
		TWINS
	}

	internal readonly struct NPCDuelReward(int itemId, int count)
	{
		public int ItemId { get; } = itemId;
		public int Count { get; } = count;

		public static NPCDuelReward GetReward<T>(int count) where T : ModItem
			=> new(ModContent.ItemType<T>(), count);
	}

	// Cache for NPC fields used in determining duel outcome -
	// used in case the NPC dies in multiplayer while the player is dueling it
	internal readonly struct NPCInfoCache(NPC npc)
	{
		public int NpcId { get; } = npc.netID;

		public string FullName { get; } = npc.FullName;

		public bool IsBoss { get; } = npc.boss || npc.netID == NPCID.EaterofWorldsHead;
	}

	internal readonly struct NamedNPCDeck(LocalizedText name, CardCollection deckList, List<NPCDuelReward> rewards, List<string> prerequisites, CardSleeve sleeve, bool isTutorial)
    {

        public NamedNPCDeck(
			string nameKey, 
			CardCollection deckList, 
			List<NPCDuelReward> rewards = null, 
			List<string> prerequisites = null, 
			CardSleeve sleeve = CardSleeve.FOREST, 
			bool isTutorial = false) : 
            this(
				Language.GetText($"Mods.TerraTCG.Cards.DeckNames.{nameKey}"), 
				deckList, 
				rewards, 
				(prerequisites ?? []).Select(p=>$"TerraTCG/Mods.TerraTCG.Cards.DeckNames.{p}").ToList(), 
				sleeve,
				isTutorial)
        {

        }
        public LocalizedText Name { get; } = name;

		public string Mod { get; } = ModContent.GetInstance<TerraTCG>().Name;

		public string Key => $"{Mod}/{Name.Key}";
        public CardCollection DeckList { get; } = deckList;
		public List<NPCDuelReward> Rewards { get; } = rewards;
		public bool IsTutorial { get; } = isTutorial;

		internal Asset<Texture2D> Sleeve => TextureCache.Instance.CardSleeves[sleeve];

		internal bool IsUnlocked(TCGPlayer player) =>
			prerequisites.Count == 0 || prerequisites.All(player.DefeatedDecks.Contains);
	}
    internal class NPCDeckMap : ModSystem
    {
        internal Dictionary<int, List<NamedNPCDeck>> NPCDecklists = new ()
        {
			// Duel-able Town NPCs
            [NPCID.Guide] = [
                new("Tutorial", BotDecks.GetStarterDeck(), isTutorial: true),
                new("ForestBeginner", BotDecks.GetStarterForestDeck(), [GetReward<ForestPack>(1)]),
                new("Forest", 
					BotDecks.GetForestDeck(), 
					[GetReward<ForestPack>(2), GetReward<InvitationToDuel>(2)], 
					["ForestBeginner"]),
                new("ForestAdvanced", 
					BotDecks.GetForestAdvancedDeck(), 
					[GetReward<ForestPack>(3), GetReward<InvitationToDuel>(2)], 
					["Forest", "WoF"])
            ],
            
			[NPCID.TownSlimeBlue] = [
                new("SlimeBeginner", BotDecks.GetStarterSlimeDeck(), [GetReward<SlimePack>(1)]),

                new("Slime", 
					BotDecks.GetSlimeDeck(), 
					[GetReward<SlimePack>(2), GetReward<InvitationToDuel>(2)], 
					["SlimeBeginner"]),

                new("SlimeAdvanced", 
					BotDecks.GetSlimeAdvancedDeck(), 
					[GetReward<SlimePack>(3), GetReward<InvitationToDuel>(2)], 
					["Slime", "WoF"])
            ],
            
			[NPCID.WitchDoctor] = [
                new("JungleBeginner", BotDecks.GetStarterJungleDeck(), [GetReward<JunglePack>(1)]),
                new("Jungle", 
					BotDecks.GetJungleDeck(), 
					[GetReward<JunglePack>(2), GetReward<InvitationToDuel>(2)], 
					["JungleBeginner"]),
                new("JungleAdvanced", 
					BotDecks.GetJungleDeck(), 
					[GetReward<JunglePack>(3), GetReward<InvitationToDuel>(2)], 
					["Jungle", "WoF"]),
            ],
            
			[NPCID.Nurse] = [
                new("BloodMoonBeginner", BotDecks.GetStarterBloodMoonDeck(), [GetReward<BloodMoonPack>(2)]),
                new("BloodMoon", 
					BotDecks.GetBloodMoonDeck(), 
					[GetReward<BloodMoonPack>(3), GetReward<InvitationToDuel>(2)], 
					["BloodMoonBeginner"]),
                new("BloodMoonAdvanced", 
					BotDecks.GetBloodMoonAdvancedDeck(), 
					[GetReward<BloodMoonPack>(4), GetReward<InvitationToDuel>(2)], 
					["BloodMoonAdvanced", "WoF"]),
            ],
            
			[NPCID.Merchant] = [
                new("SkeletonsBeginner", BotDecks.GetStarterSkeletonDeck(), [GetReward<CavernPack>(1)]),
                new("Skeletons", 
					BotDecks.GetSkeletonDeck(), 
					[GetReward<CavernPack>(2), GetReward<InvitationToDuel>(2)], 
					["SkeletonsBeginner"]),
            ],
            
			[NPCID.Clothier] = [
                new("Curse", BotDecks.GetCurseDeck(), [GetReward<DungeonPack>(1)]),
                new("CurseAdvanced", 
					BotDecks.GetSkeletronDeck(), 
					[GetReward<DungeonPack>(3), GetReward<InvitationToDuel>(2)],
					["Curse", "WoF"]),
            ],

			[NPCID.Dryad] = [
                new("EvilBeginner", BotDecks.GetStarterEvilDeck(), [GetReward<EvilPack>(1)]),
                new("Evil", 
					BotDecks.GetEvilDeck(), 
					[GetReward<EvilPack>(2), GetReward<InvitationToDuel>(2)], 
					["EvilBeginner"]),
                new("EvilAdvanced", 
					BotDecks.GetEvilAdvancedDeck(), 
					[GetReward<EvilPack>(3), GetReward<InvitationToDuel>(2)], 
					["Evil", "WoF"]),
            ],
            
			// TODO AI can't play this deck well
			//[NPCID.SkeletonMerchant] = [
   //             new("Salamander", BotDecks.GetSalamanderDeck(), [GetReward<MimicPack>(1)]),
   //         ],
            
			[NPCID.TravellingMerchant] = [
                new("Treasure", BotDecks.GetMimicDeck(), [GetReward<MimicPack>(1), GetReward<InvitationToDuel>(2)]),
            ],

            [NPCID.ArmsDealer] = [
                new("MushroomBeginner", BotDecks.GetStarterMushroomDeck(), [GetReward<MushroomPack>(1)]),
                new("Mushroom", 
					BotDecks.GetMushroomDeck(), 
					[GetReward<MushroomPack>(2), GetReward<InvitationToDuel>(2)], 
					["MushroomBeginner"]),
            ],

            [NPCID.GoblinTinkerer] = [
                new("GoblinsBeginner", BotDecks.GetStarterGoblinDeck(), [GetReward<GoblinPack>(1)]),
                new("Goblins", 
					BotDecks.GetGoblinDeck(), 
					[GetReward<GoblinPack>(2), GetReward<InvitationToDuel>(2)], 
					["GoblinsBeginner"]),
            ],

            [NPCID.Mechanic] = [
                new("SnowBeginner", BotDecks.GetStarterSnowDeck(), [GetReward<SnowPack>(1)]),
                new("Snow", 
					BotDecks.GetSnowDeck(), 
					[GetReward<SnowPack>(2), GetReward<InvitationToDuel>(2)], 
					["SnowBeginner"]),
            ],

            [NPCID.Angler] = [
                new("CrabsBeginner", BotDecks.GetStarterCrabDeck(), [GetReward<OceanPack>(1)]),
                new("Crabs", 
					BotDecks.GetCrabDeck(), 
					[GetReward<OceanPack>(2), GetReward<InvitationToDuel>(2)], 
					["CrabsBeginner"]),
                new("CrabsAdvanced", 
					BotDecks.GetCrabAdvancedDeck(), 
					[GetReward<OceanPack>(3), GetReward<InvitationToDuel>(2)], 
					["Crabs", "WoF"]),
            ],

			// Post-WoF NPCs

            [NPCID.PartyGirl] = [
                new("Hallowed", BotDecks.GetHallowedDeck(), [GetReward<HallowedPack>(2), GetReward<InvitationToDuel>(2)], ["WoF"]),
            ],

            [NPCID.BestiaryGirl] = [
                new("Bats", BotDecks.GetBatsDeck(), [GetReward<BatPack>(2), GetReward<InvitationToDuel>(2)], ["WoF"]),
                new("Critters", BotDecks.GetCrittersDeck(), [GetReward<CritterPack>(2), GetReward<InvitationToDuel>(2)], ["WoF"]),
            ],
			// Bosses
			[NPCID.QueenBee] = [
				new("QueenBee", BotDecks.GetQueenBeeDeck(), [GetReward<QueenBeePack>(2), GetReward<InvitationToDuel>(2)], sleeve: CardSleeve.JUNGLE),
			],
			[NPCID.KingSlime] = [
				new("KingSlime", BotDecks.GetKingSlimeDeck(), [GetReward<KingSlimePack>(2), GetReward<InvitationToDuel>(2)], sleeve: CardSleeve.SLIME),
			],
			[NPCID.BrainofCthulhu] = [
				new("BoC", BotDecks.GetBoCDeck(), [GetReward<BOCPack>(2), GetReward<InvitationToDuel>(2)], sleeve: CardSleeve.CRIMSON),
			],
			[NPCID.EyeofCthulhu] = [
				new("EoC", BotDecks.GetEoCDeck(), [GetReward<EOCPack>(2), GetReward<InvitationToDuel>(2)], sleeve: CardSleeve.EOC),
			],
			[NPCID.EaterofWorldsHead] = [
				new("EoW", BotDecks.GetEoWDeck(), [GetReward<EOWPack>(2), GetReward<InvitationToDuel>(2)], sleeve: CardSleeve.CORRUPT),
			],
			[NPCID.SkeletronHead] = [
				new("Skeletron", BotDecks.GetSkeletronDeck(), [GetReward<SkeletronPack>(3), GetReward<InvitationToDuel>(2)], sleeve: CardSleeve.DUNGEON),
			],
			[NPCID.WallofFlesh] = [
				new("WoF", BotDecks.GetWallOfFleshDeck(), [GetReward<WOFPack>(3), GetReward<InvitationToDuel>(2)], sleeve: CardSleeve.WOF),
			],
			[NPCID.Deerclops] = [
				new("Deerclops", BotDecks.GetDeerclopsDeck(), [GetReward<DeerclopsPack>(2), GetReward<InvitationToDuel>(2)], sleeve: CardSleeve.SNOW),
			],
			[NPCID.QueenSlimeBoss] = [
				new("QueenSlime", BotDecks.GetQueenSlimeDeck(), [GetReward<QueenSlimePack>(2), GetReward<InvitationToDuel>(2)], sleeve: CardSleeve.HALLOWED),
			],
			[NPCID.SkeletronPrime] = [
				new("SkeletronPrime", BotDecks.GetSkeletronPrimeDeck(), [GetReward<SkeletronPrimePack>(2), GetReward<InvitationToDuel>(2)], sleeve: CardSleeve.SKELETRON_PRIME),
			],
			[NPCID.TheDestroyer] = [
				new("Destroyer", BotDecks.GetDestroyerDeck(), [GetReward<DestroyerPack>(2), GetReward<InvitationToDuel>(2)], sleeve: CardSleeve.DESTROYER),
			],
			[NPCID.Retinazer] = [
				new("Destroyer", BotDecks.GetTwinsDeck(), [GetReward<TwinsPack>(2), GetReward<InvitationToDuel>(2)], sleeve: CardSleeve.TWINS),
			]
        };
    }
    internal class DuelDialogGlobalNPC : GlobalNPC
    {
        public override void GetChat(NPC npc, ref string chat)
        {
            if (ModContent.GetInstance<NPCDeckMap>().NPCDecklists.TryGetValue(npc.netID, out var lists) &&
				lists.Any(l=>l.IsUnlocked(TCGPlayer.LocalPlayer)))
            {
                ModContent.GetInstance<UserInterfaces>().StartNPCChat();
            }
        }
    }
}
