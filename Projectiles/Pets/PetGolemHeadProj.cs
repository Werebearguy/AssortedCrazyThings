using AssortedCrazyThings.Base;
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
		public const int AttackDelay = 60;

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
			DisplayName.SetDefault("Replica Golem Head");
			Main.projFrames[Projectile.type] = 2;
			Main.projPet[Projectile.type] = true;
			DrawOriginOffsetY = -10;
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.ZephyrFish);
			Projectile.aiStyle = -1;
			Projectile.width = 38;
			Projectile.height = 38;
			Projectile.tileCollide = false;
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
			if (Counter > AttackDelay)
			{
				if (Counter < (int)(AttackDelay * 1.5f))
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

			Counter++;
			if (Counter % AttackDelay == 0)
			{
				if (Main.myPlayer == Projectile.owner)
				{
					int targetIndex = AssAI.FindTarget(Projectile, Projectile.Center, 600);
					if (targetIndex != -1 && !Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height))
					{
						if (Counter == AttackDelay) Counter += AttackDelay;
						Vector2 position = Projectile.Center;
						position.Y += 6f;
						Vector2 velocity = Main.npc[targetIndex].Center + Main.npc[targetIndex].velocity * 5f - position;
						velocity.Normalize();
						velocity *= 7f;
#if TML_2022_03
						Projectile.NewProjectile(Projectile.GetProjectileSource_FromThis(), position, velocity, ModContent.ProjectileType<PetGolemHeadFireball>(), FireballDamage, 2f, Main.myPlayer, 0f, 0f);
#else
						Projectile.NewProjectile(Projectile.GetSource_FromThis(), position, velocity, ModContent.ProjectileType<PetGolemHeadFireball>(), FireballDamage, 2f, Main.myPlayer, 0f, 0f);
#endif
						Projectile.netUpdate = true;
					}
					else
					{
						if (Counter > AttackDelay)
						{
							Counter -= AttackDelay;
							Projectile.netUpdate = true;
						}
					}
				}
				Counter -= AttackDelay;
			}
		}
	}
}
