using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;
using TerraTCG.Common.Configs;
using TerraTCG.Common.GameSystem;
using TerraTCG.Common.GameSystem.Drawing;
using TerraTCG.Common.GameSystem.Drawing.Animations;
using TerraTCG.Common.GameSystem.GameState;
using TerraTCG.Common.UI.Common;
using TerraTCG.Common.UI.DeckbuildUI;
using static TerraTCG.Common.GameSystem.GameState.GameActions.IGameAction;

namespace TerraTCG.Common.UI.GameFieldUI
{
    internal class GameFieldElement : CustomClickUIElement
    {
        internal Vector2 FieldOrigin => new (
            Position.X + FieldRenderer.FIELD_WIDTH / 2,
            Position.Y + FieldRenderer.FIELD_HEIGHT);

        internal override bool IsClicked() => !((GameFieldState)Parent).actionButtons.ContainsMouse && base.IsClicked();

        private string fieldTooltip;
        private int fieldRare;

        public override bool ContainsPoint(Vector2 point)
        {
            return true; // Element occupies the whole screen
        }

        public override void Update(GameTime gameTime)
        {
            Main.LocalPlayer.mouseInterface = true;
            var localPlayer = TCGPlayer.LocalPlayer;
            localPlayer.GameFieldPosition = Position;

            fieldTooltip = "";
            fieldRare = 0;
            var gamePlayer = localPlayer.GamePlayer;
            if (gamePlayer == null || gamePlayer.Field?.Zones == null)
            {
                return;
            }
            // TODO there is probably a better place for setting animation state
            if(gamePlayer.Game.FieldAnimation?.IsComplete() ?? true)
            {
                gamePlayer.Game.FieldAnimation = null;
            }             
            foreach (var zone in gamePlayer.Game.AllZones())
            {
                zone.UpdateAnimationQueue();
            }
            var mouseField = Main.MouseScreen - Position;
            var prevMouseField = new Vector2(Main.lastMouseX, Main.lastMouseY) - Position;
            // Check if mouse-over-ing stats and set tooltip
            var (myBounds, oppBounds, _, _) = GetPlayerStatsBoundsAndScale();
            if(myBounds.Contains(Main.mouseX, Main.mouseY))
            {
                fieldTooltip = GetTooltipForResources(gamePlayer);
            } else if(oppBounds.Contains(Main.mouseX, Main.mouseY))
            {
                fieldTooltip = GetTooltipForResources(gamePlayer.Opponent);
            }

            bool wasClicked = IsClicked();
            bool clickedValidZone = false;

			localPlayer.ActiveMouseoverZone = null;
            // Check both players' fields
            foreach (var zone in gamePlayer.Game.AllZones())
            {
                if (ProjectedFieldUtils.Instance.ZoneContainsScreenVector(gamePlayer, zone, mouseField))
                {
                    var inProgressAction = gamePlayer?.InProgressAction;
                    if((inProgressAction?.CanAcceptZone(zone) ?? false) && gamePlayer.IsMyTurn)
                    {
                        fieldTooltip = gamePlayer.InProgressAction.GetZoneTooltip(zone);
						gamePlayer.PreviewResources = gamePlayer.Resources - inProgressAction.GetZoneResources(zone);
                    } else if (inProgressAction?.GetCantAcceptZoneTooltip(zone) is string tooltip && gamePlayer.IsMyTurn)
                    {
                        fieldTooltip = tooltip;
                        fieldRare = ItemRarityID.Red;
                    }

                    if(zone.HasPlacedCard())
                    {
                        localPlayer.ActiveMouseoverZone = zone;
                        localPlayer.MouseoverZone = zone;
                        localPlayer.MouseoverCard = zone.PlacedCard.Template;
						if(Main.keyState.PressingShift())
						{
							fieldTooltip = CardPreviewElement.GetCardDetailsToolTip(zone);
						}
                    }
                    if(wasClicked)
                    {
                        SoundEngine.PlaySound(SoundID.MenuTick);
                        gamePlayer.SelectZone(zone);
                        clickedValidZone = true;
                    }
                    break;
                }
            }

            // Unset the selected field zone if the player clicks away from
            // the field
            if(wasClicked && !clickedValidZone)
            {
                gamePlayer.SelectedFieldZone = null;
                gamePlayer.SelectedHandCard = null;
                gamePlayer.SelectedHandIdx = -1;
                gamePlayer.InProgressAction = null;
            }
        }

