using AssortedCrazyThings.Base;
using AssortedCrazyThings.Base.ModSupport.AoMM;
using AssortedCrazyThings.Buffs.Pets;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Pets
{
	[Content(ContentType.HostileNPCs | ContentType.DroppedPets)]
	public class MiniMegalodonProj : SimplePetProjBase
	{
		public override void SetStaticDefaults()
		{
			Main.projFrames[Projectile.type] = 10;
			Main.projPet[Projectile.type] = true;

			ProjectileID.Sets.CharacterPreviewAnimations[Projectile.type] = ProjectileID.Sets.SimpleLoop(0, 6, 5)
				.WhenNotSelected(0, 0)
				.WithOffset(-16f, 0f)
				.WithSpriteDirection(-1);

			AmuletOfManyMinionsApi.RegisterSlimePet(this, ModContent.GetInstance<MiniMegalodonBuff_AoMM>(), null);
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.EyeSpring);
			Projectile.aiStyle = -1;
			Projectile.width = 48;
			Projectile.height = 24;
			DrawOffsetX = -4;
		}

		public override void AI()
		{
			Player player = Projectile.GetOwner();
			PetPlayer modPlayer = player.GetModPlayer<PetPlayer>();
			if (player.dead)
			{
				modPlayer.MiniMegalodon = false;
			}
			if (modPlayer.MiniMegalodon)
			{
				Projectile.timeLeft = 2;
			}

			AssAI.EyeSpringAI(Projectile, out bool left, out bool right, out bool jumped, flyForever: false);

			if (AmuletOfManyMinionsApi.IsActive(this) && AmuletOfManyMinionsApi.IsAttacking(this))
			{
				//Otherwise it's inverted
				Projectile.spriteDirection = Projectile.direction = (Projectile.velocity.X < 0).ToDirectionInt();
			}

			//AssAI.EyeSpringDraw(Projectile, left, right);
			EyeSpringDraw_Custom(Projectile, left, right, jumped);
		}

		public static void EyeSpringDraw_Custom(Projectile projectile, bool left, bool right, bool jumped)
		{
			if (jumped)
			{
				//Jump detection from AI code
				projectile.frame = 1;
				projectile.frameCounter = 420; //Magic number
			}

			//0 idle/walkstart
			//1 prepare jump
			//2 jumping
			//3 midair
			//4 falling
			//5 landing
			//6-9 flying

			//Timings need to match AI - the landing sequence needs projectile.frameCounter to end up at 12 for the next AI action to start (jump)
			//2 distinct sequences:
			//(1) When jump is initiated, set frame to 1 and then go to 2 and 3 smoothly, stay in 3 until velocity changes to below 0
			//When velocity changes to above 0, set frame to 4
			//(2) When ground is touched, set frame to 5, and smoothly to 0

			if (projectile.ai[0] != 0f)
			{
				//Flying
				projectile.frameCounter++;
				if (projectile.frameCounter > 4)
				{
					projectile.frame++;
					projectile.frameCounter = 0;
				}
				if (projectile.frame < 6 || projectile.frame > 9)
				{
					projectile.frame = 6;
				}

				//Rotate the direction it's moving, head forward
				projectile.rotation = projectile.velocity.ToRotation();
				if (projectile.spriteDirection == 1)
				{
					projectile.rotation += MathHelper.Pi;
				}
			}
			else
			{
				//Grounded
				if (projectile.velocity.Y == 0.4f)
				{
					if (projectile.frame >= 6)
					{
						//Flying frames
						projectile.frameCounter = 0;
					}

					//Landing
					bool walking = projectile.velocity.X != 0f;
					projectile.frameCounter++;
					int speed = 3;
					if (projectile.frameCounter < speed)
					{
						projectile.frame = 4;
					}
					else if (projectile.frameCounter < speed * 2)
					{
						projectile.frame = 5;
					}
					else if (projectile.frameCounter < speed * 4)
					{
						projectile.frame = 0;
					}

					if (projectile.frameCounter >= speed * 4)
					{
						if (!(walking && (left | right)))
						{
							projectile.frameCounter = speed * 4;
						}
					}
				}
				else if (projectile.velocity.Y < 0)
				{
					if (projectile.frame > 4)
					{
						//If frame somehow after jump, set to jump
						projectile.frame = 1;
					}
					if (projectile.frameCounter >= 12 + 1)
					{
						projectile.frameCounter = 0;
					}

					//Jumping
					projectile.frameCounter++;
					int numFrames = 3;
					int speed = 4;
					if (projectile.frameCounter >= speed)
					{
						if (projectile.frameCounter % speed == 0 & projectile.frameCounter <= speed * numFrames)
						{
							//Ends at 1 + numFrames when frameCounter is 12
							projectile.frame++;
						}
						else if (projectile.frameCounter > speed * numFrames)
						{
							//Keep frameCounter at 12
							projectile.frameCounter = speed * numFrames;
						}
					}
				}
				else
				{
					//Falling
					projectile.frame = 4;
					projectile.frameCounter = 1;
				}
			}
		}
	}
}
