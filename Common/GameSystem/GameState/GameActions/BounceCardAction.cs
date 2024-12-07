﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using TerraTCG.Common.GameSystem.Drawing.Animations;

namespace TerraTCG.Common.GameSystem.GameState.GameActions
{
    internal class BounceCardAction(Card card, GamePlayer player) : IGameAction
    {
        private Zone zone;

        public bool CanAcceptZone(Zone zone) => player.Owns(zone) && !zone.IsEmpty();

        public bool AcceptZone(Zone zone)
        {
            this.zone = zone;
            return true;
        }

        public void Complete()
        {
            zone.Animation = new RemoveCardAnimation(zone, zone.PlacedCard, Main._drawInterfaceGameTime.TotalGameTime);
            player.Hand.Add(zone.PlacedCard.Template);
            zone.PlacedCard = null;
            player.Hand.Remove(card);
        }
    }
}