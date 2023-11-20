using Terraria;

namespace AssortedCrazyThings.Base.Chatter
{
	public class PlayerHurtByTrapChatterParams : IChatterParams
	{
		public Projectile Projectile { get; init; }
		public Player.HurtInfo HurtInfo { get; init; }

		public PlayerHurtByTrapChatterParams(Projectile projectile, Player.HurtInfo hurtInfo)
		{
			Projectile = projectile;
			HurtInfo = hurtInfo;
		}
	}
}
