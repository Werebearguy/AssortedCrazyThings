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
	public class SuspiciousNuggetProj : SimplePetProjBase
	{
		private int frame2Counter = 0;
		private int frame2 = 0;
		private float rot2 = 0;

		public override void SetStaticDefaults()
		{
			Main.projFrames[Projectile.type] = 8;
			Main.projPet[Projectile.type] = true;

			ProjectileID.Sets.CharacterPreviewAnimations[Projectile.type] = ProjectileID.Sets.SimpleLoop(1, 7 - 1, 4)
				.WhenNotSelected(0, 0)
				.WithOffset(-2f, 0f)
				.WithSpriteDirection(-1);

			AmuletOfManyMinionsApi.RegisterGroundedPet(this, ModContent.GetInstance<SuspiciousNuggetBuff_AoMM>(), null);
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.BabyGrinch);
			Projectile.width = 22;
			Projectile.height = 22;
			AIType = ProjectileID.BabyRedPanda;
			DrawOriginOffsetY = -4;
		}

		public override bool PreAI()
		{
			Player player = Projectile.GetOwner();
			player.petFlagBabyRedPanda = false; // Relic from AIType

			GetFrame();

			return true;
		}

		public bool InAir => Projectile.ai[0] != 0f;

		private void GetFrame()
		{
			if (!InAir) //not flying
			{
				rot2 = 0;
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
						if (frame2Counter > 12) //6
						{
							frame2++;
							frame2Counter = 0;
						}
						if (frame2 > 6) //frame 1 to 6 is running
						{
							frame2 = 1;
						}
					}
					else
					{
						frame2 = 0; //frame 0 is idle
						frame2Counter = 6;
					}
				}
				else if (Projectile.velocity.Y != 0f)
				{
					frame2Counter = 0;
					frame2 = 4; //frame 4 is jumping
				}
			}
			else //flying
			{
				rot2 += -Projectile.spriteDirection * 0.1f;
				frame2Counter = 0;
				frame2 = 7; //frame 7 is flying
			}
		}

		public override void AI()
		{
			Player player = Projectile.GetOwner();
			PetPlayer modPlayer = player.GetModPlayer<PetPlayer>();
			if (player.dead)
			{
				modPlayer.SuspiciousNugget = false;
			}
			if (modPlayer.SuspiciousNugget)
			{
				Projectile.timeLeft = 2;
			}
		}

		private bool drawWeapon = false;
		private int weaponFrame = 0;
		private const int weaponFrameCount = 3;
		private int weaponFrameCounter = 0;

		public override void PostAI()
		{
			Projectile.frame = frame2;
			Projectile.rotation = rot2;

			if (AmuletOfManyMinionsApi.IsActive(this))
			{
				AoMM_AI();
			}
		}

		private void AoMM_AI()
		{
			if (!AmuletOfManyMinionsApi.IsAttacking(this))
			{
				drawWeapon = false;
				weaponFrameCounter = 0;
				weaponFrame = 0;
				return;
			}

			drawWeapon = true;

			bool overlappingWithAnyEnemy = false;
			for (int i = 0; i < Main.maxNPCs; i++)
			{
				NPC npc = Main.npc[i];

				var hitbox = npc.Hitbox;
				hitbox.Inflate(6, 6);
				if (npc.CanBeChasedBy() && hitbox.Intersects(Projectile.Hitbox))
				{
					overlappingWithAnyEnemy = true;
					break;
				}
			}

			if (overlappingWithAnyEnemy || weaponFrameCounter > 0 || weaponFrame > 0)
			{
				weaponFrameCounter++;
				if (weaponFrameCounter >= 6)
				{
					weaponFrameCounter = 0;
					weaponFrame++;
					if (weaponFrame >= weaponFrameCount)
					{
						weaponFrame = 0;
					}
				}
			}
			else
			{
				weaponFrameCounter = 0;
				weaponFrame = 0;
			}
		}

		public override void PostDraw(Color lightColor)
		{
			if (!drawWeapon ||
				!AmuletOfManyMinionsApi.IsActive(this) ||
				!AmuletOfManyMinionsApi.IsAttacking(this))
			{
				return;
			}

			SpriteEffects effects = Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

			Texture2D image = ModContent.Request<Texture2D>(Texture + "_Weapon").Value;
			Rectangle bounds = image.Frame(1, weaponFrameCount, frameY: weaponFrame);

			int xOffset = Projectile.spriteDirection == -1 ? 2 : 4;
			int yOffset = -12;
			Vector2 stupidOffset = new Vector2(Projectile.width / 2 + xOffset, bounds.Height / 2 + yOffset + Projectile.gfxOffY);

			Vector2 drawPos = Projectile.position - Main.screenPosition + stupidOffset;
			Vector2 origin = bounds.Size() / 2;

			Main.EntitySpriteDraw(image, drawPos, bounds, lightColor, Projectile.rotation, origin, Projectile.scale, effects, 0);
		}
	}
}
