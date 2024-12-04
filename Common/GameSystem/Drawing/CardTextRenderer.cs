﻿using Microsoft.Xna.Framework;
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

namespace TerraTCG.Common.GameSystem.Drawing
{

    internal class CardTextRenderer : ModSystem
    {
        internal class BodyHeightInfo
        {
            internal float height;

            internal float modifierHeight;

            internal float skillHeight;
            internal float skillDescriptionHeight;

            internal float attackHeight;
            internal float attackDescriptionHeight;
        }

        internal static CardTextRenderer Instance => ModContent.GetInstance<CardTextRenderer>();

        const float BaseTextScale = 0.75f;
        const float SmallTextScale = 0.65f;

        const float HPIconScale = 0.75f;
        const float MPIconScale = 0.85f;

        const int MARGIN_L = 8;
        const int MARGIN_S = 4;

        float BaseTextHeight = 0;
        float SmallTextHeight = 0;

        private void DrawString(SpriteBatch spriteBatch, string text, Vector2 position, Color? color = null, float scale = 1f, bool centered = false)
        {
            var font = FontAssets.ItemStack.Value;
            var origin = Vector2.Zero;
            if(centered)
            {
                var size = font.MeasureString(text);
                origin = size / 2;
            }
            spriteBatch.DrawString(font, text, position, color ?? Color.White, 0, origin, scale, SpriteEffects.None, 0);
        }

        private void DrawStringWithBorder(SpriteBatch spriteBatch, string text, Vector2 position, Color? color = null, float scale = 1f, bool centered = false)
        {
            foreach(var offset in new Vector2[] { Vector2.UnitX, Vector2.UnitY })
            {
                DrawString(spriteBatch, text, position + 2 * offset, Color.Black, scale, centered);
                DrawString(spriteBatch, text, position - 2 * offset, Color.Black, scale, centered);
            }
            DrawString(spriteBatch, text, position, color, scale, centered);
        }

        public void DrawManaCost(SpriteBatch spriteBatch, int cost, Vector2 position, float scale = 1f)
        {
            var texture = TextureCache.Instance.CostIcon.Value;
            spriteBatch.Draw(texture, position, texture.Bounds, Color.White, 0, default, scale * MPIconScale, SpriteEffects.None, 0);

            // Center text on the middle of the mana star
            var textOrigin = new Vector2(texture.Bounds.Width / 2, texture.Bounds.Height * 0.67f) * MPIconScale;
            DrawStringWithBorder(spriteBatch, $"{cost}", position + textOrigin * scale, scale: scale * SmallTextScale, centered: true);
        }

        public Vector2 GetManaCostSize()
        {
            var texture = TextureCache.Instance.CostIcon.Value;
            return new Vector2(texture.Width, texture.Height) * MPIconScale;
        }
        
        private void DrawCardTopLine(SpriteBatch spriteBatch, Card card, Vector2 position, float scale = 1f)
        {
            var font = FontAssets.ItemStack.Value;
            var bounds = card.Texture.Value.Bounds;

            // Top left: Name
            var nameOffset = new Vector2(MARGIN_L, MARGIN_S);
            DrawStringWithBorder(spriteBatch, card.CardName, position + nameOffset, scale: scale * BaseTextScale);

            // Top Right: Max HP
            var hpIcon = TextureCache.Instance.HeartIcon.Value;
            var hpIconWidth = hpIcon.Bounds.Width * BaseTextScale;

            var hpBounds = font.MeasureString($"{card.MaxHealth}") * HPIconScale;
            var hpIconOffset = new Vector2(bounds.Width - hpIconWidth - MARGIN_L, MARGIN_S);
            var hpOffset = new Vector2(hpIconOffset.X - hpBounds.X - MARGIN_S, MARGIN_S);

            spriteBatch.Draw(hpIcon, position + hpIconOffset * scale, hpIcon.Bounds, Color.White, 0, default, scale * HPIconScale, SpriteEffects.None, 0);
            DrawStringWithBorder(spriteBatch, $"{card.MaxHealth}", position + hpOffset * scale, scale: scale * BaseTextScale);

            // Beneath Portrait: Type line
            var typelineRowHeight = 88;
            var typelineOffset = new Vector2(MARGIN_L, typelineRowHeight);
            DrawStringWithBorder(spriteBatch, card.TypeLine, position + typelineOffset * scale, scale: scale * SmallTextScale);
        }

