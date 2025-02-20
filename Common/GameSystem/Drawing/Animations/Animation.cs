using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using TerraTCG.Common.GameSystem.GameState;
using TerraTCG.Common.GameSystem.GameState.GameActions;

namespace TerraTCG.Common.GameSystem.Drawing.Animations
{
    internal interface IAnimation
    {
        TimeSpan StartTime { get; set; }

        TimeSpan Duration { get; }

        Zone SourceZone { set;  }

        // Draw the card within the zone itself, if applicable
        void DrawZone(SpriteBatch spriteBatch, Vector2 basePosition, float rotation);

        // Draw additional items (such as an NPC sprite) on top of the zone
        void DrawZoneOverlay(SpriteBatch spriteBatch, Vector2 basePosition, float baseScale);

        // Draw stats associated with the card active in the zone
        void DrawZoneStats(SpriteBatch spriteBatch, Vector2 basePosition, float baseScale);

        bool IsDefault() => false;
        bool IsComplete();

		internal TimeSpan RemainingTime => Duration - (TCGPlayer.TotalGameTime - StartTime);
    }

    internal class AnimationUtils 
    {
        public static void DrawZoneNPC(
            SpriteBatch spriteBatch, 
            Zone zone,
            PlacedCard card,
            Vector2 position, 
            float scale, 
            Color? color, 
            int frame = 0)
        {
            var npcId = card.Template.NPCID;
            if(npcId == 0)
            {
                return;
            }
            var gamePlayer = TCGPlayer.LocalGamePlayer;

            var effects = gamePlayer.Owns(zone) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            card.Template.DrawZoneNPC(spriteBatch, card, position, frame, color ?? Color.White, scale, effects);
        }

        public static void DrawZoneCard(
            SpriteBatch spriteBatch, PlacedCard card, Vector2 position, float rotation, Color? color)
        {
            var texture = card?.Template?.Texture;
            if(texture == null)
            {
                return;
            }

            var bounds = texture.Value.Bounds;

			FoilCardRenderer.DrawCard(spriteBatch, card.Template, position, color ?? Color.White, Zone.CARD_DRAW_SCALE, rotation, rotation == 0, false);
        }

        private static Color GetNPCHealthColor(int currentHealth, int maxHealth)
        {
            float healthFraction = (float)Math.Max(0, currentHealth) / maxHealth;

            Color start = healthFraction > 0.5f ? Color.White : Color.Yellow;
            Color end = healthFraction > 0.5f ? Color.Yellow : Color.Red;

            float lerpPoint = healthFraction > 0.5f ? (2 * (healthFraction - 0.5f)) : 2 * healthFraction;

            return start.Lerp(end, lerpPoint);
        }