        private string GetTooltipForResources(GamePlayer player)
        {
            var resources = player.Resources;
            return
                $"{Language.GetText("Mods.TerraTCG.Cards.Common.Hearts")}: {resources.Health}/{GamePlayer.MAX_HEALTH}\n" +
                $"{Language.GetText("Mods.TerraTCG.Cards.Common.Mana")}: {resources.Mana}/{player.ManaPerTurn}\n" +
                $"{Language.GetText("Mods.TerraTCG.Cards.Common.Townsfolk")}: {resources.TownsfolkMana}/1\n";
        }

        private void DrawZoneNPCs(SpriteBatch spriteBatch)
        {
            var gamePlayer = TCGPlayer.LocalGamePlayer;
            // Iterate backwards to layer closer zones on top of farther zones
            foreach (var zone in gamePlayer.Game.AllZones().Reverse())
            {
                var yLerpPoint = gamePlayer.Owns(zone) ? 0.3f : 0.8f;
                var placement = ProjectedFieldUtils.Instance.WorldSpaceToScreenSpace(gamePlayer, zone, new(0.5f, yLerpPoint));
                var scale = ProjectedFieldUtils.Instance.GetXScaleForZone(gamePlayer, zone, yLerpPoint);
                zone.DrawNPC(spriteBatch, Position + placement, scale);
            }

			// Draw stats on top of all NPCs since they can sometimes clip into the next zone
            foreach (var zone in gamePlayer.Game.AllZones().Reverse())
            {
                var yLerpPoint = gamePlayer.Owns(zone) ? 0.3f : 0.8f;
                var placement = ProjectedFieldUtils.Instance.WorldSpaceToScreenSpace(gamePlayer, zone, new(0.5f, yLerpPoint));
                var scale = ProjectedFieldUtils.Instance.GetXScaleForZone(gamePlayer, zone, yLerpPoint);
                zone.DrawNPCStats(spriteBatch, Position + placement, scale);
            }
        }

        // TODO struct for this
        private (Rectangle, Rectangle, float, float) GetPlayerStatsBoundsAndScale()
        {
            var texture = TextureCache.Instance.PlayerStatsZone;
            // My player
            var gamePlayer = TCGPlayer.LocalGamePlayer;
            var anchorZonePos = 
                ProjectedFieldUtils.Instance.WorldSpaceToScreenSpace(gamePlayer, gamePlayer.Field.Zones[3], new(0, 1));

            var pos = Position + new Vector2(
                anchorZonePos.X - texture.Width(),
                anchorZonePos.Y - texture.Height());

            var friendlyBounds = new Rectangle(
                (int)pos.X, (int)pos.Y,
                texture.Width(), texture.Height());

            // Opposing player
            var opponent = gamePlayer.Opponent;
            var scale = ProjectedFieldUtils.Instance.GetXScaleForZone(gamePlayer, opponent.Field.Zones[3], 0f);
            anchorZonePos = 
                ProjectedFieldUtils.Instance.WorldSpaceToScreenSpace(gamePlayer, opponent.Field.Zones[3], new(1, 0));
            var oppPos = Position + new Vector2(
                anchorZonePos.X + FieldRenderer.CARD_MARGIN,
                anchorZonePos.Y);
            var oppBounds = new Rectangle(
                (int)oppPos.X, (int)oppPos.Y,
                (int)(scale * texture.Width()), (int)(scale * texture.Height()));

            return (friendlyBounds, oppBounds, 1f, scale);

        }


