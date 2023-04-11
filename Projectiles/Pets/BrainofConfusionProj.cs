using AssortedCrazyThings.Base;
using AssortedCrazyThings.Base.ModSupport.AoMM;
using AssortedCrazyThings.Buffs.NPCs;
using AssortedCrazyThings.Buffs.Pets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Pets
{
	public class BrainofConfusionProj : SimplePetProjBase
	{
		public override void SetStaticDefaults()
		{
			Main.projFrames[Projectile.type] = 4;
			Main.projPet[Projectile.type] = true;

			AmuletOfManyMinionsApi.RegisterFlyingPet(this, ModContent.GetInstance<BrainofConfusionBuff_AoMM>(), 0);
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.ZephyrFish);
			Projectile.aiStyle = -1;
		}

		public override void AI()
		{
			Player player = Projectile.GetOwner();
			PetPlayer modPlayer = player.GetModPlayer<PetPlayer>();
			if (player.dead)
			{
				modPlayer.BrainofConfusion = false;
			}
			if (modPlayer.BrainofConfusion)
			{
				Projectile.timeLeft = 2;
			}
			AssAI.ZephyrfishAI(Projectile);
			AssAI.ZephyrfishDraw(Projectile);

			if (AmuletOfManyMinionsApi.IsActive(this))
			{
				AoMM_AI();
			}
		}
		
		private int PreferredTargetDistanceCache
		{
			get => (int)Projectile.ai[0];
			set => Projectile.ai[0] = value;
		}

		private int oldTier = -1;

		private int shotTimer = -1;
		private const int ShotTimerMax = 60;

		private void AoMM_AI()
		{
			if (!AmuletOfManyMinionsApi.IsAttacking(this) ||
				!AmuletOfManyMinionsApi.TryGetStateDirect(this, out var state) ||
				!state.IsInFiringRange)
			{
				//Let timer finish for the animation
				if (shotTimer >= 0)
				{
					shotTimer++;
					if (shotTimer >= ShotTimerMax)
					{
						shotTimer = -1;
					}
				}
				return;
			}

			int tier = AmuletOfManyMinionsApi.GetPetLevel(Projectile.GetOwner());
			if (oldTier != tier)
			{
				oldTier = tier;

				if (tier < NeurotoxinLoader.TierCount && AmuletOfManyMinionsApi.TryGetParamsDirect(this, out var paras))
				{
					//Default is 128
					int dist = 8 * tier + 64;
					paras.PreferredTargetDistance = dist;
					PreferredTargetDistanceCache = dist;
					//Final at 8 would be 128
					AmuletOfManyMinionsApi.UpdateParamsDirect(this, paras);
				}
			}

			shotTimer++;
			if (shotTimer >= ShotTimerMax)
			{
				shotTimer = 0;

				int radius = PreferredTargetDistanceCache + 80;

				bool doVisual = false;
				for (int i = 0; i < Main.maxNPCs; i++)
				{
					NPC npc = Main.npc[i];

					if (npc.CanBeChasedBy() && npc.DistanceSQ(Projectile.Center) < radius * radius && Collision.CanHitLine(Projectile.Center, 1, 1, npc.Center, 1, 1))
					{
						doVisual = true;

						npc.AddBuff(BuffID.Confused, 60);
						if (tier < NeurotoxinLoader.TierCount)
						{
							int neuroType = Mod.Find<ModBuff>($"{nameof(NeurotoxinBuff)}_{tier}").Type;
							npc.AddBuff(neuroType, 60);
						}
					}
				}

				if (doVisual)
				{
					int visualRadius = radius - 30;
					int dustAmount = (int)(visualRadius * MathHelper.TwoPi) / 4;
					for (int i = 0; i < dustAmount; i++)
					{
						float angle = i / (float)dustAmount * MathHelper.TwoPi;

						Vector2 outwards = Vector2.UnitY.RotatedBy(angle);
						Dust dust = Dust.NewDustPerfect(Projectile.Center + outwards * visualRadius, 62, outwards * 5f, Scale: 1.5f);
						dust.noGravity = true;
						dust.fadeIn = 1f;
						dust.noLightEmittence = true;
						dust.noLight = true;
					}
				}
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			SpriteEffects effects = Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

			Texture2D image = TextureAssets.Projectile[Projectile.type].Value;
			Rectangle bounds = image.Frame(1, Main.projFrames[Projectile.type], frameY: Projectile.frame);

			Vector2 stupidOffset = new Vector2(Projectile.width / 2, bounds.Height / 2 + Projectile.gfxOffY);

			Vector2 drawPos = Projectile.position - Main.screenPosition + stupidOffset;
			Vector2 origin = bounds.Size() / 2;

			if (AmuletOfManyMinionsApi.IsActive(this) &&
				AmuletOfManyMinionsApi.IsAttacking(this))
			{
				float ratio = shotTimer / 20f;
				if (ratio <= 1f)
				{
					Color color = lightColor * (1f - ratio);
					float scale = Projectile.scale + ratio * 0.5f;

					Main.EntitySpriteDraw(image, drawPos, bounds, color, Projectile.rotation, origin, scale, effects, 0);
				}
			}

			Main.EntitySpriteDraw(image, drawPos, bounds, lightColor, Projectile.rotation, origin, Projectile.scale, effects, 0);

			return false;
		}
	}
}
