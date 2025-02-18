using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using TerraTCG.Common.GameSystem.GameState;
using TerraTCG.Common.GameSystem.GameState.Modifiers;

namespace TerraTCG.Common.GameSystem.Drawing.Animations
{
    internal class RemoveModifierAnimation(PlacedCard placedCard, Asset<Texture2D> modifierTexture) : IAnimation
    {
        public TimeSpan StartTime { get; set; }
        public Zone SourceZone { private get; set; }
        private TimeSpan ElapsedTime => TCGPlayer.TotalGameTime - StartTime;
        public TimeSpan Duration => TimeSpan.FromSeconds(1.25f);
        private TimeSpan Period => TimeSpan.FromSeconds(2f);

        public void DrawZone(SpriteBatch spriteBatch, Vector2 basePosition, float rotation)
        {
            var zoneColor = placedCard.IsExerted ? Color.LightGray : Color.White;
            AnimationUtils.DrawZoneCard(spriteBatch, placedCard, basePosition, rotation, zoneColor);
        }

        public void DrawZoneOverlay(SpriteBatch spriteBatch, Vector2 basePosition, float baseScale)
        {
            var posOffset = IdleAnimation.IdleHoverPos(placedCard, baseScale);
            var zoneColor = IdleAnimation.OverlayColor(placedCard);
            var lerpPoint = (float)Math.Min(1, 4f * (ElapsedTime.TotalSeconds));
            var itemOffset = MathHelper.Lerp(0f, 48f, lerpPoint) + posOffset;
            AnimationUtils.DrawZoneNPC(spriteBatch, SourceZone, placedCard, basePosition + Vector2.UnitY * posOffset, baseScale, zoneColor);
            AnimationUtils.DrawZoneNPCStats(spriteBatch, SourceZone, placedCard, baseScale);

            // Draw the item itself on top of everything
            if(modifierTexture?.Value is var texture)
            {
                var bounds = texture.Bounds;
                var origin = new Vector2(bounds.Width, bounds.Height) / 2;
                spriteBatch.Draw(texture, basePosition - Vector2.UnitY * itemOffset, bounds,
                    Color.White, 0f,
                    origin, baseScale * lerpPoint, 0, 0);
            }
        }

		public void DrawZoneStats(SpriteBatch spriteBatch, Vector2 basePosition, float baseScale)
		{
            AnimationUtils.DrawZoneNPCStats(spriteBatch, SourceZone, placedCard, baseScale);
		}

        public bool IsComplete() => ElapsedTime > Duration;
    }
}
