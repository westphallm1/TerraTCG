﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using TerraTCG.Common.GameSystem.Drawing.Animations;
using TerraTCG.Common.GameSystem.GameState.Modifiers;
using static TerraTCG.Common.GameSystem.GameState.GameActions.IGameAction;

namespace TerraTCG.Common.GameSystem.GameState.GameActions
{
    internal class DeployCreatureAction(Card card, GamePlayer player) : IGameAction
    {
        private Zone zone;

        public ActionLogInfo GetLogMessage() => new(card,
            ActionText(card.SubTypes.Contains(CardSubtype.EXPERT) ? "Promoted" : "Played") + " " + card.CardName);

        public string GetZoneTooltip(Zone zone)
        {
            return ActionText(card.SubTypes.Contains(CardSubtype.EXPERT) ? "Promote" : "Play") + " " + card.CardName;
        }


        public bool CanAcceptZone(Zone zone)
        {
            if(card.SubTypes.Contains(CardSubtype.BOSS))
            {
                var noBossPresent = !zone.Siblings.Any(z => z.PlacedCard?.Template.SubTypes.Contains(CardSubtype.BOSS) ?? false);
                return noBossPresent && player.Owns(zone) && zone.IsEmpty();
            } else if(card.SubTypes.Contains(CardSubtype.EXPERT))
            {
                // Check whether the Expert creature type and placed creature type match, eg.
                // EXPERT FOREST FIGHTER -> FOREST (SLIME) FIGHTER
                var cardTypeMatches = zone.PlacedCard?.Template.SubTypes[0] == card.SubTypes[1] &&
                    zone.PlacedCard?.Template.SubTypes.Last() == card.SubTypes.Last(); 
                return player.Owns(zone) && !zone.IsEmpty() && !zone.PlacedCard.IsExerted && 
                    cardTypeMatches;
            } else
            {
                return player.Owns(zone) && zone.IsEmpty();
            }
        }

        public bool AcceptZone(Zone zone)
        {
            this.zone = zone;
            return true;
        }

        public void Complete()
        {
            if(card.SubTypes.Contains(CardSubtype.EXPERT))
            {
                zone.PromoteCard(card);
            } else
            {
                zone.PlaceCard(card);
                zone.QueueAnimation(new PlaceCardAnimation(zone.PlacedCard));
				zone.Owner.Field.ClearModifiers(zone.Owner, zone, GameEvent.CREATURE_ENTERED);
            }
            player.Hand.Remove(card);
            GameSounds.PlaySound(GameAction.PLACE_CARD);
        }

        public void Cancel()
        {
            // No-op
        }

    }
}
