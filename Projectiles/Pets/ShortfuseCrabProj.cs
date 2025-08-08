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
	[Content(ContentType.FriendlyNPCs)]
	public class ShortfuseCrabProj : SimplePetProjBase
	{
		private int frame2Counter = 0;
		private int frame2 = 0;
		public bool InAir => Projectile.ai[0] != 0f;

		public override string Texture
		{
			get
			{
				return "AssortedCrazyThings/Projectiles/Pets/ShortfuseCrabProj_0";
			}
		}

		public override void SetStaticDefaults()
		{
			Main.projFrames[Projectile.type] = 8;
			Main.projPet[Projectile.type] = true;

			ProjectileID.Sets.CharacterPreviewAnimations[Projectile.type] = ProjectileID.Sets.SimpleLoop(2, 8 - 2, 6)
				.WhenNotSelected(0, 0)
				.WithOffset(-6, -1)
				.WithSpriteDirection(-1);

			AmuletOfManyMinionsApi.RegisterGroundedPet(this, ModContent.GetInstance<ShortfuseCrabBuff_AoMM>(), null);
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.BabyGrinch);
			AIType = ProjectileID.BabyGrinch;
			Projectile.width = 28;
			Projectile.height = 38;

			DrawOriginOffsetY = 0;
			DrawOriginOffsetX = 0;
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
				modPlayer.ShortfuseCrab = false;
			}
			if (modPlayer.ShortfuseCrab)
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
				//Tilt towards direction
				Projectile.rotation = Projectile.velocity.X * 0.05f;

				//Top center, take rotation into account
				int xOff = 0;
				if (Projectile.spriteDirection == -1)
				{
					xOff = 6;
				}
				Vector2 dustPos = Projectile.Center + new Vector2(xOff, 0) + new Vector2(0, -Projectile.height / 2 + 2).RotatedBy(Projectile.rotation);

				if (frame2Counter % 2 == 0)
				{
					Dust dust = Dust.NewDustPerfect(dustPos, DustID.Torch, new Vector2(Projectile.velocity.X, Projectile.velocity.Y) * 0.5f, 50, Color.White, 1.4f);
					dust.velocity.X *= 0.2f;
					dust.velocity.Y *= 0.2f;
					dust.velocity += Main.rand.NextVector2CircularEdge(1f, 1f);
					dust.velocity.Y -= 0.2f;
					dust.noGravity = true;
					dust.noLightEmittence = true;
				}
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			PetPlayer mPlayer = Projectile.GetOwner().GetModPlayer<PetPlayer>();
			SpriteEffects effects = Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			Texture2D image = Mod.Assets.Request<Texture2D>("Projectiles/Pets/ShortfuseCrabProj_" + mPlayer.shortfuseCrabType).Value;
			Rectangle bounds = image.Frame(1, Main.projFrames[Projectile.type], frameY: Projectile.frame);

			float yOffset = -10f;
			Vector2 stupidOffset = new Vector2(bounds.Width / 2, Projectile.height / 2 + Projectile.gfxOffY + 4f + yOffset);

			Vector2 origin = bounds.Size() / 2 - new Vector2(DrawOriginOffsetX, DrawOriginOffsetY - yOffset);
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
						if (frame2Counter > 14) //6
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
					frame2 = Projectile.velocity.Y > 0f ? 1 : 7;//frame 1 is jumping, frame 7 is falling
				}
			}
			else //flying
			{
				frame2Counter++;
				frame2 = 7;
			}
		}
	}
}
