﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.UI;
using TerraTCG.Common.GameSystem;
using TerraTCG.Common.GameSystem.Drawing;
using TerraTCG.Common.GameSystem.Drawing.Animations;
using TerraTCG.Common.UI.Common;

namespace TerraTCG.Common.UI.GameFieldUI
{
    internal class GameFieldElement : CustomClickUIElement
    {
        internal Vector2 FieldOrigin => new (
            Position.X + FieldRenderer.FIELD_WIDTH / 2,
            Position.Y + FieldRenderer.FIELD_HEIGHT);

        internal override bool IsClicked() => !((GameFieldState)Parent).actionButtons.ContainsMouse && base.IsClicked();

        public override void Update(GameTime gameTime)
        {
            var localPlayer = Main.LocalPlayer.GetModPlayer<TCGPlayer>();
            localPlayer.GameFieldPosition = Position;

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
                if(zone.Animation?.IsComplete() ?? false)
                {
                    zone.Animation = zone.HasPlacedCard() ?
                         new IdleAnimation(zone, gameTime.TotalGameTime) : null;
                }
            }
            var mouseField = Main.MouseScreen - Position;

            // Check both players' fields
            foreach (var zone in gamePlayer.Game.AllZones())
            {
                if (ProjectedFieldUtils.Instance.ZoneContainsScreenVector(gamePlayer, zone, mouseField))
                {
                    Main.LocalPlayer.mouseInterface = true;
                    gamePlayer.MouseoverCard = zone?.PlacedCard?.Template ?? gamePlayer.MouseoverCard;
                    if(IsClicked())
                    {
                        gamePlayer.SelectZone(zone);
                        break;
                    }
                }
            }
        }

        private void DrawZoneNPCs(SpriteBatch spriteBatch)
        {
            var gamePlayer = Main.LocalPlayer.GetModPlayer<TCGPlayer>().GamePlayer;
            // Iterate backwards to layer closer zones on top of farther zones
            foreach (var zone in gamePlayer.Game.AllZones().Reverse())
            {
                var yLerpPoint = gamePlayer.Owns(zone) ? 0.3f : 0.8f;
                var placement = ProjectedFieldUtils.Instance.WorldSpaceToScreenSpace(gamePlayer, zone, new(0.5f, yLerpPoint));
                var scale = ProjectedFieldUtils.Instance.GetXScaleForZone(gamePlayer, zone, yLerpPoint);
                zone.DrawNPC(spriteBatch, Position + placement, scale);
            }
        }

        private void DrawPlayerStats(SpriteBatch spriteBatch)
        {
            var texture = TextureCache.Instance.PlayerStatsZone;
            // My player
            var gamePlayer = Main.LocalPlayer.GetModPlayer<TCGPlayer>().GamePlayer;
            var anchorZonePos = 
                ProjectedFieldUtils.Instance.WorldSpaceToScreenSpace(gamePlayer, gamePlayer.Field.Zones[3], new(0, 1));

            var pos = Position + new Vector2(
                anchorZonePos.X - texture.Width(),
                anchorZonePos.Y - texture.Height());
            PlayerStatRenderer.Instance.DrawPlayerStats(spriteBatch, pos, gamePlayer, 1f);

            // Opposing player
            var opponent = gamePlayer.Opponent;
            var scale = ProjectedFieldUtils.Instance.GetXScaleForZone(gamePlayer, opponent.Field.Zones[3], 0f);
            anchorZonePos = 
                ProjectedFieldUtils.Instance.WorldSpaceToScreenSpace(gamePlayer, opponent.Field.Zones[3], new(1, 0));
            var oppPos = Position + new Vector2(
                anchorZonePos.X + FieldRenderer.CARD_MARGIN,
                anchorZonePos.Y);
            PlayerStatRenderer.Instance.DrawPlayerStats(spriteBatch, oppPos, opponent, scale);
        }

        private void DrawFieldOverlays(SpriteBatch spriteBatch)
        {
            var gamePlayer = Main.LocalPlayer.GetModPlayer<TCGPlayer>().GamePlayer;
            gamePlayer.Game.FieldAnimation?.DrawFieldOverlay(spriteBatch, Position);

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            var texture = FieldRenderer.Instance.PerspectiveRenderTarget;
            if(texture != null)
            {
                spriteBatch.Draw(texture, Position, Color.White);
                DrawZoneNPCs(spriteBatch);
                DrawPlayerStats(spriteBatch);
                DrawFieldOverlays(spriteBatch);
            }
        }
    }
}
