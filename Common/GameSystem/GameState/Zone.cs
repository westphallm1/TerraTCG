using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil.Cil;
using Newtonsoft.Json.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using TerraTCG.Common.GameSystem.Drawing;
using TerraTCG.Common.GameSystem.Drawing.Animations;
using TerraTCG.Common.GameSystem.GameState.Modifiers;

namespace TerraTCG.Common.GameSystem.GameState
{
    internal enum ZoneRole
    {
        OFFENSE,
        DEFENSE,
        SKILL
    }
    internal class Zone
    {
        internal CardGame Game { get; set; }
        internal PlacedCard PlacedCard { get; set; }
        internal ZoneRole Role { get; set; }

        internal int Index { get; set; }

		internal int Row => Index / 3;
		internal int Column => Index % 3;

        private List<IAnimation> animationQueue = [];
        internal IAnimation Animation { get => animationQueue.Count > 0 ? animationQueue[0] : null; set { QueueAnimation(value); } }

        internal const float CARD_DRAW_SCALE = 2f / 3f;

        public GamePlayer Owner => Game.GamePlayers.Where(p => p.Field.Zones.Contains(this)).FirstOrDefault();

        public List<Zone> Siblings => Owner.Field.Zones;

        // Helper to get the name of the card that is (or isn't) placed in the zone
        public string CardName => PlacedCard?.Template.CardName;

        public bool HasPlacedCard() => PlacedCard != null;

        // For defense zones, check whether an enemy is in the aligned offense zone
        public bool IsBlocked() => Role == ZoneRole.DEFENSE && !Owner.Field.Zones[Index - 3].IsEmpty();

        public void PlaceCard(Card card)
        {
            PlacedCard = new PlacedCard(card)
            {
                IsExerted = true,
                PlaceTime = TCGPlayer.TotalGameTime,
                CardModifiers = [.. card.Modifiers?.Invoke() ?? []],
                FieldModifiers = [.. card.FieldModifiers?.Invoke() ?? []],
            };

			Owner.Field.CardModifiers.AddRange(PlacedCard.FieldModifiers);
            foreach (var modifier in PlacedCard.CardModifiers.Concat(Owner.Field.CardModifiers))
            {
                modifier.ModifyCardEntrance(this);
            }
        }

        public void PromoteCard(Card newCard)
        {
            var leavingCard = PlacedCard;
            QueueAnimation(new RemoveCardAnimation(leavingCard));
            QueueAnimation(new PlaceCardAnimation(new PlacedCard(newCard)));

            var dmgTaken = leavingCard.Template.MaxHealth - leavingCard.CurrentHealth;
            // Keep all item-sourced modifiers on the card post-promotion,
            // Remove debuffs
            var itemModifiers = leavingCard.CardModifiers
                .Where(m => m.Source == CardSubtype.EQUIPMENT || m.Source == CardSubtype.CONSUMABLE)
                .ToList();
            PlaceCard(newCard);
            PlacedCard.IsExerted = false;
            PlacedCard.CurrentHealth -= dmgTaken;
            PlacedCard.AddModifiers(itemModifiers);

            GameSounds.PlaySound(GameAction.PROMOTE_CARD);
        }

        internal void QueueAnimation(IAnimation animation)
        {
            // Clear out any infinite idle animations
            if(!animation.IsDefault())
            {
                animationQueue = animationQueue.Where(q => !q.IsDefault()).ToList();
            }
            animation.StartTime = TCGPlayer.TotalGameTime;
            animation.SourceZone = this;
            animationQueue.Add(animation);
        }

		internal TimeSpan QueuedAnimationDuration()
		{
			var nonDefault = animationQueue.Where(a => !a.IsDefault());
			if (!nonDefault.Any())
			{
				return TimeSpan.Zero;
			}
			return nonDefault.First().RemainingTime + new TimeSpan(nonDefault.Skip(1).Sum(a => a.Duration.Ticks));
		}

        internal void UpdateAnimationQueue()
        {
            // Ensure that there's always an empty animation waiting at the end of the queue
            if(!IsEmpty() && (animationQueue.Count == 0 || !animationQueue.Last().IsDefault()))
            {
                QueueAnimation(new IdleAnimation(PlacedCard));
            }
            if (animationQueue.Count > 0 && animationQueue[0].IsComplete())
            {
                animationQueue.RemoveAt(0);
                if(animationQueue.Count > 0)
                {
                    animationQueue[0].StartTime = TCGPlayer.TotalGameTime;
                }
            }
        }

