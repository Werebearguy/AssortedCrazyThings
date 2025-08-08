using AssortedCrazyThings.Base;
using AssortedCrazyThings.Base.ModSupport.AoMM;
using AssortedCrazyThings.Buffs.Pets;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Pets
{
	[Content(ContentType.DroppedPets)]
	public class PetDreadnautilusProj : SimplePetProjBase
	{
		public override void SetStaticDefaults()
		{
			Main.projFrames[Projectile.type] = 6;
			Main.projPet[Projectile.type] = true;

			ProjectileID.Sets.CharacterPreviewAnimations[Projectile.type] = ProjectileID.Sets.SimpleLoop(0, Main.projFrames[Projectile.type], 6)
				.WithOffset(-10, -16f)
				.WithSpriteDirection(-1)
				.WithCode(DelegateMethods.CharacterPreview.Float);

			//Shoots in a shotgun pattern
			AmuletOfManyMinionsApi.RegisterFlyingPet(this, ModContent.GetInstance<PetDreadnautilusBuff_AoMM>(), 0);
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.ZephyrFish);
			AIType = ProjectileID.ZephyrFish;
		}

		private bool setAoMMParams = false;
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
					//Does not need to update every tick
					paras.PreferredTargetDistance = 90;

					paras.AttackFramesScaleFactor *= prolongFactor;
					AmuletOfManyMinionsApi.UpdateParamsDirect(this, paras);
				}
			}

			Projectile.originalDamage = Math.Max(4, (int)(Projectile.originalDamage * prolongFactor / shotsPerBurst * 1.4f));
			return true;
		}

		public override void AI()
		{
			Player player = Projectile.GetOwner();
			PetPlayer modPlayer = player.GetModPlayer<PetPlayer>();
			if (player.dead)
			{
				modPlayer.PetDreadnautilus = false;
			}
			if (modPlayer.PetDreadnautilus)
			{
				Projectile.timeLeft = 2;
			}
			AssAI.TeleportIfTooFar(Projectile, player.MountedCenter);

			if (AmuletOfManyMinionsApi.IsAttacking(this))
			{
				AoMM_Attacking();
			}
			else
			{
				if (Projectile.DistanceSQ(player.Center) > 300f * 300f)
				{
					Projectile.direction = Projectile.spriteDirection = -Projectile.direction;
				}
			}
		}

		private void AoMM_Attacking()
		{
			if (Main.myPlayer != Projectile.owner)
			{
				return;
			}

			if (!AmuletOfManyMinionsApi.TryGetParamsDirect(this, out var paras))
			{
				return;
			}

			if (!AmuletOfManyMinionsApi.TryGetStateDirect(this, out var state)
				|| !state.ShouldFireThisFrame
				|| !state.IsInFiringRange || state.TargetNPC is not NPC targetNPC)
			{
				return;
			}

			Vector2 toTarget = (targetNPC.Center - Projectile.Center).SafeNormalize(Vector2.Zero);
			toTarget *= paras.LaunchVelocity * 0.75f;

			for (int i = 0; i < shotsPerBurst; i++)
			{
				Vector2 launchVelocity = toTarget.RotatedBy(0.15f * Utils.Remap((float)i / shotsPerBurst, 0f, (float)(shotsPerBurst - 1) / shotsPerBurst, -1f, 1f));
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, launchVelocity, ModContent.ProjectileType<PetDreadnautilusShotProj>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
			}
		}

		//TODO add this to any flying pets aswell
		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			if (AmuletOfManyMinionsApi.IsAttacking(this))
			{
				fallThrough = true;
			}

			return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
		}
	}

	[Content(ContentType.AommSupport | ContentType.DroppedPets)]
	public class PetDreadnautilusShotProj : MinionShotProj_AoMM
	{
		public override int ClonedType => ProjectileID.BloodNautilusShot;

		public override bool UseCustomTexture => true;

		public override void SafeSetDefaults()
		{
			Projectile.width = 6;
			Projectile.height = 6;
			Projectile.tileCollide = true;
			Projectile.penetrate = 2;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = -1;
		}

		public override bool PreKill(int timeLeft)
		{
			//No-op, original projectile kill dust too aggressive
			return true;
		}

		public override void OnKill(int timeLeft)
		{
			for (int i = 0; i < 20; i++)
			{
				Dust dust = Main.dust[Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 5)];
				dust.scale = Main.rand.NextFloat(1f, 1.5f);
			}
		}
	}
}
