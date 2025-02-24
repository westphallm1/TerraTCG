﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using TerraTCG.Common.GameSystem.Drawing.Animations;
using TerraTCG.Common.GameSystem.Drawing.Animations.FieldAnimations;
using static TerraTCG.Common.GameSystem.GameState.GameActions.IGameAction;

namespace TerraTCG.Common.GameSystem.GameState.GameActions
{
    internal class DrawCardAction(Card card, GamePlayer player, int drawCount=1) : TownsfolkAction(card, player), IGameAction
    {
        public override ActionLogInfo GetLogMessage() => new(card, $"{ActionText("Drew")} {drawCount} {ActionText("Cards")} {ActionText("With")} {Card.CardName}");

        public override bool CanAcceptZone(Zone zone) => false;

        public override bool AcceptZone(Zone zone) => false;

        public override Zone TargetZone() => null;

        public bool CanAcceptActionButton() => Player.Resources.TownsfolkMana > 0;

        public bool AcceptActionButton() => true;

        public override void Complete()
        {
            base.Complete();

            for(int _ = 0; _ < drawCount; _++)
            {
                Player.Hand.Add(Player.Deck.Draw());
            }
            GameSounds.PlaySound(GameAction.USE_SKILL);
        }
    }
}
