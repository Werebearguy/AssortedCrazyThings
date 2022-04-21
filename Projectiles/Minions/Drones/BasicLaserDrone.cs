using AssortedCrazyThings.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Minions.Drones
{
	/// <summary>
	/// Fires a weak laser rapidly
	/// </summary>
	public class BasicLaserDrone : DroneBase
	{
		private static readonly string nameGlow = "Projectiles/Minions/Drones/" + "BasicLaserDrone_Glowmask";
		private static readonly string nameLower = "Projectiles/Minions/Drones/" + "BasicLaserDrone_Lower";
		private static readonly string nameLowerGlow = "Projectiles/Minions/Drones/" + "BasicLaserDrone_Lower_Glowmask";

		private const int AttackDelay = 25;
		private const int SearchDelay = 30;

		private const byte STATE_IDLE = 0;
		private const byte STATE_TARGET_FOUND = 1;
		private const byte STATE_TARGET_ACQUIRED = 2;
		private const byte STATE_TARGET_FIRE = 3;

		private byte AI_STATE = 0;
		private int Direction = -1;
		private float addRotation; //same
		private NPC Target;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Basic Laser Drone");
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
			Projectile.DamageType = DamageClass.Summon;
			Projectile.minion = true;
			Projectile.minionSlots = 1f;
		}

		protected override void CustomFrame(int frameCounterMaxFar = 4, int frameCounterMaxClose = 8)
		{
			if (AI_STATE == STATE_TARGET_FIRE)
			{
				Projectile.frame = 2;
			}
			else if (AI_STATE == STATE_TARGET_FOUND || AI_STATE == STATE_TARGET_ACQUIRED)
			{
				Projectile.frame = 1;
			}
			else
			{
				Projectile.frame = 0;
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D image = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Rectangle bounds = image.Frame(1, Main.projFrames[Projectile.type], frameY: Projectile.frame);

			SpriteEffects effects = Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

			Vector2 stupidOffset = new Vector2(Projectile.width / 2, (Projectile.height - 8f) + sinY);
			Vector2 drawPos = Projectile.position - Main.screenPosition + stupidOffset;
			Vector2 drawOrigin = bounds.Size() / 2;

			Main.EntitySpriteDraw(image, drawPos, bounds, lightColor, Projectile.rotation, drawOrigin, 1f, effects, 0);

			image = Mod.Assets.Request<Texture2D>(nameGlow).Value;
			Main.EntitySpriteDraw(image, drawPos, bounds, Color.White, Projectile.rotation, drawOrigin, 1f, effects, 0);

			Vector2 rotationOffset = new Vector2(0f, -2f); //-2f
			drawPos += rotationOffset + new Vector2(0f, 2f); //lower sprite is offset by 2 because its too low, just a correction
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

		protected override bool ModifyDefaultAI(ref bool staticDirection, ref bool reverseSide, ref float veloXToRotationFactor, ref float veloSpeed, ref float offsetX, ref float offsetY)
		{
			if (AI_STATE == STATE_TARGET_FIRE)
			{
				Vector2 between = Target.Center - Projectile.Center;
				//between.Length(): 100 is "close", 1000 is "edge of screen"
				//15.6f = 1000f / 64f
				float magnitude = Utils.Clamp(between.Length() / 15.6f, 6f, 64f);
				between.Normalize();
				Vector2 offset = between * magnitude;
				offset.Y *= 0.5f;
				offsetX += offset.X;
				offsetY += (offset.Y > 0) ? -(32 - offset.Y) : 0;
			}
			return true;
		}

		protected override void ModifyDroneControllerHeld(ref float dmgModifier, ref float kbModifier)
		{
			if (AI_STATE == STATE_TARGET_FIRE)
			{
				if (Main.rand.NextBool(3)) Counter++;
			}
			dmgModifier = 1.25f;
			kbModifier = 1.25f;
		}

		private string GetState(byte status)
		{
			switch (status)
			{
				case STATE_IDLE: return "idle";
				case STATE_TARGET_FOUND: return "found";
				case STATE_TARGET_ACQUIRED: return "aquired";
				case STATE_TARGET_FIRE: return "fire";
			}
			return "";
		}

		protected override void CustomAI()
		{
			Player player = Projectile.GetOwner();
			//Main.NewText("State: " + GetState(AI_STATE));
			//Main.NewText("Counter: " + Counter);

			#region Handle State
			int targetIndex = AssAI.FindTarget(Projectile, Projectile.Center, 1000, ignoreTiles: true);
			if (targetIndex != -1)
			{
				if (AI_STATE == STATE_IDLE) AI_STATE = STATE_TARGET_FOUND;
				Target = Main.npc[targetIndex];

				targetIndex = FindClosestTargetBelow(1000);
				if (targetIndex != -1)
				{
					Target = Main.npc[targetIndex];
					if (AI_STATE != STATE_TARGET_FIRE)
					{
						AI_STATE = STATE_TARGET_ACQUIRED;
					}

					if (AI_STATE == STATE_TARGET_ACQUIRED)
					{
						if (Counter > SearchDelay)
						{
							Counter = 0;
							AI_STATE = STATE_TARGET_FIRE;
						}
					}
				}
				else
				{
					Counter = 0;
					AI_STATE = STATE_TARGET_FOUND;
				}
			}
			else
			{
				AI_STATE = STATE_IDLE;
			}

			if (AI_STATE == STATE_IDLE || AI_STATE == STATE_TARGET_FOUND)
			{
				Direction = player.direction;
			}
			else //definitely has a target (may or may not shoot)
			{
				Direction = (Target.Center.X - Projectile.Center.X > 0f).ToDirectionInt();
			}

			if (AI_STATE == STATE_IDLE) Counter = 2 * MinionPos;
			else Counter++;

			Projectile.spriteDirection = Projectile.direction = -Direction;
			#endregion

			if (AI_STATE == STATE_TARGET_FIRE)
			{
				Vector2 shootOffset = new Vector2(Projectile.width / 2 + Projectile.spriteDirection * 4f, (Projectile.height - 2f) + sinY);
				Vector2 shootOrigin = Projectile.position + shootOffset;
				Vector2 target = Target.Center + new Vector2(0f, -5f);

				Vector2 between = target - shootOrigin;
				shootOrigin += Vector2.Normalize(between) * 16f; //roughly tip of turret

				float rotationAmount = between.ToRotation();

				if (Projectile.spriteDirection == 1) //adjust rotation based on direction
				{
					rotationAmount -= MathHelper.Pi;
					if (rotationAmount > MathHelper.TwoPi)
					{
						rotationAmount = -rotationAmount;
					}
				}

				bool canShoot = true;/*shootOrigin.Y < target.Y + Target.height / 2 + 40;*/

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
					if (Counter > AttackDelay)
					{
						Counter = 0;
						if (RealOwner)
						{
							if (targetIndex != -1 && !Collision.SolidCollision(shootOrigin, 1, 1))
							{
								between = target + Target.velocity * 6f - shootOrigin;
								between.Normalize();
								between *= 6f;
#if TML_2022_03
								Projectile.NewProjectile(Projectile.GetProjectileSource_FromThis(), shootOrigin, between, ModContent.ProjectileType<PetDestroyerDroneLaser>(), CustomDmg, CustomKB, Main.myPlayer);
#else
								Projectile.NewProjectile(Projectile.GetSource_FromThis(), shootOrigin, between, ModContent.ProjectileType<PetDestroyerDroneLaser>(), CustomDmg, CustomKB, Main.myPlayer);
#endif

								//projectile.netUpdate = true;
							}
						}
					}
				}
				else
				{
					Counter = 0;
					AI_STATE = STATE_TARGET_ACQUIRED;
				}
			}
			else //if no target, addRotation should go down to projectile.rotation
			{
				//if addRotation is bigger than projectile.rotation by a small margin, reduce it down to projectile.rotation slowly
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


		private int FindClosestTargetBelow(int range = 1000)
		{
			Player player = Projectile.GetOwner();
			int targetIndex = -1;
			float distanceFromTarget = 100000f;
			Vector2 targetCenter = Projectile.Center;
			for (int k = 0; k < Main.maxNPCs; k++)
			{
				NPC npc = Main.npc[k];
				if (npc.CanBeChasedBy())
				{
					float between = Vector2.Distance(npc.Center, Projectile.Center);
					if (((between < range &&
						Vector2.Distance(player.Center, targetCenter) > between && between < distanceFromTarget) || targetIndex == -1) &&
						Projectile.Bottom.Y < npc.Top.Y + 40 && Collision.CanHitLine(Projectile.Center, 1, 1, npc.position, npc.width, npc.height))
					{
						distanceFromTarget = between;
						targetCenter = npc.Center;
						targetIndex = k;
					}
				}
			}
			return (distanceFromTarget < range) ? targetIndex : -1;
		}
	}
}
