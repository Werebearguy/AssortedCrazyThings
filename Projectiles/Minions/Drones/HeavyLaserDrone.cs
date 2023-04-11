using AssortedCrazyThings.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Utilities;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Minions.Drones
{
	/// <summary>
	/// Fires a penetrating laser beam horizontally to the player with a very long delay.
	/// Only recognizes enemies at around the y level of the player
	/// </summary>
	public class HeavyLaserDrone : DroneBase
	{
		private static readonly string nameGlow = "Projectiles/Minions/Drones/" + "HeavyLaserDrone_Glowmask";
		private static readonly string nameOverlay = "Projectiles/Minions/Drones/" + "HeavyLaserDrone_Overlay";

		private const int AttackCooldown = 180;
		private const int RecoilDuration = 60;
		private const int SearchDelay = 90; //60 but incremented 1.5f
		private const int ChargeDelay = 120;
		private const int AnimationDuration = 32;
		private const int AnimationFrameTime = 8;

		private const byte STATE_COOLDOWN = 0;
		private const byte STATE_IDLE = 1;
		private const byte STATE_CHARGE = 2;
		private const byte STATE_RECOIL = 3;

		private byte AI_STATE = 0;
		private byte PosInCharge = 0;
		private int Direction = -1;
		private float InitialDistance = 0;

		private Vector2 BarrelPos
		{
			get
			{
				Vector2 position = Projectile.Bottom;
				position.Y += sinY;
				return position;
			}
		}

		#region overlay
		private int ChargeTimer = 0;

		private bool CanOverlay => ChargeTimer >= AnimationDuration && (Projectile.frame == 3 || Projectile.frame == 7);

		private float OverlayOpacity => (ChargeTimer - AnimationDuration) / (float)byte.MaxValue;

		private bool playedOverheatSound = false;

		private SlotId overheatSound = SlotId.Invalid;

		private void IncreaseCharge()
		{
			if (ChargeTimer < byte.MaxValue) ChargeTimer++;
		}

		private void DecreaseCharge()
		{
			if (ChargeTimer > 0) ChargeTimer--;
		}
		#endregion

		public override void SetStaticDefaults()
		{
			Main.projFrames[Projectile.type] = 8;
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

		public override void SendExtraAI(BinaryWriter writer)
		{
			base.SendExtraAI(writer);
			writer.Write((byte)AI_STATE);
			writer.Write((byte)PosInCharge);
			writer.Write((byte)ChargeTimer);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			base.ReceiveExtraAI(reader);
			AI_STATE = reader.ReadByte();
			PosInCharge = reader.ReadByte();
			ChargeTimer = (int)reader.ReadByte();
		}

		protected override void CustomFrame(int frameCounterMaxFar = 4, int frameCounterMaxClose = 8)
		{
			if (AI_STATE == STATE_CHARGE)
			{
				if (ChargeTimer < AnimationDuration)
				{
					Projectile.frame = ChargeTimer / AnimationFrameTime;
				}
				else
				{
					Projectile.frame = 3;
				}
			}
			else
			{
				if (ChargeTimer <= 0)
				{
					Projectile.frame = 0;
				}
				else if (ChargeTimer < AnimationDuration)
				{
					Projectile.frame = 4 + ChargeTimer / AnimationFrameTime;
				}
				else
				{
					Projectile.frame = 7;
				}
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

			if (CanOverlay)
			{
				image = Mod.Assets.Request<Texture2D>(nameOverlay).Value;
				Main.EntitySpriteDraw(image, drawPos, image.Bounds, Color.White * OverlayOpacity, Projectile.rotation, drawOrigin, 1f, effects, 0);
			}

			return false;
		}

		protected override void ModifyDroneControllerHeld(ref float dmgModifier, ref float kbModifier)
		{
			dmgModifier = 1.1f;
			kbModifier = 1.1f;

			if (AI_STATE == STATE_COOLDOWN)
			{
				Counter += Main.rand.Next(2);
			}
		}

		protected override bool ModifyDefaultAI(ref bool staticDirection, ref bool reverseSide, ref float veloXToRotationFactor, ref float veloSpeed, ref float offsetX, ref float offsetY)
		{
			if (AI_STATE == STATE_CHARGE)
			{
				//offset x = 30 when facing right
				offsetX = Direction == 1 ? -80 : 20;
				offsetX += Math.Sign(offsetX) * PosInCharge * Projectile.width * 1.5f;
				offsetY = 10;
				veloSpeed = 0.5f;
			}
			else if (AI_STATE == STATE_RECOIL)
			{
				//150 to 50 is smooth, distance
				Vector2 pos = new Vector2(offsetX, offsetY) - new Vector2(-30, 20);
				//veloSpeed = (float)Math.Pow((double)Counter / AttackCooldown, 2) + 0.05f;
				Vector2 distanceToTargetVector = (pos + Projectile.GetOwner().Center) - Projectile.Center;
				float distanceToTarget = distanceToTargetVector.Length();
				if (Counter == 0) InitialDistance = distanceToTarget;
				//Main.NewText("proper: " + distanceToTargetVector.Length());
				float magnitude = 1f + distanceToTargetVector.LengthSquared() / (InitialDistance * InitialDistance);
				distanceToTargetVector.Normalize();
				distanceToTargetVector *= magnitude;
				//-(Counter - AttackCooldown / 5) -> goes from 36 to 0
				float accel = Utils.Clamp(-(Counter - 36), 4, 20);
				Projectile.velocity = (Projectile.velocity * (accel - 1) + distanceToTargetVector) / accel;
				Projectile.rotation = Projectile.velocity.X * 0.05f;
				return false;
			}
			return true;
		}

		protected override bool Bobbing()
		{
			if (AI_STATE == STATE_CHARGE)
			{
				sinY = 0;
				return false;
			}
			return true;
		}

		protected override void CustomAI()
		{
			//Main.NewText("State: " + AI_STATE);
			//Main.NewText("Counter: " + Counter);
			//Main.NewText("Opacity: " + ChargeTimer);
			//Main.NewText("Color: " + OverlayColor);

			#region Handle State
			if (AI_STATE == STATE_COOLDOWN)
			{
				if (Counter > AttackCooldown)
				{
					Counter = 0;
					//Main.NewText("Change from cooldown to idle");
					AI_STATE = STATE_IDLE;
					if (RealOwner) Projectile.netUpdate = true;
				}
				//else stay in cooldown and wait for counter to reach 
			}
			else if (AI_STATE == STATE_IDLE)
			{
				if (Counter > SearchDelay)
				{
					Counter = 0;
					int targetIndex = FindClosestHorizontalTarget();
					if (targetIndex != -1)
					{
						PosInCharge = (byte)GetChargePosition();
						//Main.NewText("Change from idle to charge");
						AI_STATE = STATE_CHARGE;
						if (RealOwner) Projectile.netUpdate = true;
					}
					//else stay in idle until target found
				}
			}
			else if (AI_STATE == STATE_RECOIL)
			{
				if (Counter > RecoilDuration)
				{
					Counter = 0;
					AI_STATE = STATE_COOLDOWN;
				}
			}
			#endregion

			Counter += Main.rand.Next(1, AI_STATE != STATE_IDLE ? 2 : 3);

			if (AI_STATE == STATE_CHARGE)
			{
				int targetIndex = FindClosestHorizontalTarget();

				Projectile.spriteDirection = Projectile.direction = -Direction;

				if (Counter <= ChargeDelay)
				{
					//if lose target
					if (RealOwner && targetIndex == -1)
					{
						//Counter = 0;
						//Main.NewText("Change from charge to idle cuz no target");
						AI_STATE = STATE_IDLE;
						Projectile.netUpdate = true;
					}

					float ratio = Counter / (float)ChargeDelay;
					float otherRatio = 1f - ratio;

					//make sound
					if (Projectile.soundDelay <= 0)
					{
						//TODO find some better sound to use
						Projectile.soundDelay = 20;
						float volume = 0.7f + ratio * 0.5f;
						float pitch = -0.1f + ratio * 0.4f;
						SoundEngine.PlaySound(SoundID.Item15 with { Volume = volume, Pitch = pitch }, Projectile.Center);
						//Main.PlaySound(SoundID.Item15.WithVolume(0.7f + (Counter / (float)ChargeDelay) * 0.5f), projectile.position);
						//Main.NewText("volume : " + (0.7f + volumeCounter * 0.1f));
					}

					//spawn dust
					for (int i = 0; i < 3; i++)
					{
						if (Main.rand.NextFloat() < ratio)
						{
							int dustType = 60;
							//if facing left: + Direction * 48
							int height = (int)(9 * otherRatio) + 4;
							Rectangle rect = new Rectangle((int)BarrelPos.X + Direction * (Direction == 1 ? 16 : 48), (int)BarrelPos.Y - height, 32, 2 * height);
							Dust d = Dust.NewDustDirect(rect.TopLeft(), rect.Width, rect.Height, dustType);
							d.noGravity = true;
							d.velocity.X *= 0.75f;
							d.velocity.Y *= (d.position.Y > rect.Center().Y).ToDirectionInt(); //y velocity goes "inwards"
							d.velocity *= 3 * otherRatio;
						}
					}
				}
				else
				{
					if (RealOwner)
					{
						Counter = 0;
						if (targetIndex != -1 && !Collision.SolidCollision(BarrelPos, 1, 1))
						{
							Vector2 velocity = Main.npc[targetIndex].Center - BarrelPos;
							velocity.Normalize();
							velocity *= 10f;
							Projectile.velocity += -velocity * 0.75f; //recoil
							Projectile.NewProjectile(Projectile.GetSource_FromThis(), BarrelPos, velocity, ModContent.ProjectileType<HeavyLaserDroneLaser>(), CustomDmg, CustomKB, Main.myPlayer, 0f, 0f);

							AI_STATE = STATE_RECOIL;
							ChargeTimer = byte.MaxValue;
							Projectile.netUpdate = true;
						}
						if (targetIndex == -1)
						{
							//Main.NewText("Change from charge to idle");
							AI_STATE = STATE_IDLE;
							Projectile.netUpdate = true;
						}
					}
				}
			}

			if (AI_STATE == STATE_CHARGE)
			{
				IncreaseCharge();
			}
			else
			{
				DecreaseCharge();

				if (CanOverlay && Main.rand.NextFloat() < OverlayOpacity * 0.5f)
				{
					Gore gore = Gore.NewGorePerfect(Projectile.GetSource_FromThis(), BarrelPos + new Vector2(Direction == 1 ? -8f : -Projectile.width - 4f, Direction == 1 ? -14f : -16f), Vector2.Zero, Main.rand.Next(61, 64));
					gore.position.X += Main.rand.NextFloat(8);
					gore.scale *= 0.18f;
					gore.velocity *= 0.6f;
				}
			}

			if (AI_STATE == STATE_RECOIL)
			{
				if (overheatSound == SlotId.Invalid && !playedOverheatSound)
				{
					float volume = 1.5f;
					overheatSound = SoundEngine.PlaySound(SoundID.DD2_KoboldIgniteLoop with { Volume = volume, Pitch = 0.1f }, Projectile.position);
					playedOverheatSound = true;
				}
			}
			else
			{
				playedOverheatSound = false;
			}

			if (SoundEngine.TryGetActiveSound(overheatSound, out var sound))
			{
				float f = 0.008f;
				if (sound.Volume > f)
				{
					sound.Volume -= f;
					if (sound.Volume < f)
					{
						sound.Stop();
					}
				}
			}
			else
			{
				overheatSound = SlotId.Invalid;
			}
		}

		private int FindClosestHorizontalTarget()
		{
			Player player = Projectile.GetOwner();
			int targetIndex = -1;
			float distanceFromTarget = 100000f;
			Vector2 targetCenter = player.Center;
			float margin = 200;
			int range = 1000;
			for (int k = 0; k < Main.maxNPCs; k++)
			{
				NPC npc = Main.npc[k];
				if (npc.CanBeChasedBy(Projectile))
				{
					float between = Vector2.Distance(npc.Center, player.Center);
					if (((between < range &&
						Vector2.Distance(player.Center, targetCenter) > between && between < distanceFromTarget) || targetIndex == -1) &&
						 (Collision.CanHitLine(player.Center, 1, 1, npc.position, npc.width, npc.height) || Collision.CanHit(Projectile.Center, 1, 1, npc.position, npc.width, npc.height)))
					{
						distanceFromTarget = between;
						targetCenter = npc.Center;
						targetIndex = k;
					}
				}
			}
			Direction = (targetCenter.X - player.Center.X > 0f).ToDirectionInt();
			float betweenY = targetCenter.Y - player.Top.Y; //bigger margin upwards
															//Main.NewText("betweenY: " + betweenY);
			return (Math.Abs(betweenY) < margin && distanceFromTarget < range) ? targetIndex : -1;
		}

		/// <summary>
		/// Called before switching to STATE_CHARGE. Returns the minionPos for the laser charge
		/// </summary>
		private int GetChargePosition()
		{
			int pos = 0;
			int min = Main.maxProjectiles;
			for (int i = 0; i < Main.maxProjectiles; i++)
			{
				Projectile proj = Main.projectile[i];
				if (proj.active && proj.owner == Projectile.owner && proj.ModProjectile is HeavyLaserDrone h)
				{
					if (h.AI_STATE == STATE_CHARGE)
					{
						byte projPos = h.PosInCharge;
						min = Math.Min(min, projPos);
						if (projPos > pos) pos = projPos;
					}
					//also works on itself but since AI_STATE is only switched to STATE_CHARGE AFTER this gets called it doesn't have an effect
				}
			}
			if (min > 0) return 0;

			return pos + 1;
		}
	}
}
