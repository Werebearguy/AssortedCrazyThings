using Microsoft.Xna.Framework;
using Terraria;

namespace AssortedCrazyThings.Base.Chatter.GoblinUnderlings
{
	public class GoblinUnderlingChatterGenerator : ChatterGenerator
	{
		public GoblinUnderlingChatterGenerator(string key) : base(key)
		{

		}

		public bool TryCreate(Projectile projectile, ChatterSource source, IChatterParams param = null, Vector2? position = null, Vector2? velocity = null, int? cooldownOverride = null)
		{
			if (Main.myPlayer != projectile.owner)
			{
				return false;
			}

			if (ClientConfig.Instance.SatchelofGoodiesDialogueDisabled)
			{
				return false;
			}

			Vector2 pos = position ?? projectile.Top;
			Vector2 vel = velocity ?? new Vector2(Main.rand.NextFloat(1f, 3f) * -projectile.direction, Main.rand.NextFloat(-3.5f, -2f));
			if (TryCreate(source, pos, vel, param, cooldownOverride))
			{
				PutMessageTypeOnCooldown(ChatterSource.Idle); //Always give idle message a cooldown
				return true;
			}

			return false;
		}
	}
}
