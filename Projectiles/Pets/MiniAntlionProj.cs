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
	[Content(ContentType.DroppedPets)]
	public class MiniAntlionProj : SimplePetProjBase
	{
		private int frame2Counter = 0;
		private int frame2 = 0;
		public bool InAir => Projectile.ai[0] != 0f;

		public override string Texture
		{
			get
			{
				return "AssortedCrazyThings/Projectiles/Pets/MiniAntlionProj_0";
			}
		}

		public override void SetStaticDefaults()
		{
			Main.projFrames[Projectile.type] = 11;
			Main.projPet[Projectile.type] = true;

			ProjectileID.Sets.CharacterPreviewAnimations[Projectile.type] = ProjectileID.Sets.SimpleLoop(2, 7 - 2, 6)
				.WhenNotSelected(0, 0)
				.WithOffset(-2f, 0f)
				.WithSpriteDirection(-1);

			AmuletOfManyMinionsApi.RegisterGroundedPet(this, ModContent.GetInstance<MiniAntlionBuff_AoMM>(), null);
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.BabyGrinch);
			AIType = ProjectileID.BabyGrinch;
			Projectile.width = 34;
			Projectile.height = 24;

			DrawOriginOffsetY = -6;
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
				modPlayer.MiniAntlion = false;
			}
			if (modPlayer.MiniAntlion)
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
				Projectile.rotation = Projectile.velocity.X * 0.055f;
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			PetPlayer mPlayer = Projectile.GetOwner().GetModPlayer<PetPlayer>();
			SpriteEffects effects = Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			Texture2D image = Mod.Assets.Request<Texture2D>("Projectiles/Pets/MiniAntlionProj_" + mPlayer.miniAntlionType).Value;
			Rectangle bounds = image.Frame(1, Main.projFrames[Projectile.type], frameY: Projectile.frame);

			Vector2 stupidOffset = new Vector2(bounds.Width / 2, Projectile.height / 2 + Projectile.gfxOffY + 5f);

			Vector2 origin = bounds.Size() / 2 - new Vector2(0, DrawOriginOffsetY);
			Main.EntitySpriteDraw(image, Projectile.position - Main.screenPosition + stupidOffset, bounds, lightColor, Projectile.rotation, origin, Projectile.scale, effects, 0);

			return false;
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
						if (frame2Counter > 13) //6
						{
							frame2++;
							frame2Counter = 0;
						}
						if (frame2 < 2 || frame2 > 6) //frame 2 to 6 is running
						{
							frame2 = 2;
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
					frame2 = 1; //frame 1 is jumping
				}
			}
			else //flying
			{
				frame2Counter++;
				if (frame2Counter > 1) //6
				{
					frame2++;
					frame2Counter = 0;
				}

				if (frame2 < 7 || frame2 > 10)
				{
					frame2 = 7; //flying frames 7 to 10
				}
			}
		}
	}
}
