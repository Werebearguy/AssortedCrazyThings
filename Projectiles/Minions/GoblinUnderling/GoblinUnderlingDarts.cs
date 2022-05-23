using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Minions.GoblinUnderling
{
	[Content(ContentType.Weapons)]
	public abstract class GoblinUnderlingDart : AssProjectile
	{
		public const float Gravity = 0.1f;
		public const int TicksWithoutGravity = 15;

		public bool Spawned
		{
			get => Projectile.localAI[0] == 1f;
			set => Projectile.localAI[0] = value ? 1 : 0;
		}

		public int Timer
		{
			get => (int)Projectile.ai[0];
			set => Projectile.ai[0] = value;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Goblin Underling Dart");
			Main.projFrames[Projectile.type] = 1;
			ProjectileID.Sets.MinionShot[Projectile.type] = true;
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.WoodenArrowFriendly);
			Projectile.aiStyle = -1;
			Projectile.height = 12;
			Projectile.width = 12;
			Projectile.penetrate = 1;
			Projectile.tileCollide = true;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.DamageType = DamageClass.Summon;
			Projectile.timeLeft = 180;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
			return true;
		}

		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			GoblinUnderlingSystem.CommonModifyHitNPC(Projectile, target, ref damage, ref knockback, ref hitDirection);
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			foreach (var proj in GoblinUnderlingSystem.GetLocalGoblinUnderlings())
			{
				if (proj.ModProjectile is GoblinUnderlingProj goblin)
				{
					if (!target.boss && goblin.OutOfCombat())
					{
						GoblinUnderlingSystem.TryCreate(proj, GoblinUnderlingMessageSource.Attacking);
					}

					goblin.SetInCombat();
				}
			}
		}

		public override void AI()
		{
			if (!Spawned)
			{
				Spawned = true;

				SoundEngine.PlaySound(SoundID.Item1, Projectile.Center);
			}

			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

			Timer++;
			if (Timer >= TicksWithoutGravity)
			{
				Projectile.velocity.Y += Gravity;
				if (Projectile.velocity.Y > 16f)
				{
					Projectile.velocity.Y = 16f;
				}
			}

			Projectile.direction = Projectile.spriteDirection = (Projectile.velocity.X <= 0).ToDirectionInt();
		}
	}

	public class GoblinUnderlingDart_0 : GoblinUnderlingDart
	{

	}

	public class GoblinUnderlingDart_1 : GoblinUnderlingDart
	{

	}

	public class GoblinUnderlingDart_2 : GoblinUnderlingDart
	{

	}

	public class GoblinUnderlingDart_3 : GoblinUnderlingDart
	{

	}

	public class GoblinUnderlingDart_4 : GoblinUnderlingDart
	{

	}
}
