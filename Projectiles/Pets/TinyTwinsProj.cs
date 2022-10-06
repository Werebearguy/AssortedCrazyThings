using AssortedCrazyThings.Base;
using AssortedCrazyThings.Base.ModSupport.AoMM;
using AssortedCrazyThings.Buffs.Pets;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
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

			AmuletOfManyMinionsApi.RegisterFlyingPet(this, ModContent.GetInstance<TinyTwinsBuff_AoMM>(), ModContent.ProjectileType<TinySpazmatismShotProj>());
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

			if (AmuletOfManyMinionsApi.TryGetParamsDirect(this, out var paras))
			{
				//Need to update every tick
				paras.AttackFrames = 5;

				//Does not need to update every tick
				paras.PreferredTargetDistance = 80;

				AmuletOfManyMinionsApi.UpdateParamsDirect(this, paras);
			}

			if (AmuletOfManyMinionsApi.IsActive(this))
			{
				//Default AttackFrames scaling is 60 (tier 0) to 30 (capped at tier >= 5), so scale damage at same rate
				//tier 0 ratio with fixed 5 attackframes = 12
				//tier >=5 ratio "-" = 6
				int tier = AmuletOfManyMinionsApi.GetPetLevel(player);
				float damageRange = Math.Clamp(tier, 1, 5) / 5f;
				float damageRatio = 6 + (1f - damageRange) * 6f;
				Projectile.originalDamage = Math.Max(4, (int)(Projectile.originalDamage / damageRatio)); //Min damage is 4
			}

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

			if (AmuletOfManyMinionsApi.IsActive(this))
			{
				AoMM_AI();
			}
		}

		private void AoMM_AI()
		{
			//Need state to adjust rotation
			if (!AmuletOfManyMinionsApi.TryGetStateDirect(this, out var state)
				|| !state.IsInFiringRange || state.TargetNPC is not NPC targetNPC)
			{
				return;
			}

			Vector2 between = Projectile.Center - targetNPC.Center;
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

			AmuletOfManyMinionsApi.RegisterFlyingPet(this, ModContent.GetInstance<TinyTwinsBuff_AoMM>(), ModContent.ProjectileType<TinyRetinazerShotProj>());
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

			if (AmuletOfManyMinionsApi.IsActive(this))
			{
				AoMM_AI();
			}
		}

		private void AoMM_AI()
		{
			//Need state to adjust rotation
			if (!AmuletOfManyMinionsApi.TryGetStateDirect(this, out var state)
				|| !state.IsInFiringRange || state.TargetNPC is not NPC targetNPC)
			{
				return;
			}

			Vector2 between = Projectile.Center - targetNPC.Center;
			Projectile.rotation = between.ToRotation() + 1.57f;
			Projectile.spriteDirection = Projectile.direction = -(between.X < 0).ToDirectionInt();
		}
	}

	[Content(ContentType.AommSupport | ContentType.DroppedPets)]
	public class TinySpazmatismShotProj : AssProjectile
	{
		public override string Texture => "AssortedCrazyThings/Empty";

		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.MinionShot[Projectile.type] = true;
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.EyeFire);
			Projectile.aiStyle = 23;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.timeLeft = 30;
			Projectile.DamageType = DamageClass.Summon;
			AIType = ProjectileID.EyeFire;
		}

		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			target.AddBuff(BuffID.CursedInferno, 60);
		}

		public override void OnSpawn(IEntitySource source)
		{
			Projectile.ai[0] = 7; //Start particles immediately
		}
	}

	[Content(ContentType.AommSupport | ContentType.DroppedPets)]
	public class TinyRetinazerShotProj : MinionShotProj_AoMM
	{
		public override int ClonedType => ProjectileID.MiniRetinaLaser; //Optic staff laser

		public override void OnSpawn(IEntitySource source)
		{
			//Due to increased extraUpdates (2), but too slow (0.25f) looks unnatural for a laser
			Projectile.velocity *= 0.5f;
		}
	}
}
