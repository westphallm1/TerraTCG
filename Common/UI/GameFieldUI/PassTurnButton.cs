﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Localization;
using TerraTCG.Common.GameSystem;
using TerraTCG.Common.GameSystem.Drawing;
using TerraTCG.Common.GameSystem.GameState.GameActions;
using TerraTCG.Common.UI.Common;

namespace TerraTCG.Common.UI.GameFieldUI
{
    internal class PassTurnButton : RadialButton
    {

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            var isClicked = IsClicked();
            var localPlayer = Main.LocalPlayer.GetModPlayer<TCGPlayer>();
            var gamePlayer = localPlayer.GamePlayer;
            if (!(gamePlayer?.IsMyTurn ?? false))
            {
                return;
            }

            if (ContainsMouse)
            {
                Main.LocalPlayer.mouseInterface = true;
                if(isClicked)
                {
                    gamePlayer.PassTurn();
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            var localPlayer = Main.LocalPlayer.GetModPlayer<TCGPlayer>().GamePlayer;
            if (!(localPlayer?.IsMyTurn ?? false))
            {
                return;
            }
            var bgTexture = ContainsMouse ? 
                TextureCache.Instance.ButtonHighlighted.Value : 
                TextureCache.Instance.Button.Value;
            var origin = new Vector2(bgTexture.Width, bgTexture.Height) / 2;
            spriteBatch.Draw(bgTexture, Position, bgTexture.Bounds, Color.White, 0, origin, 1f, SpriteEffects.None, 0);

            var buttonText = Language.GetTextValue($"Mods.TerraTCG.GameActions.EndTurn");
            var buttonLines = buttonText.Split('\n');
            foreach( var line in buttonLines )
            {
                // TODO
            }

        }
    }
}