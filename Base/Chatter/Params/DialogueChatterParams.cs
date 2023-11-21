using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;

namespace AssortedCrazyThings.Base.Chatter
{
	public class DialogueChatterParams : IChatterParams
	{
		public List<Projectile> Projectiles { get; init; }
		public List<Projectile> NextOrPrevProjectiles { get; init; }
		public Color Color { get; init; }
		public int Duration { get; init; }

		public DialogueChatterParams(List<Projectile> projectiles, List<Projectile> nextOrPrevProjectiles, Color color, int duration)
		{
			Projectiles = projectiles;
			NextOrPrevProjectiles = nextOrPrevProjectiles;
			Color = color;
			Duration = duration;
		}
	}
}
