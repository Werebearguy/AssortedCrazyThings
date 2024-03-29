using AssortedCrazyThings.Base;
using AssortedCrazyThings.Base.ModSupport.AoMM;
using AssortedCrazyThings.Buffs.Pets;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Pets
{
	[Content(ContentType.DroppedPets)]
	public class GobletProj : SimplePetProjBase
	{
		private int frame2Counter = 0;
		private int frame2 = 0;

		public override void SetStaticDefaults()
		{
			Main.projFrames[Projectile.type] = 14;
			Main.projPet[Projectile.type] = true;

			ProjectileID.Sets.CharacterPreviewAnimations[Projectile.type] = ProjectileID.Sets.SimpleLoop(2, 10 - 2, 5)
				.WhenNotSelected(0, 0)
				.WithOffset(-6f, 0f)
				.WithSpriteDirection(-1);

			AmuletOfManyMinionsApi.RegisterGroundedPet(this, ModContent.GetInstance<GobletBuff_AoMM>(), null);
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.BabyGrinch);
			AIType = ProjectileID.BabyGrinch;
			Projectile.width = 24; //40 for flying
			Projectile.height = 38;
			DrawOriginOffsetY = 2;
		}

		public override bool PreAI()
		{
			Player player = Projectile.GetOwner();
			player.grinch = false; // Relic from AIType

			GetFrame();

			return true;
		}

		public bool InAir => Projectile.ai[0] != 0f;

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
						if (frame2Counter > 16) //6
						{
							frame2++;
							frame2Counter = 0;
						}
						if (frame2 < 2 || frame2 > 9) //frame 2 to 9 is running
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
				//projectile.velocity.Y += 0.4f;
				//if (projectile.velocity.Y > 10f)
				//{
				//    projectile.velocity.Y = 10f;
				//}
			}
			else //flying
			{
				if (Projectile.velocity.X <= 0) Projectile.direction = -1;
				else Projectile.direction = 1;
				frame2Counter++;
				if (Projectile.velocity.Length() > 3.6f) Projectile.velocity *= 0.97f;
				if (frame2Counter > 4)
				{
					frame2++;
					frame2Counter = 0;
				}
				if (frame2 < 10 || frame2 > 13)
				{
					frame2 = 10;
				}
				Projectile.rotation = Projectile.velocity.X * 0.01f;
			}
		}

		public override void AI()
		{
			Player player = Projectile.GetOwner();
			PetPlayer modPlayer = player.GetModPlayer<PetPlayer>();
			if (player.dead)
			{
				modPlayer.Goblet = false;
			}
			if (modPlayer.Goblet)
			{
				Projectile.timeLeft = 2;
			}
		}

		public override void PostAI()
		{
			Projectile.frame = frame2;
		}
	}
}
