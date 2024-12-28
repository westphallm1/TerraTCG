﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using TerraTCG.Common.GameSystem.GameState;

namespace TerraTCG.Common.GameSystem.Drawing
{
    delegate void DrawZoneNPC(SpriteBatch spriteBatch, Card card, Vector2 position, int frame, Color? color, float scale, SpriteEffects effects);
    internal class CardOverlayRenderer : ModSystem
    {
        public static CardOverlayRenderer Instance => ModContent.GetInstance<CardOverlayRenderer>();

        public void DefaultDrawZoneNPC(
            SpriteBatch spriteBatch, Card card, Vector2 position, int frame, Color? color, float scale, SpriteEffects effects)
        {
            var texture = TextureCache.Instance.GetNPCTexture(card.NPCID);
            var bounds = texture.Frame(1, Main.npcFrameCount[card.NPCID], 0, frame);
            var origin = new Vector2(bounds.Width / 2, bounds.Height);
            spriteBatch.Draw(texture.Value, position, bounds, color ?? Color.White, 0, origin, scale, effects, 0);
        }

        public DrawZoneNPC DrawSlimeNPC(float slimeScale, Color slimeColor)
        {
            return (SpriteBatch spriteBatch, Card card, Vector2 position, int frame, Color? color, float scale, SpriteEffects effects) =>
            {
                var texture = TextureCache.Instance.GetNPCTexture(card.NPCID);
                var bounds = texture.Frame(1, Main.npcFrameCount[card.NPCID], 0, frame);
                var origin = new Vector2(bounds.Width / 2, bounds.Height);
                spriteBatch.Draw(texture.Value, position, bounds, slimeColor, 0, origin, scale * slimeScale, effects, 0);
            };
        }

        public void DrawMimicNPC(
            SpriteBatch spriteBatch, Card card, Vector2 position, int frame, Color? color, float scale, SpriteEffects effects)
        {
            // only cycle through the golden mimic's frames, not all 3.
            frame = (frame % (Main.npcFrameCount[card.NPCID] / 3)) + Main.npcFrameCount[card.NPCID] / 3;
            DefaultDrawZoneNPC(spriteBatch, card, position, frame, color, scale, effects);
        }

        public void DrawKingSlimeNPC(
            SpriteBatch spriteBatch, Card card, Vector2 position, int frame, Color? color, float scale, SpriteEffects effects)
        {
            scale *= 0.5f;
            DefaultDrawZoneNPC(spriteBatch, card, position, frame, color, scale, effects);
            var npcTexture = TextureCache.Instance.GetNPCTexture(card.NPCID).Value;
            var npcBounds = npcTexture.Frame(1, Main.npcFrameCount[card.NPCID], 0, frame);
            var texture = TextureCache.Instance.KingSlimeCrown;
            var bounds = texture.Value.Bounds;
            var origin = new Vector2(bounds.Width / 2, bounds.Height);
            spriteBatch.Draw(texture.Value, position - Vector2.UnitY * npcBounds.Height * 0.75f * scale, bounds, color ?? Color.White, 0, origin, scale, effects, 0);
        }
    }
}