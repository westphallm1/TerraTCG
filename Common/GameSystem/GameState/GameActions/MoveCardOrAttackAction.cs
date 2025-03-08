using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
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
    // "Do everything" action that encompasses any game action initiated by clicking an on-field card.
    // - Declare an attack by clicking an ally zone, then any enemy zone
    // - Move a creature by clicking two ally zones
    // - Use a skill by clicking an ally zone, then a "use skill" button
    internal class MoveCardOrAttackAction : IGameAction
    {
        private Zone endZone;
		private Zone startZone;
		private GamePlayer player;
        private ActionType actionType = ActionType.DEFAULT;

		public MoveCardOrAttackAction()  { }

		public MoveCardOrAttackAction(Zone startZone, GamePlayer player) 
		{
			this.startZone = startZone;
			this.player = player;
		}




        private string _logMessage;

        // Action might or might not result in an empty startZone, be sure to track the
        // card that the action occurred on.
        private Card actionCard;
        public ActionLogInfo GetLogMessage() => new(actionCard, _logMessage);

        public bool CanAcceptCardInHand(Card card) => false;

		public Zone StartZone => startZone;

        // State var for calculations to determine whether the inability to
        // perform an action is due to lack of available resources.
        private string InsufficientResourcesFor = "";

        public bool CanAcceptZone(Zone zone) 
        {
			InsufficientResourcesFor = "";

            if(startZone?.PlacedCard?.IsExerted ?? true)
            {
                return false;
            }

            if(actionType == ActionType.DEFAULT && player.Owns(zone) && zone.IsEmpty())
            {
				// This was previously used to check for movement, which has been removed as a mechanic
                return false;

            } else if (actionType == ActionType.DEFAULT && !player.Owns(zone) && !zone.IsEmpty() && startZone.Role == ZoneRole.OFFENSE)
            {
                return CanAttackZone(zone);
            } else if (actionType == ActionType.TARGET_ALLY && player.Owns(zone) && !zone.IsEmpty())
            {
                return true;
            } else
            {
                return false;
            }
        }


        public string GetCantAcceptZoneTooltip(Zone zone)
		{
			if (InsufficientResourcesFor == "") return null;

			var notEnoughResourceTo = player.Resources.GetDeficencyTooltip(GetZoneResources(zone));
			return string.IsNullOrEmpty(notEnoughResourceTo) ? "" : $"{notEnoughResourceTo} {InsufficientResourcesFor}";
		}

		private static Attack GetAttackWithZoneShifts(Zone srcZone, Zone dstZone)
		{
			var currentCardOrder = srcZone.Siblings.Select(z => z.PlacedCard).ToList();
			srcZone.Owner.Field.ModifyAttackSourceAndDestZones(ref srcZone, ref dstZone, preCalculating: true);
			var attack = srcZone.PlacedCard.GetAttackWithModifiers(srcZone, dstZone);
			for (int i = 0; i < currentCardOrder.Count; i++)
			{
				srcZone.Siblings[i].PlacedCard = currentCardOrder[i];
			}
			return attack;
		}
		internal static int GetAttackDamageWithZoneShifts(Zone srcZone, Zone dstZone) =>
			GetAttackWithZoneShifts(srcZone, dstZone).Damage;

		internal static int GetAttackCostWithZoneShifts(Zone srcZone, Zone dstZone) =>
			GetAttackWithZoneShifts(srcZone, dstZone).Cost;

        public string GetZoneTooltip(Zone zone)
        {

            if(actionType == ActionType.DEFAULT && player.Owns(zone) && zone.IsEmpty())
            {
                return $"{ActionText("Move")} {startZone.CardName}";
            } else if (actionType == ActionType.DEFAULT && !player.Owns(zone) && !zone.IsEmpty())
            {
				var attackDmg = GetAttackDamageWithZoneShifts(startZone, zone);
				var useResourceTo = GetZoneResources(zone).GetUsageTooltip();
				var attackCardWithCardForDamage = $"{ActionText("Attack")} {zone.CardName} {ActionText("With")} {startZone.CardName} {ActionText("For")} {attackDmg}";
				return string.IsNullOrEmpty(useResourceTo) ? attackCardWithCardForDamage : $"{useResourceTo}\n{attackCardWithCardForDamage}";
            } else if (actionType == ActionType.TARGET_ALLY && player.Owns(zone) && !zone.IsEmpty())
            {
				var useResourceTo = GetZoneResources(zone).GetUsageTooltip();
				var useCardsSkillOnCard = $"{ActionText("Use")} {startZone.CardName}{ActionText("Ownership")} {ActionText("Skill")} {ActionText("On")} {zone.CardName}";
				return string.IsNullOrEmpty(useResourceTo) ? useCardsSkillOnCard : $"{useResourceTo}\n{useCardsSkillOnCard}";
            } else
            {
                return "";
            }
        }

        public string GetActionButtonTooltip()
        {
			var useResourceTo = GetActionButtonResources().GetUsageTooltip();
			var useCardsSkill = $"{ActionText("Use")} {startZone.CardName}{ActionText("Ownership")} {ActionText("Skill")}";
			return string.IsNullOrEmpty(useResourceTo) ? useCardsSkill : $"{useResourceTo}\n{useCardsSkill}";
        }

		public PlayerResources GetZoneResources(Zone zone) => zone switch
		{
			_ when actionType != ActionType.DEFAULT => new(0, mana: GetSkillCost(), 0),
			_ when player.Owns(zone) => new(0, mana: GetMoveCost(zone), 0),
			_ => new(0, mana: GetAttackCostWithZoneShifts(startZone, zone), 0)
		};

		public PlayerResources GetActionButtonResources() => new(
			0, 
			mana: GetSkillCost(), 
			0
		);

		private bool CanAttackZone(Zone zone)
        {
			bool hasEnoughMana = player.Resources.SufficientResourcesFor(GetZoneResources(zone));
            InsufficientResourcesFor = hasEnoughMana ? "": ActionText("Attack");
            return startZone.HasPlacedCard() && hasEnoughMana &&
				startZone.PlacedCard.CurrentHealth > 0 && 
                startZone.PlacedCard.GetValidAttackZones(startZone, zone).Contains(zone);
        }

        public bool AcceptCardInHand(Card card) => false;

        public bool CanAcceptActionButton()
        {
            if(startZone.PlacedCard?.GetSkillWithModifiers(startZone, null) is Skill skill && skill.SkillType != ActionType.DEFAULT)
            {
                return startZone.HasPlacedCard() &&  actionType == ActionType.DEFAULT && 
                    player.Resources.SufficientResourcesFor(GetActionButtonResources()) &&
                    !startZone.PlacedCard.IsExerted;
            }
            return false;
        }

        public bool AcceptZone(Zone zone)
        {
            endZone = zone;
            return true;
        }

        public bool AcceptActionButton()
        {
			if (startZone.PlacedCard.Skill is Skill skill)
			{
				actionType = skill.SkillType;
				return actionType == ActionType.SKILL;
			}
			return false;
        }

		private static int GetMoveCost(Zone zone) => zone.PlacedCard.Template.MoveCost;

        private void DoMove()
        {
            // move within own field
            endZone.PlacedCard = startZone.PlacedCard;
            startZone.PlacedCard = null;
            startZone.QueueAnimation(new RemoveCardAnimation(endZone.PlacedCard));
            endZone.QueueAnimation(new PlaceCardAnimation(endZone.PlacedCard));
			player.Resources -= GetZoneResources(endZone);
            GameSounds.PlaySound(GameAction.PLACE_CARD);

            var movedCard = endZone.PlacedCard.Template;
            _logMessage = $"{ActionText("Moved")} {movedCard.CardName}";
        }

        private void DoAttack()
        {
			// Check if any modifiers change the source or destination for the attack
			startZone.Owner.Field.ModifyAttackSourceAndDestZones(ref startZone, ref endZone);
            var prevHealth = endZone.PlacedCard.CurrentHealth;
            startZone.PlacedCard.IsExerted = true;
            var attack = startZone.PlacedCard.GetAttackWithModifiers(startZone, endZone);
			player.Resources -= GetZoneResources(endZone);
            attack.DoAttack(attack, startZone, endZone);

			var pendingAnimationTime = startZone.QueuedAnimationDuration();
            startZone.QueueAnimation(new MeleeAttackAnimation(startZone.PlacedCard, endZone));
            endZone.QueueAnimation(new IdleAnimation(endZone.PlacedCard,  pendingAnimationTime + TimeSpan.FromSeconds(0.5f), prevHealth));
            endZone.QueueAnimation(new TakeDamageAnimation(endZone.PlacedCard, prevHealth));

            player.Field.ClearModifiers(player, startZone, GameEvent.AFTER_ATTACK);
            player.Opponent.Field.ClearModifiers(player, endZone, GameEvent.AFTER_RECEIVE_ATTACK);

            var startCard = startZone.PlacedCard.Template;
            var endCard = endZone.PlacedCard.Template;
            _logMessage = $"{ActionText("Attacked")} {endCard.CardName} {ActionText("With")} {startCard.CardName} {ActionText("For")} {attack.Damage}";
            GameSounds.PlaySound(GameAction.ATTACK);
        }

		private int GetSkillCost() => startZone.PlacedCard.GetSkillWithModifiers(startZone, null).Cost;

		private void DoSkill()
        {
            var skill = startZone.PlacedCard.GetSkillWithModifiers(startZone, null);
            startZone.PlacedCard.IsExerted = true;
			player.Resources -= GetZoneResources(null);
            skill.DoSkill(player, startZone, endZone);
            startZone.QueueAnimation(new ActionAnimation(startZone.PlacedCard));
            endZone?.QueueAnimation(new ActionAnimation(endZone.PlacedCard));
            GameSounds.PlaySound(GameAction.USE_SKILL);

            var startCard = startZone.PlacedCard.Template;
            _logMessage = $"{ActionText("Used")} {startCard.CardName}{ActionText("Ownership")} {ActionText("Skill")}";
            if(endZone?.PlacedCard?.Template is Card endCard)
            {
                _logMessage += $" {ActionText("On")} {endCard.CardName}";
            }
        }

        public void Complete()
        {
            actionCard = startZone.PlacedCard.Template;
            if(actionType != ActionType.DEFAULT)
            {
                DoSkill();
            } else if (player.Owns(endZone))
            {
                DoMove();
            } else  
            {
                DoAttack();
            }
        }

        public Color HighlightColor(Zone zone)
        {
            if(actionType == ActionType.DEFAULT)
            {
                return TCGPlayer.LocalGamePlayer.Owns(zone) ? Color.LightSkyBlue : Color.Crimson;
            } else
            {
                return Color.Goldenrod;
            }
        }

        public void Cancel()
        {
            // No-op
        }

		public void Send(BinaryWriter writer)
		{
			// Invariable info - player & start zone
			writer.Write(player.Index);
			writer.Write((byte)startZone.Index);
			// Variable info - end zone and/or action type
			writer.Write((byte)actionType);
			writer.Write((byte)(endZone?.Index ?? 255));
		}

		public void Receive(BinaryReader reader, CardGame game)
		{
			player = game.GamePlayers[reader.ReadByte()];
			startZone = player.Field.Zones[reader.ReadByte()];
			actionType = (ActionType)reader.ReadByte();
			if(reader.ReadByte() is var endZoneIdx && endZoneIdx != 255)
			{
				if(actionType == ActionType.TARGET_ALLY)
				{
					endZone = player.Field.Zones[endZoneIdx];
				} else
				{
					endZone = player.Opponent.Field.Zones[endZoneIdx];
				}
			}
		}
	}
}
