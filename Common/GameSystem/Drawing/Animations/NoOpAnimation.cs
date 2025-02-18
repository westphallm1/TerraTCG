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
    internal class NoOpAnimation(TimeSpan duration = default) : IAnimation
    {
        public TimeSpan StartTime { get; set; }

		public TimeSpan Duration => duration;
        public Zone SourceZone { private get; set; }

        private TimeSpan ElapsedTime => TCGPlayer.TotalGameTime - StartTime;

        public void DrawZone(SpriteBatch spriteBatch, Vector2 basePosition, float rotation)
        {
            // No-op : Do nothing
        }

        public void DrawZoneOverlay(SpriteBatch spriteBatch, Vector2 basePosition, float baseScale)
        {
            // No-op : Do nothing
        }

		public void DrawZoneStats(SpriteBatch spriteBatch, Vector2 basePosition, float baseScale)
		{
            // No-op : Do nothing
		}

        public bool IsComplete() => ElapsedTime > duration;

    }
}
