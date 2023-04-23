using AssortedCrazyThings.Base;
using AssortedCrazyThings.Base.ModSupport.AoMM;
using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Minions;
using AssortedCrazyThings.Projectiles.Minions.Drones;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Pets
{
	[Content(ContentType.DroppedPets)]
	public class PetGolemHeadProj : DroneBase
	{
		public int attackDelay = 60;

		private const int FireballDamage = 20;

		public override bool IsCombatDrone
		{
			get
			{
				return false;
			}
		}

		public override void SetStaticDefaults()
		{
			Main.projFrames[Projectile.type] = 2;
			Main.projPet[Projectile.type] = true;

			ProjectileID.Sets.CharacterPreviewAnimations[Projectile.type] = ProjectileID.Sets.SimpleLoop(0, 1)
				.WithOffset(-6, -15f)
				.WithSpriteDirection(-1);

			//Manual shooting
			AmuletOfManyMinionsApi.RegisterFlyingPet(this, ModContent.GetInstance<PetGolemHeadBuff_AoMM>(), 0);
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.ZephyrFish);
			Projectile.aiStyle = -1;
			Projectile.width = 38;
			Projectile.height = 38;
			Projectile.tileCollide = false;
			DrawOriginOffsetY = -10;
		}

		public override bool PreAI()
		{
			//Don't do any aomm movement
			AmuletOfManyMinionsApi.ReleaseControl(this);
			return base.PreAI();
		}

		protected override void CheckActive()
		{
			Player player = Projectile.GetOwner();
			PetPlayer modPlayer = player.GetModPlayer<PetPlayer>();
			if (player.dead)
			{
				modPlayer.PetGolemHead = false;
			}
			if (modPlayer.PetGolemHead)
			{
				Projectile.timeLeft = 2;
			}
		}

		protected override void CustomFrame(int frameCounterMaxFar = 4, int frameCounterMaxClose = 8)
		{
			if (Counter > attackDelay)
			{
				if (Counter < (int)(attackDelay * 1.5f))
				{
					Projectile.frame = 1;
				}
				else
				{
					Projectile.frame = 0;
				}
			}
			else
			{
				Projectile.frame = 0;
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D image = TextureAssets.Projectile[Projectile.type].Value;
			Rectangle bounds = image.Frame(1, Main.projFrames[Projectile.type], frameY: Projectile.frame);

			SpriteEffects effects = Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

			Sincounter = Sincounter > 360 ? 0 : Sincounter + 1;
			sinY = (float)((Math.Sin((Sincounter / 180f) * MathHelper.TwoPi) - 1) * 4);

			Vector2 stupidOffset = new Vector2(Projectile.width / 2, Projectile.height / 2 + sinY);
			Vector2 drawPos = Projectile.position - Main.screenPosition + stupidOffset;
			Vector2 drawOrigin = bounds.Size() / 2;

			Main.EntitySpriteDraw(image, drawPos, bounds, lightColor, Projectile.rotation, drawOrigin, 1f, effects, 0);
			Main.EntitySpriteDraw(ModContent.Request<Texture2D>(Texture + "_Glowmask").Value, drawPos, bounds, Color.White, Projectile.rotation, drawOrigin, 1f, effects, 0);

			return false;
		}

		protected override bool Bobbing()
		{
			return false;
		}

		protected override bool ModifyDefaultAI(ref bool staticDirection, ref bool reverseSide, ref float veloXToRotationFactor, ref float veloSpeed, ref float offsetX, ref float offsetY)
		{
			AssAI.FlickerwickPetAI(Projectile, lightPet: false, lightDust: false, staticDirection: true, veloSpeed: 0.5f, offsetX: -30f, offsetY: -100f);
			return false;
		}

		protected override void CustomAI()
		{
			Projectile.rotation = 0f;

			attackDelay = 60;
			int damage = FireballDamage;
			float kb = 2f;
			int targetIndex = -1;
			float speed = 7f;

			if (AmuletOfManyMinionsApi.IsActive(this) &&
				 AmuletOfManyMinionsApi.TryGetParamsDirect(this, out var paras) &&
				 AmuletOfManyMinionsApi.TryGetStateDirect(this, out var state) &&
				 state.IsInFiringRange && state.TargetNPC is NPC targetNPC)
			{
				targetIndex = targetNPC.whoAmI;
				attackDelay = paras.AttackFrames;
				damage = Projectile.damage;
				kb = Projectile.knockBack;
				speed = paras.LaunchVelocity;
			}

			Counter++;
			if (Counter % attackDelay == 0)
			{
				if (Main.myPlayer == Projectile.owner)
				{
					if (targetIndex == -1 && !Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height))
					{
						targetIndex = AssAI.FindTarget(Projectile, Projectile.Center, 600);
					}

					if (targetIndex != -1)
					{
						if (Counter == attackDelay) Counter += attackDelay;
						Vector2 position = Projectile.Center;
						position.Y += 6f;
						NPC target = Main.npc[targetIndex];
						Vector2 targetCenter = target.Center + 3 * target.velocity;
						Vector2 velocity = targetCenter - position;
						velocity.Normalize();
						velocity *= speed;
						AssUtils.ModifyVelocityForGravity(position, targetCenter, PetGolemHeadFireball.Gravity, ref velocity, PetGolemHeadFireball.TicksWithoutGravity);

						Projectile.NewProjectile(Projectile.GetSource_FromThis(), position, velocity, ModContent.ProjectileType<PetGolemHeadFireball>(), damage, kb, Main.myPlayer);
						Projectile.netUpdate = true;
					}
					else
					{
						if (Counter > attackDelay)
						{
							Counter -= attackDelay;
							Projectile.netUpdate = true;
						}
					}
				}
				Counter -= attackDelay;
			}
		}
	}
}
