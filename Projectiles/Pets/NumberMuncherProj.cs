using AssortedCrazyThings.Base;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;

namespace AssortedCrazyThings.Projectiles.Pets
{
	public class NumberMuncherProj : SimplePetProjBase
	{
		private int frame2Counter = 0;
		private int frame2 = 0;
		public bool InAir => Projectile.ai[0] != 0f;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Number Muncher");
			Main.projFrames[Projectile.type] = 10;
			Main.projPet[Projectile.type] = true;
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.BabyGrinch);
			AIType = ProjectileID.BabyGrinch;
			Projectile.width = 20;
			Projectile.height = 22;

			DrawOriginOffsetY = -2;
		}

		public override bool PreAI()
		{
			Player player = Projectile.GetOwner();
			player.grinch = false; // Relic from AIType

			GetFrame();

			return true;
		}

		public override void AI()
		{
			Player player = Projectile.GetOwner();
			PetPlayer modPlayer = player.GetModPlayer<PetPlayer>();
			if (player.dead)
			{
				modPlayer.NumberMuncher = false;
			}
			if (modPlayer.NumberMuncher)
			{
				Projectile.timeLeft = 2;
			}
		}

		public override void PostAI()
		{
			Projectile.frameCounter = 0;
			Projectile.frame = frame2;

			if (InAir)
			{
				//Rotate the direction it's moving, head forward
				Projectile.rotation = Projectile.velocity.ToRotation();
				if (Projectile.spriteDirection == 1)
				{
					Projectile.rotation += MathHelper.Pi;
				}

				Projectile.rotation -= Projectile.spriteDirection * MathHelper.PiOver2;

				//Propulsion dust
				float dustChance = Math.Clamp(Math.Abs(Projectile.velocity.Length()) / 5f, 0.3f, 0.9f);
				if (Main.rand.NextFloat() < dustChance)
				{
					Vector2 dustOrigin = Projectile.Center - Projectile.velocity.SafeNormalize(Vector2.Zero) * 12;
					Dust dust = Dust.NewDustDirect(dustOrigin - Vector2.One * 4f, 8, 8, DustID.Cloud, -Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f, 50, default(Color), 1.7f);
					dust.velocity.X *= 0.2f;
					dust.velocity.Y *= 0.2f;
					dust.noGravity = true;
				}
			}
		}

		private void GetFrame()
		{
			if (!InAir) //not flying
			{
				if (Projectile.velocity.Y == 0f)
				{
					float xAbs = Math.Abs(Projectile.velocity.X);
					if (Projectile.velocity.X == 0f)
					{
						frame2 = 0;
						frame2Counter = 0;
					}
					else if (xAbs > 0.5f)
					{
						frame2Counter += (int)xAbs;
						frame2Counter++;
						if (frame2Counter > 20) //6
						{
							frame2++;
							frame2Counter = 0;
						}
						if (frame2 > 9) //frame 2 to 9 is running
						{
							frame2 = 2;
						}
					}
					else
					{
						frame2 = 0; //frame 0 is idle
						frame2Counter = 10;
					}
				}
				else if (Projectile.velocity.Y != 0f)
				{
					frame2Counter = 0;
					frame2 = 1; //frame 1 is jumping
				}
			}
			else //flying
			{
				if (frame2 != 1)
				{
					frame2 = 1; //flying frame 1
				}
			}
		}
	}
}
