using AssortedCrazyThings.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Weapons
{
	[Content(ContentType.Bosses)]
	public class TalonstormProj : AssProjectile
	{
		private class BoneDrawParams
		{
			//All of these Out of TwoPi
			public float phase = 0f;
			public float orbitPerTick = 0;
			public float rotationPerTick = 0;

			public float distance = 0f;
			public bool behind = false;
		}

		public const int MaxExisting = 1;
		public const int BoneCount = 8;
		public const int Lifetime = 18000;
		public const int AttackCooldown = 20;

		private static Asset<Texture2D>[] boneTextures;

		//-1 on other clients, but it's only relevant for owner
		public int clawWhoAmI = -1;

		public int AttackTimer
		{
			get => (int)Projectile.ai[0];
			set => Projectile.ai[0] = value;
		}

		public int TimeLeftTimer
		{
			get => (int)Projectile.ai[1];
			set => Projectile.ai[1] = value;
		}

		public int CheckOthersTimer
		{
			get => (int)Projectile.localAI[0];
			set => Projectile.localAI[0] = value;
		}

		public float VisualTimer
		{
			get => Projectile.localAI[1];
			set => Projectile.localAI[1] = value;
		}

		private float sincounter = 0;
		private float sinY = 0;

		private BoneDrawParams[] boneDrawParams = new BoneDrawParams[BoneCount];
		private bool boneDrawParamsInit = false;

		public override void Load()
		{
			boneTextures = new Asset<Texture2D>[BoneCount];
			for (int i = 0; i < boneTextures.Length; i++)
			{
				int num = i;
				//Extra pieces using existing textures
				if (i == 6)
				{
					num = 0;
				}
				if (i == 7)
				{
					num = 3;
				}
				boneTextures[i] = ModContent.Request<Texture2D>(Texture + "Bit_" + num);
			}
		}

		public override void Unload()
		{
			boneTextures = null;
		}

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Talonstorm");
			Main.projFrames[Projectile.type] = 6;
		}

		public override void SetDefaults()
		{
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.netImportant = true;
			Projectile.width = 26;
			Projectile.height = 26;
			Projectile.aiStyle = -1;
			Projectile.penetrate = -1;
			Projectile.timeLeft = Lifetime;

			DrawOffsetX = -(18 - Projectile.width) / 2;
			DrawOriginOffsetY = -4;
		}

		public override void AI()
		{
			Projectile.LoopAnimation(7);

			if (TimeLeftTimer == 0)
			{
				for (int i = 0; i < 10; i++)
				{
					Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 135, 0, 0, 0, default(Color), 2f);
					dust.noGravity = true;
					dust.velocity = Projectile.Center - dust.position;
					dust.velocity.Normalize();
					dust.velocity *= -1f;
				}
			}

			sincounter = sincounter >= 240 ? 0 : sincounter + 1;
			sinY = (float)(Math.Sin((sincounter / 120f) * MathHelper.TwoPi) * 3);

			TimeLeftTimer++;
			if (TimeLeftTimer >= Lifetime)
			{
				Projectile.alpha += 5;
				if (Projectile.alpha > 255)
				{
					Projectile.alpha = 255;
					Projectile.Kill();
				}
			}
			else if (Projectile.owner == Main.myPlayer)
			{
				Attack();
			}

			UpdateOrbits();
			CheckOthers();
		}

		private void CheckOthers()
		{
			CheckOthersTimer++;
			if (CheckOthersTimer >= 10)
			{
				CheckOthersTimer = 0;
				int otherCount = 0;
				int otherIndex = 0;
				float otherai1 = 0f;
				for (int i = 0; i < Main.maxProjectiles; i++)
				{
					//check for others
					Projectile other = Main.projectile[i];
					if (other.active && other.owner == Projectile.owner && other.type == Projectile.type && other.ai[1] < Lifetime)
					{
						otherCount++;
						if (other.ai[1] > otherai1)
						{
							otherIndex = i;
							otherai1 = other.ai[1];
						}
					}
				}

				if (otherCount > MaxExisting)
				{
					Projectile projectile = Main.projectile[otherIndex];
					projectile.netUpdate = true;
					projectile.ai[1] = Lifetime;
				}
			}
		}

		private void Attack()
		{
			//Should only be called ownerside
			AttackTimer++;
			int clawType = ModContent.ProjectileType<TalonstormClawProj>();
			bool existingClawGone = true;
			if (clawWhoAmI > -1 && clawWhoAmI <= Main.maxProjectiles)
			{
				Projectile other = Main.projectile[clawWhoAmI];
				if (other.active && other.owner == Projectile.owner && other.type == clawType)
				{
					existingClawGone = false;
				}
			}

			if (existingClawGone)
			{
				clawWhoAmI = -1;
				if (AttackTimer >= AttackCooldown)
				{
					int targetIndex = -1;
					float maxDistSQ = TalonstormClawProj.MaxRangeSQ;
					float distSQ;
					for (int j = 0; j < Main.maxNPCs; j++)
					{
						NPC npc = Main.npc[j];
						if (!npc.CanBeChasedBy())
						{
							continue;
						}
						distSQ = Projectile.DistanceSQ(npc.Center);
						if (distSQ < maxDistSQ && Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, npc.position, npc.width, npc.height))
						{
							maxDistSQ = distSQ;
							targetIndex = j;
						}
					}

					if (targetIndex > -1)
					{
						AttackTimer = 0;
						var source = Projectile.GetSource_FromThis();
						clawWhoAmI = Projectile.NewProjectile(source, Projectile.Center, Main.npc[targetIndex].DirectionFrom(Projectile.Center) * TalonstormClawProj.PunchSpeed, clawType, Projectile.damage, Projectile.knockBack, Projectile.owner);
					}
				}
			}
		}

		private void UpdateOrbits()
		{
			VisualTimer++;
			if (boneDrawParams == null)
			{
				boneDrawParams = new BoneDrawParams[BoneCount];
			}

			if (boneDrawParams[0] == null)
			{
				boneDrawParamsInit = true;
				int len = boneDrawParams.Length;
				for (int i = 0; i < len; i++)
				{
					boneDrawParams[i] = GetBoneDrawParams(i, len);
				}
			}
		}

		private static BoneDrawParams GetBoneDrawParams(int index, int length)
		{
			int iOff2 = (index + 2) % length;
			int iOff3 = (index + 3) % length;
			var p = new BoneDrawParams
			{
				//Between 0 & TwoPi
				phase = index * (MathHelper.TwoPi / length),
				//Baseline every 10 ticks, adjust up/down
				orbitPerTick = MathHelper.TwoPi / 80f + 0.03f * (float)Math.Sin(iOff3 / (float)length * MathHelper.TwoPi),
				rotationPerTick = MathHelper.TwoPi / 100f + 0.02f * iOff2 * (iOff3 - 1 > 0).ToDirectionInt(),
				distance = 2.8f + (length - index) * 2,
				behind = index < length / 2,
			};
			return p;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.Lerp(lightColor, Color.White, 0.4f) * Projectile.Opacity;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture2D = TextureAssets.Projectile[Projectile.type].Value;
			Rectangle frame = texture2D.Frame(1, Main.projFrames[Projectile.type], frameY: Projectile.frame);
			Vector2 drawOrigin = new Vector2((frame.Width - Projectile.width) / 2 + Projectile.width / 2, Projectile.height / 2);

			Color color = Projectile.GetAlpha(lightColor);

			DrawBones(color, true);

			DrawBody(texture2D, frame, drawOrigin, color);

			DrawBones(color, false);

			return false;
		}

		private void DrawBody(Texture2D texture2D, Rectangle frame, Vector2 drawOrigin, Color color)
		{
			Draw(texture2D, Projectile.position + new Vector2(4, -4), frame, drawOrigin + new Vector2(0, -4), color * 0.5f, scale: Projectile.scale + (2f - sinY) / 15f);
			Draw(texture2D, Projectile.position + new Vector2(4, -4), frame, drawOrigin + new Vector2(0, -4), color);
		}

		private void DrawBones(Color color, bool behind)
		{
			if (boneDrawParamsInit)
			{
				for (int i = 0; i < boneDrawParams.Length; i++)
				{
					var @params = boneDrawParams[i];
					if (@params.behind == behind)
					{
						Texture2D boneTexture = boneTextures[i].Value;
						DrawBoneWithTrail(boneTexture, color, @params);
					}
				}
			}
		}

		private void DrawBoneWithTrail(Texture2D boneTexture, Color color, BoneDrawParams @params)
		{
			for (int j = 3; j >= 0; j--)
			{
				DrawBone(boneTexture, VisualTimer - j * 3, Color.Lerp(Color.Transparent, color, (3 - j) / 3f) * 0.8f, @params);
			}
			DrawBone(boneTexture, VisualTimer, color, @params);
		}

		private void DrawBone(Texture2D boneTexture, float timer, Color color, BoneDrawParams @params)
		{
			float rot = @params.rotationPerTick * timer;
			Vector2 orbit = Vector2.UnitY * @params.distance;
			orbit = orbit.RotatedBy(@params.orbitPerTick * timer + @params.phase);

			Vector2 boneDrawOrigin = boneTexture.Size() / 2;

			Draw(boneTexture, Projectile.Center + orbit - boneDrawOrigin, null, boneDrawOrigin, color, rot);
		}

		private void Draw(Texture2D texture, Vector2 position, Rectangle? frame, Vector2 drawOrigin, Color color, float rotation = 0f, float scale = 1f)
		{
			Vector2 drawPos = position - Main.screenPosition + drawOrigin + new Vector2(0, sinY + Projectile.gfxOffY);
			Main.EntitySpriteDraw(texture, drawPos, frame, color, Projectile.rotation + rotation, drawOrigin, scale, SpriteEffects.None, 0);
		}
	}
}
