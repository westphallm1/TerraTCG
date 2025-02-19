using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using TerraTCG.Common.GameSystem.BotPlayer;
using TerraTCG.Common.GameSystem.CardData;
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

		// Force Skeletron and WoF into a central zone
		private readonly Card[] CenterOnlyCards = [BotDecks.GetCard<Skeletron>(), BotDecks.GetCard<WallOfFlesh>(), BotDecks.GetCard<SkeletronPrime>()];

		// Prevent playing any cards adjacent to the WoF
		private readonly Card[] WholeRowCards = [BotDecks.GetCard<WallOfFlesh>()];

        public string GetZoneTooltip(Zone zone)
        {
            return ActionText(card.SubTypes.Contains(CardSubtype.EXPERT) ? "Promote" : "Play") + " " + card.CardName;
        }
        public string GetCantAcceptZoneTooltip(Zone zone)
		{
			if(card.SubTypes.Contains(CardSubtype.BOSS) && zone.Siblings.Any(z => z.PlacedCard?.Template.SubTypes.Contains(CardSubtype.BOSS) ?? false))
			{
				return ActionText("OnlyOneBoss");
			}
			return null;
		}


        public bool CanAcceptZone(Zone zone)
        {
			if(zone.Siblings.Any(z=> z.Index / 3 == zone.Index / 3 && WholeRowCards.Contains(z.PlacedCard?.Template)))
			{
				return false;
			}

            if(card.SubTypes.Contains(CardSubtype.BOSS))
            {
                var noBossPresent = !zone.Siblings.Any(z => z.PlacedCard?.Template.SubTypes.Contains(CardSubtype.BOSS) ?? false);
				return noBossPresent && player.Owns(zone) && zone.IsEmpty() &&
					(zone.Column == 1 || !CenterOnlyCards.Contains(card));
            } else if(card.SubTypes.Contains(CardSubtype.EXPERT))
            {
                // Check whether the Expert creature type and placed creature type match, eg.
                // EXPERT FOREST FIGHTER -> FOREST (SLIME) FIGHTER
                return player.Owns(zone) && !zone.IsEmpty() && !zone.PlacedCard.IsExerted && card.CanPromote(zone, card); 
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
