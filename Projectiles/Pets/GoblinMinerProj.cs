using AssortedCrazyThings.Base;
using AssortedCrazyThings.Base.Chatter.GoblinUnderlings;
using AssortedCrazyThings.Base.Handlers.ProgressionTierHandler;
using AssortedCrazyThings.Base.Handlers.SpawnedNPCHandler;
using AssortedCrazyThings.Items.Weapons;
using AssortedCrazyThings.Projectiles.Minions.GoblinUnderlings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Pets
{
	[Content(ContentType.OtherPets)]
	public class GoblinMinerProj : AssProjectile
	{
		public const int PickFrameCount = 4;
		public const float Gravity = 0.4f;

		public const int StuckTimerMax = 3 * 60;
		public int stuckTimer = 0;

		public int afkTimer = 0;
		public int minionPos = 0; //For overlapping purposes, it reuses the minion ordering

		private bool skipDefaultMovement = false;

		public int AttackState { get; private set; }

		public int MovementState
		{
			get => (int)Projectile.ai[0];
			set => Projectile.ai[0] = value;
		}

		public int Timer
		{
			get => (int)Projectile.ai[1];
			set => Projectile.ai[1] = value;
		}

		public bool IdleOrMoving => MovementState == 0;

		public bool Flying
		{
			get => MovementState == 1;
			set => MovementState = value ? 1 : 0;
		}

		public bool MeleeAttacking
		{
			get => AttackState == 1;
			set => AttackState = value ? 1 : 0;
		}

		public bool RangedAttacking
		{
			get => AttackState == 2;
			set => AttackState = value ? 2 : 0;
		}

		public bool GeneralAttacking
		{
			get => AttackState > 0;
			set
			{
				if (!value)
				{
					//Only allow false to disable attack
					AttackState = 0;
				}
			}
		}

		public int PickFrameNumber
		{
			get => (int)Projectile.localAI[0];
			set => Projectile.localAI[0] = value;
		}

		public Vector2 targetDestination = default;
		public bool hasTargetDestination => targetDestination != default;

		public override void SetStaticDefaults()
		{
			Main.projFrames[Projectile.type] = 20;
			Main.projPet[Projectile.type] = true;
			ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
		}

		public override void SetDefaults()
		{
			Projectile.width = 24;
			Projectile.height = 40;

			Projectile.aiStyle = -1;
			Projectile.DamageType = DamageClass.Summon; //Can deal damage
			Projectile.penetrate = -1;
			Projectile.timeLeft *= 5;
			Projectile.decidesManualFallThrough = true;
			Projectile.manualDirectionChange = true;

			Projectile.usesLocalNPCImmunity = true;
			//TODO miner adjust swing speed (and hit speed) dynamically based on pick power
			Projectile.localNPCHitCooldown = 18; //Facilitates attack 5 * 4 duration (default pirates), dynamic in AI
		}

		public override bool PreDraw(ref Color lightColor)
		{
			//Custom draw to just center on the hitbox
			Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
			Rectangle sourceRect = texture.Frame(1, Main.projFrames[Projectile.type], 0, Projectile.frame);
			Vector2 drawOrigin = sourceRect.Size() / 2f;

			SpriteEffects spriteEffects = Projectile.spriteDirection > 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

			Vector2 center = Projectile.position + Projectile.Size / 2f + new Vector2(0, Projectile.gfxOffY + 4f - 2f) - Main.screenPosition;
			Color color = lightColor;

			float rotation = Projectile.rotation;
			float scale = Projectile.scale;
			Main.EntitySpriteDraw(texture, center, sourceRect, color, rotation, drawOrigin, scale, spriteEffects, 0);

			if (MeleeAttacking)
			{
				texture = ModContent.Request<Texture2D>(Texture + "_Pick").Value;
				sourceRect = texture.Frame(1, PickFrameCount, 0, PickFrameNumber);
				drawOrigin = sourceRect.Size() / 2f;
				Main.EntitySpriteDraw(texture, center, sourceRect, color, rotation, drawOrigin, scale, spriteEffects, 0);
			}

			return false;
		}

		public override void ModifyDamageHitbox(ref Rectangle hitbox)
		{
			//Save original coordinates
			int centerX = hitbox.Center.X;
			int bottomY = hitbox.Bottom;

			int increase = 0; //TODO look at meleeAttackHitboxIncrease for sensible stats
			hitbox.Inflate(increase, increase / 2); //Top shouldn't grow as much, as its only going to grow upwards

			//Restore coordinates
			hitbox.Y = bottomY - hitbox.Height;
			hitbox.X = centerX - hitbox.Width / 2;
		}

		public override bool MinionContactDamage()
		{
			return true;
		}

		public override bool PreAI()
		{
			Player player = Projectile.GetOwner();
			PetPlayer petPlayer = player.GetModPlayer<PetPlayer>();
			if (player.dead)
			{
				petPlayer.GoblinMiner = false;
			}
			if (petPlayer.GoblinMiner)
			{
				Projectile.timeLeft = 2;
			}

			GoblinUnderlingPlayer goblinPlayer = player.GetModPlayer<GoblinUnderlingPlayer>();
			minionPos = goblinPlayer.numUnderlings;
			goblinPlayer.numUnderlings++;

			//Has to be in PreAI so that damage works in AI properly
			SetScaledDamage(player);

			//TODO miner debugging code
			if (Main.mouseRight && Main.mouseRightRelease)
			{
				if (!hasTargetDestination)
				{
					targetDestination = Main.MouseWorld;
				}
				else if (Utils.CenteredRectangle(targetDestination, new Vector2(20)).Contains(Main.MouseWorld.ToPoint()))
				{
					targetDestination = default;
				}
			}

			if (hasTargetDestination)
			{
				Dust.QuickDust(targetDestination.ToTileCoordinates(), Color.White);
			}

			return true;
		}

		public override void AI()
		{
			Player player = Projectile.GetOwner();

			SetFrame(); //Has to be before due to velocity checks and attacking overriding frame

			MinerAI(player);

			Projectile.localNPCHitCooldown = GetNextTimerValue(PickFrameCount) - 2;
		}

		private void SetScaledDamage(Player player)
		{
			//TODO miner set damage based on pickaxe divided by 3
			Projectile.damage = 5;
		}

		private int GetNextTimerValue(int attackFrameCount)
		{
			//TODO miner
			int time = 6; //to 4 with 200 pick power;

			return time * attackFrameCount;
		}

		private void MinerAI(Player player)
		{
			skipDefaultMovement = false;

			//if target is outside of meleeAttackRange (but inside globalAttackRange), minion stops moving horizontally
			//on the edge of the meleeAttackRange, then initiates attacking behavior but with ranged projectiles instead of melee
			int meleeAttackRange = 400; //25 * 16 = 400
			float awayDistMax = 500f;
			float awayDistYMax = 400f; //300, increased to reduce amount of "bouncing" when player is standing on far up tiles or hooked up
			Vector2 destination = GoblinUnderlingProj.GetIdleLocation(Projectile, player, minionPos);

			if (Timer == 0 && Projectile.HandleStuck(destination.X, ref stuckTimer, StuckTimerMax))
			{
				Flying = true;
				Projectile.tileCollide = false;
			}

			Projectile.shouldFallThrough = player.Bottom.Y - 12f > Projectile.Bottom.Y;
			Projectile.friendly = false;
			int pickCooldown = 0;
			int pickFrameCount = PickFrameCount;
			int nextTimerValue = GetNextTimerValue(pickFrameCount);

			if (hasTargetDestination)
			{
				destination = targetDestination;

				if (Projectile.Hitbox.Contains(destination.ToPoint()))
				{
					MeleeAttacking = true;
				}

				PickDestinationAndAttack(meleeAttackRange, 300, pickFrameCount, destination);
			}

			GoFlyingPrematurely(player, awayDistMax, awayDistYMax, hasTargetDestination);

			if (GeneralAttacking && Timer < 0)
			{
				Projectile.friendly = false;
				if (AttackCoolup(nextTimerValue))
				{
					//TODO implement when needed
					//return;
				}
			}
			else if (GeneralAttacking)
			{
				AttackAction(pickFrameCount, nextTimerValue, destination);

				if (AttackCooldown(pickCooldown))
				{
					//Stay in attack mode if possible
					if (hasTargetDestination)
					{
						PickDestinationAndAttack(meleeAttackRange, 300, pickFrameCount, destination);
					}
				}
			}

			if (!skipDefaultMovement)
			{
				if (IdleOrMoving)
				{
					GroundedMovement(player, 0.5f, awayDistMax, awayDistYMax, ref destination, hasTargetDestination);
				}
				else if (Flying)
				{
					FlyingMovement(player);
				}
			}

			Projectile.spriteDirection = -Projectile.direction;
		}

		private void AttackAction(int pickFrameCount, int nextTimerValue, Vector2 destination)
		{
			MiningAnimation(pickFrameCount, nextTimerValue);

			if (MeleeAttacking)
			{
				Flying = false;
				Projectile.friendly = true;
				if (hasTargetDestination && Timer > nextTimerValue * 0.8f)
				{
					if (Projectile.Hitbox.Contains(destination.ToPoint()))
					{
						Projectile.velocity.X *= Projectile.velocity.Y == 0f ? 0.75f : 0.85f;
					}
				}
			}
		}

		private void PickDestinationAndAttack(int meleeAttackRange, int rangedAttackRangeFromProj, int attackFrameCount, Vector2 destination)
		{
			bool oldAttacking = GeneralAttacking;
			if (hasTargetDestination)
			{
				//DecideAttack(meleeAttackRange, rangedAttackRangeFromProj, npc, projDistance, playerDistance, out bool allowJump);

				bool allowJump = true;
				if (allowJump)
				{
					bool canJump = Projectile.velocity.Y == 0f;
					if (Projectile.wet && Projectile.velocity.Y > 0f && !Projectile.shouldFallThrough)
					{
						canJump = true;
					}

					//TODO miner if line of sight:
					if (destination.Y < Projectile.Center.Y - 20f && canJump)
					{
						float num25 = (destination.Y - Projectile.Center.Y) * -1f;
						float num26 = Gravity;
						float velY = (float)Math.Sqrt(num25 * 2f * num26);
						if (velY > 26f)
						{
							velY = 26f;
						}

						Projectile.velocity.Y = -velY;
					}
					//Else do the terrain checking in GroundedMovement from the try block
				}

				//If an attack was decided
				if (!oldAttacking && GeneralAttacking)
				{
					Timer = GetNextTimerValue(attackFrameCount);

					Projectile.netUpdate = true;
					Projectile.direction = (destination.X - Projectile.Center.X >= 0f).ToDirectionInt();
				}
			}
		}

		private void DecideAttack(int meleeAttackRange, int rangedAttackRangeFromProj, NPC npc, float projDistance, float playerDistance, out bool allowJump)
		{
			allowJump = false;
			float toTargetMaxDist = 20f;

			//If target in range, handle fallthrough, jumping, and deciding when to initiate attack
			Projectile.shouldFallThrough = npc.Center.Y > Projectile.Bottom.Y;

			//only go into melee if NPC is either grounded, or 4 blocks above standable ground
			bool canGoMelee = npc.velocity.Y == 0f;
			if (!canGoMelee)
			{
				int tilesToCheck = 4;
				int npcY = (int)npc.Bottom.Y / 16;
				int x = (int)npc.Bottom.X / 16;

				bool atleastOneSolid = false;
				for (int y = npcY; y < npcY + tilesToCheck + 1; y++)
				{
					//If atleast one of the tiles below the NPC are active and walkable
					if (WorldGen.InWorld(x, y) && WorldGen.ActiveAndWalkableTile(x, y))
					{
						atleastOneSolid = true;
						break;
					}
				}

				if (atleastOneSolid)
				{
					canGoMelee = true;
				}
			}

			canGoMelee &= !Flying;

			if ((playerDistance <= meleeAttackRange || projDistance < 120f) && canGoMelee)
			{
				//Melee range
				allowJump = true;

				if (projDistance < toTargetMaxDist || npc.Hitbox.Intersects(Projectile.Hitbox))
				{
					float len = Projectile.velocity.Length();
					if (len > 10f)
					{
						Projectile.velocity /= len / 10f;
					}

					MeleeAttacking = true;
				}
			}
			else
			{
				//Ranged range
				if (npc.noTileCollide || npc.noGravity)
				{
					//Should not try going through platforms if NPC is flying
					Projectile.shouldFallThrough = false;
				}

				int rangedRange = rangedAttackRangeFromProj;
				rangedRange += minionPos * Projectile.width;
				//If enemy too far up, jump to try hitting it
				//This causes "deadzone" where no ranged attack takes place before jumping is allowed. jumping in this section doesn't make much sense tho as it violates the ranged attack range anyway
				//if (npc.Bottom.Y + rangedRange * 1.2f < Projectile.Center.Y)
				//{
				//	allowJump = true;
				//}


				if (projDistance < rangedRange && Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, npc.position, npc.width, npc.height))
				{
					RangedAttacking = true;
				}
			}
		}

		private void GroundedMovement(Player player, float movementSpeedMult, float awayDistMax, float awayDistYMax, ref Vector2 destination, bool hasDestination)
		{
			//Here destination can either be player or NPC
			if (!hasTargetDestination)
			{
				if (Projectile.Distance(player.Center) > 60f && Projectile.Distance(destination) > 60f && Math.Sign(destination.X - player.Center.X) != Math.Sign(Projectile.Center.X - player.Center.X))
				{
					destination = player.Center;
				}

				Rectangle rect = Utils.CenteredRectangle(destination, Projectile.Size);
				for (int i = 0; i < 20; i++)
				{
					if (Collision.SolidCollision(rect.TopLeft(), rect.Width, rect.Height))
					{
						break;
					}

					rect.Y += 16;
					destination.Y += 16f;
				}

				Vector2 position = player.Center - Projectile.Size / 2f;
				Vector2 postCollision = Collision.TileCollision(position, destination - player.Center, Projectile.width, Projectile.height);
				destination = position + postCollision;
				if (Projectile.Distance(destination) < 32f)
				{
					float distPlayerToDestination = player.Distance(destination);
					if (player.Distance(Projectile.Center) < distPlayerToDestination)
					{
						destination = Projectile.Center;
					}
				}

				Vector2 fromDestToPlayer = player.Center - destination;
				if (fromDestToPlayer.Length() > awayDistMax || Math.Abs(fromDestToPlayer.Y) > awayDistYMax)
				{
					Rectangle rect2 = Utils.CenteredRectangle(player.Center, Projectile.Size);
					Vector2 fromPlayerToDest = destination - player.Center;
					Vector2 topLeft = rect2.TopLeft();
					for (float i = 0f; i < 1f; i += 0.05f)
					{
						Vector2 newTopLeft = rect2.TopLeft() + fromPlayerToDest * i;
						if (Collision.SolidCollision(rect2.TopLeft() + fromPlayerToDest * i, rect2.Width, rect2.Height))
						{
							break;
						}

						topLeft = newTopLeft;
					}

					destination = topLeft + Projectile.Size / 2f;
				}
			}

			Projectile.tileCollide = true;
			float velXChange = 0.5f; //0.5f
			float velXChangeMargin = 4f; //4f
			float velXChangeMax = 4f; //4f
			float velXChangeSmall = 0.1f;

			if (hasTargetDestination)
			{
				//Formula: margin/max is 12.5 x change
				velXChange = 1f * movementSpeedMult;
				velXChangeMargin = velXChange * 12.5f;
				velXChangeMax = velXChangeMargin;

				//velXChange = 0.4f; //1f
				//velXChangeMargin = 5f; //8f
				//velXChangeMax = 5f; //8f
			}

			if (velXChangeMax < Math.Abs(player.velocity.X) + Math.Abs(player.velocity.Y))
			{
				velXChangeMax = Math.Abs(player.velocity.X) + Math.Abs(player.velocity.Y);
				velXChange = 0.7f;
			}

			int xOff = 0;
			bool canJumpOverTiles = false;
			float toDestinationX = destination.X - Projectile.Center.X;
			Vector2 toDestination = destination - Projectile.Center;
			if (Math.Abs(toDestinationX) > 5f)
			{
				if (toDestinationX < 0f)
				{
					xOff = -1;
					if (Projectile.velocity.X > -velXChangeMargin)
					{
						Projectile.velocity.X -= velXChange;
					}
					else
					{
						Projectile.velocity.X -= velXChangeSmall;
					}
				}
				else
				{
					xOff = 1;
					if (Projectile.velocity.X < velXChangeMargin)
					{
						Projectile.velocity.X += velXChange;
					}
					else
					{
						Projectile.velocity.X += velXChangeSmall;
					}
				}
			}
			else
			{
				Projectile.velocity.X *= 0.9f;
				if (Math.Abs(Projectile.velocity.X) < velXChange * 2f)
				{
					Projectile.velocity.X = 0f;
				}
			}

			bool tryJumping = Math.Abs(toDestination.X) >= 64f || (toDestination.Y <= -48f && Math.Abs(toDestination.X) >= 8f);
			if (xOff != 0 && tryJumping)
			{
				int x = (int)Projectile.Center.X / 16;
				int startY = (int)Projectile.position.Y / 16;
				x += xOff;
				x += (int)Projectile.velocity.X;
				for (int y = startY; y < startY + Projectile.height / 16 + 1; y++)
				{
					if (WorldGen.SolidTile(x, y))
					{
						canJumpOverTiles = true;
					}
				}
			}

			Collision.StepUp(ref Projectile.position, ref Projectile.velocity, Projectile.width, Projectile.height, ref Projectile.stepSpeed, ref Projectile.gfxOffY);
			float nextVelocityY = Utils.GetLerpValue(0f, 100f, toDestination.Y, clamped: true) * Utils.GetLerpValue(-2f, -6f, Projectile.velocity.Y, clamped: true);
			if (Projectile.velocity.Y == 0f && canJumpOverTiles)
			{
				for (int k = 0; k < 3; k++)
				{
					int x = (int)(Projectile.Center.X) / 16;
					if (k == 0)
					{
						x = (int)Projectile.Left.X / 16;
					}

					if (k == 2)
					{
						x = (int)(Projectile.Right.X) / 16;
					}

					int y = (int)(Projectile.Bottom.Y) / 16;
					if (!WorldGen.SolidTile(x, y) && !Main.tile[x, y].IsHalfBlock && Main.tile[x, y].Slope <= 0 && (!TileID.Sets.Platforms[Main.tile[x, y].TileType] || !Main.tile[x, y].HasTile || Main.tile[x, y].IsActuated))
					{
						continue;
					}

					try
					{
						x = (int)(Projectile.Center.X) / 16;
						y = (int)(Projectile.Center.Y) / 16;
						x += xOff;
						x += (int)Projectile.velocity.X;
						if (!WorldGen.SolidTile(x, y - 1) && !WorldGen.SolidTile(x, y - 2))
						{
							Projectile.velocity.Y = -5.1f;
						}
						else if (!WorldGen.SolidTile(x, y - 2))
						{
							Projectile.velocity.Y = -7.1f;
						}
						else if (WorldGen.SolidTile(x, y - 5))
						{
							Projectile.velocity.Y = -11.1f;
						}
						else if (WorldGen.SolidTile(x, y - 4))
						{
							Projectile.velocity.Y = -10.1f;
						}
						else
							Projectile.velocity.Y = -9.1f;
					}
					catch
					{
						Projectile.velocity.Y = -9.1f;
					}
				}

				if (destination.Y - Projectile.Center.Y < -48f)
				{
					float height = destination.Y - Projectile.Center.Y;
					height *= -1f;
					if (height < 60f)
					{
						Projectile.velocity.Y = -6f;
					}
					else if (height < 80f)
					{
						Projectile.velocity.Y = -7f;
					}
					else if (height < 100f)
					{
						Projectile.velocity.Y = -8f;
					}
					else if (height < 120f)
					{
						Projectile.velocity.Y = -9f;
					}
					else if (height < 140f)
					{
						Projectile.velocity.Y = -10f;
					}
					else if (height < 160f)
					{
						Projectile.velocity.Y = -11f;
					}
					else if (height < 190f)
					{
						Projectile.velocity.Y = -12f;
					}
					else if (height < 210f)
					{
						Projectile.velocity.Y = -13f;
					}
					else if (height < 270f)
					{
						Projectile.velocity.Y = -14f;
					}
					else if (height < 310f)
					{
						Projectile.velocity.Y = -15f;
					}
					else
					{
						Projectile.velocity.Y = -16f;
					}
				}

				if (Projectile.wet && nextVelocityY == 0f)
				{
					Projectile.velocity.Y *= 2f;
				}
			}

			if (Projectile.velocity.X > velXChangeMax)
			{
				Projectile.velocity.X = velXChangeMax;
			}

			if (Projectile.velocity.X < -velXChangeMax)
			{
				Projectile.velocity.X = -velXChangeMax;
			}

			if (Projectile.velocity.X < 0f)
			{
				Projectile.direction = -1;
			}

			if (Projectile.velocity.X > 0f)
			{
				Projectile.direction = 1;
			}

			if (Projectile.velocity.X == 0f)
			{
				Projectile.direction = (player.Center.X > Projectile.Center.X).ToDirectionInt();
			}

			if (Projectile.velocity.X > velXChange && xOff == 1)
			{
				Projectile.direction = 1;
			}

			if (Projectile.velocity.X < -velXChange && xOff == -1)
			{
				Projectile.direction = -1;
			}

			Projectile.velocity.Y += Gravity + nextVelocityY * 1f;
			if (Projectile.velocity.Y > 10f)
			{
				Projectile.velocity.Y = 10f;
			}
		}

		private void FlyingMovement(Player player)
		{
			Projectile.tileCollide = false;
			float velChange = 0.2f;
			float toPlayerSpeed = 10f;
			int maxLen = 200;
			if (toPlayerSpeed < Math.Abs(player.velocity.X) + Math.Abs(player.velocity.Y))
			{
				toPlayerSpeed = Math.Abs(player.velocity.X) + Math.Abs(player.velocity.Y);
			}

			Vector2 toPlayer = player.Center - Projectile.Center;
			float len = toPlayer.Length();

			AssAI.TeleportIfTooFar(Projectile, player.Center);
			AssAI.FixProjectileOverlap(Projectile, 1f, 0.05f, GoblinUnderlingTierSystem.GoblinUnderlingProjs.Keys.ToArray());

			if (len < maxLen && player.velocity.Y == 0f && Projectile.Bottom.Y <= player.Bottom.Y && !Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height))
			{
				//Reset back from flying
				Flying = false;
				Projectile.netUpdate = true;
				if (Projectile.velocity.Y < -6f)
				{
					Projectile.velocity.Y = -6f;
				}
			}

			if (!(len < 60f))
			{
				toPlayer.Normalize();
				toPlayer *= toPlayerSpeed;
				if (Projectile.velocity.X < toPlayer.X)
				{
					Projectile.velocity.X += velChange;
					if (Projectile.velocity.X < 0f)
					{
						Projectile.velocity.X += velChange * 1.5f;
					}
				}

				if (Projectile.velocity.X > toPlayer.X)
				{
					Projectile.velocity.X -= velChange;
					if (Projectile.velocity.X > 0f)
					{
						Projectile.velocity.X -= velChange * 1.5f;
					}
				}

				if (Projectile.velocity.Y < toPlayer.Y)
				{
					Projectile.velocity.Y += velChange;
					if (Projectile.velocity.Y < 0f)
					{
						Projectile.velocity.Y += velChange * 1.5f;
					}
				}

				if (Projectile.velocity.Y > toPlayer.Y)
				{
					Projectile.velocity.Y -= velChange;
					if (Projectile.velocity.Y > 0f)
					{
						Projectile.velocity.Y -= velChange * 1.5f;
					}
				}
			}

			if (Projectile.velocity.X != 0f && !GeneralAttacking)
			{
				Projectile.direction = Math.Sign(Projectile.velocity.X);
			}
		}

		private void ResetAttack()
		{
			Timer = 0;
			GeneralAttacking = false;
			Projectile.netUpdate = true;
		}

		private bool AttackCoolup(int nextTimerValue)
		{
			Timer += 1;
			if (nextTimerValue >= 0)
			{
				ResetAttack();
				return true;
			}

			return false;
		}

		private bool AttackCooldown(int pickCooldown)
		{
			Timer -= 1;
			if (Timer <= 0)
			{
				if (pickCooldown <= 0)
				{
					ResetAttack();
					return true;
				}

				Timer = -pickCooldown;
			}

			return false;
		}

		private void GoFlyingPrematurely(Player player, float awayDistMax, float awayDistYMax, bool hasDestination)
		{
			if (IdleOrMoving && !hasDestination)
			{
				if (player.rocketDelay2 > 0 && player.wings != 45)
				{
					Flying = true;
					Projectile.netUpdate = true;
				}

				Vector2 toPlayer = player.Center - Projectile.Center;
				if (!AssAI.TeleportIfTooFar(Projectile, player.Center) && toPlayer.Length() > awayDistMax || Math.Abs(toPlayer.Y) > awayDistYMax)
				{
					Flying = true;
					Projectile.netUpdate = true;
					if (Projectile.velocity.Y > 0f && toPlayer.Y < 0f)
					{
						Projectile.velocity.Y = 0f;
					}

					if (Projectile.velocity.Y < 0f && toPlayer.Y > 0f)
					{
						Projectile.velocity.Y = 0f;
					}
				}
			}
		}

		private void MiningAnimation(int pickFrameCount, int nextTimerValue)
		{
			Projectile.spriteDirection = -Projectile.direction;
			Projectile.rotation = 0f;

			int pickAttackFrame = 12;
			bool hasJumpingAttackFrames = true;
			PickFrameNumber = Utils.Clamp((int)(((float)nextTimerValue - Timer) / ((float)nextTimerValue / pickFrameCount)), 0, pickFrameCount - 1);
			Projectile.frame = pickAttackFrame + PickFrameNumber;

			if (hasJumpingAttackFrames && (Projectile.velocity.Y != 0f || Flying))
			{
				Projectile.frame += pickFrameCount;
			}
		}

		private void SetFrame()
		{
			//Idle, Jump, Walk, Walk, Walk, Walk, Walk, Walk, Walk, Walk, Walk, Walk, Mine, Mine, Mine, Mine. Jump Attack, Jump Attack, Jump Attack, Jump Attack
			if (Flying)
			{
				//Flying animation
				Projectile.frameCounter = 0;
				Projectile.frame = 1;

				Projectile.rotation = 0f;

				//Propulsion dust
				float dustChance = Math.Clamp(Math.Abs(Projectile.velocity.Length()) / 3f, 0.5f, 1f);
				if (Main.rand.NextFloat() < dustChance)
				{
					int dirOffset = 0;
					if (Projectile.direction == -1)
					{
						dirOffset = -4;
					}
					Vector2 dustOrigin = Projectile.Bottom + new Vector2(dirOffset, -2) - Projectile.velocity.SafeNormalize(Vector2.Zero) * 2;

					for (int i = 0; i < 2; i++)
					{
						Vector2 bootOffset = new Vector2((i == 0).ToDirectionInt() * 5, 0);
						Dust dust = Dust.NewDustDirect(dustOrigin - Vector2.One * 2f + bootOffset, 4, 4, DustID.Cloud, -Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f, 50, default(Color), 1.3f);
						dust.velocity.X *= 0.2f;
						dust.velocity.Y *= 0.2f;
						dust.noGravity = true;
					}
				}
			}
			else
			{
				Projectile.rotation = 0f;
				if (Projectile.velocity.Y == 0f)
				{
					if (Projectile.velocity.X == 0f)
					{
						Projectile.frame = 0;
						Projectile.frameCounter = 0;
					}
					else if (Math.Abs(Projectile.velocity.X) >= 0.5f)
					{
						Projectile.frameCounter += (int)Math.Abs(Projectile.velocity.X);
						Projectile.frameCounter++;
						if (Projectile.frameCounter > 10)
						{
							Projectile.frame++;
							Projectile.frameCounter = 0;
						}

						if (Projectile.frame > 11 || Projectile.frame < 2)
						{
							Projectile.frame = 2;
						}
					}
					else
					{
						Projectile.frame = 0;
						Projectile.frameCounter = 5;
					}
				}
				else if (Projectile.velocity.Y != 0f)
				{
					Projectile.frameCounter = 0;
					Projectile.frame = 1;
				}
			}
		}
	}
}
