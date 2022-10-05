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
	public class DynamiteBunnyProj : SimplePetProjBase
	{
		public override string Texture
		{
			get
			{
				return "AssortedCrazyThings/Projectiles/Pets/DynamiteBunnyProj_0"; //temp
			}
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dynamite Bunny");
			Main.projFrames[Projectile.type] = Main.projFrames[ProjectileID.Bunny];
			Main.projPet[Projectile.type] = true;

			AmuletOfManyMinionsApi.RegisterGroundedPet(this, ModContent.GetInstance<DynamiteBunnyBuff_AoMM>(), null);
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.Bunny);
			AIType = ProjectileID.Bunny;
			DrawOriginOffsetY = -7;
		}

		public override bool PreAI()
		{
			Player player = Projectile.GetOwner();
			player.bunny = false; // Relic from AIType
			return true;
		}

		public short prevDynamiteBunnyType = -1;

		public const int ExplosionTimerDecr = 4;
		public const int ExplosionTimerMax = 255 + ExplosionTimerDecr * 60;
		private int explosionTimer = 0;

		private int aommExplosionHitCount = 0;
		public int aommExplosionHitCountMax = 4;

		public override void AI()
		{
			Player player = Projectile.GetOwner();
			PetPlayer modPlayer = player.GetModPlayer<PetPlayer>();
			if (player.dead)
			{
				modPlayer.DynamiteBunny = false;
			}
			if (modPlayer.DynamiteBunny)
			{
				Projectile.timeLeft = 2;
			}
			AssAI.TeleportIfTooFar(Projectile, player.MountedCenter);

			if (explosionTimer > 0)
			{
				explosionTimer -= ExplosionTimerDecr;

				if (explosionTimer < 0)
				{
					explosionTimer = 0;
				}
			}

			Projectile.alpha = Math.Min(255, explosionTimer);

			byte dynamiteBunnyType = modPlayer.dynamiteBunnyType;
			if (prevDynamiteBunnyType == -1)
			{
				prevDynamiteBunnyType = dynamiteBunnyType;
			}

			if (prevDynamiteBunnyType != dynamiteBunnyType)
			{
				explosionTimer = ExplosionTimerMax;

				SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);

				for (int i = 0; i < 10; i++) //40
				{
					Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Smoke, 0f, 0f, 100, default(Color), 2f);
					dust.velocity *= 2f; //3f
					if (Main.rand.NextBool(2))
					{
						dust.scale = 0.5f;
						dust.fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
					}
				}
				for (int i = 0; i < 17; i++) //70
				{
					Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 6, 0f, 0f, 100, default(Color), 3f);
					dust.noGravity = true;
					dust.velocity *= 4f; //5f
					dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 6, 0f, 0f, 100, default(Color), 2f);
					dust.velocity *= 2f;
				}
				for (int i = 0; i < 2; i++) //3
				{
					float scaleFactor10 = 0.33f;
					if (i == 1)
					{
						scaleFactor10 = 0.66f;
					}
					if (i == 2)
					{
						scaleFactor10 = 1f;
					}
					var entitySource = Projectile.GetSource_FromAI();
					Gore gore = Main.gore[Gore.NewGore(entitySource, Projectile.Center - new Vector2(24f), default(Vector2), Main.rand.Next(61, 64), 1f)];
					gore.velocity *= scaleFactor10;
					gore.velocity.X += 1f;
					gore.velocity.Y += 1f;
					gore = Main.gore[Gore.NewGore(entitySource, Projectile.Center - new Vector2(24f), default(Vector2), Main.rand.Next(61, 64), 1f)];
					gore.velocity *= scaleFactor10;
					gore.velocity.X += -1f;
					gore.velocity.Y += 1f;
					gore = Main.gore[Gore.NewGore(entitySource, Projectile.Center - new Vector2(24f), default(Vector2), Main.rand.Next(61, 64), 1f)];
					gore.velocity *= scaleFactor10;
					gore.velocity.X += 1f;
					gore.velocity.Y += -1f;
					gore = Main.gore[Gore.NewGore(entitySource, Projectile.Center - new Vector2(24f), default(Vector2), Main.rand.Next(61, 64), 1f)];
					gore.velocity *= scaleFactor10;
					gore.velocity.X += -1f;
					gore.velocity.Y += -1f;
				}
			}

			prevDynamiteBunnyType = dynamiteBunnyType;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (!AmuletOfManyMinionsApi.IsActive(this))
			{
				return;
			}

			aommExplosionHitCount++;
			if (aommExplosionHitCount >= aommExplosionHitCountMax)
			{
				aommExplosionHitCount = 0;

				Projectile grenate = Projectile.NewProjectileDirect(Projectile.GetSource_OnHit(target), Projectile.Center, Vector2.Zero, ProjectileID.Grenade, (int)(Projectile.damage * 0.75f), Projectile.knockBack, Main.myPlayer);
				grenate.timeLeft = 3;
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			PetPlayer mPlayer = Projectile.GetOwner().GetModPlayer<PetPlayer>();
			SpriteEffects effects = Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			Texture2D image = Mod.Assets.Request<Texture2D>("Projectiles/Pets/DynamiteBunnyProj_" + mPlayer.dynamiteBunnyType).Value;
			Rectangle bounds = image.Frame(1, Main.projFrames[Projectile.type], frameY: Projectile.frame);

			Vector2 stupidOffset = new Vector2(Projectile.width / 2, Projectile.height / 2 + DrawOriginOffsetY + Projectile.gfxOffY);

			Main.EntitySpriteDraw(image, Projectile.position - Main.screenPosition + stupidOffset, bounds, Projectile.GetAlpha(lightColor), Projectile.rotation, bounds.Size() / 2, Projectile.scale, effects, 0);

			return false;
		}
	}
}
