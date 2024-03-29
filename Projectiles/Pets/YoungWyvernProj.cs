using AssortedCrazyThings.Base;
using AssortedCrazyThings.Base.ModSupport.AoMM;
using AssortedCrazyThings.Buffs.Pets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Pets
{
	public class YoungWyvernProj : SimplePetProjBase
	{
		public int frame = 0;
		public int frameCounter = 0;

		public bool InAir => Projectile.ai[0] != 0f;

		public override string Texture
		{
			get
			{
				return "AssortedCrazyThings/Projectiles/Pets/YoungWyvernProj_0"; //temp
			}
		}

		public override void SetStaticDefaults()
		{
			Main.projFrames[Projectile.type] = 12;
			Main.projPet[Projectile.type] = true;

			ProjectileID.Sets.CharacterPreviewAnimations[Projectile.type] = ProjectileID.Sets.SimpleLoop(2, 8 - 2, 5)
				.WhenNotSelected(0, 0)
				.WithOffset(-10f, 0f)
				.WithSpriteDirection(-1);

			AmuletOfManyMinionsApi.RegisterGroundedPet(this, ModContent.GetInstance<YoungWyvernBuff_AoMM>(), null);
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.BlackCat);
			DrawOffsetX = -6;
			Projectile.width = 44;
			Projectile.height = 30;
			AIType = ProjectileID.BlackCat;
		}

		public override bool PreAI()
		{
			Player player = Projectile.GetOwner();
			player.blackCat = false; // Relic from AIType
			return true;
		}

		public override void AI()
		{
			Player player = Projectile.GetOwner();
			PetPlayer modPlayer = player.GetModPlayer<PetPlayer>();
			if (player.dead)
			{
				modPlayer.YoungWyvern = false;
			}
			if (modPlayer.YoungWyvern)
			{
				Projectile.timeLeft = 2;
			}

			//BLACK CAT
			//bool flag11 = Projectile.position.X - Projectile.oldPosition.X == 0f;
			//if (InAir)
			//{
			//    rotation = base.Projectile.velocity.X * 0.05f;
			//    frameCounter++;
			//    if (frameCounter >= 6)
			//    {
			//        frame++;
			//        frameCounter = 0;
			//    }

			//    if (frame > 10)
			//        frame = 6;

			//    if (frame < 6)
			//        frame = 6;
			//}
			//else
			//{
			//    if (base.Projectile.velocity.Y >= 0f && (double)base.Projectile.velocity.Y <= 0.8)
			//    {
			//        if (flag11)
			//        {
			//            frame = 0;
			//            frameCounter = 0;
			//        }
			//        else if ((double)base.Projectile.velocity.X < -0.8 || (double)base.Projectile.velocity.X > 0.8)
			//        {
			//            frameCounter += (int)Math.Abs(base.Projectile.velocity.X);
			//            frameCounter++;
			//            if (frameCounter > 8)
			//            {
			//                frame++;
			//                frameCounter = 0;
			//            }

			//            if (frame > 5)
			//                frame = 2;
			//        }
			//        else
			//        {
			//            frame = 0;
			//            frameCounter = 0;
			//        }
			//    }
			//    else
			//    {
			//        frameCounter = 0;
			//        frame = 1;
			//    }
			//}
		}

		public override void PostAI()
		{
			//Black cat AI values, so we have to check those
			if (!InAir)
			{
				//DESERT TIGER
				int lastFrame = 8; //frame #8 onwards is flying

				//if (fancy desert tigers)
				//    lastFrame = 10;

				//Projectile.rotation = 0f;
				if (Projectile.velocity.Y >= 0f && Projectile.velocity.Y <= 0.8f)
				{
					if (Projectile.position.X - Projectile.oldPosition.X == 0f)
					{
						frame = 0;
						frameCounter = 0;
					}
					else if (Math.Abs(Projectile.velocity.X) >= 0.5f)
					{
						frameCounter += (int)Math.Abs(Projectile.velocity.X);
						frameCounter++;
						if (frameCounter > 10)
						{
							frame++;
							frameCounter = 0;
						}

						if (frame >= lastFrame || frame < 2)
						{
							frame = 2;
						}
					}
					else
					{
						frame = 0;
						frameCounter = 0;
					}
				}
				else if (Projectile.velocity.Y != 0f)
				{
					frameCounter = 0;
					frame = 1;
					//if (fancy desert tigers)
					//    frame = 9;
				}
			}
			else
			{
				Projectile.spriteDirection = (Projectile.velocity.X <= 0f).ToDirectionInt();

				AssExtensions.LoopAnimationInt(ref frame, ref frameCounter, 6, 8, Main.projFrames[Projectile.type] - 1);
			}

			Projectile.frame = frame;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			PetPlayer mPlayer = Projectile.GetOwner().GetModPlayer<PetPlayer>();
			SpriteEffects effects = Projectile.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			Texture2D image = Mod.Assets.Request<Texture2D>("Projectiles/Pets/YoungWyvernProj_" + mPlayer.youngWyvernType).Value;
			Rectangle bounds = image.Frame(1, Main.projFrames[Projectile.type], frameY: Projectile.frame);

			Vector2 stupidOffset = new Vector2(Projectile.width / 2 + DrawOffsetX, Projectile.height / 2 + Projectile.gfxOffY + 4f);

			Main.EntitySpriteDraw(image, Projectile.position - Main.screenPosition + stupidOffset, bounds, lightColor, Projectile.rotation, bounds.Size() / 2, Projectile.scale, effects, 0);

			return false;
		}
	}
}
