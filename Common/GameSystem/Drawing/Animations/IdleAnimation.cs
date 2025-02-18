using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using TerraTCG.Common.GameSystem.GameState;

namespace TerraTCG.Common.GameSystem.Drawing.Animations
{
    internal class IdleAnimation(
        PlacedCard placedCard, TimeSpan duration = default, int? healthOverride = null, bool? exertedOverride = null) : IAnimation
    {
        public TimeSpan StartTime { get; set; }
        public Zone SourceZone { private get; set; }

		public TimeSpan Duration => duration;

        private static TimeSpan Period { get; } = TimeSpan.FromSeconds(2f);
        private TimeSpan ElapsedTime => TCGPlayer.TotalGameTime - StartTime;

        public void DrawZone(SpriteBatch spriteBatch, Vector2 basePosition, float rotation)
        {
            if (placedCard == null)
            {
                return;
            }
            AnimationUtils.DrawZoneCard(spriteBatch, placedCard, basePosition, rotation, ZoneColor(placedCard, exertedOverride));
        }

        public void DrawZoneOverlay(SpriteBatch spriteBatch, Vector2 basePosition, float baseScale)
        {
            if(placedCard == null) return;
            var posOffset = IdleHoverPos(placedCard, baseScale);
            var zoneColor = OverlayColor(placedCard, exertedOverride);
            AnimationUtils.DrawZoneNPC(spriteBatch, SourceZone, placedCard, basePosition + new Vector2(0, posOffset), baseScale, zoneColor);
        }

		public void DrawZoneStats(SpriteBatch spriteBatch, Vector2 basePosition, float baseScale)
		{
            AnimationUtils.DrawZoneNPCStats(spriteBatch, SourceZone, placedCard, baseScale, health: healthOverride ?? placedCard.CurrentHealth);
		}

        public bool IsComplete() => duration != default && ElapsedTime > duration;
        public bool IsDefault() => duration == default;

        public static float IdleHoverPos(PlacedCard placedCard, float baseScale)
        {
            var idleElapsed = TCGPlayer.TotalGameTime - placedCard.PlaceTime;
            return baseScale * 3f * MathF.Sin(MathF.Tau * (float) (idleElapsed.TotalSeconds / Period.TotalSeconds));
        }

        public static Color ZoneColor(PlacedCard placedCard, bool? exertedOverride = null) => exertedOverride ?? placedCard.IsExerted ? Color.LightGray : Color.White;
        public static Color OverlayColor(PlacedCard placedCard, bool? exertedOverride = null) => exertedOverride ?? placedCard.IsExerted ? Color.Gray : Color.White;
    }
}
