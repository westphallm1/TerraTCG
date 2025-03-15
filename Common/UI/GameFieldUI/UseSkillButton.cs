using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using TerraTCG.Common.Configs;
using TerraTCG.Common.GameSystem;
using TerraTCG.Common.GameSystem.Drawing;
using TerraTCG.Common.GameSystem.GameState.GameActions;
using TerraTCG.Common.UI.Common;
using TerraTCG.Common.UI.DeckbuildUI;

namespace TerraTCG.Common.UI.GameFieldUI
{
    internal class UseSkillButton : RadialButton
    {
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            var isClicked = IsClicked();
            var localPlayer = TCGPlayer.LocalPlayer;
            var gamePlayer = localPlayer.GamePlayer;
            if (!(gamePlayer?.InProgressAction?.CanAcceptActionButton() ?? false))
            {
                Left.Set(Main.screenWidth, 0f);
                Top.Set(Main.screenHeight, 0f);
                return;
            }
            PlayTickIfMouseEntered(Position, CLICK_RADIUS);
            // Move to the button to the middle of the two game fields, above the player's deck zone
            if(gamePlayer.SelectedFieldZone != null)
            {
                var zone = gamePlayer.SelectedFieldZone;
                var placement = ProjectedFieldUtils.Instance.WorldSpaceToScreenSpace(gamePlayer, zone, new(0.5f, -0.2f));
                var center = localPlayer.GameFieldPosition + placement;

                Left.Set(center.X, 0f);
                Top.Set(center.Y, 0f);
            } else if (gamePlayer.SelectedHandCard != null)
            {
                // TODO place the button above the card in hand
                var handElem = ((GameFieldState)Parent).handElement;
                var cardPos = handElem.GetCardPosition(gamePlayer.SelectedHandIdx);
                var center = cardPos + new Vector2(HandElement.CARD_WIDTH / 2, - 12f);

                Left.Set(center.X, 0f);
                Top.Set(center.Y, 0f);
            }

            if (ContainsMouse)
            {
				gamePlayer.PreviewResources = gamePlayer.Resources - gamePlayer.InProgressAction.GetActionButtonResources();

				Main.LocalPlayer.mouseInterface = true;
                if(isClicked)
                {
                    gamePlayer.SelectActionButton(ActionType.SKILL);
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            var gamePlayer = TCGPlayer.LocalGamePlayer;
            if (!(gamePlayer?.InProgressAction?.CanAcceptActionButton() ?? false))
            {
                return;
            }
            var bgTexture = TextureCache.Instance.Button.Value;
            var highlightTexture = TextureCache.Instance.ButtonHighlighted.Value;
            var fgTexture = TextureCache.Instance.StarIcon.Value;

            float brightness = 0.5f + 0.5f * MathF.Sin(MathF.Tau * (float)TCGPlayer.TotalGameTime.TotalSeconds / 2f);
            var origin = new Vector2(bgTexture.Width, bgTexture.Height) / 2;
            var starOrigin = new Vector2(fgTexture.Width, fgTexture.Height) / 2;
            spriteBatch.Draw(bgTexture, Position, bgTexture.Bounds, Color.White, 0, origin, 1f, SpriteEffects.None, 0);
            spriteBatch.Draw(highlightTexture, Position, bgTexture.Bounds, Color.White * brightness, 0, origin, 1f, SpriteEffects.None, 0);
            spriteBatch.Draw(fgTexture, Position, fgTexture.Bounds, Color.White, 0, starOrigin, 1f, SpriteEffects.None, 0);

            if(ContainsMouse && ModContent.GetInstance<ClientConfig>().ShowTooltips)
            {
                DeckbuildState.SetTooltip(gamePlayer.InProgressAction.GetActionButtonTooltip());
            }
        }
    }
}
