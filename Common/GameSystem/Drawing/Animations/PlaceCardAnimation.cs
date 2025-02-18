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
    internal class PlaceCardAnimation(PlacedCard placedCard) : IAnimation
    {
        public TimeSpan StartTime { get; set;  } 
        public Zone SourceZone { private get; set; }

        public TimeSpan Duration { get; } = TimeSpan.FromSeconds(0.25f);

        private TimeSpan ElapsedTime => TCGPlayer.TotalGameTime - StartTime;

        public void DrawZone(SpriteBatch spriteBatch, Vector2 basePosition, float rotation)
        {
            float transparency = Math.Min(1, (float)(ElapsedTime.TotalSeconds/ Duration.TotalSeconds));
            var zoneColor = placedCard.IsExerted ? Color.LightGray : Color.White;
            AnimationUtils.DrawZoneCard(spriteBatch, placedCard, basePosition, rotation, zoneColor * transparency);
        }

        public void DrawZoneOverlay(SpriteBatch spriteBatch, Vector2 basePosition, float baseScale)
        {
            var scale = MathHelper.Lerp(0, baseScale, (float) (ElapsedTime.TotalSeconds/ Duration.TotalSeconds));
            var transparency = Math.Min(1, (float)(ElapsedTime.TotalSeconds/ Duration.TotalSeconds));
            var zoneColor = placedCard.IsExerted ? Color.Gray : Color.White;
            AnimationUtils.DrawZoneNPC(spriteBatch, SourceZone, placedCard, basePosition, scale, zoneColor * transparency);
        }
		public void DrawZoneStats(SpriteBatch spriteBatch, Vector2 basePosition, float baseScale)
		{
            var transparency = Math.Min(1, (float)(ElapsedTime.TotalSeconds/ Duration.TotalSeconds));
            AnimationUtils.DrawZoneNPCStats(spriteBatch, SourceZone, placedCard, baseScale, transparency: transparency);
		}

        public bool IsComplete() =>
            TCGPlayer.TotalGameTime > StartTime + Duration;
    }
}
