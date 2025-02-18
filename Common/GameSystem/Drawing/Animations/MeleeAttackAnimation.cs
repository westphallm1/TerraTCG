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
    internal class MeleeAttackAnimation(PlacedCard placedCard, Zone targetZone) : IAnimation
    {
        public TimeSpan StartTime { get; set; } 
        public Zone SourceZone { private get; set; }

        public TimeSpan Duration { get; } = TimeSpan.FromSeconds(1f);
        private TimeSpan ElapsedTime => TCGPlayer.TotalGameTime - StartTime;

        private float WindupDuration => (float)Duration.TotalSeconds * 0.25f;
        private float SwingDuration => (float)Duration.TotalSeconds * 0.5f;

        private Vector2 Destination
        {
            get
            {
                var localPlayer = TCGPlayer.LocalPlayer;
                var gamePlayer = localPlayer.GamePlayer;
                var yLerpPoint = gamePlayer.Owns(targetZone) ? 0.3f : 0.8f;
                var placement = ProjectedFieldUtils.Instance.WorldSpaceToScreenSpace(gamePlayer, targetZone, new(0.5f, yLerpPoint));
                return localPlayer.GameFieldPosition + placement;
            }
        }

        public void DrawZone(SpriteBatch spriteBatch, Vector2 basePosition, float rotation) {
            var drawColor = Color.White;
            if(ElapsedTime.TotalSeconds > WindupDuration + SwingDuration)
            {
                var colorLerp = (ElapsedTime.TotalSeconds - (WindupDuration + SwingDuration)) / WindupDuration;
                drawColor = Color.Lerp(Color.White, Color.LightGray, (float)colorLerp);
            }
            AnimationUtils.DrawZoneCard(spriteBatch, placedCard, basePosition, rotation, drawColor);
        }

        public void DrawZoneOverlay(SpriteBatch spriteBatch, Vector2 basePosition, float baseScale)
        {
            float lerpPoint;
            var drawColor = Color.White;
            if(ElapsedTime.TotalSeconds <= WindupDuration)
            {
                lerpPoint = -0.25f * MathF.Sin(MathF.PI * (float) (ElapsedTime.TotalSeconds / WindupDuration));
            } else if (ElapsedTime.TotalSeconds <= WindupDuration + SwingDuration)
            {
                lerpPoint = MathF.Sin(MathF.PI * (float) ((ElapsedTime.TotalSeconds - WindupDuration) / SwingDuration));
            } else
            {
                var colorLerp = (ElapsedTime.TotalSeconds - (WindupDuration + SwingDuration)) / WindupDuration;
                drawColor = Color.Lerp(Color.White, Color.LightGray, (float)colorLerp);
                lerpPoint = -0.25f * MathF.Sin(MathF.PI * (float) ((ElapsedTime.TotalSeconds - (WindupDuration + SwingDuration)) / WindupDuration));
            }

            // Do two walk cycles in the span of the animation
            var npcId = placedCard.Template.NPCID;
            int frame = 0;
            if(npcId > 0)
            {
                int totalFrames = 2 * Main.npcFrameCount[npcId];
                float currentFrame = MathHelper.Lerp(0, totalFrames, (float)(ElapsedTime.TotalSeconds / Duration.TotalSeconds));
                frame = (int)currentFrame % Main.npcFrameCount[npcId];
            }

            var currentX = MathHelper.Lerp(basePosition.X, Destination.X, lerpPoint);
            var currentY = MathHelper.Lerp(basePosition.Y, Destination.Y, lerpPoint);
            AnimationUtils.DrawZoneNPC(spriteBatch, SourceZone, placedCard, new(currentX, currentY), baseScale, color: drawColor, frame: frame);
        }
		public void DrawZoneStats(SpriteBatch spriteBatch, Vector2 basePosition, float baseScale)
		{
            AnimationUtils.DrawZoneNPCStats(spriteBatch, SourceZone, placedCard, baseScale);
		}

        public bool IsComplete() =>
            TCGPlayer.TotalGameTime > StartTime + Duration;
    }
}
