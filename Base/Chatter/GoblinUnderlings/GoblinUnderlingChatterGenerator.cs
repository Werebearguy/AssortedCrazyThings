using AssortedCrazyThings.Projectiles.Minions.GoblinUnderlings;
using Microsoft.Xna.Framework;
using Terraria;

namespace AssortedCrazyThings.Base.Chatter.GoblinUnderlings
{
	public class GoblinUnderlingChatterGenerator : ChatterGenerator
	{
		public GoblinUnderlingChatterGenerator(string key, Color color) : base(key, color)
		{

		}

		public bool TryCreate(GoblinUnderlingChatterHandler.ChatterTracker tracker, Projectile projectile, ChatterSource source, IChatterParams param = null, Vector2? position = null, Vector2? velocity = null, int? cooldownOverride = null)
		{
			if (Main.myPlayer != projectile.owner)
			{
				return false;
			}

			if (ClientConfig.Instance.GoblinUnderlingChatterDisabled)
			{
				return false;
			}

			Vector2 pos = position ?? projectile.Top;
			Vector2 vel = velocity ?? new Vector2(Main.rand.NextFloat(1f, 3f) * -projectile.direction, Main.rand.NextFloat(-3.5f, -2f));

			//AssUtils.Print($"Try spawn for {projectile.Name[0..6]} {source}");
			if (TryCreate(source, pos, vel, param, cooldownOverride))
			{
				tracker.Count(GoblinUnderlingTierSystem.GoblinUnderlingProjs[projectile.type], source);
				//tracker.Report();

				PutMessageTypeOnCooldown(ChatterSource.Idle); //Always give idle message a cooldown
				return true;
			}

			return false;
		}
	}
}
