using AssortedCrazyThings.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Weapons
{
	[Content(ContentType.Bosses)]
	public class TalonstormClawProj : AssProjectile
	{
		public static readonly float MaxRangeSQ = 14 * 16 * 14 * 16;
		public static readonly float PunchSpeed = 8f;
		public static readonly float RetractSpeed = 7f;

		public static readonly int ChainFrameCount = 6;
		public static Asset<Texture2D> ChainAsset;

		public override void Load()
		{
			ChainAsset = ModContent.Request<Texture2D>(Texture + "Chain");
		}

		public override void Unload()
		{
			ChainAsset = null;
		}

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Talonstorm");
			Main.projFrames[Projectile.type] = 1;
		}

		public bool Retracting
		{
			get => Projectile.ai[1] == 1f;
			set => Projectile.ai[1] = value ? 1f : 0f;
		}

		public int ParentIdentity
		{
			get => (int)Projectile.ai[0] - 1;
			set => Projectile.ai[0] = value + 1;
		}

		//Since the index might be different between clients, using ai[] for it will break stuff
		public int ParentIndex
		{
			get => (int)Projectile.localAI[1] - 1;
			set => Projectile.localAI[1] = value + 1;
		}

		public bool HasParent => ParentIndex >= 0 && ParentIndex < Main.maxProjectiles;

		public int chainFrameCounter;
		public int chainFrame;

		public override void SetDefaults()
		{
			Projectile.ignoreWater = true;
			Projectile.width = 24;
			Projectile.height = 24;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 300;
			Projectile.scale = 1f;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.alpha = 255;
			Projectile.extraUpdates = 1;
			Projectile.hide = true;
			Projectile.tileCollide = false;

			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 10;
		}

		public override void OnSpawn(IEntitySource source)
		{
			if (source is not EntitySource_Parent parentSource)
			{
				return;
			}

			if (parentSource.Entity is not Projectile parent)
			{
				return;
			}

			ParentIdentity = parent.identity;
		}

		public override void AI()
		{
			if (!CheckParent(out Projectile parent))
			{
				return;
			}

			FadeInOut(parent);
			AssExtensions.LoopAnimationInt(ref chainFrame, ref chainFrameCounter, 6, 0, ChainFrameCount - 1);

			Movement(parent);
		}

		private bool CheckParent(out Projectile parent)
		{
			parent = null;
			if (ParentIdentity <= -1 || ParentIdentity > Main.maxProjectiles)
			{
				Projectile.Kill();
				return false;
			}

			if (ParentIndex <= -1)
			{
				//Find parent based on identity
				Projectile test = AssUtils.NetGetProjectile(Projectile.owner, ParentIdentity, ModContent.ProjectileType<TalonstormProj>(), out int index);
				if (test != null)
				{
					//Important not to use test.whoAmI here
					ParentIndex = index;
				}
			}

			if (ParentIndex > -1 && ParentIndex <= Main.maxProjectiles)
			{
				parent = Main.projectile[ParentIndex];
			}

			if (parent == null)
			{
				//If the parent was not found, despawn
				Projectile.Kill();
				return false;
			}

			parent = Main.projectile[ParentIndex];
			if (!parent.active || parent.type != ModContent.ProjectileType<TalonstormProj>())
			{
				Projectile.Kill();
				return false;
			}

			return true;
		}

		private void FadeInOut(Projectile parent)
		{
			int fadeOutSpeed = 50;
			bool closeToDespawn = Projectile.timeLeft < 255f / fadeOutSpeed;
			if (Projectile.alpha > 0 && !Retracting && !closeToDespawn)
			{
				Projectile.alpha -= 50;
				if (Projectile.alpha <= 0)
				{
					Projectile.alpha = 0;
				}
			}

			float retractFadeOutDistSQ = 255f / fadeOutSpeed * (RetractSpeed * (1 + Projectile.extraUpdates));
			retractFadeOutDistSQ *= retractFadeOutDistSQ;
			if (Projectile.alpha < 255 && (closeToDespawn || Retracting && Projectile.DistanceSQ(parent.Center) < retractFadeOutDistSQ))
			{
				Projectile.alpha += fadeOutSpeed;
				if (Projectile.alpha > 255)
				{
					Projectile.alpha = 255;
				}
			}
		}

		private void Movement(Projectile parent)
		{
			if (!Projectile.tileCollide && !Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height))
			{
				//once out of blocks, collide with them again
				Projectile.tileCollide = true;
			}

			float parentDistSQ = Projectile.DistanceSQ(parent.Center);
			if (!Retracting && parentDistSQ > 48 * 48 + MaxRangeSQ)
			{
				//Force retract in case nothing got hit
				Retracting = true;
			}

			Projectile.rotation = 0f;

			if (Retracting)
			{
				//Move towards parent
				float retractDistSQ = 16 + RetractSpeed * (1 + Projectile.extraUpdates);
				retractDistSQ *= retractDistSQ;
				if (parentDistSQ <= retractDistSQ)
				{
					Projectile.Kill();
					return;
				}
				else
				{
					Projectile.velocity = Projectile.DirectionTo(parent.Center) * RetractSpeed;
				}
			}
			else
			{
				//Else it continues with the velocity given on spawn
				Projectile.spriteDirection = Projectile.direction = (Projectile.velocity.X > 0).ToDirectionInt();
			}
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			width = 8;
			height = 8;
			return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Retracting = true;
			Projectile.netUpdate = true;
			return base.OnTileCollide(oldVelocity);
		}

		public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
		{
			if (!HasParent)
			{
				return;
			}

			Projectile parent = Main.projectile[ParentIndex];
			modifiers.HitDirectionOverride = (target.Center.X > parent.Center.X).ToDirectionInt();
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			Retracting = true;
			Projectile.netUpdate = true;
		}

		public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
		{
			//So it draws behind the parent
			behindProjectiles.Add(index);
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.Lerp(lightColor, Color.White, 0.4f) * Projectile.Opacity;
		}

		public override bool PreDrawExtras()
		{
			if (!HasParent)
			{
				return true;
			}

			Projectile parent = Main.projectile[ParentIndex];
			Vector2 to = parent.Center;
			Texture2D texture = ChainAsset.Value;
			Projectile projectile = Projectile;
			Vector2 position = projectile.Center;
			Rectangle frame = texture.Frame(1, ChainFrameCount, frameY: chainFrame);
			SpriteEffects effect = projectile.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			Vector2 origin = frame.Size() / 2;
			float distBetweenSegments = 28;
			Vector2 between = to - position;
			bool keepDrawing = true;
			if (float.IsNaN(position.X) && float.IsNaN(position.Y))
			{
				keepDrawing = false;
			}

			if (float.IsNaN(between.X) && float.IsNaN(between.Y))
			{
				keepDrawing = false;
			}

			while (keepDrawing)
			{
				if (between.Length() < distBetweenSegments + 1)
				{
					break;
				}

				Vector2 normalized = between;
				normalized.Normalize();
				position += normalized * distBetweenSegments;
				between = to - position;
				Color color = Lighting.GetColor((int)position.X / 16, (int)position.Y / 16);
				color = projectile.GetAlpha(color);
				Main.EntitySpriteDraw(texture, position - Main.screenPosition, frame, color, 0f, origin, 0.8f, effect, 0);
			}

			return true;
		}
	}
}
