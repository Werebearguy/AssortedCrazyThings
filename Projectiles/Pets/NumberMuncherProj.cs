using AssortedCrazyThings.Base;
using AssortedCrazyThings.Base.ModSupport.AoMM;
using AssortedCrazyThings.Buffs.Pets;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Pets
{
	public class NumberMuncherProj : SimplePetProjBase
	{
		private int frame2Counter = 0;
		private int frame2 = 0;
		public bool InAir => Projectile.ai[0] != 0f;

		public override void SetStaticDefaults()
		{
			Main.projFrames[Projectile.type] = 10;
			Main.projPet[Projectile.type] = true;

			ProjectileID.Sets.CharacterPreviewAnimations[Projectile.type] = ProjectileID.Sets.SimpleLoop(2, 10 - 2, 6)
				.WhenNotSelected(0, 0)
				.WithOffset(2f, 0f)
				.WithSpriteDirection(-1);

			AmuletOfManyMinionsApi.RegisterGroundedPet(this, ModContent.GetInstance<NumberMuncherBuff_AoMM>(), ModContent.ProjectileType<NumberMuncherShotProj>());
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

			if (AmuletOfManyMinionsApi.IsActive(this) &&
				AmuletOfManyMinionsApi.TryGetStateDirect(this, out var state) && state.IsInFiringRange &&
				state.TargetNPC is NPC targetNPC)
			{
				Projectile.spriteDirection = Projectile.direction = ((targetNPC.Center.X - Projectile.Center.X) < 0).ToDirectionInt();
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

	public class NumberMuncherShotProj : MinionShotProj_AoMM
	{
		public override int ClonedType => ProjectileID.Bullet;

		public override void SafeSetDefaults()
		{
			Projectile.hide = true;
		}

		int textWhoAmI = -1;

		public string GenerateRandomText()
		{
			int mode = Main.rand.Next(9);
			switch (mode)
			{
				case 0:
				case 1:
				case 2:
					{
						//Addition
						int result = Main.rand.Next(1, 101);
						int first = result - Main.rand.Next(1, result + 1);
						return $"{first} + {result - first} = {result}";
					}
				case 3:
				case 4:
				case 5:
					{
						//Subtraction
						int first = Main.rand.Next(1, 101);
						int second = Main.rand.Next(1, first + 1);
						return $"{first} - {second} = {first - second}";
					}
				case 6:
				case 7:
					{
						//Multiplication
						int first = Main.rand.Next(1, 51);
						int second = Main.rand.Next(1, 101) / first;
						return $"{first} x {second} = {first * second}";
					}
				case 8:
					{
						//Division
						int first = Main.rand.Next(2, 101);
						int second = first / Main.rand.Next(1, first + 1);
						return $"{first} / {second} = {first / second}";
					}
				default:
					return "ERROR";
			}
		}

		public override void SafeAI()
		{
			if (Main.netMode == NetmodeID.Server)
			{
				return;
			}

			if (textWhoAmI == -1)
			{
				Projectile.velocity *= 0.6f;
				textWhoAmI = PopupText.NewText(new AdvancedPopupRequest
				{
					Text = GenerateRandomText(),
					Color = Color.White,
					DurationInFrames = 10,
					Velocity = Vector2.Zero
				},
				Projectile.Center);
			}

			if (textWhoAmI <= -1 || textWhoAmI >= Main.maxItemText || Main.popupText[textWhoAmI] is not PopupText popupText)
			{
				if (Main.myPlayer == Projectile.owner)
				{
					Projectile.Kill();
				}
				return;
			}

			Projectile.ai[1]++;
			popupText.lifeTime = 60;
			popupText.alpha = 255;
			popupText.scale = Math.Min(1f, Projectile.ai[1] / 10f);
			Vector2 value = FontAssets.MouseText.Value.MeasureString(popupText.name);
			popupText.position = Projectile.Center - value / 2f;
		}

		public override void Kill(int timeLeft)
		{
			if (textWhoAmI <= -1 || textWhoAmI >= Main.maxItemText || Main.popupText[textWhoAmI] is not PopupText popupText)
			{
				return;
			}

			popupText.lifeTime = 60;
			popupText.velocity = Projectile.velocity; //Keep going after disconnecting
		}
	}
}
