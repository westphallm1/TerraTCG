﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.UI;
using TerraTCG.Common.GameSystem.Drawing;

namespace TerraTCG.Common.UI.GameFieldUI
{
    internal class GameFieldState : UIState
    {
        private GameFieldElement gameField;

        public override void OnInitialize()
        {
            base.OnInitialize();
            gameField = new();
            SetRectangle(gameField, Main.screenWidth / 2, (Main.screenHeight - FieldRenderer.FIELD_HEIGHT) / 2, 50, 50);
            Append(gameField);
        }
        private void SetRectangle(UIElement uiElement, float left, float top, float width, float height)
        {
            uiElement.Left.Set(left, 0f);
            uiElement.Top.Set(top, 0f);
            uiElement.Width.Set(width, 0f);
            uiElement.Height.Set(height, 0f);
        }
    }
}
