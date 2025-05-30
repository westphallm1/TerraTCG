﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent;
using Terraria.Localization;
using TerraTCG.Common.GameSystem.GameState;

namespace TerraTCG.Common.GameSystem.Drawing.Animations.FieldAnimations
{
    internal class AFKWarningAnimation(TimeSpan startTime, Turn turn) : IFieldAnimation
    {
        public TimeSpan StartTime { get; } = startTime;

        public TimeSpan Duration { get; } = TimeSpan.FromSeconds(1.5f);
        public TimeSpan ElapsedTime => TCGPlayer.TotalGameTime - StartTime;

        public void DrawFieldOverlay(SpriteBatch spriteBatch, Vector2 basePosition)
        {
            var gamePlayer = TCGPlayer.LocalGamePlayer;
            var textPos = ProjectedFieldUtils.Instance.WorldSpaceToScreenSpace(gamePlayer, gamePlayer.Field.Zones[1], new(0.5f, -0.25f));
            var font = FontAssets.MouseText.Value;
			var text = Language.GetText("Mods.TerraTCG.Cards.Common.AFKWarning").Format(turn.ActivePlayer.Controller.Name);
            var scale = Math.Min(1, 2 * MathF.Sin(MathF.PI * (float)(ElapsedTime.TotalSeconds / Duration.TotalSeconds)));
            var color = turn.ActivePlayer == gamePlayer ? Color.Coral : Color.SkyBlue;
            CardTextRenderer.Instance.DrawStringWithBorder(spriteBatch, text, basePosition + textPos, color: color, scale: scale, centered: true, font: font);
        }

        public bool IsComplete() => ElapsedTime > Duration;
    }
}
