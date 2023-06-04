using AssortedCrazyThings.Base;
using AssortedCrazyThings.Base.ModSupport.AoMM;
using AssortedCrazyThings.Buffs.Pets;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Pets
{
	public class BabyCrimeraProj : SimplePetProjBase
	{
		public override void SetStaticDefaults()
		{
			Main.projFrames[Projectile.type] = 2;
			Main.projPet[Projectile.type] = true;

			ProjectileID.Sets.CharacterPreviewAnimations[Projectile.type] = ProjectileID.Sets.SimpleLoop(0, Main.projFrames[Projectile.type], 8)
				.WithOffset(-8, -20f)
				.WithSpriteDirection(-1)
				.WithCode(DelegateMethods.CharacterPreview.Float);

			AmuletOfManyMinionsApi.RegisterFlyingPet(this, ModContent.GetInstance<BabyCrimeraBuff_AoMM>(), null);
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.BabyEater);
			AIType = ProjectileID.BabyEater;

			Projectile.width = 34;
			Projectile.height = 34;
		}

		public override bool PreAI()
		{
			Player player = Projectile.GetOwner();
			player.eater = false; // Relic from AIType
			return true;
		}

		public override void AI()
		{
			Player player = Projectile.GetOwner();
			PetPlayer modPlayer = player.GetModPlayer<PetPlayer>();
			if (player.dead)
			{
				modPlayer.BabyCrimera = false;
			}
			if (modPlayer.BabyCrimera)
			{
				Projectile.timeLeft = 2;
			}
			AssAI.TeleportIfTooFar(Projectile, player.MountedCenter);
		}
	}
}
