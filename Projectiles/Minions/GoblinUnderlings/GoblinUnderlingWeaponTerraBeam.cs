using AssortedCrazyThings.Base.Chatter;
using AssortedCrazyThings.Base.Chatter.GoblinUnderlings;
using AssortedCrazyThings.Projectiles.Minions.GoblinUnderlings.Eager;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Minions.GoblinUnderlings
{
	[Content(ContentType.Weapons)]
	public class GoblinUnderlingWeaponTerraBeam : AssProjectile
	{
		public bool Spawned
		{
			get => Projectile.ai[1] == 1f;
			set => Projectile.ai[1] = value ? 1 : 0;
		}

		public int PulseTimer
		{
			get => (int)Projectile.localAI[0];
			set => Projectile.localAI[0] = value;
		}

		public const int AppearTimerMax = 8;
		public const int AppearTimerTransparentBefore = 2;
		public const int AppearTimerTransition = AppearTimerMax - AppearTimerTransparentBefore;
		public int AppearTimer
		{
			get => (int)Projectile.localAI[1];
			set => Projectile.localAI[1] = value;
		}

		public int ownedGoblinWhoAmI = -1;

		public bool FromGoblin => ownedGoblinWhoAmI != -1;

		public override void SetStaticDefaults()
		{
			Main.projFrames[Projectile.type] = 1;
			ProjectileID.Sets.MinionShot[Projectile.type] = true;
		}

		public override void SetDefaults()
		{
			Projectile.width = 16;
			Projectile.height = 16;
			//Projectile.aiStyle = 27;
			Projectile.penetrate = 3;
			//Projectile.light = 0.5f;
			Projectile.alpha = 255;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Summon;
			Projectile.timeLeft = 120;

			DrawOriginOffsetX = -(Projectile.width / 2 - 32f / 2);
			DrawOffsetX = (int)-DrawOriginOffsetX * 2;

			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 10;
		}

		public override void OnSpawn(IEntitySource source)
		{
			if (source is EntitySource_Parent parentSource
				&& parentSource.Entity is Projectile parentProjectile
				&& GoblinUnderlingTierSystem.GoblinUnderlingProjs.ContainsKey(parentProjectile.type))
			{
				ownedGoblinWhoAmI = parentProjectile.whoAmI;
			}
		}

		public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
		{
			if (FromGoblin)
			{
				GoblinUnderlingHelperSystem.CommonModifyHitNPC(Main.projectile[ownedGoblinWhoAmI].type, Projectile, target, ref modifiers);
			}
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			Projectile.damage = (int)(Projectile.damage * 0.85f);

			if (target.boss || !FromGoblin || Main.projectile[ownedGoblinWhoAmI] is not Projectile parent || parent.ModProjectile is not EagerUnderlingProj goblin)
			{
				return;
			}

			if (goblin.OutOfCombat())
			{
				ModContent.GetInstance<GoblinUnderlingChatterHandler>().OnAttacking(parent, target, hit, damageDone);
			}

			goblin.SetInCombat();
		}

		public override Color? GetAlpha(Color lightColor)
		{
			if (AppearTimer >= AppearTimerMax)
			{
				return new Color(255, 255, 255, Projectile.alpha);
			}

			if (AppearTimer < AppearTimerTransparentBefore)
			{
				return Color.Transparent;
			}

			int c = (int)((AppearTimer - AppearTimerTransparentBefore) / (float)AppearTimerTransition * 255f);
			return new Color(c, c, c, c);
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			SoundEngine.PlaySound(SoundID.Item10 with { Volume = 0.7f }, Projectile.Center);

			for (int i = 4; i < 16; i++)
			{
				float oldvelX = Projectile.oldVelocity.X * (15f / i) * 0.5f;
				float oldVelY = Projectile.oldVelocity.Y * (15f / i) * 0.5f;
				Dust dust = Dust.NewDustDirect(new Vector2(Projectile.oldPosition.X - oldvelX, Projectile.oldPosition.Y - oldVelY), 8, 8, 107, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default(Color), 1.3f);
				dust.noGravity = true;
				dust.velocity *= 0.3f;
				dust = Dust.NewDustDirect(new Vector2(Projectile.oldPosition.X - oldvelX, Projectile.oldPosition.Y - oldVelY), 8, 8, 107, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default(Color), 1.1f);
				dust.velocity *= 0.04f;
			}

			return base.OnTileCollide(oldVelocity);
		}

		public override void AI()
		{
			Lighting.AddLight(Projectile.Center, new Vector3(0.1f, 0.5f, 0.3f));

			if (Spawned)
			{
				Spawned = true;
				SoundEngine.PlaySound(SoundID.Item60, Projectile.Center);
			}

			if (AppearTimer > 5)
			{
				Dust dust = Dust.NewDustDirect(new Vector2(Projectile.position.X - Projectile.velocity.X * 2f + 2f, Projectile.position.Y + 2f - Projectile.velocity.Y * 2f), 8, 8, 107, Projectile.oldVelocity.X, Projectile.oldVelocity.Y * 0.5f, 100, default(Color), 1f);
				dust.velocity *= -0.25f;

				dust = Dust.NewDustDirect(new Vector2(Projectile.position.X - Projectile.velocity.X * 2f + 2f, Projectile.position.Y + 2f - Projectile.velocity.Y * 2f), 8, 8, 107, Projectile.oldVelocity.X, Projectile.oldVelocity.Y * 0.5f, 100, default(Color), 1f);
				dust.velocity *= -0.25f;
				dust.position -= Projectile.velocity * 0.25f;
			}

			if (AppearTimer < AppearTimerMax)
			{
				AppearTimer++;
			}
			else
			{
				if (PulseTimer == 0)
				{
					Projectile.scale -= 0.02f;
					Projectile.alpha += 30;
					if (Projectile.alpha >= 250)
					{
						Projectile.alpha = 255;
						PulseTimer = 1;
					}
				}
				else if (PulseTimer == 1)
				{
					Projectile.scale += 0.02f;
					Projectile.alpha -= 30;
					if (Projectile.alpha <= 0)
					{
						Projectile.alpha = 0;
						PulseTimer = 0;
					}
				}
			}

			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;

			if (Projectile.velocity.Y > 16f)
			{
				Projectile.velocity.Y = 16f;
			}
		}
	}
}
