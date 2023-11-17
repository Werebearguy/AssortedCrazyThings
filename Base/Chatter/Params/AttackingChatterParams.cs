using Terraria;

namespace AssortedCrazyThings.Base.Chatter
{
	public class AttackingChatterParams : IChatterParams
	{
		public Projectile Projectile { get; init; }
		public NPC Target { get; init; }
		public NPC.HitInfo Hit { get; init; }
		public int DamageDone { get; init; }

		public AttackingChatterParams(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
		{
			Projectile = projectile;
			Target = target;
			Hit = hit;
			DamageDone = damageDone;
		}
	}
}
