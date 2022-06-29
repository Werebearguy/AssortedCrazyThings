using AssortedCrazyThings.Base;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Pets
{
	[Content(ContentType.DroppedPets)]
	public class TinySpazmatismProj : SimplePetProjBase
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tiny Spazmatism");
			Main.projFrames[Projectile.type] = 2;
			Main.projPet[Projectile.type] = true;
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.BabyEater);
			Projectile.aiStyle = -1;
			//AIType = ProjectileID.BabyEater;
			Projectile.width = 30;
			Projectile.height = 30;
			DrawOriginOffsetY = -10;
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
				modPlayer.TinyTwins = false;
			}
			if (modPlayer.TinyTwins)
			{
				Projectile.timeLeft = 2;
			}

			AssAI.BabyEaterAI(Projectile, velocityFactor: 1.5f, sway: 0.5f);
			AssAI.BabyEaterDraw(Projectile);

			AssAI.TeleportIfTooFar(Projectile, player.MountedCenter);
		}

		public override bool PreDraw(ref Color lightColor)
		{
			for (int i = 0; i < Main.maxProjectiles; i++)
			{
				Projectile projectile = Main.projectile[i];
				if (projectile.active && Projectile.owner == projectile.owner && projectile.type == ModContent.ProjectileType<TinyRetinazerProj>())
				{
					AssUtils.DrawTether("AssortedCrazyThings/Projectiles/Pets/TinyTwinsProj_Chain", Projectile.Center, projectile.Center);
					break;
				}
			}
			return true;
		}

		public override void PostAI()
		{
			Vector2 between = Projectile.Center - Projectile.GetOwner().Center;
			Projectile.rotation = between.ToRotation() + 1.57f;
			Projectile.spriteDirection = Projectile.direction = -(between.X < 0).ToDirectionInt();
		}
	}

	[Content(ContentType.DroppedPets)]
	public class TinyRetinazerProj : SimplePetProjBase
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tiny Retinazer");
			Main.projFrames[Projectile.type] = 2;
			Main.projPet[Projectile.type] = true;
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.ZephyrFish);
			AIType = ProjectileID.ZephyrFish;
			Projectile.width = 30;
			Projectile.height = 30;
			DrawOriginOffsetY = -10;
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
				modPlayer.TinyTwins = false;
			}
			if (modPlayer.TinyTwins)
			{
				Projectile.timeLeft = 2;
			}
			AssAI.TeleportIfTooFar(Projectile, player.MountedCenter);
		}

		public override void PostAI()
		{
			if (Projectile.frame > 1) Projectile.frame = 0;

			Vector2 between = Projectile.Center - Projectile.GetOwner().Center;
			Projectile.rotation = between.ToRotation() + 1.57f;
			Projectile.spriteDirection = Projectile.direction = -(between.X < 0).ToDirectionInt();
		}
	}
}
