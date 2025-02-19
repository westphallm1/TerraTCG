using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerraTCG.Common.GameSystem.GameState.Modifiers
{
    internal class SameColumnDamageModifier(int amount, List<GameEvent> removeOn = null) : ICardModifier
    {
        public int Amount => amount;

        public Asset<Texture2D> Texture { get; set; }
		public Card SourceCard { get; set; }

        public void ModifyAttack(ref Attack attack, Zone sourceZone, Zone destZone)
        {
			if(sourceZone.Column == (destZone?.Index ?? -1) % 3)
			{
				attack.Damage += amount;
			}
        }

        public bool ShouldRemove(GameEventInfo eventInfo) =>
            removeOn?.Contains(eventInfo.Event) ?? false;

    }
}
