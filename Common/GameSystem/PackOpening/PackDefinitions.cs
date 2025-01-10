﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using TerraTCG.Common.GameSystem.Drawing;
using TerraTCG.Common.GameSystem.GameState;
using TerraTCG.Common.UI;
using static TerraTCG.Common.GameSystem.PackOpening.CardPools;

namespace TerraTCG.Common.GameSystem.PackOpening
{
	internal readonly struct Pack(int count, params CardCollection[] pools)
	{
		List<CardCollection> Pools { get; } = [.. pools];
		int Count { get; } = count;

        private static Card SelectCardFromPools(List<Card> current, List<CardCollection> pools)
		{
			int poolIdx = Main.rand.Next(pools.Count);
			var pool = pools[poolIdx];
			// prevent duplicates
			var deduplicated = pool.Cards.Where(c => !current.Contains(c)).ToList();
			if(deduplicated.Count > 0)
			{
				return deduplicated[Main.rand.Next(deduplicated.Count)];
			}

			return pool.Cards[Main.rand.Next(pool.Cards.Count)];
		}

		internal void OpenPack(TCGPlayer recipient)
		{
			// first card in pack always comes from first
			// card in pool
			List<Card> packCards = [SelectCardFromPools([], [Pools[0]])];
			// TODO this is an inefficient way to give a 1/11 chance at any card
			List<CardCollection> wildcardPool = [.. Pools, .. Pools, .. Pools, .. Pools, .. Pools, AllCards];
			for(int i = 1; i < Count; i++)
			{
				packCards.Add(SelectCardFromPools(packCards, wildcardPool));
			}
			// Shuffle the card order so it's not obvious the first card
			// is from the first pool
			var shuffledCards = packCards.OrderBy(p => Main.rand.Next()).ToList();

            recipient.AddCardsToCollection(shuffledCards);

            CardWithTextRenderer.Instance.ToRender = shuffledCards;
            ModContent.GetInstance<UserInterfaces>().StartPackOpening();
		}
	}
	internal class PackDefinitions
	{

		public static Pack ForestPack => new(3, ForestCards, CommonCards);
		public static Pack CavernPack => new(3, CavernCards, CommonCards);
		public static Pack JunglePack => new(3, JungleCards, CommonCards);
		public static Pack BloodMoonPack => new(3, BloodMoonCards, CommonCards);
		public static Pack GoblinPack => new(3, GoblinCards, CommonCards);
		public static Pack DungeonPack => new(3, DungeonCards, CommonCards);
		public static Pack EvilPack => new(3, EvilCards, CommonCards);
		public static Pack OceanPack => new(3, OceanCards, CommonCards);
		public static Pack MushroomPack => new(3, MushroomCards, CommonCards);
		public static Pack SlimePack => new(3, SlimeCards, CommonCards);

		public static Pack QueenBeePack => new(3, QueenBeePromoCards, JungleCards);
		public static Pack BOCPack => new(3, BOCPromoCards, EvilCards);
		public static Pack KingSlimePack => new(3, KingSlimePromoCards, SlimeCards);
		public static Pack MimicPack => new(3, MimicPromoCards, CavernCards);

	}
}