using AssortedCrazyThings.Base;
using Terraria;

namespace AssortedCrazyThings.Projectiles.Pets
{
	//check this file for more info vvvvvvvv
	public class FailureSlimeProj : BabySlimeBase
	{
		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Failure Slime");
		}

		public override void SafeSetDefaults()
		{
			Projectile.width = 34;
			Projectile.height = 30;
			Projectile.alpha = 0;

			Projectile.minion = false;
		}

		public override bool PreAI()
		{
			PetPlayer modPlayer = Projectile.GetOwner().GetModPlayer<PetPlayer>();
			if (Projectile.GetOwner().dead)
			{
				modPlayer.FailureSlime = false;
			}
			if (modPlayer.FailureSlime)
			{
				Projectile.timeLeft = 2;
			}
			return true;
		}
	}
}
