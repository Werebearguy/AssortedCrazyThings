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
	public class PetCloudfishProj : SimplePetProjBase
	{
		public override string Texture
		{
			get
			{
				return "AssortedCrazyThings/Projectiles/Pets/PetCloudfishProj_0"; //temp
			}
		}

		public override void SetStaticDefaults()
		{
			Main.projFrames[Projectile.type] = 6;
			Main.projPet[Projectile.type] = true;

			ProjectileID.Sets.CharacterPreviewAnimations[Projectile.type] = ProjectileID.Sets.SimpleLoop(0, Main.projFrames[Projectile.type], 7)
				.WithOffset(-6, -16f)
				.WithSpriteDirection(-1)
				.WithCode(DelegateMethods.CharacterPreview.Float);

			AmuletOfManyMinionsApi.RegisterFlyingPet(this, ModContent.GetInstance<PetCloudfishBuff_AoMM>(), 0);
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.ZephyrFish);
			Projectile.aiStyle = -1;
		}

		public override bool PreAI()
		{
			//Don't do any aomm movement
			AmuletOfManyMinionsApi.ReleaseControl(this);

			if (AmuletOfManyMinionsApi.TryGetParamsDirect(this, out var paras) &&
				AmuletOfManyMinionsApi.TryGetStateDirect(this, out var state))
			{
				paras.AttackFrames = 20 - state.PetLevel;
				AmuletOfManyMinionsApi.UpdateParamsDirect(this, paras);
			}

			Projectile.originalDamage = Math.Max(4, (int)(Projectile.originalDamage * 0.8f));

			return base.PreAI();
		}

		public override void AI()
		{
			Player player = Projectile.GetOwner();
			PetPlayer modPlayer = player.GetModPlayer<PetPlayer>();
			if (player.dead)
			{
				modPlayer.PetCloudfish = false;
			}
			if (modPlayer.PetCloudfish)
			{
				Projectile.timeLeft = 2;
			}

			if (AmuletOfManyMinionsApi.IsAttacking(this) && AmuletOfManyMinionsApi.TryGetStateDirect(this, out var state)
				&& state.TargetNPC is NPC targetNPC)
			{
				//scan for 8 tiles away from center for suitable location, since breaking LOS cancels attacking
				Vector2 targetPos = targetNPC.Center + targetNPC.velocity * 0.8f;
				int steps = 8;
				while (steps > 0)
				{
					Vector2 checkPos = targetPos - new Vector2(0, steps * 16);
					if(!Collision.SolidCollision(checkPos - Projectile.Size / 2, Projectile.width, Projectile.height) && 
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
				if (Projectile.velocity.X > 2f && Projectile.direction == 1)
				{
					Projectile.direction = -1;
				}
				else if (Projectile.velocity.X < -2f && Projectile.direction != 1)
				{
					Projectile.direction = 1;
				}
				Projectile.spriteDirection = Projectile.direction;

				int nearDistSQ = 4 * state.MaxSpeed * state.MaxSpeed;
				if (Projectile.DistanceSQ(targetPos) > nearDistSQ)
				{
					AmuletOfManyMinionsApi.AoMMMovement(Projectile, vec, state.MaxSpeed, state.Inertia);
				}

				int shootDistSQ = 2 * nearDistSQ;
				if (Projectile.DistanceSQ(targetPos) < shootDistSQ)
				{
					Projectile.velocity.Y *= 0.9f;
					if (state.ShouldFireThisFrame && Projectile.owner == Main.myPlayer)
					{
						int rainSpawnX = (int)(Projectile.position.X + 6f + Main.rand.Next(Projectile.width - 12));
						int rainSpawnY = (int)(Projectile.position.Y + Projectile.height + 4f);
						Projectile.NewProjectile(Projectile.GetSource_FromThis(), rainSpawnX, rainSpawnY, 0f, 8f, ModContent.ProjectileType<PetCloudfishShotProj>(), Projectile.damage, 0f, Projectile.owner, targetNPC.Center.Y);
					}
				}
			}
			else
			{
				AssAI.ZephyrfishAI(Projectile);
			}

			AssAI.ZephyrfishDraw(Projectile, frameCounter: 6);
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
			Texture2D image = Mod.Assets.Request<Texture2D>("Projectiles/Pets/PetCloudfishProj_" + mPlayer.petCloudfishType).Value;
			Rectangle bounds = image.Frame(1, Main.projFrames[Projectile.type], frameY: Projectile.frame);

			Vector2 stupidOffset = new Vector2(Projectile.width / 2, Projectile.height / 2 + 2f + Projectile.gfxOffY);

			Main.EntitySpriteDraw(image, Projectile.position - Main.screenPosition + stupidOffset, bounds, lightColor, Projectile.rotation, bounds.Size() / 2, Projectile.scale, effects, 0);

			return false;
		}
	}

	public class PetCloudfishShotProj : MinionShotProj_AoMM
	{
		public override int ClonedType => ProjectileID.RainFriendly;

		public ref float TargetY => ref Projectile.ai[0];

		public override void SafeSetDefaults()
		{
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
			Projectile.usesIDStaticNPCImmunity = true;
			Projectile.idStaticNPCHitCooldown = 60;
		}

		public override void SafeAI()
		{
			if (!Projectile.tileCollide && Projectile.Bottom.Y > TargetY)
			{
				Projectile.tileCollide = true;
			}
		}

		public override void ModifyDamageHitbox(ref Rectangle hitbox)
		{
			hitbox.Inflate(0, 6);
		}
	}
}
