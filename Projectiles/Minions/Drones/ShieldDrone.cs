using AssortedCrazyThings.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Minions.Drones
{
	/// <summary>
	/// Creates a damage reducing shield
	/// Checks if its active for the player in AssPlayer.PreUpdate, then resets shield
	/// </summary>
	public class ShieldDrone : DroneBase
	{
		private static readonly string nameLamps = "Projectiles/Minions/Drones/" + "ShieldDrone_Lamps";
		private static readonly string nameLower = "Projectiles/Minions/Drones/" + "ShieldDrone_Lower";
		private float addRotation; //same
		private const int ShieldDelay = 360;
		private float LowerOutPercent = 0f;

		private float ShieldCounter
		{
			get
			{
				return Projectile.ai[0];
			}
			set
			{
				Projectile.ai[0] = value;
			}
		}

		private bool CanShield
		{
			get
			{
				AssPlayer mPlayer = Projectile.GetOwner().GetModPlayer<AssPlayer>();
				return mPlayer.shieldDroneReduction < AssPlayer.shieldDroneReductionMax && LowerOutPercent == 1f;
			}
		}

		private int Stage
		{
			get
			{
				AssPlayer mPlayer = Projectile.GetOwner().GetModPlayer<AssPlayer>();
				return mPlayer.shieldDroneReduction / AssPlayer.ShieldIncreaseAmount;
			}
		}

		public override bool IsCombatDrone
		{
			get
			{
				return false;
			}
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shield Drone");
			Main.projFrames[Projectile.type] = 6;
			Main.projPet[Projectile.type] = true;
			ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.DD2PetGhost);
			Projectile.aiStyle = -1;
			Projectile.width = 34;
			Projectile.height = 30;
			Projectile.alpha = 0;
			Projectile.minion = true;
			Projectile.minionSlots = 1f;
		}

		protected override void CustomFrame(int frameCounterMaxFar = 4, int frameCounterMaxClose = 8)
		{
			Projectile.frame = Stage;

			float intensity = 700f - 25f * Projectile.frame;
			Vector2 lightPos = Projectile.Top + new Vector2(0f, sinY);
			Vector3 lightCol = default;
			if (Projectile.frame == 5)
			{
				lightCol = new Vector3(124, 251, 34);
			}
			else if (Projectile.frame > 2)
			{
				lightCol = new Vector3(200, 150, 0f);
			}
			else if (Projectile.frame > 0)
			{
				lightCol = new Vector3(153, 63, 66);
			}
			Lighting.AddLight(lightPos, lightCol / intensity);
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D image = Mod.Assets.Request<Texture2D>(nameLower).Value;
			Rectangle bounds = image.Frame(1, Main.projFrames[Projectile.type], frameY: Projectile.frame);

			SpriteEffects effects = Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

			Vector2 stupidOffset = new Vector2(Projectile.width / 2, Projectile.height / 2 + sinY);
			Vector2 drawPos = Projectile.position - Main.screenPosition + stupidOffset;
			Vector2 drawOrigin = bounds.Size() / 2;

			if (LowerOutPercent > 0f)
			{
				//Vector2 rotationOffset = new Vector2(0f, -16 + LowerOutPercent * 16);
				Vector2 rotationOffset = new Vector2(0f, 16 * (LowerOutPercent - 1f));

				//rotation origin is (projectile.position + stupidOffset) - drawOrigin; //not including Main.screenPosition
				Main.EntitySpriteDraw(image, drawPos + rotationOffset, bounds, lightColor, addRotation, drawOrigin, 1f, effects, 0);
			}

			image = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Main.EntitySpriteDraw(image, drawPos, bounds, lightColor, Projectile.rotation, drawOrigin, 1f, effects, 0);

			image = Mod.Assets.Request<Texture2D>(nameLamps).Value;
			Main.EntitySpriteDraw(image, drawPos, bounds, Color.White, Projectile.rotation, drawOrigin, 1f, effects, 0);


			return false;
		}

		protected override void ModifyDroneControllerHeld(ref float dmgModifier, ref float kbModifier)
		{
			if (CanShield) ShieldCounter += 0.333f;
		}

		protected override bool ModifyDefaultAI(ref bool staticDirection, ref bool reverseSide, ref float veloXToRotationFactor, ref float veloSpeed, ref float offsetX, ref float offsetY)
		{
			AssAI.FlickerwickPetAI(Projectile, lightPet: false, lightDust: false, staticDirection: true, vanityPet: true, veloXToRotationFactor: 0.5f, offsetX: -30f, offsetY: -50f);
			return false;
		}

		protected override void CustomAI()
		{
			Player player = Projectile.GetOwner();
			AssPlayer mPlayer = player.GetModPlayer<AssPlayer>();

			if (CanShield)
			{
				Vector2 shootOffset = new Vector2(0, sinY);
				Vector2 shootOrigin = Projectile.Center + shootOffset;
				Vector2 target = player.MountedCenter + new Vector2(0f, -5f);

				Vector2 between = target - shootOrigin;
				shootOrigin += Vector2.Normalize(between) * 19f; //roughly tip of turret
				target += -Vector2.Normalize(between) * 12f; //roughly center of head with a buffer

				float rotationAmount = between.ToRotation();

				if (Projectile.spriteDirection == 1) //adjust rotation based on direction
				{
					rotationAmount -= MathHelper.PiOver2;
					if (rotationAmount > MathHelper.TwoPi)
					{
						rotationAmount = -rotationAmount;
					}
				}

				bool canShoot = shootOrigin.Y < target.Y + player.height / 2;

				if (Projectile.spriteDirection == -1) //reset canShoot properly if rotation is too much (aka target is too fast for the drone to catch up)
				{
					if (rotationAmount <= Projectile.rotation)
					{
						canShoot = false;
						rotationAmount = Projectile.rotation;
					}
				}
				else
				{
					if (rotationAmount <= Projectile.rotation - MathHelper.Pi)
					{
						canShoot = false;
						rotationAmount = Projectile.rotation;
					}
				}
				addRotation = addRotation.AngleLerp(rotationAmount, 0.1f);

				if (canShoot) //when target below drone
				{
					ShieldCounter++;

					if (ShieldCounter > ShieldDelay)
					{
						ShieldCounter = 0;
						if (RealOwner) mPlayer.shieldDroneReduction += AssPlayer.ShieldIncreaseAmount;
						CombatText.NewText(player.getRect(), Color.LightBlue, AssPlayer.ShieldIncreaseAmount);
						AssUtils.QuickDustLine(16, shootOrigin, target, between.Length() / 3, Color.White, alpha: 120, scale: 1.5f);
					}
				}
			}
			else //if fully shielded, addRotation should go down to projectile.rotation
			{
				addRotation = addRotation.AngleLerp(Projectile.rotation, 0.1f);
			}

			if (Stage < 5)
			{
				if (LowerOutPercent < 1f)
				{
					LowerOutPercent += 0.015f;
					if (LowerOutPercent > 1f) LowerOutPercent = 1f;
				}
			}
			else
			{
				if (LowerOutPercent > 0f)
				{
					LowerOutPercent -= 0.015f;
					if (LowerOutPercent < 0f) LowerOutPercent = 0f;
				}
			}
		}
	}
}
