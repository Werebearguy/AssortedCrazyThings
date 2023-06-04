using AssortedCrazyThings.Base;
using AssortedCrazyThings.Base.ModSupport.AoMM;
using AssortedCrazyThings.Buffs.Pets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Pets
{
	[Content(ContentType.DroppedPets)]
	public class CuteLamiaPetProj : SimplePetProjBase
	{
		private int frame2Counter = 0;
		private int frame2 = 0;
		public bool InAir => Projectile.ai[0] != 0f;

		public override string Texture
		{
			get
			{
				return "AssortedCrazyThings/Projectiles/Pets/CuteLamiaPetProj_0";
			}
		}

		public override void SetStaticDefaults()
		{
			Main.projFrames[Projectile.type] = 12;
			Main.projPet[Projectile.type] = true;

			ProjectileID.Sets.CharacterPreviewAnimations[Projectile.type] = ProjectileID.Sets.SimpleLoop(3, 8-3, 6)
				.WhenNotSelected(0, 0)
				.WithOffset(-6, -1)
				.WithSpriteDirection(-1);

			AmuletOfManyMinionsApi.RegisterGroundedPet(this, ModContent.GetInstance<CuteLamiaPetBuff_AoMM>(), ModContent.ProjectileType<CuteLamiaPetShotProj>());
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.BabyGrinch);
			AIType = ProjectileID.BabyGrinch;
			Projectile.width = 28;
			Projectile.height = 40;

			DrawOriginOffsetY = -6;
			DrawOriginOffsetX = 4;
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
				modPlayer.CuteLamiaPet = false;
			}
			if (modPlayer.CuteLamiaPet)
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
			}

			if (AmuletOfManyMinionsApi.IsActive(this) &&
				AmuletOfManyMinionsApi.TryGetStateDirect(this, out var state) && state.IsInFiringRange &&
				state.TargetNPC is NPC targetNPC)
			{
				Projectile.spriteDirection = Projectile.direction = ((targetNPC.Center.X - Projectile.Center.X) < 0).ToDirectionInt();
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			PetPlayer mPlayer = Projectile.GetOwner().GetModPlayer<PetPlayer>();
			SpriteEffects effects = Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			Texture2D image = Mod.Assets.Request<Texture2D>("Projectiles/Pets/CuteLamiaPetProj_" + mPlayer.cuteLamiaPetType).Value;
			Rectangle bounds = image.Frame(1, Main.projFrames[Projectile.type], frameY: Projectile.frame);

			float yOffset = -10f;
			Vector2 stupidOffset = new Vector2(bounds.Width / 2, Projectile.height / 2 + Projectile.gfxOffY + 5f + yOffset);

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
						if (frame2 < 3 || frame2 > 7) //frame 3 to 7 is running
						{
							frame2 = 3;
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
					frame2 = Projectile.velocity.Y > 0f ? 1 : 2;//frame 1 is jumping, frame 2 is falling
				}
			}
			else //flying
			{
				frame2Counter++;
				float speed = Projectile.velocity.LengthSquared() > 3f * 3f ? 4 : 6;
				if (frame2Counter > speed) //6
				{
					frame2++;
					frame2Counter = 0;
				}

				if (frame2 < 8 || frame2 > 11)
				{
					frame2 = 8; //flying frames 8 to 11
				}
			}
		}
	}

	[Content(ContentType.AommSupport | ContentType.DroppedPets)]
	public class CuteLamiaPetShotProj : MinionShotProj_AoMM
	{
		public override int ClonedType => ProjectileID.AmethystBolt;

		public override SoundStyle? SpawnSound => SoundID.Item8;
	}
}
