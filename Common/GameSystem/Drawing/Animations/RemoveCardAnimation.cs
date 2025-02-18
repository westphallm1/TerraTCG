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
    internal class RemoveCardAnimation(PlacedCard leavingCard) : IAnimation
    {
        public TimeSpan StartTime { get; set; } 
        public Zone SourceZone { private get; set; }

        public TimeSpan Duration { get; } = TimeSpan.FromSeconds(0.25f);

        private TimeSpan ElapsedTime => TCGPlayer.TotalGameTime - StartTime;

        public void DrawZone(SpriteBatch spriteBatch, Vector2 basePosition, float rotation)
        {
            var transparency = Math.Max(0, 1 - (float)(ElapsedTime.TotalSeconds/ Duration.TotalSeconds));
            var zoneColor = IdleAnimation.ZoneColor(leavingCard);
            AnimationUtils.DrawZoneCard(spriteBatch, leavingCard, basePosition, rotation, zoneColor * transparency);
        }

        public void DrawZoneOverlay(SpriteBatch spriteBatch, Vector2 basePosition, float baseScale)
        {
            var scale = MathHelper.Lerp(baseScale, 0, (float) (ElapsedTime.TotalSeconds/ Duration.TotalSeconds));
            var transparency = Math.Max(0, 1 - (float)(ElapsedTime.TotalSeconds/ Duration.TotalSeconds));
            AnimationUtils.DrawZoneNPC(
                spriteBatch, SourceZone, leavingCard, basePosition, scale, Color.White * transparency);
        }

		public void DrawZoneStats(SpriteBatch spriteBatch, Vector2 basePosition, float baseScale)
		{
            var transparency = Math.Max(0, 1 - (float)(ElapsedTime.TotalSeconds/ Duration.TotalSeconds));
            AnimationUtils.DrawZoneNPCStats(
                spriteBatch, 
                SourceZone, 
                leavingCard,
                baseScale, 
                transparency: transparency, 
                health: leavingCard.CurrentHealth);
		}

        public bool IsComplete() =>
            TCGPlayer.TotalGameTime > StartTime + Duration;
    }
}