        private BodyHeightInfo GetBodyTextHeight(Card card)
        {
            // Calculate the total height of card text/abilities
            var heightInfo = new BodyHeightInfo();
            
            if(card.HasModifier)
            {
                heightInfo.modifierHeight = heightInfo.height;
                var lineCount = card.ModifierDescription.Split('\n').Length;
                heightInfo.height += lineCount * SmallTextHeight + MARGIN_S;
            }

            if(card.HasSkill)
            {
                heightInfo.skillHeight = heightInfo.height;
                heightInfo.height += BaseTextHeight;
            }
            if(card.HasSkillDescription)
            {
                heightInfo.skillDescriptionHeight = heightInfo.height;
                var lineCount = card.SkillDescription.Split('\n').Length;
                heightInfo.height += lineCount * SmallTextHeight;
            }
            if(card.HasSkill || card.HasSkillDescription)
            {
                heightInfo.height += MARGIN_S;
            }

            if(card.HasAttack)
            {
                heightInfo.attackHeight = heightInfo.height;
                heightInfo.height += BaseTextHeight;
            }
            if(card.HasAttackDescription)
            {
                heightInfo.attackDescriptionHeight = heightInfo.height;
                var lineCount = card.AttackDescription.Split('\n').Length;
                heightInfo.height += lineCount * SmallTextHeight;
            }

            return heightInfo;
        }
        private void DrawBodyText(SpriteBatch spriteBatch, Card card, Vector2 position, float scale = 1f)
        {
            var font = FontAssets.ItemStack.Value;
            var bounds = card.Texture.Value.Bounds;
            var heightInfo = GetBodyTextHeight(card);
            var centerOfBody = 130;

            var startY = centerOfBody - heightInfo.height / 2;

            var manaSize = GetManaCostSize();
            var baseTextX = 1.5f * MARGIN_L + manaSize.X;

            // Modifier
            if(card.HasModifier)
            {
                var rowY = startY + heightInfo.modifierHeight;
                var modifierLines = card.ModifierDescription.Split("\n");
                foreach (var line in modifierLines)
                {
                    var posOffset = new Vector2(1.5f * MARGIN_L, rowY);
                    DrawString(spriteBatch, line, position + posOffset * scale, Color.Black, SmallTextScale * scale);
                    if(line == modifierLines[0])
                    {
                        var keyword = line.Split(":")[0];
                        DrawStringWithBorder(
                            spriteBatch, keyword, position + posOffset * scale, Color.SkyBlue, SmallTextScale * scale);
                    }
                    rowY += SmallTextHeight;
                }
            }
            // Skill 
            if(card.HasSkill)
            {
                var skillRowHeight = startY + heightInfo.skillHeight;
                var manaOffset = new Vector2(1.5f * MARGIN_L, skillRowHeight - MARGIN_S / 2);
                var skillTextOffset = new Vector2(baseTextX, skillRowHeight);
                var keyword = card.SkillName.Split(":")[0];
                DrawManaCost(spriteBatch, card.Skills[0].Cost, position + manaOffset * scale, scale);
                DrawString(spriteBatch, card.SkillName, position + skillTextOffset * scale, Color.Black, scale * BaseTextScale);
                DrawStringWithBorder(
                    spriteBatch, keyword, position + skillTextOffset * scale, Color.Gold, scale * BaseTextScale);
            }

            // Skill Description
            if(card.HasSkillDescription)
            {
                var skillTextOffset = new Vector2(1.5f * MARGIN_L, startY + heightInfo.skillDescriptionHeight);
                DrawString(spriteBatch, card.SkillDescription, position + skillTextOffset * scale, Color.Black, scale * SmallTextScale);
            }

            // Attack
            if(card.HasAttack)
            {
                var attack = card.Attacks[0];
                var attackRowHeight = startY + heightInfo.attackHeight;
                var manaOffset = new Vector2(1.5f * MARGIN_L, attackRowHeight - MARGIN_S / 2);
                var attackTextOffset = new Vector2(baseTextX, attackRowHeight);
                DrawManaCost(spriteBatch, attack.Cost, position + manaOffset * scale, scale);
                DrawString(spriteBatch, card.AttackName, position + attackTextOffset * scale, Color.Black, scale * BaseTextScale);

                var dmgBounds = font.MeasureString($"{attack.Damage}") * BaseTextScale;
                var dmgOffset = new Vector2(bounds.Width - dmgBounds.X - 1.5f * MARGIN_L, attackRowHeight);
                DrawString(spriteBatch, $"{attack.Damage}", position + dmgOffset * scale, Color.Black, scale * BaseTextScale);
            }
            // Attack Description
            if(card.HasAttackDescription)
            {
                var attackTextOffset = new Vector2(1.5f * MARGIN_L, startY + heightInfo.attackDescriptionHeight);
                DrawString(spriteBatch, card.AttackDescription, position + attackTextOffset * scale, Color.Black, scale * SmallTextScale);
            }
        }

        public void DrawCardText(SpriteBatch spriteBatch, Card card, Vector2 position, float scale = 1f)
        {
            var font = FontAssets.ItemStack.Value;
            BaseTextHeight = font.MeasureString("Hello, world!").Y * BaseTextScale * 0.8f;
            SmallTextHeight = font.MeasureString("Hello, world!").Y * SmallTextScale * 0.8f;
            // Top Row: Name + HP
            DrawCardTopLine(spriteBatch, card, position, scale);

            DrawBodyText(spriteBatch, card, position, scale);
        }
    }
}