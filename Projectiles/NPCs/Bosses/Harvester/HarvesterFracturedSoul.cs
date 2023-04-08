using AssortedCrazyThings.Base;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace AssortedCrazyThings.Projectiles.NPCs.Bosses.Harvester
{
	[Content(ContentType.Bosses)]
	public class HarvesterFracturedSoul : AssProjectile
	{
		public float MaxSpeed => Projectile.ai[0];

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fractured Soul");

			Main.projFrames[Projectile.type] = 4;
		}

		public override void SetDefaults()
		{
			Projectile.aiStyle = -1;
			Projectile.alpha = 255;
			Projectile.hostile = true;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 240;

			Projectile.width = 16;
			Projectile.height = 16;
			DrawOriginOffsetY = -12;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return new Color(255, 255, 255, (int)Utils.WrappedLerp(0f, 255f, (Projectile.timeLeft % 40) / 40f)) * Projectile.Opacity;
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			width = 10;
			height = 10;

			return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
		}

		public override void AI()
		{
			Movement();

			Animation();

			Visuals();
		}

		private void Movement()
		{
			int plr = Player.FindClosest(Projectile.Center, 1, 1);
			Player target = Main.player[plr];

			if (!Projectile.tileCollide && target.Center.Y < Projectile.Bottom.Y)
			{
				//similar to some weapons with things dropping from the sky
				Projectile.tileCollide = true;
			}

			Projectile.spriteDirection = Projectile.direction = (Projectile.velocity.X > 0).ToDirectionInt();

			Projectile.ai[1] += 1f;
			if (Projectile.ai[1] > 30f && Projectile.ai[1] < 110f)
			{
				float speed = Projectile.velocity.Length();
				Vector2 toPlayer = target.Center - Projectile.Center;
				toPlayer.Normalize();
				toPlayer *= speed;
				Projectile.velocity = (Projectile.velocity * 24f + toPlayer) / 25f;
				Projectile.velocity.Normalize();
				Projectile.velocity *= speed;
			}

			float maxSpeed = MaxSpeed;
			if (maxSpeed <= 0)
			{
				maxSpeed = 18;
			}
			if (Projectile.velocity.Length() < maxSpeed)
			{
				Projectile.velocity *= 1.02f;
			}
		}

		private void Visuals()
		{
			int y = DrawOriginOffsetY;
			Dust dust = Dust.NewDustDirect(Projectile.position + new Vector2(0, y), Projectile.width, Projectile.height - y, 135, 0f, 0f, 0, default(Color), 1.5f);
			dust.noGravity = true;
			dust.noLight = true;
			dust.velocity *= 0.65f;
			if (Main.rand.NextBool(5))
			{
				dust.fadeIn = 0.6f;
			}

			if (Projectile.localAI[0] == 0f)
			{
				Projectile.localAI[0] = 1f;
				SoundEngine.PlaySound(SoundID.Item8, Projectile.Center);

				for (int i = 0; i < 10; i++)
				{
					int d = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 135, Projectile.velocity.X, Projectile.velocity.Y, 0, default(Color), 2f);
					dust = Main.dust[d];
					dust.noGravity = true;
					dust.velocity = Projectile.Center - dust.position;
					dust.velocity.Normalize();
					dust.velocity *= -5f;
					dust.velocity += Projectile.velocity / 2f;
				}
			}
		}

		private void Animation()
		{
			if (Projectile.alpha > 0)
			{
				Projectile.alpha -= 30;

				if (Projectile.alpha < 0)
				{
					Projectile.alpha = 0;
				}
			}

			Projectile.LoopAnimation(4);
		}

		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);
			for (int i = 0; i < 20; i++)
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