        public static void DrawZoneNPCStats(
            SpriteBatch spriteBatch, 
            Zone zone, 
            PlacedCard card,
            float fontScale = 1f, 
            float transparency = 1f, 
            int? health = null)
        {
            // Health
            var npcId = card?.Template?.NPCID ??  0;
            if(npcId == 0)
            {
                return;
            }

            health ??= card.CurrentHealth;
            var attack = card.GetAttackWithModifiers(zone, null); // TODO don't explicitly pass null
			var attackColor = Color.White;
			int expectedDmg = 0;

			// If this is an allied zone and the player is mousing over an enemy zone,
			// draw the actual attack damage that would occur in that attack
			if(zone.Owner == TCGPlayer.LocalGamePlayer.Opponent && 
				TCGPlayer.LocalPlayer.ActiveMouseoverZone == zone && 
				TCGPlayer.LocalGamePlayer.InProgressAction is MoveCardOrAttackAction action &&
				action.CanAcceptZone(zone) && zone.HasPlacedCard())
			{
				expectedDmg = MoveCardOrAttackAction.GetAttackDamageWithZoneShifts(action.StartZone, zone);
			}

            var font = FontAssets.ItemStack.Value;

            var localPlayer = TCGPlayer.LocalPlayer;
            var gamePlayer = localPlayer.GamePlayer;
            // right-justify health above the NPC
            {
                var zoneOffset = gamePlayer.Owns(zone) ? new Vector2(0.75f, 0.7f) : new Vector2(0.75f, 0.3f);
                var placement = ProjectedFieldUtils.Instance.WorldSpaceToScreenSpace(gamePlayer, zone, zoneOffset);
                var center = localPlayer.GameFieldPosition + placement;
                var textOffset = font.MeasureString($"{health}");
                var textPos = center - textOffset;
                var color = GetNPCHealthColor((int)health, card.Template.MaxHealth);
                CardTextRenderer.Instance.DrawStringWithBorder(spriteBatch, $"{health}", textPos, color * transparency, fontScale);
                var heartPos = center - new Vector2(-4, textOffset.Y);
                var heartTexture = TextureCache.Instance.HeartIcon.Value;
                spriteBatch.Draw(heartTexture, heartPos, heartTexture.Bounds, Color.White * transparency, 0, default, 0.75f * fontScale, SpriteEffects.None, 0);

				// if in-progress attack action is being performed, draw the expected damage of the attack
				if(expectedDmg > 0)
				{
					var dmgPos = textPos - Vector2.UnitY * textOffset.Y * fontScale * 0.75f;
					CardTextRenderer.Instance.DrawStringWithBorder(spriteBatch, $"-{expectedDmg}", dmgPos, Color.Red * transparency, fontScale);
				}
            }

            // left-justify attack damage above npc
            if(card.Template.HasAttackText)
			{
                var zoneOffset = gamePlayer.Owns(zone) ? new Vector2(0.1f, 0.7f) : new Vector2(0.15f, 0.3f);
                var placement = ProjectedFieldUtils.Instance.WorldSpaceToScreenSpace(gamePlayer, zone, zoneOffset);
                var center = localPlayer.GameFieldPosition + placement;
                var textOffset = font.MeasureString($"{attack.Damage}");
                var textPos = center - textOffset;
                CardTextRenderer.Instance.DrawStringWithBorder(spriteBatch, $"{attack.Damage}", textPos, attackColor * transparency, fontScale);
                var swordPos = textPos + new Vector2(textOffset.X, 0);
                var swordTexture = TextureCache.Instance.AttackIcon.Value;
                spriteBatch.Draw(swordTexture, swordPos, swordTexture.Bounds, Color.White * transparency, 0, default, fontScale, SpriteEffects.None, 0);

                // Draw the attack mana cost below damage
                var manaPos = swordPos + new Vector2(-4, 12);
                CardTextRenderer.Instance.DrawManaCost(spriteBatch, attack.Cost, manaPos, fontScale);
            }

            // Draw buff icons at the bottom/top of the card
            {
                var zoneOffset = new Vector2(0f, 0.85f);
                var placement = ProjectedFieldUtils.Instance.WorldSpaceToScreenSpace(gamePlayer, zone, zoneOffset);
                var center = localPlayer.GameFieldPosition + placement;
                foreach(var (modifier, stack) in zone.GetKeywordModifiers().OrderBy(kv=>kv.Key))
                {
                    var count = Math.Max(1, stack);
                    var basePos = center + new Vector2(2, -2) * (count - 1);
                    var iconTexture = TextureCache.Instance.ModifierIconTextures[modifier].Value;
                    for(int i = count - 1; i >= 0; i--)
                    {
                        spriteBatch.Draw(iconTexture, basePos, iconTexture.Bounds, Color.White * transparency, 0, default, 1, SpriteEffects.None, 0);
                        basePos -= new Vector2(2, -2);
                    }
                    center.X += iconTexture.Width;
                }
            }

        }
    }

    internal static class ColorUtil
    {
        internal static Color Lerp(this Color start, Color end, float lerpPoint)
        {
            return new Color(
                (byte)MathHelper.Lerp(end.R, start.R, lerpPoint),
                (byte)MathHelper.Lerp(end.G, start.G, lerpPoint),
                (byte)MathHelper.Lerp(end.B, start.B, lerpPoint));
        }
    }
}
