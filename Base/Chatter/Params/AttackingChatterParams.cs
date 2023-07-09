using Terraria;

namespace AssortedCrazyThings.Base.Chatter
{
	public class AttackingChatterParams : IChatterParams
	{
		public NPC Target { get; init; }
		public NPC.HitInfo Hit { get; init; }
		public int DamageDone { get; init; }

		public AttackingChatterParams(NPC target, NPC.HitInfo hit, int damageDone)
		{
			Target = target;
			Hit = hit;
			DamageDone = damageDone;
		}
	}
}
