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
    internal class ActionAnimation(PlacedCard placedCard) : IAnimation
    {
        public TimeSpan StartTime { get; set; }
        public Zone SourceZone { private get; set; }
        public TimeSpan Duration { get; } = TimeSpan.FromSeconds(1f);
        private TimeSpan ElapsedTime => TCGPlayer.TotalGameTime - StartTime;

        public void DrawZone(SpriteBatch spriteBatch, Vector2 basePosition, float rotation) =>
            AnimationUtils.DrawZoneCard(spriteBatch, placedCard, basePosition, rotation, IdleAnimation.ZoneColor(placedCard));

        public void DrawZoneOverlay(SpriteBatch spriteBatch, Vector2 basePosition, float baseScale)
        {
            var posOffset = baseScale * (-5f + 5f * MathF.Cos(2*MathF.Tau * (float) (ElapsedTime.TotalSeconds / Duration.TotalSeconds)));
            var zoneColor = IdleAnimation.OverlayColor(placedCard);
            AnimationUtils.DrawZoneNPC(spriteBatch, SourceZone, placedCard, basePosition + new Vector2(0, posOffset), baseScale, zoneColor);
        }

		public void DrawZoneStats(SpriteBatch spriteBatch, Vector2 basePosition, float baseScale)
		{
            AnimationUtils.DrawZoneNPCStats(spriteBatch, SourceZone, placedCard, baseScale);
		}

		public bool IsComplete() => ElapsedTime > Duration;
    }
}
