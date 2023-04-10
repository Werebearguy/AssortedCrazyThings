using AssortedCrazyThings.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Minions.Drones
{
	/// <summary>
	/// Fires a salvo of homing rockets with a long delay
	/// </summary>
	public class MissileDrone : DroneBase
	{
		private static readonly string nameGlow = "Projectiles/Minions/Drones/" + "MissileDrone_Glowmask";

		public const int AttackCooldown = 360; //120 but incremented by 1.5f
		public const int AttackDelay = 60;
		public const int AttackDuration = 60;

		//in cooldown: lamps turn on in order for "charge up"

		private const byte STATE_COOLDOWN = 0;
		private const byte STATE_IDLE = 1;
		private const byte STATE_FIRING = 2;

		private byte AI_STATE = 0;
		private int RocketNumber = 0;

		private Vector2 ShootOrigin
		{
			get
			{
				Vector2 position = Projectile.Top;
				position.Y += sinY + 2f;
				return position;
			}
		}

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Missile Drone");
			Main.projFrames[Projectile.type] = 4;
			Main.projPet[Projectile.type] = true;
			ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
			ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
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
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			base.ReceiveExtraAI(reader);
			AI_STATE = reader.ReadByte();
		}

		protected override void CustomFrame(int frameCounterMaxFar = 4, int frameCounterMaxClose = 8)
		{
			if (AI_STATE == STATE_FIRING)
			{
				if (RocketNumber > 0)
				{
					Projectile.frame = 3;
				}
				else
				{
					Projectile.frame = 2;
				}
			}
			else if (AI_STATE == STATE_COOLDOWN)
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

		protected override void CustomAI()
		{
			Player player = Projectile.GetOwner();
			//Main.NewText("##");
			//Main.NewText(AI_STATE);

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
				if (Counter > AttackDelay)
				{
					Counter = 0;
					int targetIndex = AssAI.FindTarget(Projectile, Projectile.Center, 900);
					if (targetIndex != -1)
					{
						Vector2 aboveCheck = new Vector2(0, -16 * 8);
						if (Collision.CanHitLine(ShootOrigin, 1, 1, ShootOrigin + aboveCheck, 1, 1) &&
							Collision.CanHitLine(ShootOrigin + aboveCheck, 1, 1, ShootOrigin + aboveCheck + new Vector2(-16 * 5, 0), 1, 1) &&
							Collision.CanHitLine(ShootOrigin + aboveCheck, 1, 1, ShootOrigin + aboveCheck + new Vector2(16 * 5, 0), 1, 1))
						{
							//Main.NewText("Change from idle to firing");
							AI_STATE = STATE_FIRING;
							if (RealOwner) Projectile.netUpdate = true;
						}
					}
					//else stay in idle until target found
				}
			}
			else if (AI_STATE == STATE_FIRING)
			{
				if (Counter > AttackDuration)
				{
					Counter = 0;
					RocketNumber = 0;
					AI_STATE = STATE_COOLDOWN;
					//no sync since this counts down automatically after STATE_FIRING
				}
			}
			#endregion

			if (AI_STATE == STATE_FIRING)
			{
				int targetIndex = AssAI.FindTarget(Projectile, Projectile.Center, 900);

				if (targetIndex != -1)
				{
					Projectile.direction = Projectile.spriteDirection = -(Main.npc[targetIndex].Center.X - player.Center.X > 0f).ToDirectionInt();
					if (RealOwner)
					{
						int firerate = AttackDelay / 4;
						RocketNumber = Counter / firerate;
						if (Counter % firerate == 0 && RocketNumber > 0 && RocketNumber < 4)
						{
							if (!Collision.SolidCollision(ShootOrigin, 1, 1))
							{
								//Main.NewText(Counter);
								Vector2 velocity = new Vector2(Main.rand.NextFloat(-1f, 1f) - Projectile.direction * 0.5f, -5);
								Projectile.NewProjectile(Projectile.GetSource_FromThis(), ShootOrigin, velocity, ModContent.ProjectileType<MissileDroneRocket>(), CustomDmg, CustomKB, Main.myPlayer);
								Projectile.velocity.Y += 2f;
								Projectile.netUpdate = true;
							}
						}
					}
				}
			}

			Counter += Main.rand.Next(1, AI_STATE != STATE_IDLE ? 2 : 3);
		}
	}
}
