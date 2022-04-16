using AssortedCrazyThings.Base;
using Terraria;

namespace AssortedCrazyThings.Projectiles.Pets
{
	[Content(ContentType.HostileNPCs)]
	//check this file for more info vvvvvvvv
	public class ChunkySlimeProj : BabySlimeBase
	{
		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("ChunkySlimeProj");
		}

		public override void SafeSetDefaults()
		{
			Projectile.width = 32;
			Projectile.height = 30;
			Projectile.alpha = 0;

			Projectile.minion = false;
		}

		public override bool PreAI()
		{
			PetPlayer modPlayer = Projectile.GetOwner().GetModPlayer<PetPlayer>();
			if (Projectile.GetOwner().dead)
			{
				modPlayer.ChunkySlime = false;
			}
			if (modPlayer.ChunkySlime)
			{
				Projectile.timeLeft = 2;
			}
			return true;
		}
	}
}
