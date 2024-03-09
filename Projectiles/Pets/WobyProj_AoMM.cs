using AssortedCrazyThings.Base;
using AssortedCrazyThings.Base.ModSupport.AoMM;
using AssortedCrazyThings.Buffs.Pets;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Pets
{
	[Content(ContentType.AommSupport | ContentType.DroppedPets)]
	public class WobyProj_AoMM : SimplePetProjBase
	{
		private int frame2Counter = 0;
		private int frame2 = 0;
		public bool InAir => Projectile.ai[0] != 0f;

		public override void SetStaticDefaults()
		{
			Main.projFrames[Projectile.type] = 11;
			Main.projPet[Projectile.type] = true;

			ProjectileID.Sets.CharacterPreviewAnimations[Projectile.type] = ProjectileID.Sets.SimpleLoop(4, 11 - 5, 5)
				.WhenNotSelected(0, 4, 6)
				.WithOffset(-6f, -1f)
				.WithSpriteDirection(-1);

			AmuletOfManyMinionsApi.RegisterGroundedPet(this, ModContent.GetInstance<WobyBuff_AoMM>(), null);
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.BabyGrinch);
			AIType = ProjectileID.BabyGrinch;
			//Height and width kept the same as original pet but damage hitbox is adjusted
			Projectile.width = 38;
			Projectile.height = 38;

			DrawOriginOffsetY = -14;
			DrawOffsetX = -20;
		}

		public override void ModifyDamageHitbox(ref Rectangle hitbox)
		{
			int bottom = hitbox.Bottom;
			hitbox.Inflate(6, 4);
			hitbox.Y = bottom - hitbox.Height;
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
				modPlayer.Woby_AoMM = false;
			}
			if (modPlayer.Woby_AoMM)
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
				Projectile.rotation += Projectile.velocity.X * 0.05f;

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
			//Frames: Idle, Idle, Idle, Idle, Walk, Walk, Walk, Walk, Walk, Walk, Jump / Fly
			if (!InAir) //not flying
			{
				if (Projectile.velocity.Y == 0f)
				{
					float xAbs = Math.Abs(Projectile.velocity.X);
					if (Projectile.velocity.X == 0f)
					{
						frame2Counter++;
						if (frame2Counter > 5)
						{
							frame2++;
							frame2Counter = 0;
						}

						if (frame2 > 3)
						{
							frame2 = 0;
						}
					}
					else if (xAbs > 0.5f)
					{
						int increase = Math.Clamp((int)(3 * xAbs), 2, 3);
						frame2Counter += increase;
						if (frame2Counter > 7)
						{
							frame2++;
							frame2Counter = 0;
						}
						if (frame2 < 4 || frame2 > 9) //frame 4 to 9 is running
						{
							frame2 = 4;
						}
					}
					else
					{
						frame2 = 0; //frame 0 is idle
						frame2Counter = 7;
					}
				}
				else if (Projectile.velocity.Y != 0f)
				{
					frame2Counter = 0;
					frame2 = 10;
				}
			}
			else //flying
			{
				frame2Counter = 0;
				frame2 = 10;
			}
		}
	}
}
