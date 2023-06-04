using AssortedCrazyThings.Base;
using AssortedCrazyThings.Base.ModSupport.AoMM;
using AssortedCrazyThings.Buffs.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Pets
{
	[Content(ContentType.HostileNPCs)]
	//check this file for more info vvvvvvvv
	public class ChunkySlimeProj : BabySlimeBase
	{
		public override void SafeSetStaticDefaults()
		{
			AmuletOfManyMinionsApi.RegisterSlimePet(this, ModContent.GetInstance<ChunkySlimeBuff_AoMM>(), null);
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
