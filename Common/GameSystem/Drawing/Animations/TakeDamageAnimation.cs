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
    internal class TakeDamageAnimation(PlacedCard placedCard, int startHealth) : IAnimation
    {
        public TimeSpan StartTime { get; set; } 
        public Zone SourceZone { private get; set; }
        public TimeSpan Duration { get; } = TimeSpan.FromSeconds(0.5f);
        private TimeSpan ElapsedTime => TCGPlayer.TotalGameTime - StartTime;

        // TODO playing sounds from inside an animation is not good
        private bool hasPlayedSound = false;

        public void DrawZone(SpriteBatch spriteBatch, Vector2 basePosition, float rotation)
        {
            if(!hasPlayedSound)
            {
                hasPlayedSound = true;
                GameSounds.PlaySound(GameAction.TAKE_DAMAGE);
            }
            var zoneColor = IdleAnimation.ZoneColor(placedCard);
            AnimationUtils.DrawZoneCard(spriteBatch, placedCard, basePosition, rotation, zoneColor);
        }

        public void DrawZoneOverlay(SpriteBatch spriteBatch, Vector2 basePosition, float baseScale)
        {
            // flash the npc transparent as when the player takes damage
            var zoneColor = IdleAnimation.OverlayColor(placedCard);
            var impactSign = TCGPlayer.LocalGamePlayer.Owns(SourceZone) ? -1 : 1;
            var posOffset =  impactSign * baseScale * 5f * MathF.Sin(MathF.Tau * (float) (ElapsedTime.TotalSeconds / 0.5f));

            var flashSign = MathF.Sin(8f * MathF.Tau * (float)ElapsedTime.TotalSeconds);
            var transparency = flashSign > 0 ? 0.8f : 0.4f;
            AnimationUtils.DrawZoneNPC(spriteBatch, SourceZone, placedCard, basePosition + new Vector2(0, posOffset), baseScale, zoneColor * transparency);
        }

		public void DrawZoneStats(SpriteBatch spriteBatch, Vector2 basePosition, float baseScale)
		{
            var health = MathHelper.Lerp(startHealth, placedCard.CurrentHealth, 2 * (float)ElapsedTime.TotalSeconds);
            AnimationUtils.DrawZoneNPCStats(spriteBatch, SourceZone, placedCard, baseScale, health: (int)health);
		}

        public bool IsComplete() => ElapsedTime > Duration;
    }
}