        private void DrawOffenseIcon(SpriteBatch spriteBatch, Vector2 position, float rotation)
        {
            var texture = TextureCache.Instance.OffenseIcon;
            var frameWidth = texture.Value.Width / 4;
            var frameHeight = texture.Value.Height / 6;
            var bounds = new Rectangle(2 * frameWidth, 0, frameWidth, frameHeight);
            var origin = new Vector2(bounds.Width, bounds.Height) / 2;
            spriteBatch.Draw(texture.Value, position, bounds, Color.White, rotation, origin, 1f, SpriteEffects.None, 0);
        }

        private void DrawDefenseIcon(SpriteBatch spriteBatch, Vector2 position, float rotation)
        {
            var texture = TextureCache.Instance.DefenseIcon;
            var bounds = texture.Value.Bounds;
            var origin = new Vector2(bounds.Width, bounds.Height) / 2;
            spriteBatch.Draw(texture.Value, position, bounds, Color.White, rotation, origin, 1f, SpriteEffects.None, 0);
        }

        internal Dictionary<ModifierType, int> GetKeywordModifiers()
		{
			// Easy - Get the modifiers that apply directly on the card
			var modifierMap = PlacedCard?.GetKeywordModifiers(this) ?? [];

			// More challenging - get field modifiers that also apply to the card
			var fieldModifiersForCard = Owner.Field.CardModifiers.Where(m => m.AppliesToZone(this));

			foreach(var modifier in fieldModifiersForCard.Where(m=>m.Category != ModifierType.NONE))
			{
                if (!modifierMap.TryGetValue(modifier.Category, out int currentAmount))
                {
                    currentAmount = 0;
                }
                modifierMap[modifier.Category] = currentAmount + modifier.Amount;
            }
            return modifierMap;
		}

        internal bool IsEmpty() => PlacedCard == null;

        internal void Draw(SpriteBatch spriteBatch, Vector2 position, float rotation)
        {
            var gamePlayer = TCGPlayer.LocalGamePlayer;
            var texture = TextureCache.Instance.Zone;
            var bounds = texture.Value.Bounds;
            var origin = new Vector2(bounds.Width, bounds.Height) / 2;
            spriteBatch.Draw(texture.Value, position + origin, bounds, Color.White, rotation, origin, 1f, SpriteEffects.None, 0);
            if(Role == ZoneRole.OFFENSE)
            {
                DrawOffenseIcon(spriteBatch, position + origin, rotation);
            } else
            {
                DrawDefenseIcon(spriteBatch, position + origin, rotation);
            }

            // Draw the placed card
            Animation?.DrawZone(spriteBatch, position, rotation);

            if(gamePlayer.SelectedFieldZone == this)
            {
                texture = TextureCache.Instance.ZoneHighlighted;
                bounds = texture.Value.Bounds;
                origin = new Vector2(bounds.Width, bounds.Height) / 2;

                spriteBatch.Draw(texture.Value, position + origin, bounds, Color.White, rotation, origin, 1f, SpriteEffects.None, 0);
            } else if (gamePlayer.InProgressAction?.CanAcceptZone(this) ?? false)
            {
                texture = TextureCache.Instance.ZoneSelectable;
                bounds = texture.Value.Bounds;
                origin = new Vector2(bounds.Width, bounds.Height) / 2;

                float brightness = 0.5f + 0.5f * MathF.Sin(MathF.Tau * (float)TCGPlayer.TotalGameTime.TotalSeconds / 2f);
                var color = gamePlayer.InProgressAction.HighlightColor(this);

                spriteBatch.Draw(texture.Value, position + origin, bounds, color * brightness, rotation, origin, 1f, SpriteEffects.None, 0);
            }
        }

        internal void DrawNPC(SpriteBatch spriteBatch, Vector2 position, float scale)
        {
            Animation?.DrawZoneOverlay(spriteBatch, position, scale);
        }

        internal void DrawNPCStats(SpriteBatch spriteBatch, Vector2 position, float scale)
        {
            Animation?.DrawZoneStats(spriteBatch, position, scale);
        }

		internal void ClearAnimationQueue()
		{
			animationQueue = [];
		}
	}
}
