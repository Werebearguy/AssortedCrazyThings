using AssortedCrazyThings.Base;
using AssortedCrazyThings.Base.Handlers.OutOfCombatHandler;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace AssortedCrazyThings.Projectiles.Accessories
{
	//Mostly a copy of HarvesterFracturedSoul but friendly
	[Content(ContentType.Bosses)]
	public class SigilOfTheBeakProj : AssProjectile
	{
		public static readonly int DefaultSpeed = 16;
		public static readonly int DefaultInertia = 32;
		public static readonly int DefaultTimeToHome = 25;
		public float LifeRatio => Projectile.ai[0];

		public int Timer
		{
			get => (int)Projectile.ai[1];
			set => Projectile.ai[1] = value;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fractured Soul");

			Main.projFrames[Projectile.type] = 4;
			OutOfCombatSystem.IgnoredFriendlyProj.Add(Projectile.type);
		}

		public override void SetDefaults()
		{
			Projectile.aiStyle = -1;
			Projectile.alpha = 255;
			Projectile.friendly = true;
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
			int targetIndex = -1;
			float maxDistSQ = float.MaxValue;
			float distSQ;
			for (int j = 0; j < Main.maxNPCs; j++)
			{
				NPC npc = Main.npc[j];
				if (!npc.CanBeChasedBy())
				{
					continue;
				}
				distSQ = Projectile.DistanceSQ(npc.Center);
				if (distSQ < maxDistSQ && (!Projectile.tileCollide || Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, npc.position, npc.width, npc.height)))
				{
					maxDistSQ = distSQ;
					targetIndex = j;
				}
			}

			if (targetIndex > -1)
			{
				NPC target = Main.npc[targetIndex];

				if (!Projectile.tileCollide && !Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height))
				{
					//once out of blocks, collide with them again
					Projectile.tileCollide = true;
				}

				Timer++;
				int timeToHome = (int)(DefaultTimeToHome * Utils.Remap(LifeRatio, 0f, 1f, 0.25f, 1f));
				if (Timer > timeToHome)
				{
					int newTimer = Timer - timeToHome;
					float speed = DefaultSpeed * Math.Min(1, newTimer / 30f);
					Vector2 toTarget = target.Center - Projectile.Center;
					toTarget.Normalize();
					toTarget *= speed;
					float inertia = DefaultInertia * Utils.Remap(LifeRatio, 0f, 1f, 0.25f, 1f);
					if (inertia == 0)
					{
						inertia = DefaultInertia;
					}
					Projectile.velocity = (Projectile.velocity * inertia + toTarget) / (inertia + 1);
				}
			}

			Projectile.spriteDirection = Projectile.direction = (Projectile.velocity.X > 0).ToDirectionInt();
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
				SoundEngine.PlaySound(SoundID.Item8.WithVolumeScale(0.8f), Projectile.Center);

				for (int i = 0; i < 10; i++)
				{
					int d = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 135, Projectile.velocity.X, Projectile.velocity.Y, 0, default(Color), 2f);
					dust = Main.dust[d];
					dust.noGravity = true;
					dust.velocity = Projectile.Center - dust.position;
					dust.velocity.Normalize();
					dust.velocity *= -3f;
					dust.velocity += Projectile.velocity / 2f;
				}
			}
		}

		private void Animation()
		{
			if (Projectile.alpha > 0)
			{
				Projectile.alpha -= 25;

				if (Projectile.alpha < 0)
				{
					Projectile.alpha = 0;
				}
			}

			Projectile.LoopAnimation(4);
		}

		public override void Kill(int timeLeft)
		{
			if (timeLeft <= 0)
			{
				SoundEngine.PlaySound(SoundID.Item8.WithVolumeScale(0.7f).WithPitchOffset(-0.2f), Projectile.Center);
			}
			else
			{
				SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);
			}

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
