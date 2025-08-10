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
	public class KingGuppyProj : SimplePetProjBase
	{
		public override string Texture
		{
			get
			{
				return "AssortedCrazyThings/Projectiles/Pets/KingGuppyProj_0"; //temp
			}
		}

		public override void SetStaticDefaults()
		{
			Main.projFrames[Projectile.type] = 4;
			Main.projPet[Projectile.type] = true;

			ProjectileID.Sets.CharacterPreviewAnimations[Projectile.type] = ProjectileID.Sets.SimpleLoop(0, Main.projFrames[Projectile.type], 6)
				.WithOffset(-6, -16f)
				.WithSpriteDirection(-1)
				.WithCode(DelegateMethods.CharacterPreview.Float);

			AmuletOfManyMinionsApi.RegisterFlyingPet(this, ModContent.GetInstance<KingGuppyBuff_AoMM>(), 0);
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.ZephyrFish);
			Projectile.aiStyle = -1;
		}

		private bool setAoMMParams = false;

		public override bool PreAI()
		{
			//Don't do any aomm movement
			AmuletOfManyMinionsApi.ReleaseControl(this);

			if (AmuletOfManyMinionsApi.TryGetParamsDirect(this, out var paras))
			{
				if (!setAoMMParams)
				{
					setAoMMParams = true;

					paras.AttackFramesScaleFactor *= 3.5f;
					paras.MaxSpeedScaleFactor *= 0.75f;
					paras.InertiaScaleFactor *= 0.6f;

					AmuletOfManyMinionsApi.UpdateParamsDirect(this, paras);
				}

				AmuletOfManyMinionsApi.UpdateParamsDirect(this, paras);
			}

			Projectile.originalDamage = Projectile.originalDamage * 4;

			return true;
		}

		public override void AI()
		{
			Player player = Projectile.GetOwner();
			PetPlayer modPlayer = player.GetModPlayer<PetPlayer>();
			if (player.dead)
			{
				modPlayer.KingGuppy = false;
			}
			if (modPlayer.KingGuppy)
			{
				Projectile.timeLeft = 2;
			}

			if (AmuletOfManyMinionsApi.IsAttacking(this) && AmuletOfManyMinionsApi.TryGetStateDirect(this, out var state)
			   && state.TargetNPC is NPC targetNPC)
			{
				//scan for 8 tiles away from center for suitable location, since breaking LOS cancels attacking
				Vector2 targetPos = targetNPC.Center + targetNPC.velocity * 5f;
				int steps = 8;
				while (steps > 0)
				{
					Vector2 checkPos = targetPos - new Vector2(0, steps * 16);
					if (!Collision.SolidCollision(checkPos - Projectile.Size / 2, Projectile.width, Projectile.height) &&
						Collision.CanHitLine(checkPos, 1, 1, targetNPC.Center, 1, 1))
					{
						targetPos = checkPos;
						break;
					}
					steps--;
				}

				Vector2 vec = Projectile.DirectionTo(targetPos);
				Projectile.rotation = Projectile.velocity.X * 0.05f;

				Projectile.manualDirectionChange = true;
				if (Projectile.velocity.X > 0.5f && Projectile.direction == 1)
				{
					Projectile.direction = -1;
				}
				else if (Projectile.velocity.X < -0.5f && Projectile.direction != 1)
				{
					Projectile.direction = 1;
				}
				Projectile.spriteDirection = Projectile.direction;

				int nearDistSQ = 2 * state.MaxSpeed * state.MaxSpeed;
				if (Projectile.DistanceSQ(targetPos) > nearDistSQ)
				{
					AmuletOfManyMinionsApi.AoMMMovement(Projectile, vec, state.MaxSpeed, state.Inertia);
				}

				Projectile.velocity.Y *= 0.9f;
				if (state.ShouldFireThisFrame && Projectile.owner == Main.myPlayer)
				{
					Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Bottom, new Vector2(0, 1), ModContent.ProjectileType<KingGuppyShotProj>(), Projectile.damage, 0f, Projectile.owner, ai1: state.PetLevel);
				}
			}
			else
			{
				AssAI.ZephyrfishAI(Projectile);
			}

			AssAI.ZephyrfishDraw(Projectile);
			AssAI.TeleportIfTooFar(Projectile, player.MountedCenter);
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			if (AmuletOfManyMinionsApi.IsAttacking(this))
			{
				fallThrough = true;
			}

			return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
		}

		public override bool PreDraw(ref Color lightColor)
		{
			PetPlayer mPlayer = Projectile.GetOwner().GetModPlayer<PetPlayer>();
			SpriteEffects effects = Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			Texture2D image = Mod.Assets.Request<Texture2D>("Projectiles/Pets/KingGuppyProj_" + mPlayer.kingGuppyType).Value;
			Rectangle bounds = image.Frame(1, Main.projFrames[Projectile.type], frameY: Projectile.frame);

			Texture2D bubble = Mod.Assets.Request<Texture2D>("Projectiles/Pets/FishBubble").Value;
			Vector2 stupidOffset = default;
			if (!Projectile.wet && !Projectile.isAPreviewDummy)
			{
				stupidOffset.Y += Projectile.gfxOffY;
				Main.EntitySpriteDraw(bubble, Projectile.Center - Main.screenPosition + stupidOffset, bubble.Frame(), lightColor, Projectile.rotation, bubble.Size() / 2, Projectile.scale, effects, 0);
			}

			stupidOffset = new Vector2(Projectile.width / 2, Projectile.height / 2 + 2f + Projectile.gfxOffY);
			Main.EntitySpriteDraw(image, Projectile.position - Main.screenPosition + stupidOffset, bounds, lightColor, Projectile.rotation, bounds.Size() / 2, Projectile.scale, effects, 0);

			return false;
		}
	}

	[Content(ContentType.AommSupport | ContentType.OtherPets)]
	public class KingGuppyShotProj : AssProjectile
	{
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.MinionShot[Projectile.type] = true;
			Main.projFrames[Projectile.type] = 1;
		}

		public ref float Level => ref Projectile.ai[1];

		public bool Exploding
		{
			get => Projectile.ai[2] == 1f;
			set => Projectile.ai[2] = value ? 1f : 0f;
		}

		private int timer = 0;

		private const int explWidth = 80;
		private const int explHeight = 70;

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.Bullet);
			Projectile.width = 26;
			Projectile.height = 22;
			Projectile.DamageType = DamageClass.Summon;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 1800;
			Projectile.alpha = 255;
			Projectile.idStaticNPCHitCooldown = 10;
			Projectile.usesIDStaticNPCImmunity = true;

			AIType = ProjectileID.Bullet;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.Lerp(Color.White, lightColor, 0.5f) * Projectile.Opacity;
		}

		public override void AI()
		{
			Projectile.rotation = 0f;
			if (Main.myPlayer == Projectile.owner)
			{
				if (CheckClick())
				{
					Exploding = true;
					Projectile.timeLeft = 3;
					Projectile.direction = (Main.LocalPlayer.Center.X < Projectile.Center.X).ToDirectionInt();
					Projectile.NetSync();
				}
			}

			if (Exploding)
			{
				Projectile.alpha = 255;
				Projectile.Resize(explWidth + (int)(5 * Level), explHeight + (int)(4 * Level));
			}
			else
			{
				float baseScale = 1.05f + Level * 0.05f;
				float scaleRange = 0.15f;
				Projectile.scale = Utils.Remap((float)Math.Sin(Main.GlobalTimeWrappedHourly / 1.5f * MathHelper.TwoPi), -1f, 1f, baseScale - scaleRange, baseScale + scaleRange);
				if (Main.rand.NextBool(16))
				{
					Dust dust = Main.dust[Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 15, 0f, 0f, 100, default(Color), 1.7f)];
					dust.velocity *= 0.3f;
					dust.noGravity = true;
				}
			}

			if (timer > 0)
			{
				timer++;
				Projectile.alpha = timer;
				if (timer > 255)
				{
					Projectile.Kill();
				}
			}
		}

		public override void OnKill(int timeLeft)
		{
			if (!Exploding)
			{
				return;
			}

			SoundEngine.PlaySound(SoundID.Item27, Projectile.Center);

			for (int k = 0; k < 8; k++)
			{
				int num9 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 15, 0f, 0f, 100, default(Color), 2f);
				Main.dust[num9].velocity *= 0.3f;
				Main.dust[num9].noGravity = true;

				int num10 = Main.rand.Next(2, 5);
				for (int i = 0; i < num10; i++)
				{
					Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 15, 0f, 0f, 100, default(Color), 1.5f);
					dust.velocity *= 0.3f;
					dust.position = Projectile.Center;
					dust.noGravity = true;
					dust.velocity += Main.rand.NextVector2Circular(5f, 4f);
					dust.fadeIn = 2.2f;
				}
			}
		}

		public override bool? CanHitNPC(NPC target)
		{
			if (Exploding)
			{
				return base.CanHitNPC(target);
			}
			return false;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Projectile.velocity *= 0;
			if (timer == 0)
			{
				timer++;
			}
			return false;
		}

		private bool CheckClick()
		{
			if (timer < 200 && Main.mouseLeft && Main.mouseLeftRelease)
			{
				var hb = Projectile.Hitbox;
				hb.Inflate(6, 6);
				return hb.Contains(Main.MouseWorld.ToPoint());
			}
			return false;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			AssUtils.DrawAroundOrigin(Projectile, lightColor);

			return false;
		}
	}
}
