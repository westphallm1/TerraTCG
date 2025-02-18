using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using TerraTCG.Common.GameSystem.Drawing;
using TerraTCG.Common.GameSystem.GameState.Modifiers;

namespace TerraTCG.Common.GameSystem.GameState
{
    internal class Field
    {
		internal CardGame Game { get; set; }
        internal List<Zone> Zones { get; set; }

        internal List<ICardModifier> CardModifiers { get; private set; } = [];

        public Field(CardGame game)
        {
            Zones = [
                new() { Role = ZoneRole.OFFENSE, Index = 0, Game = game},
                new() { Role = ZoneRole.OFFENSE, Index = 1, Game = game},
                new() { Role = ZoneRole.OFFENSE, Index = 2, Game = game},
                new() { Role = ZoneRole.DEFENSE, Index = 3, Game = game},
                new() { Role = ZoneRole.DEFENSE, Index = 4, Game = game},
                new() { Role = ZoneRole.DEFENSE, Index = 5, Game = game},
            ];
        }

        internal void ClearModifiers(GamePlayer turnPlayer, Zone zoneToClear, GameEvent gameEvent)
        {
            var eventInfo = new GameEventInfo { Zone = zoneToClear, IsMyTurn = zoneToClear.Owner.IsMyTurn, Event = gameEvent, TurnPlayer = turnPlayer };

            CardModifiers = CardModifiers.Where(m => !m.ShouldRemove(eventInfo)).ToList();
            if(zoneToClear.PlacedCard is PlacedCard card)
            {
                card.CardModifiers = card.CardModifiers.Where(m => !m.ShouldRemove(eventInfo)).ToList();
            }
        }

		internal void ModifyAttackSourceAndDestZones(ref Zone sourceZone, ref Zone destZone)
		{
			var sourceZoneModifiers = sourceZone.PlacedCard?.CardModifiers ?? [];

			foreach(var modifier in sourceZoneModifiers.Concat(CardModifiers))
			{
				modifier.ModifyAttackZones(ref sourceZone, ref destZone);
			}
		}

        internal void Draw(SpriteBatch spriteBatch, Vector2 position, float rotation)
        {
            // Draw the front row
            for(int i = 0; i < Zones.Count / 2; i++)
            {
                var offset = position + new Vector2((FieldRenderer.CARD_WIDTH + FieldRenderer.CARD_MARGIN) * i, 0).RotatedBy(rotation);
                Zones[i].Draw(spriteBatch, offset, rotation);
            }
            // Draw the back row
            for(int i = Zones.Count/2; i < Zones.Count; i++)
            {
                var offset = position + new Vector2(
                    (FieldRenderer.CARD_WIDTH + FieldRenderer.CARD_MARGIN) * (i - Zones.Count/2), 
                    FieldRenderer.CARD_HEIGHT + FieldRenderer.CARD_MARGIN).RotatedBy(rotation);
                Zones[i].Draw(spriteBatch, offset, rotation);
            }

            // Draw the deck zone
            var deckPosition = position + new Vector2(
                (FieldRenderer.CARD_WIDTH + FieldRenderer.CARD_MARGIN) * (Zones.Count/2), 
                FieldRenderer.CARD_HEIGHT + FieldRenderer.CARD_MARGIN).RotatedBy(rotation);
            var texture = TextureCache.Instance.Zone;
            var bounds = texture.Value.Bounds;
            var origin = new Vector2(bounds.Width, bounds.Height) / 2;
            spriteBatch.Draw(texture.Value, deckPosition + origin, bounds, Color.White, rotation, origin, 1f, SpriteEffects.None, 0);
            // Draw cards in deck (if any)
            // TODO get my player some other way
            var myPlayer = TCGPlayer.LocalGamePlayer;
            var player = rotation == 0 ? myPlayer : myPlayer.Opponent;
            int deckCount = player.Deck.Cards.Count;
            for(int i = 0; i < (deckCount + 1) / 2; i++)
            {
                deckPosition += rotation == 0 ? new Vector2(2, -2) : new Vector2(-2, -2);
                texture = (i % 2 == 0 || i == (deckCount + 1) / 2 - 1) ? player.Controller.Sleeve : TextureCache.Instance.ZoneHighlighted;
                spriteBatch.Draw(texture.Value, deckPosition + origin, bounds, Color.White, rotation, origin, 1f, SpriteEffects.None, 0);
            }
        }
    }
}
