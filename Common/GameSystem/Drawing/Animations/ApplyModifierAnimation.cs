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
    internal class ApplyModifierAnimation(PlacedCard placedCard, Asset<Texture2D> modifierTexture) : IAnimation
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
            var lerpPoint = (float)(ElapsedTime.TotalSeconds < 1f ? 0f : 4f * (ElapsedTime.TotalSeconds - 1f));
            var itemOffset = MathHelper.Lerp(48f, 0f, lerpPoint) + posOffset;
            DrawLightRays(spriteBatch, basePosition - Vector2.UnitY * itemOffset, baseScale * (1 - lerpPoint));
            AnimationUtils.DrawZoneNPC(spriteBatch, SourceZone, placedCard, basePosition + Vector2.UnitY * posOffset, baseScale, zoneColor);

            // Draw the item itself on top of everything
            if(modifierTexture?.Value is var texture)
            {
                var bounds = texture.Bounds;
                var origin = new Vector2(bounds.Width, bounds.Height) / 2;
                spriteBatch.Draw(texture, basePosition - Vector2.UnitY * itemOffset, bounds,
                    Color.White, 0f,
                    origin, baseScale * (1 - lerpPoint), 0, 0);
            }
        }
		public void DrawZoneStats(SpriteBatch spriteBatch, Vector2 basePosition, float baseScale)
		{
            AnimationUtils.DrawZoneNPCStats(spriteBatch, SourceZone, placedCard, baseScale);
		}

        // via AmuletOfManyMinions
        private void DrawLightRays(SpriteBatch spriteBatch, Vector2 position, float baseScale)
        {
            var texture = TextureCache.Instance.LightRay.Value;
            var bounds = texture.Bounds;
            var origin = new Vector2(bounds.Width, bounds.Height) / 2;
            var baseAngle = MathF.Sin(MathF.PI * (float) (ElapsedTime.TotalSeconds / Duration.TotalSeconds));
            var rayCount = 7;
            var verticalSquish = 0.5f;
            for (int i = 0; i < rayCount; i++)
            {
                float localAngle = baseAngle + MathHelper.TwoPi * i / rayCount;
                float localIntensity = MathF.Sin(1.75f * localAngle);
                float scale = (0.5f + 0.25f * localIntensity) * baseScale;
                float brightness = 0.65f + 0.25f * localIntensity;
                Vector2 drawOffset = localAngle.ToRotationVector2() * scale * bounds.Height / 2 * verticalSquish;
                spriteBatch.Draw(texture, position + drawOffset, bounds,
                    Color.White * brightness, localAngle + MathF.PI / 2,
                    origin, new Vector2(scale, verticalSquish * scale), 0, 0);

            }
        }

        public bool IsComplete() => ElapsedTime > Duration;
    }
}
