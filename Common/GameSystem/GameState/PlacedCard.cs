using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerraTCG.Common.GameSystem.GameState.Modifiers;

namespace TerraTCG.Common.GameSystem.GameState
{
    internal class PlacedCard(Card template)
    {
        internal Card Template { get; set; } = template;

        internal int CurrentHealth { get; set; } = template.MaxHealth;

        internal List<ICardModifier> CardModifiers { get; set; } = [];

        internal List<ICardModifier> FieldModifiers { get; set; } = [];

		internal Skill? Skill { get; set; } = template.Skills?.FirstOrDefault();

        internal TimeSpan PlaceTime { get; set; }
        internal bool IsExerted { get; set; } = false;

        internal bool IsDamaged => CurrentHealth < Template.MaxHealth;

        internal void Heal(int amount)
        {
            CurrentHealth = Math.Min(Template.MaxHealth, CurrentHealth + amount);
        }

        internal void AddModifiers(List<ICardModifier> modifiers)
        {
            CardModifiers.AddRange(modifiers);
        }

        internal Dictionary<ModifierType, int> GetKeywordModifiers(Zone myZone)
        {
            var modifierMap = new Dictionary<ModifierType, int>();
            if(IsExerted)
            {
                modifierMap[ModifierType.PAUSED] = 1;
            }
            foreach(var modifier in CardModifiers.Where(m=>m.AppliesToZone(myZone) && m.Category != ModifierType.NONE))
            {
                if (!modifierMap.TryGetValue(modifier.Category, out int currentAmount))
                {
                    currentAmount = 0;
                }
                modifierMap[modifier.Category] = currentAmount + modifier.Amount;
            }
            return modifierMap;
        }

        public Attack GetAttackWithModifiers(Zone startZone, Zone endZone)
        {
            var attack = Template.Attacks[0].Copy();
            foreach (var modifier in CardModifiers.Concat(startZone.Owner.Field.CardModifiers))
            {
                modifier.ModifyAttack(ref attack, startZone, endZone);
            }
            if (endZone?.PlacedCard is PlacedCard endCard)
            {
                foreach (var modifier in endCard.CardModifiers.Concat(endZone.Owner.Field.CardModifiers))
                {
                    modifier.ModifyIncomingAttack(ref attack, startZone, endZone);
                }
            }
            return attack;
        }

        public Skill GetSkillWithModifiers(Zone startZone, Zone endZone)
        {
            if(Skill is not Skill skill)
            {
                return default;
            }

            foreach (var modifier in CardModifiers.Concat(startZone.Owner.Field.CardModifiers))
            {
                modifier.ModifySkill(ref skill, startZone, endZone);
            }
            if (endZone?.PlacedCard is PlacedCard endCard)
            {
                foreach (var modifier in endCard.CardModifiers.Concat(endZone.Owner.Field.CardModifiers))
                {
                    modifier.ModifySkill(ref skill, startZone, endZone);
                }
            }
            return skill;
        }

        public Skill ModifyIncomingSkill(Card sourceCard)
        {
            var skill = sourceCard.Skills[0].Copy();
            foreach(var modifier in CardModifiers)
            {
                modifier.ModifyIncomingSkill(ref skill, sourceCard);

            }
            return skill;
        }

        internal List<Zone> GetValidAttackZones(Zone startZone, Zone endZone) 
        {
            // default list of zones: Attack zones and unblocked defense zones
            var targetZones = endZone.Siblings.Where(z => !z.IsEmpty() && !z.IsBlocked()).ToList();

            foreach(var modifier in CardModifiers.Concat(startZone.Owner.Field.CardModifiers))
            {
                modifier.ModifyZoneSelection(startZone, endZone, ref targetZones);
            }

            foreach (var modifier in endZone.Owner.Field.CardModifiers)
            {
                modifier.ModifyIncomingZoneSelection(startZone, endZone, ref targetZones);
            }
            foreach(var modifier in endZone.Siblings.Where(z=>!z.IsEmpty()).SelectMany(z=>z.PlacedCard.CardModifiers))
            {
                modifier.ModifyIncomingZoneSelection(startZone, endZone, ref targetZones);
            }

            return targetZones;
        }

    }
}
