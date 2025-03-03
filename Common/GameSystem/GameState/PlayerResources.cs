﻿using log4net.Appender;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Localization;
using static TerraTCG.Common.GameSystem.GameState.GameActions.IGameAction;

namespace TerraTCG.Common.GameSystem.GameState
{
    readonly struct PlayerResources(int health, int mana, int townsfolkMana)
    {
        public int Health { get; } = health;
        public int Mana { get; } = mana;
        public int TownsfolkMana { get; } = townsfolkMana;

        public TimeSpan SetTime { get; } = TCGPlayer.TotalGameTime;

        public PlayerResources UseResource(int health = 0, int mana = 0, int townsfolkMana = 0)
            => new(Health - health, Mana - mana, TownsfolkMana - townsfolkMana);

		public static PlayerResources operator -(PlayerResources a, PlayerResources b)
			=> a.UseResource(b.Health, b.Mana, b.TownsfolkMana);

		public string ToTooltipString()
		{
			var builder = new StringBuilder();
			int appendCount = 0;

			Append(Health, Language.GetTextValue("Mods.TerraTCG.Cards.Common.Hearts"));
			Append(Mana, Language.GetTextValue("Mods.TerraTCG.Cards.Common.Mana"));
			Append(TownsfolkMana, Language.GetTextValue("Mods.TerraTCG.Cards.Common.Townsfolk"));

			if (appendCount > 0)
				builder.Append($" {ActionText("To")}");

			return builder.ToString();

			void Append(int amount, string resource)
			{
				if (amount == 0) return;
				if (appendCount == 0) builder.Append($"{ActionText("Use")} ");
				if (appendCount > 0) builder.Append(", ");
				builder.Append($"{amount} {resource}");
				appendCount++;
			}
		}
    }
}
