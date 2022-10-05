using AssortedCrazyThings.Base;
using AssortedCrazyThings.Base.ModSupport.AoMM;
using AssortedCrazyThings.Buffs.Pets;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Pets
{
	public class DrumstickElementalProj : SimplePetProjBase
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Drumstick Elemental");
			Main.projFrames[Projectile.type] = 4;
			Main.projPet[Projectile.type] = true;

			//Shoot manually in burst of 3
			AmuletOfManyMinionsApi.RegisterFlyingPet(this, ModContent.GetInstance<DrumstickElementalBuff_AoMM>(), 0);
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.ZephyrFish);
			AIType = ProjectileID.ZephyrFish;
		}

		private bool setAoMMParams = false;
		private int shotTimer = 0;
		private const float burstDurationRatio = 0.5f;
		private const int shotsPerBurst = 3;

		public override bool PreAI()
		{
			Player player = Projectile.GetOwner();
			player.zephyrfish = false; // Relic from AIType

			float prolongFactor = 1.5f;
			if (!setAoMMParams)
			{
				setAoMMParams = true;

				if (AmuletOfManyMinionsApi.TryGetParamsDirect(this, out var paras))
				{
					paras.AttackFramesScaleFactor *= prolongFactor;
					AmuletOfManyMinionsApi.UpdateParamsDirect(this, paras);
				}
			}

			Projectile.originalDamage = (int)(Projectile.originalDamage * prolongFactor / shotsPerBurst * 1.2f);

			return true;
		}

		public override void AI()
		{
			Player player = Projectile.GetOwner();
			PetPlayer modPlayer = player.GetModPlayer<PetPlayer>();
			if (player.dead)
			{
				modPlayer.DrumstickElemental = false;
			}
			if (modPlayer.DrumstickElemental)
			{
				Projectile.timeLeft = 2;
			}
			AssAI.TeleportIfTooFar(Projectile, player.MountedCenter);

			if (AmuletOfManyMinionsApi.IsActive(this))
			{
				AoMM_AI();
			}
		}

		private void AoMM_AI()
		{
			if (Main.myPlayer != Projectile.owner)
			{
				return;
			}

			if (!AmuletOfManyMinionsApi.IsAttacking(this) || !AmuletOfManyMinionsApi.TryGetParamsDirect(this, out var paras))
			{
				return;
			}

			if (!AmuletOfManyMinionsApi.TryGetStateDirect(this, out var state)
				|| !state.IsInFiringRange || state.TargetNPC is not NPC targetNPC)
			{
				return;
			}

			int interval = paras.AttackFrames;
			shotTimer++;
			if (shotTimer > interval)
			{
				shotTimer = 0;
			}

			if (!AssUtils.CanShootBurst(shotTimer, interval, burstDurationRatio, shotsPerBurst))
			{
				return;
			}

			Vector2 toTarget = (targetNPC.Center - Projectile.Center).SafeNormalize(Vector2.Zero);
			toTarget *= paras.LaunchVelocity;
			Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, toTarget, ModContent.ProjectileType<DrumstickElementalShotProj>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
		}
	}

	[Content(ContentType.AommSupport | ContentType.OtherPets)]
	public class DrumstickElementalShotProj : AssProjectile
	{
		public int SelectedFrame
		{
			get => (int)Projectile.ai[0] - 1;
			set => Projectile.ai[0] = value + 1;
		}

		public bool HasSelectedFrame => SelectedFrame >= 0;

		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.MinionShot[Projectile.type] = true;
			Main.projFrames[Projectile.type] = 5;
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.Bullet);
			Projectile.DamageType = DamageClass.Summon;
			Projectile.timeLeft = 180;
			Projectile.alpha = 150;

			AIType = ProjectileID.Bullet;
		}

		public override void OnSpawn(IEntitySource source)
		{
			SelectedFrame = Main.rand.Next(Main.projFrames[Projectile.type]);
		}

		public override void AI()
		{
			if (HasSelectedFrame)
			{
				Projectile.frame = SelectedFrame;
				SelectedFrame = -1;
			}
		}
	}
}