        private void DrawPlayerStats(SpriteBatch spriteBatch)
        {
            var (myBounds, oppBounds, myScale, oppScale) = GetPlayerStatsBoundsAndScale();
            // My player
            var gamePlayer = TCGPlayer.LocalGamePlayer;
            PlayerStatRenderer.Instance.DrawPlayerStats(spriteBatch, new(myBounds.X, myBounds.Y), gamePlayer, myScale);

            // Opposing player
            var opponent = gamePlayer.Opponent;
            PlayerStatRenderer.Instance.DrawPlayerStats(spriteBatch, new(oppBounds.X, oppBounds.Y), opponent, oppScale);
        }

		private void DrawPlayerDeckCounts(SpriteBatch spriteBatch)
		{
			if(!ModContent.GetInstance<ClientConfig>().ShowActionLog)
			{
				return;
			}
			var localPlayer = TCGPlayer.LocalPlayer;
			var gamePlayer = localPlayer.GamePlayer;
			var opponent = gamePlayer.Opponent;

			var myListPos = ProjectedFieldUtils.Instance.WorldSpaceToScreenSpace(gamePlayer, gamePlayer.Field.Zones[5], new(1.65f, 1f));

			var deckCount = $"{gamePlayer.Deck.Cards.Count}/20";
			var font = FontAssets.ItemStack.Value;
			var yOffset = font.MeasureString(deckCount).Y/2;

			var drawPos = localPlayer.GameFieldPosition + myListPos + Vector2.UnitY * yOffset;
			CardTextRenderer.Instance.DrawStringWithBorder(spriteBatch, deckCount, drawPos, centered: true, font: font);

			var oppListPos = ProjectedFieldUtils.Instance.WorldSpaceToScreenSpace(gamePlayer, opponent.Field.Zones[2], new(-0.65f, 0f));
			deckCount = $"{opponent.Deck.Cards.Count}/20";
			drawPos = localPlayer.GameFieldPosition + oppListPos + Vector2.UnitY * yOffset;
			CardTextRenderer.Instance.DrawStringWithBorder(spriteBatch, deckCount, drawPos, centered: true, font: font);

		}

        private void DrawFieldOverlays(SpriteBatch spriteBatch)
        {
            TCGPlayer.LocalGamePlayer.Game.FieldAnimation?.DrawFieldOverlay(spriteBatch, Position);
        }

        internal static void DrawMapBg(SpriteBatch spriteBatch)
        {
            var texture = FieldRenderer.Instance.MapBGRenderTarget;
            var mapScaleX = Main.screenWidth / (float)texture.Width;
            var mapScaleY = Main.screenHeight / (float)texture.Height;
            var scale = Math.Max(mapScaleX, mapScaleY);
            var origin = new Vector2(texture.Width, texture.Height) / 2;
            var drawPos = new Vector2(Main.screenWidth, Main.screenHeight) / 2;

            spriteBatch.Draw(texture, drawPos, texture.Bounds, Color.White * TCGPlayer.FieldTransitionPoint, 0, origin, scale, SpriteEffects.None, 0);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            var texture = FieldRenderer.Instance.PerspectiveRenderTarget;
            if(texture != null && TCGPlayer.LocalGamePlayer != null)
            {
                // draw the perspective-rendered game field
                spriteBatch.Draw(texture, Position, Color.White);
                DrawZoneNPCs(spriteBatch);
                DrawPlayerStats(spriteBatch);
				DrawPlayerDeckCounts(spriteBatch);
                DrawFieldOverlays(spriteBatch);

                if(fieldTooltip != "" && ModContent.GetInstance<ClientConfig>().ShowTooltips)
                {
                    DeckbuildState.SetTooltip(fieldTooltip, fieldRare);
                }
            }
        }
    }
}
