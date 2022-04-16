using AssortedCrazyThings.Base;
using Terraria;

namespace AssortedCrazyThings.Projectiles.Pets
{
	[Content(ContentType.FriendlyNPCs)]
	//check this file for more info vvvvvvvv
	public class FairySlimeProj : BabySlimeBase
	{
		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Fairy Slime");
		}

		public override void SafeSetDefaults()
		{
			Projectile.width = 32;
			Projectile.height = 30;

			Projectile.minion = false;
		}

		public override bool PreAI()
		{
			PetPlayer modPlayer = Projectile.GetOwner().GetModPlayer<PetPlayer>();
			if (Projectile.GetOwner().dead)
			{
				modPlayer.FairySlime = false;
			}
			if (modPlayer.FairySlime)
			{
				Projectile.timeLeft = 2;
			}
			return true;
		}
	}
}
