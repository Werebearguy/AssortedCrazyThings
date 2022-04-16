using AssortedCrazyThings.Base;
using Terraria;
using Terraria.ID;

namespace AssortedCrazyThings.Projectiles.Pets
{
	[Content(ContentType.CuteSlimes)]
	public class CuteGastropodProj : SimplePetProjBase
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cute Gastropod");
			Main.projFrames[Projectile.type] = 4;
			Main.projPet[Projectile.type] = true;
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.ZephyrFish);
			AIType = ProjectileID.ZephyrFish;
		}

		public override bool PreAI()
		{
			Player player = Projectile.GetOwner();
			player.zephyrfish = false; // Relic from AIType
			return true;
		}

		public override void AI()
		{
			Player player = Projectile.GetOwner();
			PetPlayer modPlayer = player.GetModPlayer<PetPlayer>();
			if (player.dead)
			{
				modPlayer.CuteGastropod = false;
			}
			if (modPlayer.CuteGastropod)
			{
				Projectile.timeLeft = 2;
			}
			AssAI.TeleportIfTooFar(Projectile, player.MountedCenter);
		}
	}
}
