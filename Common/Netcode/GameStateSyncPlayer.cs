﻿using Microsoft.Xna.Framework;
using Newtonsoft.Json.Bson;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using TerraTCG.Common.GameSystem;
using TerraTCG.Common.GameSystem.GameState;
using TerraTCG.Common.Netcode.Packets;

namespace TerraTCG.Common.Netcode
{
	// ModPlayer that syncs basic info about each players in-game/looking for game
	// state, used to recover from in-game desyncs
	internal class GameStateSyncPlayer : ModPlayer
	{
		// Whether the client thinks it's in game
		internal bool InGame { get; set; }

		// Whether the client thinks it's in the "looking for game" state
		internal bool LookingForGame { get; set; }

		// If the client is in-game, the position in the event
		// sequence that it thinks it's in
		internal TurnOrder TurnOrder { get; set; }

		private TimeSpan LastSyncTime { get; set; }

		public void UpdateLocalState()
		{
			if(Main.gameTimeCache.TotalGameTime - LastSyncTime > TimeSpan.FromSeconds(2f))
			{
				BroadcastSyncState();
			}
		}

		public void BroadcastSyncState()
		{
			if(Player.whoAmI != Main.myPlayer)
			{
				return;
			}

			var localPlayer = TCGPlayer.LocalGamePlayer;
			InGame = localPlayer != null;
			LookingForGame &= !InGame;

			if(localPlayer?.Game.CurrentTurn is Turn turn)
			{
				TurnOrder = new TurnOrder {
					TurnIndex = turn.TurnCount,
					ActionIndex = turn.ActionLog.Count,
				};
			} else
			{
				TurnOrder = new();
			}

			new PlayerGameStatePacket(Player).Send();
			LastSyncTime = Main.gameTimeCache.TotalGameTime;
		}

		public override void PreUpdate()
		{
			if(Player.whoAmI == Main.myPlayer)
			{
				UpdateLocalState();
			}
		}

		public void NetworkUpdate(bool inGame, bool lookingForGame, TurnOrder turnOrder)
		{
			if(lookingForGame && !LookingForGame)
			{
				Main.NewText($"{Player.name} is looking for a game of TerraTCG!", Color.DeepSkyBlue);
			} 
			if(InGame && !inGame)
			{
				ResetSyncState();
			}

			InGame = inGame;
			LookingForGame = lookingForGame;
			TurnOrder = turnOrder;
		}

		public void ResetSyncState()
		{
			// Clear out any queued messages for the player this game - prevent re-sending
			// stale messages when the player starts another game
			GameActionPacketQueue.Instance.ClearPlayerQueue(Player);

			if(TCGPlayer.LocalGamePlayer?.Opponent.Controller is NetSyncGamePlayerController controller &&
				controller.PlayerId == Player.whoAmI)
			{
				// If the standard net syncing failed to exit the local player out of their game
				// with this opponent, try to exit again here
				controller.GamePlayer.Surrender();
			}
		}

		public override void PlayerDisconnect()
		{
			ResetSyncState();
		}
	}
}
