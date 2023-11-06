using AssortedCrazyThings.Base;
using AssortedCrazyThings.Base.ModSupport.AoMM;
using AssortedCrazyThings.Base.SwarmDraw;
using AssortedCrazyThings.Base.SwarmDraw.SwarmofCthulhuDraw;
using AssortedCrazyThings.Buffs.Pets;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace AssortedCrazyThings.Projectiles.Pets
{
	[Content(ContentType.DroppedPets)]
	public class SwarmofCthulhuProj : SimplePetProjBase
	{
		public override void SetStaticDefaults()
		{
			Main.projFrames[Projectile.type] = 1; //Dummy
			Main.projPet[Projectile.type] = true;

			AmuletOfManyMinionsApi.RegisterFlyingPet(this, ModContent.GetInstance<SwarmofCthulhuBuff_AoMM>(), 0);
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.BabyEater);
			Projectile.alpha = 255;
			Projectile.hide = true;
			Projectile.aiStyle = -1;
		}

		private bool setAoMMParas = false;

		public override bool PreAI()
		{
			//Don't do any aomm movement
			AmuletOfManyMinionsApi.ReleaseControl(this);
			if (!setAoMMParas)
			{
				setAoMMParas = true;

				if (AmuletOfManyMinionsApi.TryGetParamsDirect(this, out var paras))
				{
					paras.AttackFramesScaleFactor *= 0.65f;
					AmuletOfManyMinionsApi.UpdateParamsDirect(this, paras);
				}
			}

			Projectile.originalDamage = Math.Max(4, (int)(Projectile.originalDamage * 0.6f));

			return base.PreAI();
		}

		public override void AI()
		{
			Player player = Projectile.GetOwner();
			PetPlayer modPlayer = player.GetModPlayer<PetPlayer>();
			if (player.dead)
			{
				modPlayer.SwarmofCthulhu = false;
			}
			if (modPlayer.SwarmofCthulhu)
			{
				Projectile.timeLeft = 2;
			}

			Projectile.Center = player.Center;
			Projectile.gfxOffY = player.gfxOffY;

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

			if (!AmuletOfManyMinionsApi.IsActive(this) ||
				 !AmuletOfManyMinionsApi.TryGetParamsDirect(this, out var paras) ||
				 !AmuletOfManyMinionsApi.TryGetStateDirect(this, out var state) ||
				 !state.IsInFiringRange || !state.ShouldFireThisFrame)
			{
				return;
			}

			int damage = Projectile.damage;
			float kb = Projectile.knockBack;
			float speed = paras.LaunchVelocity;
			Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Main.rand.NextVector2Unit() * speed, ModContent.ProjectileType<SwarmofCthulhuShotProj>(), damage, kb, Main.myPlayer);
		}

		public override void PostDraw(Color lightColor)
		{
			Projectile.GetOwner().GetModPlayer<SwarmDrawPlayer>().isSwarmofCthulhuDummyDrawing = Projectile.isAPreviewDummy;
		}
	}

	[Content(ContentType.AommSupport | ContentType.DroppedPets)]
	public class SwarmofCthulhuShotProj : AssProjectile
	{
		public override string Texture => SwarmofCthulhuDrawUnit.assetName;


		private UnifiedRandom rng;

		public UnifiedRandom Rng
		{
			get
			{
				if (rng == null)
				{
					rng = new UnifiedRandom(RandomSeed / (1 + Projectile.identity));
				}
				return rng;
			}
		}

		public int RandomSeed
		{
			get => (int)Projectile.ai[0];
			set => Projectile.ai[0] = value;
		}

		public int WobbleTimer
		{
			get => (int)Projectile.ai[1];
			set => Projectile.ai[1] = value;
		}

		private bool homingMode = true;

		float startingSpeed = 0f;

		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.MinionShot[Projectile.type] = true;
			Main.projFrames[Projectile.type] = 4;
		}

		public override void SetDefaults()
		{
			Projectile.width = 6;
			Projectile.height = 6;
			Projectile.friendly = true;
			Projectile.timeLeft = 240;
			Projectile.alpha = 255;
			Projectile.penetrate = 5; //Used for tracking bounces, when enemy is hit it sets to 0
			Projectile.DamageType = DamageClass.Summon;

			Projectile.usesIDStaticNPCImmunity = true;
			Projectile.idStaticNPCHitCooldown = 8;
		}

		public override void OnSpawn(IEntitySource source)
		{
			RandomSeed = (int)DateTime.Now.Ticks;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (Projectile.velocity.X != oldVelocity.X)
			{
				Projectile.velocity.X *= -1;
			}
			if (Projectile.velocity.Y != oldVelocity.Y)
			{
				Projectile.velocity.Y *= -1;
			}

			Projectile.penetrate--;
			return Projectile.penetrate <= 0;
		}

		public override bool? CanHitNPC(NPC target)
		{
			return Projectile.penetrate > 0 ? null : false; //Wack workaround in case hitbox overlaps with more than 1 NPC in the same tick
		}

		public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
		{
			if (target.defense >= 20)
			{
				modifiers.ArmorPenetration += 10;
			}
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			Projectile.penetrate = 0;
		}

		public override void OnKill(int timeLeft)
		{
			for (int i = 0; i < 10; i++)
			{
				Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 5, Math.Sign(Projectile.velocity.X), Math.Sign(Projectile.velocity.Y));
			}
		}

		public override void AI()
		{
			Visuals();

			if (startingSpeed == 0f)
			{
				startingSpeed = Projectile.velocity.Length();
				Projectile.velocity *= 0.25f;
			}

			homingMode = Projectile.GetOwner().DistanceSQ(Projectile.Center) > 64 * 64;

			if (homingMode)
			{
				int targetIndex = AssAI.FindTarget(Projectile, Projectile.Center, 1000);
				if (targetIndex != -1)
				{
					NPC npc = Main.npc[targetIndex];
					Vector2 velocity = npc.Center - Projectile.Center;
					velocity.Normalize();
					velocity *= Math.Min(10, startingSpeed);

					int level = Math.Min(9, AmuletOfManyMinionsApi.GetPetLevel(Projectile.GetOwner()));

					float accel = Math.Max(2, 30 * (1 - level / 9f));
					Projectile.velocity = (Projectile.velocity * (accel - 1) + velocity) / accel;
				}
			}

			if (++WobbleTimer >= 4)
			{
				WobbleTimer = 0;
				float factor = homingMode ? 0.8f : 0.4f;
				Projectile.velocity = Projectile.velocity.RotatedBy(Rng.NextFloat(factor) - factor * 0.5f);
			}

			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.Pi;
		}

		private void Visuals()
		{
			if (Projectile.alpha > 0)
			{
				Projectile.alpha -= 20;

				if (Projectile.alpha < 0)
				{
					Projectile.alpha = 0;
				}
			}

			Projectile.LoopAnimation(4);
		}
	}
}
