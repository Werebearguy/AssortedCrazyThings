using AssortedCrazyThings.Base;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace AssortedCrazyThings.Projectiles.Weapons
{
	[Content(ContentType.Weapons)]
	public class BreathOfSpazmatismProj : AssProjectile
	{
		public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.Flames; //Greyscale image, so can be recolored

		public static readonly AssUtils.FlamethrowerColors colors = new(
			new Color(80, 160, 15, 200),
			new Color(120, 160, 15, 70),
			new Color(80, 160, 15, 100),
			new Color(40, 80, 40, 100)
			);

		public ref float Timer => ref Projectile.localAI[0];

		public override void SetStaticDefaults()
		{
			Main.projFrames[Projectile.type] = Main.projFrames[ProjectileID.Flames];
			//Trails seem to not be drawn tho so useless
			//ProjectileID.Sets.TrailingMode[Projectile.type] = ProjectileID.Sets.TrailingMode[ProjectileID.Flames];
			//ProjectileID.Sets.TrailCacheLength[Projectile.type] = ProjectileID.Sets.TrailCacheLength[ProjectileID.Flames];
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.Flames);
			Projectile.aiStyle = -1;
		}

		public override bool? CanDamage()
		{
			if (Timer >= 54f)
			{
				return false;
			}

			return base.CanDamage();
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			Projectile.damage = (int)(Projectile.damage * 0.85f);

			target.AddBuff(BuffID.CursedInferno, 10 * 60);
		}

		public override void ModifyDamageHitbox(ref Rectangle hitbox)
		{
			int num = (int)Utils.Remap(Timer, 0f, 72f, 10f, 40f);
			hitbox.Inflate(num, num);
		}

		public override void AI()
		{
			//Dust code adjusted slightly
			Timer += 1f;
			int startSlowdown = 60;
			int afterSlowdown = 12;
			int lifetime = startSlowdown + afterSlowdown;
			if (Timer >= lifetime)
			{
				Projectile.Kill();
			}

			if (Timer >= startSlowdown)
			{
				Projectile.velocity *= 0.95f;
			}

			bool unusedFlag = false && Projectile.ai[0] == 1f;
			int blackSmokeDust = 50;
			int fireDust = blackSmokeDust;
			if (unusedFlag)
			{
				blackSmokeDust = 0;
				fireDust = startSlowdown;
			}

			if (Timer < fireDust && Main.rand.NextFloat() < 0.25f)
			{
				//(flag ? 135 : 6);
				short dustType = (short)(unusedFlag ? 135 : 75);
				Dust dust = Dust.NewDustDirect(Projectile.Center + Main.rand.NextVector2Circular(60f, 60f) * Utils.Remap(Timer, 0f, 72f, 0.5f, 1f), 4, 4, dustType, Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 100);
				if (Main.rand.NextBool(4))
				{
					dust.noGravity = true;
					dust.scale *= 2f;
					dust.velocity *= 1.5f;
				}
				else
				{
					dust.velocity *= 0.5f;
				}

				dust.scale *= 1f;
				dust.velocity *= 1.2f;
				dust.velocity += Projectile.velocity * 1f * Utils.Remap(Timer, 0f, startSlowdown * 0.75f, 1f, 0.1f) * Utils.Remap(Timer, 0f, startSlowdown * 0.1f, 0.1f, 1f);
				dust.velocity *= 0.5f;
			}

			if (blackSmokeDust > 0 && Timer >= blackSmokeDust && Main.rand.NextFloat() < 0.5f)
			{
				Vector2 center = Main.player[Projectile.owner].Center;
				Vector2 velocity = (Projectile.Center - center).SafeNormalize(Vector2.Zero).RotatedByRandom(MathHelper.Pi / 16f) * 7f;
				short dustType = 31; //Black "smoke"
				Dust dust = Dust.NewDustDirect(Projectile.Center + Main.rand.NextVector2Circular(50f, 50f) - velocity * 2f, 4, 4, dustType, 0f, 0f, 150, colors.FadeOut);
				dust.noGravity = true;
				dust.velocity = velocity;
				dust.scale *= 1.1f + Main.rand.NextFloat() * 0.2f;
				dust.customData = -0.3f - 0.15f * Main.rand.NextFloat();
			}
		}

		public override void PostAI()
		{
			//Not needed, cursed inferno type weapons work in water
			//if (Projectile.wet && !Projectile.lavaWet)
			//{
			//	Projectile.Kill();
			//}
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (Projectile.velocity != oldVelocity)
			{
				Projectile.velocity = oldVelocity * 0.95f;
				Projectile.position -= Projectile.velocity;
			}

			return false;
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			if (!projHitbox.Intersects(targetHitbox))
			{
				return false;
			}

			return Collision.CanHit(Projectile.Center, 0, 0, targetHitbox.Center.ToVector2(), 0, 0);
		}

		public override bool PreDraw(ref Color lightColor)
		{
			AssUtils.DrawFlamethrowerProjectile(Projectile, colors);
			return false;
		}
	}
}
