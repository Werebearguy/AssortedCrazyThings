using AssortedCrazyThings.Base;
using AssortedCrazyThings.Base.Handlers.CharacterPreviewAnimationsHandler;
using AssortedCrazyThings.Base.ModSupport.AoMM;
using AssortedCrazyThings.Buffs.Pets;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Pets
{
	[Content(ContentType.FriendlyNPCs)]
	public class ChunkyProj : SimplePetProjBase
	{
		public override void SetStaticDefaults()
		{
			Main.projFrames[Projectile.type] = 2;
			Main.projPet[Projectile.type] = true;

			ProjectileID.Sets.CharacterPreviewAnimations[Projectile.type] = ProjectileID.Sets.SimpleLoop(0, Main.projFrames[Projectile.type], 8)
				.WithOffset(-4, -8f)
				.WithSpriteDirection(-1)
				.WithCode(DocileDemonEyeProj.FloatAndRotate);

			AmuletOfManyMinionsApi.RegisterFlyingPet(this, ModContent.GetInstance<ChunkyandMeatballBuff_AoMM>(), null);
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.BabyEater);
			AIType = ProjectileID.BabyEater;
			Projectile.width = 22;
			Projectile.height = 34;
		}

		public override bool PreAI()
		{
			Player player = Projectile.GetOwner();
			player.eater = false; // Relic from AIType

			Projectile.originalDamage = (int)(Projectile.originalDamage * 0.7f);

			return true;
		}

		public override void AI()
		{
			Player player = Projectile.GetOwner();
			PetPlayer modPlayer = player.GetModPlayer<PetPlayer>();
			if (player.dead)
			{
				modPlayer.ChunkyandMeatball = false;
			}
			if (modPlayer.ChunkyandMeatball)
			{
				Projectile.timeLeft = 2;
			}
			AssAI.TeleportIfTooFar(Projectile, player.MountedCenter);
		}

		public override void PostAI()
		{
			Projectile.spriteDirection = Projectile.direction = (Projectile.velocity.X < 0).ToDirectionInt();
		}
	}

	[Content(ContentType.FriendlyNPCs)]
	public class MeatballProj : SimplePetProjBase
	{
		public override void SetStaticDefaults()
		{
			Main.projFrames[Projectile.type] = 2;
			Main.projPet[Projectile.type] = true;

			SecondaryPetHandler.AddToMainProj(ModContent.ProjectileType<ChunkyProj>(), Projectile.type);

			ProjectileID.Sets.CharacterPreviewAnimations[Projectile.type] = ProjectileID.Sets.SimpleLoop(0, Main.projFrames[Projectile.type], 8)
				.WithOffset(-8, -18f)
				.WithSpriteDirection(-1)
				.WithCode(DocileDemonEyeProj.FloatAndRotate);

			AmuletOfManyMinionsApi.RegisterFlyingPet(this, ModContent.GetInstance<ChunkyandMeatballBuff_AoMM>(), null);
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.BabyEater);
			AIType = ProjectileID.BabyEater;
			Projectile.width = 22;
			Projectile.height = 34;
		}

		public override bool PreAI()
		{
			Player player = Projectile.GetOwner();
			player.eater = false; // Relic from AIType

			Projectile.originalDamage = (int)(Projectile.originalDamage * 0.7f);

			return true;
		}

		public override void AI()
		{
			Player player = Projectile.GetOwner();
			PetPlayer modPlayer = player.GetModPlayer<PetPlayer>();
			if (player.dead)
			{
				modPlayer.ChunkyandMeatball = false;
			}
			if (modPlayer.ChunkyandMeatball)
			{
				Projectile.timeLeft = 2;
			}
			AssAI.TeleportIfTooFar(Projectile, player.MountedCenter);
		}

		public override void PostAI()
		{
			Projectile.spriteDirection = Projectile.direction = (Projectile.velocity.X < 0).ToDirectionInt();
		}
	}
}
