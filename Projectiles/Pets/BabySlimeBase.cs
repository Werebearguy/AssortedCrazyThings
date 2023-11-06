using AssortedCrazyThings.Base;
using AssortedCrazyThings.Projectiles.Minions;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Pets
{
	/// <summary>
	/// ai0/1, localai0/1 used
	/// </summary>
	public abstract class BabySlimeBase : AssProjectile
	{
		public const int StuckTimerMax = 3 * 60;
		public int stuckTimer = 0;

		public bool shootSpikes = false;
		private static readonly byte shootDelay = 60; //either +1 or +0 every tick, so effectively every 90 ticks
		public byte flyingFrameSpeed = 6;
		public byte walkingFrameSpeed = 20;
		public float customMinionSlots = 1f;
		public bool alignFront = false;

		public virtual bool UseJumpingFrame => true;

		public bool InAir => Projectile.ai[0] != 0f;

		public sealed override void SetStaticDefaults()
		{
			Main.projFrames[Projectile.type] = UseJumpingFrame ? 7 : 6;
			Main.projPet[Projectile.type] = true;

			if (!Projectile.minion)
			{
				var settings = ProjectileID.Sets.SimpleLoop(0, 2, 6)
					.WithOffset(-4, 0)
					.WhenNotSelected(0, 2, walkingFrameSpeed)
					.WithSpriteDirection(-1)
					.WithCode(SlimePet2);

				if (UseJumpingFrame)
				{
					settings.WhenSelected(2, 1, 6);
				}

				ProjectileID.Sets.CharacterPreviewAnimations[Projectile.type] = settings;
			}

			SafeSetStaticDefaults();
		}

		public static void SlimePet2(Projectile proj, bool walking)
		{
			if (walking)
			{
				float percent = (float)Main.timeForVisualEffects % 30f / 30f;
				float change = Utils.MultiLerp(percent, 0f, 0f, 16f, 20f, 20f, 16f, 0f/*, 0f*/);
				proj.position.Y -= change;

				var babySlime = (BabySlimeBase)proj.ModProjectile;
				if (change == 0f && babySlime.UseJumpingFrame)
				{
					proj.frame = 0;
				}
			}
		}

		public virtual void SafeSetStaticDefaults()
		{

		}

		public sealed override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.BabySlime);
			Projectile.aiStyle = -1; //26
									 //AIType = ProjectileID.BabySlime;
			Projectile.alpha = 50;

			//set those in SafeSetDefaults in the projectile that inherits from this
			//projectile.width = 38;
			//projectile.height = 40;

			Projectile.usesIDStaticNPCImmunity = true;
			Projectile.idStaticNPCHitCooldown = 10;
			Projectile.manualDirectionChange = true;

			flyingFrameSpeed = 6;
			walkingFrameSpeed = 20;

			DrawOffsetX = -14;
			DrawOriginOffsetY = -2;

			SafeSetDefaults();

			Projectile.minionSlots = Projectile.minion ? customMinionSlots : 0f;
		}

		public virtual void SafeSetDefaults()
		{
			//used to set dimensions (if necessary)
			//also used to set projectile.minion
		}

		public override bool MinionContactDamage()
		{
			return Projectile.minion;
		}

		public override void AI()
		{
			BabySlimeAI();
			Draw();
		}

		public void Draw()
		{
			if (InAir)
			{
				int flyingFrameStart = UseJumpingFrame ? 3 : 2;
				int flyingFrameEnd = Main.projFrames[Projectile.type] - 1;

				if (Projectile.velocity.X > 0.5f)
				{
					Projectile.spriteDirection = -1;
				}
				else if (Projectile.velocity.X < -0.5f)
				{
					Projectile.spriteDirection = 1;
				}

				Projectile.frameCounter++;
				if (Projectile.frameCounter > flyingFrameSpeed)
				{
					Projectile.frame++;
					Projectile.frameCounter = 0;
				}
				if (Projectile.frame < flyingFrameStart || Projectile.frame > flyingFrameEnd)
				{
					Projectile.frame = flyingFrameStart;
				}
				Projectile.rotation = Projectile.velocity.X * 0.1f;
			}
			else
			{
				if (Projectile.direction == -1)
				{
					Projectile.spriteDirection = 1;
				}
				if (Projectile.direction == 1)
				{
					Projectile.spriteDirection = -1;
				}

				int walkingFrameEnd = 1;

				if (UseJumpingFrame && Projectile.velocity.Y != 0.4f) //Standing on the ground: Y == 0.4f as per AI
				{
					walkingFrameEnd++;
					Projectile.frameCounter = 0;
					Projectile.frame = 2;
				}

				if (Projectile.velocity.Y >= 0f && Projectile.velocity.Y <= 0.8f)
				{
					if (Projectile.velocity.X == 0f)
					{
						Projectile.frameCounter++;
					}
					else
					{
						Projectile.frameCounter += 3;
					}
				}
				else
				{
					Projectile.frameCounter += 5;
				}

				if (Projectile.frameCounter >= walkingFrameSpeed)
				{
					Projectile.frameCounter -= walkingFrameSpeed;
					Projectile.frame++;
				}

				if (Projectile.frame > walkingFrameEnd)
				{
					Projectile.frame = 0;
				}

				Projectile.rotation = 0f;
			}
		}

		public byte pickedTexture = 1;

		public byte PickedTexture
		{
			get
			{
				return (byte)(pickedTexture - 1);
			}
			set
			{
				pickedTexture = (byte)(value + 1);
			}
		}

		public bool HasTexture
		{
			get
			{
				return PickedTexture != 0;
			}
		}

		public int JumpTimer
		{
			get
			{
				return (int)Projectile.localAI[0];
			}
			set
			{
				Projectile.localAI[0] = value;
			}
		}

		public byte ShootTimer
		{
			get
			{
				return (byte)Projectile.localAI[1];
			}
			set
			{
				Projectile.localAI[1] = value;
			}
		}
		int targetNPC = -1;

		public void BabySlimeAI()
		{
			bool left = false;
			bool right = false;
			bool flag3 = false;
			bool flag4 = false;

			Player player = Projectile.GetOwner();

			int initialOffset = Projectile.minion ? 10 : 25;
			if (!Projectile.minion) Projectile.minionPos = 0;
			int directionalOffset = 40 * (Projectile.minionPos + 1) * player.direction * -alignFront.ToDirectionInt();
			if (player.Center.X < Projectile.Center.X - initialOffset + directionalOffset)
			{
				left = true;
			}
			else if (player.Center.X > Projectile.Center.X + initialOffset + directionalOffset)
			{
				right = true;
			}

			float totalOffset = player.direction * initialOffset + directionalOffset;
			if (Projectile.HandleStuck(player.Center.X - totalOffset, ref stuckTimer, StuckTimerMax))
			{
				Projectile.ai[0] = 1f;
				Projectile.tileCollide = false;
			}

			if (Projectile.ai[1] == 0f)
			{
				int num38 = 500;
				num38 += 40 * Projectile.minionPos;
				if (JumpTimer > 0)
				{
					num38 += 600;
				}

				if (player.rocketDelay2 > 0 && player.wings != 45)
				{
					Projectile.ai[0] = 1f;
				}

				Vector2 center = Projectile.Center;
				float x = player.Center.X - center.X;
				float y = player.Center.Y - center.Y;
				float distanceSQ = x * x + y * y;
				if (distanceSQ > 2000f * 2000f)
				{
					Projectile.Center = player.Center;
				}
				else if (distanceSQ > num38 * num38 || (Math.Abs(y) > 300f && JumpTimer <= 0))
				{
					if (y > 0f && Projectile.velocity.Y < 0f)
					{
						Projectile.velocity.Y = 0f;
					}
					if (y < 0f && Projectile.velocity.Y > 0f)
					{
						Projectile.velocity.Y = 0f;
					}
					Projectile.ai[0] = 1f;
				}
			}

			if (InAir)
			{
				float veloDelta = 0.2f;
				int playerRange = 200;

				Projectile.tileCollide = false;
				float desiredVeloX = player.Center.X - Projectile.Center.X;

				desiredVeloX -= 40 * player.direction;

				bool foundCloseTarget = false;
				int targetIndex = -1;

				if (Projectile.minion)
				{
					float range = 700f;
					for (int k = 0; k < Main.maxNPCs; k++)
					{
						NPC npc = Main.npc[k];
						if (npc.CanBeChasedBy())
						{
							float distance = Math.Abs(player.Center.X - npc.Center.X) + Math.Abs(player.Center.Y - npc.Center.Y);
							if (distance < range)
							{
								if (Collision.CanHit(Projectile.position, Projectile.width, Projectile.height, npc.position, npc.width, npc.height))
								{
									targetIndex = k;
								}
								foundCloseTarget = true;
								break;
							}
						}
					}
				}

				if (!foundCloseTarget)
				{
					desiredVeloX -= 40 * Projectile.minionPos * player.direction;
				}
				if (foundCloseTarget && targetIndex >= 0)
				{
					Projectile.ai[0] = 0f;
				}

				float desiredVeloY = player.Center.Y - Projectile.Center.Y;

				float between = (float)Math.Sqrt(desiredVeloX * desiredVeloX + desiredVeloY * desiredVeloY);
				//float num54 = num52;

				if (between < playerRange && player.velocity.Y == 0f && Projectile.position.Y + Projectile.height <= player.position.Y + player.height && !Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height))
				{
					Projectile.ai[0] = 0f;
					if (Projectile.velocity.Y < -6f)
					{
						Projectile.velocity.Y = -6f;
					}
				}
				if (between < 60f)
				{
					desiredVeloX = Projectile.velocity.X;
					desiredVeloY = Projectile.velocity.Y;
				}
				else
				{
					between = 10f / between;
					desiredVeloX *= between;
					desiredVeloY *= between;
				}

				if (Projectile.velocity.X < desiredVeloX)
				{
					Projectile.velocity.X += veloDelta;
					if (Projectile.velocity.X < 0f)
					{
						Projectile.velocity.X += veloDelta * 1.5f;
					}
				}
				if (Projectile.velocity.X > desiredVeloX)
				{
					Projectile.velocity.X -= veloDelta;
					if (Projectile.velocity.X > 0f)
					{
						Projectile.velocity.X -= veloDelta * 1.5f;
					}
				}
				if (Projectile.velocity.Y < desiredVeloY)
				{
					Projectile.velocity.Y += veloDelta;
					if (Projectile.velocity.Y < 0f)
					{
						Projectile.velocity.Y += veloDelta * 1.5f;
					}
				}
				if (Projectile.velocity.Y > desiredVeloY)
				{
					Projectile.velocity.Y -= veloDelta;
					if (Projectile.velocity.Y > 0f)
					{
						Projectile.velocity.Y -= veloDelta * 1.5f;
					}
				}

				Projectile.direction = (Projectile.velocity.X > 0).ToDirectionInt();
			}
			else
			{
				float offset = 40 * Projectile.minionPos;
				JumpTimer -= 1;
				if (JumpTimer < 0)
				{
					JumpTimer = 0;
				}
				if (Projectile.ai[1] > 0f)
				{
					Projectile.ai[1] -= 1f;
				}
				else
				{
					float targetX = Projectile.position.X;
					float targetY = Projectile.position.Y;
					float distance = 100000f;
					float otherDistance = distance;
					targetNPC = -1;

					//------------------------------------------------------------------------------------
					//DISABLE MINION TARGETING------------------------------------------------------------
					//------------------------------------------------------------------------------------

					if (Projectile.minion)
					{
						NPC ownerMinionAttackTargetNPC2 = Projectile.OwnerMinionAttackTargetNPC;
						if (ownerMinionAttackTargetNPC2 != null && ownerMinionAttackTargetNPC2.CanBeChasedBy())
						{
							float x = ownerMinionAttackTargetNPC2.Center.X;
							float y = ownerMinionAttackTargetNPC2.Center.Y;
							float num94 = Math.Abs(Projectile.Center.X - x) + Math.Abs(Projectile.Center.Y - y);
							if (num94 < distance)
							{
								if (targetNPC == -1 && num94 <= otherDistance)
								{
									otherDistance = num94;
									targetX = x;
									targetY = y;
								}
								if (Collision.CanHit(Projectile.position, Projectile.width, Projectile.height, ownerMinionAttackTargetNPC2.position, ownerMinionAttackTargetNPC2.width, ownerMinionAttackTargetNPC2.height))
								{
									distance = num94;
									targetX = x;
									targetY = y;
									targetNPC = ownerMinionAttackTargetNPC2.whoAmI;
								}
							}
						}
						if (targetNPC == -1)
						{
							for (int k = 0; k < Main.maxNPCs; k++)
							{
								NPC npc = Main.npc[k];
								if (npc.CanBeChasedBy())
								{
									float npcX = npc.Center.X;
									float npcY = npc.Center.Y;
									float between = Math.Abs(Projectile.Center.X - npcX) + Math.Abs(Projectile.Center.Y - npcY);
									if (between < distance)
									{
										if (targetNPC == -1 && between <= otherDistance)
										{
											otherDistance = between;
											targetX = npcX;
											targetY = npcY;
										}
										if (Collision.CanHit(Projectile.position, Projectile.width, Projectile.height, npc.position, npc.width, npc.height))
										{
											distance = between;
											targetX = npcX;
											targetY = npcY;
											targetNPC = k;
										}
									}
								}
							}
						}
					}

					if (targetNPC == -1 && otherDistance < distance)
					{
						distance = otherDistance;
					}

					float num104 = 300f;
					if (Projectile.position.Y > Main.worldSurface * 16.0)
					{
						num104 = 150f;
					}

					if (distance < num104 + offset && targetNPC == -1)
					{
						float num105 = targetX - Projectile.Center.X;
						if (num105 < -5f)
						{
							left = true;
							right = false;
						}
						else if (num105 > 5f)
						{
							right = true;
							left = false;
						}
					}

					//bool flag9 = false;

					if (targetNPC >= 0 && distance < 800f + offset)
					{
						Projectile.friendly = true;
						JumpTimer = 60;
						float distanceX = targetX - Projectile.Center.X;
						if (distanceX < -10f)
						{
							left = true;
							right = false;
						}
						else if (distanceX > 10f)
						{
							right = true;
							left = false;
						}
						if (targetY < Projectile.Center.Y - 100f && distanceX > -50f && distanceX < 50f && Projectile.velocity.Y == 0f)
						{
							float distanceAbsY = Math.Abs(targetY - Projectile.Center.Y);
							//jumping velocities
							if (distanceAbsY < 100f) //120f
							{
								Projectile.velocity.Y = -10f;
							}
							else if (distanceAbsY < 210f)
							{
								Projectile.velocity.Y = -13f;
							}
							else if (distanceAbsY < 270f)
							{
								Projectile.velocity.Y = -15f;
							}
							else if (distanceAbsY < 310f)
							{
								Projectile.velocity.Y = -17f;
							}
							else if (distanceAbsY < 380f)
							{
								Projectile.velocity.Y = -18f;
							}
						}

						if (shootSpikes)
						{
							//PickedTexture * 3 makes it so theres a small offset for minion shooting based on their texture, which means that if you have different slimes out,
							//they don't all shoot in sync
							if (Projectile.owner == Main.myPlayer)
							{
								NPC npc = Main.npc[targetNPC];
								if (ShootTimer > (shootDelay + PickedTexture * 3) && distance < 200f &&
									Collision.CanHit(Projectile.position, Projectile.width, Projectile.height, npc.position, npc.width, npc.height))
								{
									for (int k = 0; k < 3; k++)
									{
										Vector2 velo = new Vector2(k - 1, -2f);
										velo.X *= 1f + Main.rand.Next(-40, 41) * 0.02f;
										velo.Y *= 1f + Main.rand.Next(-40, 41) * 0.02f;
										velo.Normalize();
										velo *= 3f + Main.rand.Next(-40, 41) * 0.01f;
										Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Bottom.Y - 8f, velo.X, velo.Y, ModContent.ProjectileType<SlimePackMinionSpike>(), Projectile.damage / 2, 0f, Main.myPlayer, ai1: PickedTexture);
										ShootTimer = (byte)(PickedTexture * 3);
									}
								}
							}
							if (ShootTimer <= shootDelay + PickedTexture * 3) ShootTimer = (byte)(ShootTimer + Main.rand.Next(2));
						}
					}
					else
					{
						Projectile.friendly = false;
					}
				}

				if (Projectile.ai[1] != 0f)
				{
					left = false;
					right = false;
				}
				else if (JumpTimer == 0)
				{
					Projectile.direction = player.direction;
				}

				Projectile.tileCollide = true;

				float veloXthreshold = 0.2f;
				float maxVeloXthreshold = 6f;

				if (maxVeloXthreshold < Math.Abs(player.velocity.X) + Math.Abs(player.velocity.Y))
				{
					veloXthreshold = 0.3f;
					maxVeloXthreshold = Math.Abs(player.velocity.X) + Math.Abs(player.velocity.Y);
				}

				if (left)
				{
					if (Projectile.velocity.X > -3.5f)
					{
						Projectile.velocity.X -= veloXthreshold;
					}
					else
					{
						Projectile.velocity.X -= veloXthreshold * 0.25f;
					}
				}
				else if (right)
				{
					if (Projectile.velocity.X < 3.5f)
					{
						Projectile.velocity.X += veloXthreshold;
					}
					else
					{
						Projectile.velocity.X += veloXthreshold * 0.25f;
					}
				}
				else
				{
					Projectile.velocity.X *= 0.9f;
					if (Projectile.velocity.X >= 0f - veloXthreshold && Projectile.velocity.X <= veloXthreshold)
					{
						Projectile.velocity.X = 0f;
					}
				}

				int i = (int)Projectile.Center.X / 16;
				int j = (int)Projectile.Center.Y / 16;
				if (left | right)
				{
					if (left)
					{
						i--;
					}
					if (right)
					{
						i++;
					}
					i += (int)Projectile.velocity.X;
					if ((Main.netMode == NetmodeID.Server || Main.sectionManager.TileLoaded(i, j)) && WorldGen.SolidTile(i, j))
					{
						flag4 = true;
					}
				}
				if (player.position.Y + player.height - 8f > Projectile.position.Y + Projectile.height)
				{
					flag3 = true;
				}
				Collision.StepUp(ref Projectile.position, ref Projectile.velocity, Projectile.width, Projectile.height, ref Projectile.stepSpeed, ref Projectile.gfxOffY);

				if ((Main.netMode == NetmodeID.Server || Main.sectionManager.TileLoaded(i, j) && Main.sectionManager.TileLoaded(i, j - 5)) && Projectile.velocity.Y == 0f)
				{
					if (!flag3 && (Projectile.velocity.X < 0f || Projectile.velocity.X > 0f))
					{
						int i2 = (int)Projectile.Center.X / 16;
						int j2 = (int)Projectile.Center.Y / 16 + 1;
						if (left)
						{
							i2--;
						}
						if (right)
						{
							i2++;
						}
						WorldGen.SolidTile(i2, j2);
					}
					if (flag4)
					{
						i = (int)Projectile.Center.X / 16;
						j = (int)Projectile.Bottom.Y / 16 + 1;
						if (WorldGen.SolidTileAllowBottomSlope(i, j) || Main.tile[i, j].IsHalfBlock || Main.tile[i, j].Slope > 0)
						{
							try
							{
								i = (int)Projectile.Center.X / 16;
								j = (int)Projectile.Center.Y / 16;
								if (left)
								{
									i--;
								}
								if (right)
								{
									i++;
								}
								i += (int)Projectile.velocity.X;
								if (!WorldGen.SolidTile(i, j - 1) && !WorldGen.SolidTile(i, j - 2))
								{
									Projectile.velocity.Y = -5.1f;
								}
								else if (!WorldGen.SolidTile(i, j - 2))
								{
									Projectile.velocity.Y = -7.1f;
								}
								else if (WorldGen.SolidTile(i, j - 5))
								{
									Projectile.velocity.Y = -11.1f;
								}
								else if (WorldGen.SolidTile(i, j - 4))
								{
									Projectile.velocity.Y = -10.1f;
								}
								else
								{
									Projectile.velocity.Y = -9.1f;
								}
							}
							catch
							{
								Projectile.velocity.Y = -9.1f;
							}
						}
					}
					else if (left | right)
					{
						Projectile.velocity.Y -= 6f;
					}
				}
				if (Projectile.velocity.X > maxVeloXthreshold)
				{
					Projectile.velocity.X = maxVeloXthreshold;
				}
				if (Projectile.velocity.X < -maxVeloXthreshold)
				{
					Projectile.velocity.X = -maxVeloXthreshold;
				}
				if (Projectile.velocity.X != 0f) Projectile.direction = (Projectile.velocity.X > 0f).ToDirectionInt();
				if (Projectile.velocity.X > veloXthreshold && right)
				{
					Projectile.direction = 1;
				}
				if (Projectile.velocity.X < -veloXthreshold && left)
				{
					Projectile.direction = -1;
				}

				if (Projectile.wet && player.Bottom.Y < Projectile.Bottom.Y && JumpTimer == 0f)
				{
					if (Projectile.velocity.Y > -4f)
					{
						Projectile.velocity.Y -= 0.2f;
					}
					if (Projectile.velocity.Y > 0f)
					{
						Projectile.velocity.Y *= 0.95f;
					}
				}
				else
				{
					Projectile.velocity.Y += 0.4f;
				}
				if (Projectile.velocity.Y > 10f)
				{
					Projectile.velocity.Y = 10f;
				}
			}
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			if (targetNPC != -1)
			{
				NPC target = Main.npc[targetNPC];
				Vector2 toTarget = Projectile.DirectionTo(target.Center);
				fallThrough = target.Center.Y > Projectile.Bottom.Y && Math.Abs(toTarget.X) < 300;
			}
			else
			{
				fallThrough = false;
			}
			return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
		}
	}
}
