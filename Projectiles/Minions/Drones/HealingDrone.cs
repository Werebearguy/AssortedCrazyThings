using AssortedCrazyThings.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Minions.Drones
{
	/// <summary>
	/// Heals the player if below max health. Heals faster when below 50% health
	/// </summary>
	public class HealingDrone : DroneBase
	{
		private static readonly string nameGlow = "Projectiles/Minions/Drones/" + "HealingDrone_Glowmask";
		private static readonly string nameLower = "Projectiles/Minions/Drones/" + "HealingDrone_Lower";
		private static readonly string nameLowerGlow = "Projectiles/Minions/Drones/" + "HealingDrone_Lower_Glowmask";
		private float addRotation; //same
		private const int HealDelay = 80;

		private float HealCounter
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

		private bool CanHeal
		{
			get
			{
				return Projectile.GetOwner().statLife < Projectile.GetOwner().statLifeMax2;
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
			// DisplayName.SetDefault("Healing Drone");
			Main.projFrames[Projectile.type] = 3;
			Main.projPet[Projectile.type] = true;
			ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.DD2PetGhost);
			Projectile.aiStyle = -1;
			Projectile.width = 38;
			Projectile.height = 30;
			Projectile.alpha = 0;
			Projectile.minion = true;
			Projectile.minionSlots = 1f;
		}

		protected override void CustomFrame(int frameCounterMaxFar = 4, int frameCounterMaxClose = 8)
		{
			Player player = Projectile.GetOwner();

			Vector2 lightPos = Projectile.position + new Vector2(Projectile.spriteDirection == 1 ? 0f : Projectile.width, Projectile.height / 2);

			if (player.statLife < player.statLifeMax2 / 2)
			{
				Lighting.AddLight(lightPos, new Vector3(153 / 700f, 63 / 700f, 66 / 700f));
				Projectile.frame = 2;
			}
			else if (CanHeal)
			{
				Lighting.AddLight(lightPos, new Vector3(240 / 700f, 198 / 700f, 0f));
				Projectile.frame = 1;
			}
			else
			{
				Lighting.AddLight(lightPos, new Vector3(124 / 700f, 251 / 700f, 34 / 700f));
				Projectile.frame = 0;
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D image = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Rectangle bounds = image.Frame(1, Main.projFrames[Projectile.type], frameY: Projectile.frame);

			SpriteEffects effects = Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

			Vector2 stupidOffset = new Vector2(Projectile.width / 2, Projectile.height - 8f + sinY);
			Vector2 drawPos = Projectile.position - Main.screenPosition + stupidOffset;
			Vector2 drawOrigin = bounds.Size() / 2;

			Main.EntitySpriteDraw(image, drawPos, bounds, lightColor, Projectile.rotation, drawOrigin, 1f, effects, 0);

			image = Mod.Assets.Request<Texture2D>(nameGlow).Value;
			Main.EntitySpriteDraw(image, drawPos, bounds, Color.White, Projectile.rotation, drawOrigin, 1f, effects, 0);

			Vector2 rotationOffset = new Vector2(0f, -2f); //-2f)
			drawPos += rotationOffset;
			drawOrigin += rotationOffset;

			//AssUtils.ShowDustAtPos(135, projectile.position + stupidOffset);

			//AssUtils.ShowDustAtPos(136, projectile.position + stupidOffset - drawOrigin);

			//rotation origin is (projectile.position + stupidOffset) - drawOrigin; //not including Main.screenPosition
			image = Mod.Assets.Request<Texture2D>(nameLower).Value;
			Main.EntitySpriteDraw(image, drawPos, bounds, lightColor, addRotation, drawOrigin, 1f, effects, 0);

			image = Mod.Assets.Request<Texture2D>(nameLowerGlow).Value;
			Main.EntitySpriteDraw(image, drawPos, bounds, Color.White, addRotation, drawOrigin, 1f, effects, 0);

			return false;
		}

		protected override void ModifyDroneControllerHeld(ref float dmgModifier, ref float kbModifier)
		{
			if (CanHeal) HealCounter += 0.333f;
		}

		protected override bool ModifyDefaultAI(ref bool staticDirection, ref bool reverseSide, ref float veloXToRotationFactor, ref float veloSpeed, ref float offsetX, ref float offsetY)
		{
			AssAI.FlickerwickPetAI(Projectile, lightPet: false, lightDust: false, reverseSide: true, veloXToRotationFactor: 0.5f, offsetX: 16f, offsetY: CanHeal ? -16f : 4f); //2f
			return false;
		}

		protected override void CustomAI()
		{
			Player player = Projectile.GetOwner();

			if (CanHeal)
			{
				Vector2 shootOffset = new Vector2(Projectile.width / 2 + Projectile.spriteDirection * 4f, (Projectile.height - 2f) + sinY);
				Vector2 shootOrigin = Projectile.position + shootOffset;
				Vector2 target = player.MountedCenter + new Vector2(0f, -5f);

				Vector2 between = target - shootOrigin;
				shootOrigin += Vector2.Normalize(between) * 16f; //roughly tip of turret
				target += -Vector2.Normalize(between) * 12f; //roughly center of head with a buffer

				float rotationAmount = between.ToRotation();

				if (Projectile.spriteDirection == 1) //adjust rotation based on direction
				{
					rotationAmount -= MathHelper.Pi;
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
					HealCounter++;
					int delay = HealDelay - (player.statLife < player.statLifeMax2 / 2 ? 20 : 0);

					if (HealCounter > delay)
					{
						HealCounter = 0;
						int heal = 1;
						player.statLife += heal;
						//if it would be just clientside, test this:
						//if (Main.netMode != NetmodeID.Singleplayer) NetMessage.SendData(MessageID.PlayerHealth, -1, -1, null, player.whoAmI);
						player.HealEffect(heal, false);
						AssUtils.QuickDustLine(61, shootOrigin, target, between.Length() / 3, Color.White, alpha: 120, scale: 2f);
					}

					//if (Sincounter % delay == 30) //only shoot once every 1.333 or 1.5 seconds, when target below drone and when turret aligned properly
					//{
					//    int heal = 1;
					//    player.statLife += heal;
					//    player.HealEffect(heal, false);
					//}
					//if (Sincounter % delay == 35)
					//{
					//    AssUtils.QuickDustLine(61, shootOrigin, target, between.Length() / 3, Color.White, alpha: 120, scale: 2f);
					//}
				}
			}
			else //if above 50%, addRotation should go down to projectile.rotation
			{
				////if addRotation is bigger than projectile.rotation by a small margin, reduce it down to projectile.rotation slowly
				//if (Math.Abs(addRotation) > Math.Abs(projectile.rotation) + 0.006f)
				//{
				//    float rotDiff = projectile.rotation - addRotation;
				//    if (Math.Abs(rotDiff) < 0.005f)
				//    {
				//        addRotation = projectile.rotation;
				//    }
				//    else
				//    {
				//        addRotation += addRotation * -0.15f;
				//    }
				//}
				//else
				//{
				//    //fix rotation so it doesn't get adjusted anymore
				//    addRotation = projectile.rotation;
				//}
				addRotation = addRotation.AngleLerp(Projectile.rotation, 0.1f);
			}
		}
	}
}
