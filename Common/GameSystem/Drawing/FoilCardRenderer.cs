﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using TerraTCG.Common.Configs;
using TerraTCG.Common.GameSystem.GameState;

namespace TerraTCG.Common.GameSystem.Drawing
{
	internal class FoilCardRenderer
	{
		public static void DrawCard(
			SpriteBatch spriteBatch, Card card, Vector2 pos, Color color, float scale, float rotation, bool playerOwned = true, bool details = true, bool posShift = true)
		{
			// If the player owns a foil copy of the card, draw it foil
			if(playerOwned && TCGPlayer.LocalPlayer.FoilCollection.Cards.Contains(card) && ModContent.GetInstance<ClientConfig>().ShowFoils)
			{
				DrawCardWithFoiling(spriteBatch, card, pos, color, scale, details, posShift);
			} else
			{
				var texture = card.Texture;
				var bounds = texture.Value.Bounds;
				Vector2 origin = default;
				if(rotation != 0)
				{
					origin = new Vector2(bounds.Width, bounds.Height) / 2;
					pos += origin * scale;
				}

				spriteBatch.Draw(
					texture.Value, pos, bounds, color, rotation, origin, scale, SpriteEffects.None, 0);

				if(rotation == 0)
				{
					// If the card is rotated towards the player, draw its text
					CardTextRenderer.Instance.DrawCardText(spriteBatch, card, pos, scale, textColor: color, details: details);
				} 
			}

		}
		public static void DrawCardWithFoiling(
			SpriteBatch spriteBatch, Card card, Vector2 pos, Color color, float scale, bool details = true, bool posShift = true)
		{
			var textureCache = TextureCache.Instance;
			// Draw the back of the card
			spriteBatch.Draw(card.Texture.Value, pos, card.Texture.Value.Bounds, color, 0, default, scale, SpriteEffects.None, 0);

			// Draw a gold border over the regular border
			var highlightTexture = TextureCache.Instance.ZoneHighlighted;
			spriteBatch.Draw(highlightTexture.Value, pos, highlightTexture.Value.Bounds, color, 0, default, scale * 1.5f, SpriteEffects.None, 0f);

			var rotation = (float)TCGPlayer.TotalGameTime.TotalSeconds/2f + (posShift ? pos.Y + pos.X : 0);

			// Draw foiling over the card
			var foilOrigin = 128f * new Vector2(MathF.Cos(rotation), 0.5f * MathF.Sin(rotation));
			var foilPos = new Vector2(textureCache.Foiling.Width(), textureCache.Foiling.Height()) / 2 + foilOrigin;
			var foilBounds = card.Texture.Value.Bounds;
			foilBounds.Location += new Point((int)foilPos.X, (int)foilPos.Y);
			spriteBatch.Draw(textureCache.Foiling.Value, pos, foilBounds, color * 0.5f, 0, default, scale, SpriteEffects.None, 0);

			// Draw the non-foiled parts of the card
			if (textureCache.CardFoilMasks.TryGetValue(card.FullName, out var mask) || 
				textureCache.FoilMasks.TryGetValue(card.SortType, out mask))
			{
				spriteBatch.Draw(mask.Value, pos, card.Texture.Value.Bounds, color, 0, default, scale, SpriteEffects.None, 0);
			}

			CardTextRenderer.Instance.DrawCardText(spriteBatch, card, pos, scale, textColor: color, details: details);
		}
	}
}
