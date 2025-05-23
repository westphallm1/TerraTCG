﻿using Microsoft.Xna.Framework;
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
using TerraTCG.Common.GameSystem.GameState;
using TerraTCG.Common.UI;
using TerraTCG.Content.NPCs;

namespace TerraTCG.Content.Items
{
	internal class InvitationToDuel : ModItem
	{
		const int MAX_BOSS_DIST_SQ = 1020 * 1020;

		public override void SetDefaults()
		{
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useAnimation = 90;
            Item.useTime = 90;
            Item.rare = ItemRarityID.Blue;
			Item.maxStack = 9999;
			Item.width = 32;
			Item.height = 32;
			Item.consumable = true;
            Item.UseSound = SoundID.Item156;
		}

		private static void StartGameAndRigBossHand(IGamePlayerController myPlayer, IGamePlayerController opponent)
		{
			// Ensure the boss always starts with itself in hand after shuffling
			var bossCard = opponent.Deck.Cards[0];
			var game = ModContent.GetInstance<GameModSystem>().StartGame(myPlayer, opponent);
			if (!game.GamePlayers[1].Hand.Cards.Contains(bossCard))
			{
				// TODO is it OK to just overwrite a card like this? Probably not too many
				// practical ramifications
				game.GamePlayers[1].Hand.Cards[0] = bossCard;
				// Make sure the boss can't get a second copy during the duel
				game.GamePlayers[1].Deck.Cards.Remove(bossCard);
			}
		}

		private static void StartDuelWithNearestBoss(Player player)
		{
			var boss = GetNearestDuelableBoss(player);
			if(boss == null && TCGPlayer.LocalPlayer.NPCInfo.NpcId == 0)
			{
				return;
			}
			var npcId = boss?.netID ?? TCGPlayer.LocalPlayer.NPCInfo.NpcId; 
			var bossLists = ModContent.GetInstance<NPCDeckMap>().NPCDecklists[npcId];
			// Exit out of the duel dialogue if the player does not have a valid decklist
			if(!TCGPlayer.LocalPlayer.Deck.ValidateDeck())
			{
				Main.NewText(Language.GetTextValue("Mods.TerraTCG.Cards.Common.DeckNotValid"), Color.Red);
				return;
			}
			var bossList = bossLists.First();
			var myPlayer = TCGPlayer.LocalPlayer;
			var opponent = new SimpleBotPlayer()
			{
				Deck = bossList.DeckList,
				Rewards = bossList.Rewards,
				DeckName = bossList.Key,
				Sleeve = bossList.Sleeve,
			};
			if(boss != null)
			{
				myPlayer.NPCInfo = new(boss);
			}
			StartGameAndRigBossHand(myPlayer, opponent);
		}

		private static NPC GetNearestDuelableBoss(Player player) => Main.npc
				.Where(npc => npc.active && npc.whoAmI < Main.maxNPCs && npc.boss || (npc.netID == NPCID.EaterofWorldsHead))
				.Where(npc => Vector2.DistanceSquared(player.Center, npc.Center) < MAX_BOSS_DIST_SQ)
				.Where(npc => ModContent.GetInstance<NPCDeckMap>().NPCDecklists.ContainsKey(npc.netID))
				.FirstOrDefault();

		// Via ExampleMagicMirror from ExampleMod
		public override void UseStyle(Player player, Rectangle heldItemFrame)
		{
			// This is client side only
			if(player.whoAmI != Main.myPlayer)
			{
				return;
			}

			if (Main.rand.NextBool())
			{
				Dust.NewDust(player.position, player.width, player.height, DustID.MagicMirror, 0f, 0f, 150, Color.White, 1.1f); // Makes dust from the player's position and copies the hitbox of which the dust may spawn. Change these arguments if needed.
			}

			// This sets up the itemTime correctly.
			if (player.itemTime == 0)
			{
				player.ApplyItemTime(Item);
				if(GetNearestDuelableBoss(player) is NPC boss)
				{
					Main.NewText($"{boss.FullName} {Language.GetTextValue("Mods.TerraTCG.Cards.Common.AcceptedInvitation")}");
				}
			}
			else if (player.itemTime == 10)
			{
				for (int d = 0; d < 70; d++)
				{
					Dust.NewDust(player.position, player.width, player.height, DustID.MagicMirror, player.velocity.X * 0.5f, player.velocity.Y * 0.5f, 150, default, 1.5f);
				}
			} else if (player.itemTime == 1)
			{
				StartDuelWithNearestBoss(player);
			}
		}

		public override bool CanUseItem(Player player)
		{
			if (player.whoAmI == Main.myPlayer && GetNearestDuelableBoss(player) is NPC boss)
			{
				if(!TCGPlayer.LocalPlayer.Deck.ValidateDeck())
				{
					Main.NewText(Language.GetTextValue("Mods.TerraTCG.Cards.Common.DeckNotValid"), Color.Red);
					return false;
				}
				TCGPlayer.LocalPlayer.NPCInfo = new(boss);
				return true;
			}
			return false;
		}
	}
}
