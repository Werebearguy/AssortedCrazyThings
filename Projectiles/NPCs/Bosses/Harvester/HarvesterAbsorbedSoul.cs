using Microsoft.Xna.Framework;
using Terraria;

namespace AssortedCrazyThings.Projectiles.NPCs.Bosses.Harvester
{
	[Content(ContentType.Bosses)]
	public class HarvesterAbsorbedSoul : AssProjectile
	{
		public override string Texture => "AssortedCrazyThings/Empty";

		public int Timer
		{
			get => (int)Projectile.ai[0];
			set => Projectile.ai[0] = value;
		}

		public override void SetDefaults()
		{
			Projectile.width = 8;
			Projectile.height = 8;
			//Projectile.aiStyle = 122;
			Projectile.hostile = true;
			Projectile.alpha = 255;
			Projectile.ignoreWater = true;
			Projectile.timeLeft = 3600;
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
			//Projectile.extraUpdates = 2;
		}

		public override void AI()
		{
			if (!TryGetBabyHarvester(out BabyHarvesterProj babyHarvester))
			{
				Projectile.active = false;
				return;
			}

			Movement(babyHarvester);

			Visuals();
		}

		private static bool TryGetBabyHarvester(out BabyHarvesterProj babyHarvester)
		{
			babyHarvester = null;
			if (!BabyHarvesterHandler.TryFindBabyHarvester(out Projectile proj, out _))
			{
				return false;
			}

			if (!(proj.ModProjectile is BabyHarvesterProj babyHarvester2 && babyHarvester2.HasValidPlayerOwner))
			{
				return false;
			}

			babyHarvester = babyHarvester2;
			return true;
		}

		private void Movement(BabyHarvesterProj babyHarvester)
		{
			Projectile other = babyHarvester.Projectile;
			Vector2 otherCenter = other.Center;
			otherCenter.X += other.direction * 10;
			Vector2 toHarvester = otherCenter - Projectile.Center;

			if (Projectile.localAI[0] == 0f)
			{
				//AssUtils.Print(Main.time + " appeared homing soul");

				Projectile.localAI[0] = 1f;

				for (int i = 0; i < 20; i++)
				{
					int d = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 135, 0f, 0f, 100);
					Dust dust = Main.dust[d];
					dust.noGravity = true;
					dust.velocity *= 1.5f;
					dust.scale = 1.3f;
				}

				//Give it initial velocity bump up
				Projectile.velocity.Y += -4;
			}
			else
			{
				Timer++;
				if (Timer > 8)
				{
					toHarvester = toHarvester.SafeNormalize(Vector2.UnitX * Projectile.direction) * 10;
					float inertia = 12;
					Projectile.velocity = (Projectile.velocity * (inertia - 1) + toHarvester) / inertia;
				}
			}

			var hitbox = other.Hitbox;
			hitbox.Inflate(0, -6);
			if (Projectile.Hitbox.Intersects(hitbox))
			{
				if (Main.myPlayer == Projectile.owner)
				{
					babyHarvester.AddSoulsEaten();
				}

				Projectile.Kill();
			}
		}

		private void Visuals()
		{
			int d = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 135, 0f, 0f, 100);
			Dust dust = Main.dust[d];
			dust.noGravity = true;
			dust.scale = 1.3f;
		}

		public override void Kill(int timeLeft)
		{
			//AssUtils.Print(Main.time + " killed homing soul");

			for (int i = 0; i < 8; i++)
			{
				int d = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 135, 0f, 0f, 100);
				Dust dust = Main.dust[d];
				dust.noGravity = true;
				dust.velocity *= 1.3f;
				dust.scale = 1.3f;
			}
		}
	}
}
