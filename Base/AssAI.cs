using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Base
{
	/// <summary>
	/// contains AI for stuff that only uses ai[], used with thing.aiStyle = -1
	/// </summary>
	public static class AssAI
	{
		/// <summary>
		/// Makes the projectile teleport if it is too far away from the given location
		/// </summary>
		public static bool TeleportIfTooFar(Projectile projectile, Vector2 desiredCenter, int distance = 2000)
		{
			if (projectile.DistanceSQ(desiredCenter) > distance * distance)
			{
				projectile.Center = desiredCenter;
				if (Main.myPlayer == projectile.owner) projectile.netUpdate = true;
				return true;
			}
			return false;
		}

		/// <summary>
		/// Finds target in range of relativeCenter. Returns index of target
		/// </summary>
		public static int FindTarget(Entity ent, Vector2 relativeCenter, float range, bool ignoreTiles = false)
		{
			int targetIndex = -1;
			float distanceFromTarget = 10000000f;
			Vector2 targetCenter = relativeCenter;
			range *= range;
			for (int k = 0; k < Main.maxNPCs; k++)
			{
				NPC npc = Main.npc[k];
				if (npc.CanBeChasedBy())
				{
					//Collision.CanHitLine(projectile.position, projectile.width, projectile.height, npc.position, npc.width, npc.height)
					float between = Vector2.DistanceSquared(npc.Center, relativeCenter);
					if ((between < range && Vector2.DistanceSquared(relativeCenter, targetCenter) > between && between < distanceFromTarget) || targetIndex == -1)
					{
						if (ignoreTiles || Collision.CanHitLine(ent.position, ent.width, ent.height, npc.position, npc.width, npc.height))
						{
							distanceFromTarget = between;
							targetCenter = npc.Center;
							targetIndex = k;
						}
					}
				}
			}
			return distanceFromTarget < range ? targetIndex : -1;
		}

		/// <summary>
		/// Utility for minion AI that handles a custom stuck timer for when the X coordinate doesn't change for a given duration.
		/// </summary>
		/// <param name="projectile">The projectile</param>
		/// <param name="restingX">The X coordinate of the idle location (to not increment the timer when near that)</param>
		/// <param name="stuckTimer">The timer</param>
		/// <param name="stuckTimerMax">The maximum duration</param>
		/// <returns>Returns true if the timer overflows</returns>
		public static bool HandleStuck(this Projectile projectile, float restingX, ref int stuckTimer, int stuckTimerMax)
		{
			if (projectile.position.X == projectile.oldPosition.X && Math.Abs(projectile.Center.X - restingX) >= 50)
			{
				stuckTimer++;

				if (stuckTimer >= stuckTimerMax)
				{
					stuckTimer = 0;
					return true;
				}
			}
			else
			{
				if (stuckTimer >= 2)
				{
					stuckTimer -= 2;
				}
			}

			return false;
		}

		#region Flickerwick
		public static void FlickerwickPetDraw(Projectile projectile, int frameCounterMaxFar, int frameCounterMaxClose)
		{
			projectile.LoopAnimation((projectile.velocity.Length() > 6f) ? frameCounterMaxFar : frameCounterMaxClose);
		}

		/// <summary>
		/// No use of ai[] or LocalAI[].
		/// Default offset is x = 30, y = -20
		/// </summary>
		public static void FlickerwickPetAI(Projectile projectile, bool lightPet = true, bool lightDust = true, bool staticDirection = false, bool reverseSide = false, bool vanityPet = false, float veloXToRotationFactor = 1f, float veloSpeed = 1f, float lightFactor = 1f, Vector3 lightColor = default(Vector3), float offsetX = 0f, float offsetY = 0f)
		{
			//veloSpeed not bigger than veloDistanceChange * 0.5f
			Player player = projectile.GetOwner();
			float veloDistanceChange = 2f; //6f

			int dir = player.direction;
			if (staticDirection)
			{
				if (reverseSide)
				{
					dir = -1;
				}
				else
				{
					dir = 1;
				}
			}
			else
			{
				if (reverseSide)
				{
					dir = -dir;
				}
			}

			//up and down bobbing
			//projectile.localAI[0] += 1f;
			//if (projectile.localAI[0] > 120f)
			//{
			//    projectile.localAI[0] = 0f;
			//}
			//value.Y += (float)Math.Cos((double)(projectile.localAI[0] * 0.05235988f)) * 2f;

			Vector2 dustOffset = new Vector2((projectile.spriteDirection == -1) ? -6 : -2, -20f).RotatedBy(projectile.rotation);

			Vector2 desiredCenterRelative = new Vector2(dir * (offsetX + 30), -20f + offsetY);

			projectile.direction = projectile.spriteDirection = dir;

			//if (reverseSide)
			//{
			//    desiredCenterRelative.X = -desiredCenterRelative.X;
			//    //value2.X = -value2.X;
			//    projectile.direction = -projectile.direction;
			//    projectile.spriteDirection = -projectile.spriteDirection;
			//}

			if (lightDust && Main.rand.Next(24) == 0)
			{
				Dust dust = Dust.NewDustDirect(projectile.Center + dustOffset, 4, 4, 135, 0f, 0f, 100);
				if (Main.rand.Next(3) != 0)
				{
					dust.noGravity = true;
					dust.velocity.Y += -3f;
					dust.noLight = true;
				}
				else if (Main.rand.Next(2) != 0)
				{
					dust.noLight = true;
				}
				dust.velocity *= 0.5f;
				dust.velocity.Y += -0.9f;
				dust.scale += 0.1f + Main.rand.NextFloat() * 0.6f;
				dust.shader = GameShaders.Armor.GetSecondaryShader(Main.GetProjectileDesiredShader(projectile.whoAmI), player);
			}

			if (lightPet)
			{
				if (lightColor == default(Vector3)) lightColor = new Vector3(0.3f, 0.5f, 1f);
				//flickerwick is new Vector3(0.3f, 0.5f, 1f)
				Vector3 vector = DelegateMethods.v3_1 = lightColor * lightFactor;
				Utils.PlotTileLine(projectile.Center, projectile.Center + projectile.velocity * 6f, 20f, DelegateMethods.CastLightOpen);
				Utils.PlotTileLine(projectile.Left, projectile.Right, 20f, DelegateMethods.CastLightOpen);
				Utils.PlotTileLine(player.Center, player.Center + player.velocity * 6f, 40f, DelegateMethods.CastLightOpen);
				Utils.PlotTileLine(player.Left, player.Right, 40f, DelegateMethods.CastLightOpen);
			}

			Vector2 desiredCenter = player.MountedCenter + desiredCenterRelative;
			Vector2 betweenDirection = desiredCenter - projectile.Center;
			float betweenSQ = betweenDirection.LengthSquared();
			//if (between > 1000f)
			//{
			//    projectile.Center = player.Center + desiredCenterRelative;
			//}
			TeleportIfTooFar(projectile, desiredCenter, 1000);
			if (betweenSQ < veloDistanceChange * veloDistanceChange)
			{
				projectile.velocity *= 0.25f;
			}
			if (betweenDirection != Vector2.Zero)
			{
				if (betweenSQ < (veloDistanceChange * 0.5f) * (veloDistanceChange * 0.5f))
				{
					projectile.velocity = betweenDirection * veloSpeed;
				}
				else
				{
					projectile.velocity = betweenDirection * 0.1f * veloSpeed;
				}
			}
			if (projectile.velocity.LengthSquared() > 6f * 6f)
			{
				float rotationVelo = projectile.velocity.X * 0.08f * veloXToRotationFactor + projectile.velocity.Y * projectile.spriteDirection * 0.02f;
				if (Math.Abs(projectile.rotation - rotationVelo) >= 3.14159274f)
				{
					if (rotationVelo < projectile.rotation)
					{
						projectile.rotation -= 6.28318548f;
					}
					else
					{
						projectile.rotation += 6.28318548f;
					}
				}
				float rotationAcc = 12f;
				projectile.rotation = (projectile.rotation * (rotationAcc - 1f) + rotationVelo) / rotationAcc;
			}
			else
			{
				if (projectile.rotation > 3.14159274f)
				{
					projectile.rotation -= 6.28318548f;
				}
				if (projectile.rotation > -0.005f && projectile.rotation < 0.005f)
				{
					projectile.rotation = 0f;
				}
				else
				{
					projectile.rotation *= 0.96f;
				}
			}
		}
		#endregion

		#region EyeSpring

		/// <summary>
		/// Almost proper working Eye Spring clone
		/// </summary>
		public static void EyeSpringAI(Projectile projectile, bool flyForever = false)
		{
			Player player = projectile.GetOwner();
			if (!player.active)
			{
				projectile.active = false;
			}
			else
			{
				bool flag = false;
				bool flag2 = false;
				bool flag3 = false;
				bool flag4 = false;
				int num = 85;
				if (player.position.X + (float)(player.width / 2) < projectile.position.X + (float)(projectile.width / 2) - (float)num)
				{
					flag = true;
				}
				else if (player.position.X + (float)(player.width / 2) > projectile.position.X + (float)(projectile.width / 2) + (float)num)
				{
					flag2 = true;
				}
				if (projectile.ai[1] == 0f)
				{
					int num38 = 500;
					if (player.rocketDelay2 > 0)
					{
						projectile.ai[0] = 1f;
					}
					Vector2 vector6 = new Vector2(projectile.position.X + (float)projectile.width * 0.5f, projectile.position.Y + (float)projectile.height * 0.5f);
					float num39 = player.position.X + (float)(player.width / 2) - vector6.X;
					float num40 = player.position.Y + (float)(player.height / 2) - vector6.Y;
					float num41 = (float)Math.Sqrt((double)(num39 * num39 + num40 * num40));
					if (num41 > 2000f)
					{
						TeleportIfTooFar(projectile, player.Center);
						//projectile.position.X = player.position.X + (float)(player.width / 2) - (float)(projectile.width / 2);
						//projectile.position.Y = player.position.Y + (float)(player.height / 2) - (float)(projectile.height / 2);
					}
					else if (num41 > (float)num38 || (Math.Abs(num40) > 300f) || flyForever)
					{
						projectile.ai[0] = 1f;
					}
				}
				if (projectile.ai[0] != 0f)
				{
					float num42 = 0.2f;
					int num43 = 200;
					projectile.tileCollide = false;
					Vector2 vector7 = new Vector2(projectile.position.X + (float)projectile.width * 0.5f, projectile.position.Y + (float)projectile.height * 0.5f);
					float num44 = player.position.X + (float)(player.width / 2) - vector7.X;
					float num51 = player.position.Y + (float)(player.height / 2) - vector7.Y;
					float num52 = (float)Math.Sqrt((double)(num44 * num44 + num51 * num51));
					float num53 = 10f;
					//float num54 = num52;
					if (num52 < (float)num43 && player.velocity.Y == 0f && projectile.position.Y + (float)projectile.height <= player.position.Y + (float)player.height && !Collision.SolidCollision(projectile.position, projectile.width, projectile.height))
					{
						projectile.ai[0] = 0f;
						if (projectile.velocity.Y < -6f)
						{
							projectile.velocity.Y = -6f;
						}
					}
					if (num52 < 60f)
					{
						num44 = projectile.velocity.X;
						num51 = projectile.velocity.Y;
					}
					else
					{
						num52 = num53 / num52;
						num44 *= num52;
						num51 *= num52;
					}
					if (projectile.velocity.X < num44)
					{
						projectile.velocity.X += num42;
						if (projectile.velocity.X < 0f)
						{
							projectile.velocity.X += num42 * 1.5f;
						}
					}
					if (projectile.velocity.X > num44)
					{
						projectile.velocity.X -= num42;
						if (projectile.velocity.X > 0f)
						{
							projectile.velocity.X -= num42 * 1.5f;
						}
					}
					if (projectile.velocity.Y < num51)
					{
						projectile.velocity.Y += num42;
						if (projectile.velocity.Y < 0f)
						{
							projectile.velocity.Y += num42 * 1.5f;
						}
					}
					if (projectile.velocity.Y > num51)
					{
						projectile.velocity.Y -= num42;
						if (projectile.velocity.Y > 0f)
						{
							projectile.velocity.Y -= num42 * 1.5f;
						}
					}
					if ((double)projectile.velocity.X > 0.5)
					{
						projectile.spriteDirection = -1;
					}
					else if ((double)projectile.velocity.X < -0.5)
					{
						projectile.spriteDirection = 1;
					}
					projectile.frameCounter++;
					if (projectile.frameCounter > 4)
					{
						projectile.frame++;
						projectile.frameCounter = 0;
					}
					if (projectile.frame < 6 || projectile.frame > 7)
					{
						projectile.frame = 6;
					}
					projectile.rotation = projectile.velocity.ToRotation() + 1.57f;
				}
				else
				{
					//Vector2 vector9 = Vector2.Zero;
					if (projectile.ai[1] != 0f)
					{
						flag = false;
						flag2 = false;
					}
					projectile.rotation = 0f;
					projectile.tileCollide = true;
					float num111 = 8f;
					float num110 = 0.4f;
					if (flag)
					{
						if (projectile.velocity.X > -3.5f)
						{
							projectile.velocity.X -= num110;
						}
						else
						{
							projectile.velocity.X -= num110 * 0.25f;
						}
					}
					else if (flag2)
					{
						if (projectile.velocity.X < 3.5f)
						{
							projectile.velocity.X += num110;
						}
						else
						{
							projectile.velocity.X += num110 * 0.25f;
						}
					}
					else
					{
						projectile.velocity.X *= 0.9f;
						if (projectile.velocity.X >= 0f - num110 && projectile.velocity.X <= num110)
						{
							projectile.velocity.X = 0f;
						}
					}
					if (flag | flag2)
					{
						int num112 = (int)(projectile.position.X + (float)(projectile.width / 2)) / 16;
						int j = (int)(projectile.position.Y + (float)(projectile.height / 2)) / 16;
						if (flag)
						{
							int num30 = num112;
							num112 = num30 - 1;
						}
						if (flag2)
						{
							int num30 = num112;
							num112 = num30 + 1;
						}
						num112 += (int)projectile.velocity.X;
						if (WorldGen.SolidTile(num112, j))
						{
							flag4 = true;
						}
					}
					if (player.position.Y + player.height - 8f > projectile.position.Y + projectile.height)
					{
						flag3 = true;
					}
					if (projectile.frameCounter < 10)
					{
						flag4 = false;
					}
					//projectile.stepSpeed = 1f;
					//projectile.gfxOffY = 0f;
					//Collision.StepUp(ref projectile.position, ref projectile.velocity, projectile.width, projectile.height, ref stepSpeed, ref gfxOffY);
					Collision.StepUp(ref projectile.position, ref projectile.velocity, projectile.width, projectile.height, ref projectile.stepSpeed, ref projectile.gfxOffY);
					if (projectile.velocity.Y == 0f)
					{
						if (!flag3 && (projectile.velocity.X < 0f || projectile.velocity.X > 0f))
						{
							int num113 = (int)(projectile.position.X + (projectile.width / 2)) / 16;
							int j2 = (int)(projectile.position.Y + (projectile.height / 2)) / 16 + 1;
							if (flag)
							{
								int num30 = num113;
								num113 = num30 - 1;
							}
							if (flag2)
							{
								int num30 = num113;
								num113 = num30 + 1;
							}
							WorldGen.SolidTile(num113, j2);
						}
						if (flag4)
						{
							int num114 = (int)(projectile.position.X + projectile.width / 2) / 16;
							int num115 = (int)(projectile.position.Y + projectile.height) / 16 + 1;
							if (WorldGen.SolidTile(num114, num115) || Main.tile[num114, num115].IsHalfBlock || Main.tile[num114, num115].Slope > 0)
							{
								try
								{
									num114 = (int)(projectile.position.X + projectile.width / 2) / 16;
									num115 = (int)(projectile.position.Y + projectile.height / 2) / 16;
									if (flag)
									{
										int num30 = num114;
										num114 = num30 - 1;
									}
									if (flag2)
									{
										int num30 = num114;
										num114 = num30 + 1;
									}
									num114 += (int)projectile.velocity.X;
									if (!WorldGen.SolidTile(num114, num115 - 1) && !WorldGen.SolidTile(num114, num115 - 2))
									{
										projectile.velocity.Y = -5.1f;
									}
									else if (!WorldGen.SolidTile(num114, num115 - 2))
									{
										projectile.velocity.Y = -7.1f;
									}
									else if (WorldGen.SolidTile(num114, num115 - 5))
									{
										projectile.velocity.Y = -11.1f;
									}
									else if (WorldGen.SolidTile(num114, num115 - 4))
									{
										projectile.velocity.Y = -10.1f;
									}
									else
									{
										projectile.velocity.Y = -9.1f;
									}
								}
								catch
								{
									projectile.velocity.Y = -9.1f;
								}
							}
						}
					}
					if (projectile.velocity.X > num111)
					{
						projectile.velocity.X = num111;
					}
					if (projectile.velocity.X < 0f - num111)
					{
						projectile.velocity.X = 0f - num111;
					}
					if (projectile.velocity.X < 0f)
					{
						projectile.direction = -1;
					}
					if (projectile.velocity.X > 0f)
					{
						projectile.direction = 1;
					}
					if (projectile.velocity.X > num110 && flag2)
					{
						projectile.direction = 1;
					}
					if (projectile.velocity.X < 0f - num110 && flag)
					{
						projectile.direction = -1;
					}


					//fix cause im dumb and didnt copy ai code correctly
					if (!flag && !flag2 && projectile.ai[0] == 0f)
					{
						projectile.direction = (player.Center - projectile.Center).X > 0 ? 1 : -1;
					}


					if (projectile.direction == -1)
					{
						projectile.spriteDirection = 1;
					}
					if (projectile.direction == 1)
					{
						projectile.spriteDirection = -1;
					}

					if (projectile.velocity.Y == 0f)
					{
						if (projectile.frame > 5)
						{
							projectile.frameCounter = 0;
						}
						if (projectile.velocity.X == 0f)
						{
							int num116 = 3;
							projectile.frameCounter++;
							if (projectile.frameCounter < num116)
							{
								projectile.frame = 0;
							}
							else if (projectile.frameCounter < num116 * 2)
							{
								projectile.frame = 1;
							}
							else if (projectile.frameCounter < num116 * 3)
							{
								projectile.frame = 2;
							}
							else if (projectile.frameCounter < num116 * 4)
							{
								projectile.frame = 3;
							}
							else
							{
								projectile.frameCounter = num116 * 4;
							}
						}
						else
						{
							projectile.velocity.X *= 0.8f;
							projectile.frameCounter++;
							int num117 = 3;
							if (projectile.frameCounter < num117)
							{
								projectile.frame = 0;
							}
							else if (projectile.frameCounter < num117 * 2)
							{
								projectile.frame = 1;
							}
							else if (projectile.frameCounter < num117 * 3)
							{
								projectile.frame = 2;
							}
							else if (projectile.frameCounter < num117 * 4)
							{
								projectile.frame = 3;
							}
							else if (flag | flag2)
							{
								projectile.velocity.X *= 2f;
								projectile.frame = 4;
								projectile.velocity.Y = -6.1f;
								projectile.frameCounter = 0;
								int num30;
								for (int num118 = 0; num118 < 4; num118 = num30 + 1)
								{
									/*int num119 = 0; Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y + (float)projectile.height - 2f), projectile.width, 4, 5)*/
									;
									//Dust dust = Main.dust[num119];
									//dust.velocity += projectile.velocity;
									//dust = Main.dust[num119];
									//dust.velocity *= 0.4f;
									num30 = num118;
								}
							}
							else
							{
								projectile.frameCounter = num117 * 4;
							}
						}
					}
					else if (projectile.velocity.Y < 0f)
					{
						projectile.frameCounter = 0;
						projectile.frame = 5;
					}
					else
					{
						projectile.frame = 4;
						projectile.frameCounter = 3;
					}
					projectile.velocity.Y += 0.4f;
					if (projectile.velocity.Y > 10f)
					{
						projectile.velocity.Y = 10f;
					}
				}
			}
		}
		#endregion

		#region Zephyrfish
		public static void ZephyrfishDraw(Projectile projectile, int frameCounter = 5)
		{
			projectile.frameCounter++;
			if (projectile.frameCounter > frameCounter)
			{
				projectile.frame++;
				projectile.frameCounter = 0;
			}
			if (projectile.frame >= Main.projFrames[projectile.type]) //3
			{
				projectile.frame = 0;
			}
		}

		/// <summary>
		/// Stays around a certain offset position around the parent. Does not use ai/localai
		/// </summary>
		public static void ZephyrfishAI(Projectile projectile, Entity parent = null, float velocityFactor = 1f, float sway = 1f, bool random = true, byte swapSides = 0, float offsetX = 0f, float offsetY = 0f)
		{
			//velocityFactor: 
			//kinda wonky, leave at 1f

			//sway: 
			//tells by how much increase/decrease the left/right sway of the idle pet

			//swapSides:
			// 0: always behind
			//-1: always left
			// 1: always right

			//offsetX/Y
			//offsetting the desired center the pet hovers around

			if (parent == null) parent = projectile.GetOwner();

			Vector2 parentCenter = parent.Center;
			if (parent is Player)
			{
				parentCenter = ((Player)parent).MountedCenter;
			}

			float veloDelta = 0.3f;
			projectile.tileCollide = false; //false
			int someDistance = 100;
			Vector2 between = parentCenter - projectile.Center;

			Vector2 desiredCenter = random ? new Vector2(Main.rand.Next(-10, 21), Main.rand.Next(-10, 21)) : Vector2.Zero;

			Vector2 offset = new Vector2(60f + offsetX, -60f + offsetY);

			if (swapSides == 1)
			{
				offset.X = -offset.X;
			}
			else if (swapSides == 0)
			{
				offset.X *= -parent.direction;
			}

			//desiredCenter += new Vector2(60f * -player.direction, -60f);
			between += desiredCenter + offset;

			TeleportIfTooFar(projectile, parentCenter + desiredCenter + offset);

			float distance = between.Length();
			//float magnitude = 6f;
			if (distance < someDistance && parent.velocity.Y == 0f && projectile.position.Y + projectile.height <= parent.position.Y + parent.height && !Collision.SolidCollision(projectile.position, projectile.width, projectile.height))
			{
				//projectile.ai[0] = 0;
				if (projectile.velocity.Y < -6f)
				{
					projectile.velocity.Y = -6f;
				}
			}
			float swayDistance = 50 * sway;
			if (distance < swayDistance) //50
			{
				if (Math.Abs(projectile.velocity.X) > 2f || Math.Abs(projectile.velocity.Y) > 2f)
				{
					projectile.velocity *= 0.99f;
				}
				veloDelta = 0.01f;
			}
			else
			{
				if (distance < swayDistance * 2) //100
				{
					veloDelta = 0.1f;
				}
				//between: 0.3f
				if (distance > swayDistance * 6) //300
				{
					veloDelta = 0.4f;
				}
				between.Normalize();
				between *= 6f;
				between *= velocityFactor;
			}
			veloDelta *= velocityFactor;
			if (projectile.velocity.X < between.X)
			{
				projectile.velocity.X += veloDelta;
				if (veloDelta > 0.05f && projectile.velocity.X < 0f)
				{
					projectile.velocity.X += +veloDelta;
				}
			}
			if (projectile.velocity.X > between.X)
			{
				projectile.velocity.X += -veloDelta;
				if (veloDelta > 0.05f && projectile.velocity.X > 0f)
				{
					projectile.velocity.X += -veloDelta;
				}
			}
			if (projectile.velocity.Y < between.Y)
			{
				projectile.velocity.Y += veloDelta;
				if (veloDelta > 0.05f && projectile.velocity.Y < 0f)
				{
					projectile.velocity.Y += veloDelta * 2f;
				}
			}
			if (projectile.velocity.Y > between.Y)
			{
				projectile.velocity.Y += -veloDelta;
				if (veloDelta > 0.05f && projectile.velocity.Y > 0f)
				{
					projectile.velocity.Y += -veloDelta * 2f;
				}
			}
			projectile.manualDirectionChange = true;
			if (projectile.velocity.X > 0.25f && projectile.direction == 1)
			{
				projectile.direction = -1;
			}
			else if (projectile.velocity.X < -0.25f && projectile.direction != 1)
			{
				projectile.direction = 1;
			}
			projectile.spriteDirection = projectile.direction;

			//fix, direction gets set automatically by "manualDirectionChange = false" projectiled on velocity.X
			//if (projectile.velocity.X > 0.25f)
			//{
			//    projectile.ai[0] = -1;
			//}
			//else if (projectile.velocity.X < -0.25f)
			//{
			//    projectile.ai[0] = 1;
			//}
			//projectile.direction = (int)projectile.ai[0];
			//projectile.spriteDirection = projectile.direction;
			projectile.rotation = projectile.velocity.X * 0.05f;
		}
		#endregion

		#region BabyEater
		public static void BabyEaterDraw(Projectile projectile, int frameCounter = 6)
		{
			projectile.frameCounter++;
			if (projectile.frameCounter > frameCounter)
			{
				projectile.frame++;
				projectile.frameCounter = 0;
			}
			if (projectile.frame >= Main.projFrames[projectile.type]) //2
			{
				projectile.frame = 0;
			}
		}

		public static void BabyEaterAI(Projectile projectile, Entity parent = null, Vector2 originOffset = default(Vector2), float velocityFactor = 1f, float sway = 1f)
		{
			//velocityFactor: 
			//kinda wonky, leave at 1f

			//sway: 
			//tells by how much increase/decrease the left/right sway radius of the idle pet

			if (parent == null) parent = projectile.GetOwner();

			if (!parent.active)
			{
				projectile.active = false;
				return;
			}

			Vector2 parentCenter = parent.Center;
			if (parent is Player)
			{
				parentCenter = ((Player)parent).MountedCenter;
			}
			parentCenter += originOffset;

			float veloDelta = 0.1f;
			projectile.tileCollide = false;
			int someDistance = 300;
			Vector2 between = parentCenter - projectile.Center;
			float distance = between.Length();
			float magnitude = 7f;

			TeleportIfTooFar(projectile, parentCenter, 1380);

			if (distance < someDistance && parent.velocity.Y == 0f && projectile.position.Y + projectile.height <= parent.position.Y + parent.height + originOffset.Y && !Collision.SolidCollision(projectile.position, projectile.width, projectile.height))
			{
				if (projectile.velocity.Y < -6f)
				{
					projectile.velocity.Y = -6f;
				}
			}
			float swayDistance = 150f * sway;
			if (distance < swayDistance)
			{
				if (Math.Abs(projectile.velocity.X) > 2f || Math.Abs(projectile.velocity.Y) > 2f)
				{
					projectile.velocity *= 0.99f;
				}
				veloDelta = 0.01f;
				if (between.X < -2f)
				{
					between.X = -2f;
				}
				if (between.X > 2f)
				{
					between.X = 2f;
				}
				if (between.Y < -2f)
				{
					between.Y = -2f;
				}
				if (between.Y > 2f)
				{
					between.Y = 2f;
				}
			}
			else
			{
				if (distance > swayDistance * 2f)
				{
					veloDelta = 0.2f;
				}
				between.Normalize();
				between *= magnitude;
			}

			veloDelta *= velocityFactor;

			if (Math.Abs(between.X) > Math.Abs(between.Y))
			{
				if (projectile.velocity.X < between.X)
				{
					projectile.velocity.X += veloDelta;
					if (veloDelta > 0.05f && projectile.velocity.X < 0f)
					{
						projectile.velocity.X += veloDelta;
					}
				}
				if (projectile.velocity.X > between.X)
				{
					projectile.velocity.X += -veloDelta;
					if (veloDelta > 0.05f && projectile.velocity.X > 0f)
					{
						projectile.velocity.X += -veloDelta;
					}
				}
			}
			if (Math.Abs(between.X) <= Math.Abs(between.Y) || veloDelta == 0.05f)
			{
				if (projectile.velocity.Y < between.Y)
				{
					projectile.velocity.Y += veloDelta;
					if (veloDelta > 0.05f && projectile.velocity.Y < 0f)
					{
						projectile.velocity.Y += veloDelta;
					}
				}
				if (projectile.velocity.Y > between.Y)
				{
					projectile.velocity.Y += -veloDelta;
					if (veloDelta > 0.05f && projectile.velocity.Y > 0f)
					{
						projectile.velocity.Y += -veloDelta;
					}
				}
			}
			projectile.rotation = projectile.velocity.ToRotation() - 1.57f;
		}
		#endregion

		#region StardustDragon


		//ProjectileID.Sets.
		//NeedsUUID = true;
		//DontAttachHideToAlpha =true;

		//if minion = true:
		//ProjectileID.Sets.MinionSacrificable[projectile.type] = false, cause the replacing code for worm minions is complicated
		//damage set in NewProjectile/item
		//scales in size with the amount of segments
		public static void StardustDragonSetDefaults(Projectile projectile, int size = 24, bool minion = true, WormType wormType = WormType.None)
		{
			if (minion)
			{
				//if (projectile.type == 625 || projectile.type == 628)
				//{
				//    projectile.netImportant = true;
				//}
				if (wormType == WormType.Body1 || wormType == WormType.Body2)
				{
					projectile.minionSlots = 0.5f;
				}
				projectile.DamageType = DamageClass.Summon;
				projectile.minion = true;
				//projectile.hide = true;
				projectile.netImportant = true;
			}
			projectile.Size = new Vector2(size);
			projectile.penetrate = -1;
			projectile.timeLeft *= 5;
			projectile.friendly = true;
			projectile.ignoreWater = true;
			projectile.tileCollide = false;
			projectile.alpha = 255;
		}
		//wormTypes = new int[] {head, body1, body2, tail} //projectiletype

		//if minion = true:
		//float scaleFactor = MathHelper.Clamp(projectile.localAI[0], 0f, 50f);
		//projectile.scale = 1f + scaleFactor * 0.01f;
		public static void StardustDragonAI(Projectile projectile, int[] wormTypes, int segmentDistance = 16)
		{
			Player player = projectile.GetOwner();

			if (projectile.minion && (int)Main.time % 120 == 0)
			{
				projectile.netUpdate = true;
			}
			if (!player.active)
			{
				projectile.active = false;
				return;
			}
			bool head = projectile.type == wormTypes[0];
			int defScaleFactor = 30;
			//if (Main.rand.Next(30) == 0)
			//{
			//    int num1049 = Dust.NewDust(projectile.position, projectile.width, projectile.height, 135, 0f, 0f, 0, default(Color), 2f);
			//    Main.dust[num1049].noGravity = true;
			//    Main.dust[num1049].fadeIn = 2f;
			//    Point point4 = Main.dust[num1049].position.ToTileCoordinates();
			//    if (WorldGen.InWorld(point4.X, point4.Y, 5) && WorldGen.SolidTile(point4.X, point4.Y))
			//    {
			//        Main.dust[num1049].noLight = true;
			//    }
			//}

			if (projectile.alpha > 0)
			{
				//for (int i = 0; i < 2; i++)
				//{
				//    int dust = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 135, 0f, 0f, 100, default(Color), 2f);
				//    Main.dust[dust].noGravity = true;
				//    Main.dust[dust].noLight = true;
				//}
				projectile.alpha -= 42;
				if (projectile.alpha < 0)
				{
					projectile.alpha = 0;
					return;
				}
			}

			if (head)
			{
				Vector2 desiredCenter = player.Center;
				int targetIndex = -1;
				TeleportIfTooFar(projectile, desiredCenter);
				if (projectile.minion)
				{
					float maxProjDistance = 490000f;
					float maxPlayerDistance = 1000000f;
					NPC ownerMinionAttackTargetNPC5 = projectile.OwnerMinionAttackTargetNPC;
					if (ownerMinionAttackTargetNPC5 != null && ownerMinionAttackTargetNPC5.CanBeChasedBy())
					{
						float distance1 = projectile.DistanceSQ(ownerMinionAttackTargetNPC5.Center);
						if (distance1 < maxProjDistance * 2f)
						{
							targetIndex = ownerMinionAttackTargetNPC5.whoAmI;
						}
					}
					if (targetIndex < 0)
					{
						for (int i = 0; i < Main.maxNPCs; i++)
						{
							NPC npc = Main.npc[i];
							if (npc.CanBeChasedBy() && player.DistanceSQ(npc.Center) < maxPlayerDistance)
							{
								float distance2 = projectile.DistanceSQ(npc.Center);
								if (distance2 < maxProjDistance)
								{
									targetIndex = i;
								}
							}
						}
					}
				}
				if (targetIndex != -1)
				{
					NPC npc = Main.npc[targetIndex];
					Vector2 betweenNPC = npc.Center - projectile.Center;
					float veloFactor = 0.4f;
					if (betweenNPC.Length() < 600f)
					{
						veloFactor = 0.6f;
					}
					if (betweenNPC.Length() < 300f)
					{
						veloFactor = 0.8f;
					}
					if (betweenNPC.Length() > npc.Size.Length() * 0.75f)
					{
						projectile.velocity += Vector2.Normalize(betweenNPC) * veloFactor * 1.5f;
						if (Vector2.Dot(projectile.velocity, betweenNPC) < 0.25f)
						{
							projectile.velocity *= 0.8f;
						}
					}
					float targetMagnitude = 30f;
					if (projectile.velocity.Length() > targetMagnitude)
					{
						projectile.velocity = Vector2.Normalize(projectile.velocity) * targetMagnitude;
					}
				}
				else
				{
					float idleVelo = 0.2f;
					Vector2 betweenPlayer = desiredCenter - projectile.Center;
					if (betweenPlayer.Length() < 200f)
					{
						idleVelo = 0.12f;
					}
					if (betweenPlayer.Length() < 140f)
					{
						idleVelo = 0.06f;
					}
					if (betweenPlayer.Length() > 100f)
					{
						if (Math.Abs(desiredCenter.X - projectile.Center.X) > 20f)
						{
							projectile.velocity.X += idleVelo * Math.Sign(desiredCenter.X - projectile.Center.X);
						}
						if (Math.Abs(desiredCenter.Y - projectile.Center.Y) > 10f)
						{
							projectile.velocity.Y += idleVelo * Math.Sign(desiredCenter.Y - projectile.Center.Y);
						}
					}
					else if (projectile.velocity.Length() > 2f)
					{
						projectile.velocity *= 0.96f;
					}
					if (Math.Abs(projectile.velocity.Y) < 1f)
					{
						projectile.velocity.Y += -0.1f;
					}
					float idleMagnitude = 15f;
					if (projectile.velocity.Length() > idleMagnitude)
					{
						projectile.velocity = Vector2.Normalize(projectile.velocity) * idleMagnitude;
					}
				}
				projectile.rotation = projectile.velocity.ToRotation() + 1.57079637f;
				int direction = projectile.direction;
				projectile.direction = projectile.spriteDirection = (projectile.velocity.X > 0f) ? 1 : -1;
				if (projectile.minion && direction != projectile.direction)
				{
					projectile.netUpdate = true;
				}
				float scaleFactor = MathHelper.Clamp(projectile.localAI[0], 0f, 50f);
				if (!projectile.minion) scaleFactor = 0;
				projectile.position = projectile.Center;
				projectile.scale = 1f + scaleFactor * 0.01f;
				projectile.width = projectile.height = (int)(defScaleFactor * projectile.scale);
				projectile.Center = projectile.position;
			}
			else
			{
				Vector2 pCenter = Vector2.Zero;
				float parentRotation = 0f;
				float positionOffset = 0f;
				float scaleOffset = 1f;

				//some custom syncing it seems like, when summoning/replacing it
				if (projectile.ai[1] == 1f)
				{
					projectile.ai[1] = 0f;
					projectile.netUpdate = true;
				}

				Projectile parent = null;
				for (short i = 0; i < Main.maxProjectiles; i++)
				{
					Projectile proj = Main.projectile[i];
					if (proj.active && proj.owner == projectile.owner && proj.identity == (int)projectile.ai[0]/* && proj.type == projectile.type*/)
					{
						parent = proj;
						break;
					}
				}
				if (parent != null)
				{
					if (parent.active && (parent.type == wormTypes[0] || parent.type == wormTypes[1] || parent.type == wormTypes[2]))
					{
						pCenter = parent.Center;
						//Vector2 velocity2 = parent.velocity;
						parentRotation = parent.rotation;
						scaleOffset = MathHelper.Clamp(parent.scale, 0f, 50f);
						if (!projectile.minion) scaleOffset = 1;
						positionOffset = segmentDistance;
						parent.localAI[0] = projectile.localAI[0] + 1f;
						if (parent.type != wormTypes[0])
						{
							parent.localAI[1] = projectile.whoAmI;
						}
						if (projectile.owner == Main.myPlayer && parent.type == wormTypes[0] && projectile.type == wormTypes[3])
						{
							parent.Kill();
							projectile.Kill();
							return;
						}
					}

					projectile.velocity = Vector2.Zero;
					Vector2 newVelocity = pCenter - projectile.Center;
					if (parentRotation != projectile.rotation)
					{
						float rotatedBy = MathHelper.WrapAngle(parentRotation - projectile.rotation);
						newVelocity = newVelocity.RotatedBy(rotatedBy * 0.1f);
					}
					projectile.rotation = newVelocity.ToRotation() + 1.57079637f;
					projectile.position = projectile.Center;
					projectile.scale = scaleOffset;
					projectile.width = projectile.height = (int)(defScaleFactor * projectile.scale);
					projectile.Center = projectile.position;
					if (newVelocity != Vector2.Zero)
					{
						projectile.Center = pCenter - Vector2.Normalize(newVelocity) * positionOffset * scaleOffset;
					}
					projectile.spriteDirection = (newVelocity.X > 0f) ? 1 : -1;
				}
			}
		}

		#endregion

		public static void ModifiedGoldfishAI(NPC npc, float scareRange, bool faceAway = true)
		{
			if (npc.direction == 0)
			{
				npc.TargetClosest();
			}
			if (npc.wet)
			{
				bool hasPlayer = false;
				npc.TargetClosest(faceTarget: false);
				Vector2 centerpos = npc.Center;
				Player player = Main.player[npc.target];
				Vector2 playerpos = player.Center;
				float distancex = playerpos.X - centerpos.X;
				float distancey = playerpos.Y - centerpos.Y;
				float distSQ = distancex * distancex + distancey * distancey;
				if (player.wet && distSQ < scareRange * scareRange)
				{
					if (!player.dead)
					{
						hasPlayer = true;
					}
				}
				if (!hasPlayer)
				{
					if (npc.collideX)
					{
						npc.velocity.X *= -1;
						npc.direction *= -1;
						npc.netUpdate = true;
					}
					if (npc.collideY)
					{
						npc.netUpdate = true;
						if (npc.velocity.Y > 0f)
						{
							npc.velocity.Y = Math.Abs(npc.velocity.Y) * -1f;
							npc.directionY = -1;
							npc.ai[0] = -1f;
						}
						else if (npc.velocity.Y < 0f)
						{
							npc.velocity.Y = Math.Abs(npc.velocity.Y);
							npc.directionY = 1;
							npc.ai[0] = 1f;
						}
					}
				}
				if (hasPlayer) //if target is in water
				{
					npc.TargetClosest(faceTarget: false);
					npc.direction = (distancex >= 0f).ToDirectionInt();
					npc.directionY = (distancey >= 0f).ToDirectionInt();

					if (faceAway)
					{
						npc.direction *= -1;
						npc.directionY *= -1;
					}

					npc.velocity.X += npc.direction * 0.1f;
					npc.velocity.Y += npc.directionY * 0.1f;

					if (npc.velocity.X > 3f)
					{
						npc.velocity.X = 3f;
					}
					if (npc.velocity.X < -3f)
					{
						npc.velocity.X = -3f;
					}
					if (npc.velocity.Y > 2f)
					{
						npc.velocity.Y = 2f;
					}
					if (npc.velocity.Y < -2f)
					{
						npc.velocity.Y = -2f;
					}
				}
				else
				{
					npc.velocity.X += npc.direction * 0.1f;
					if (npc.velocity.X < -1f || npc.velocity.X > 1f)
					{
						npc.velocity.X *= 0.95f;
					}
					if (npc.ai[0] == -1f)
					{
						npc.velocity.Y -= 0.01f;
						if (npc.velocity.Y < -0.3f)
						{
							npc.ai[0] = 1f;
						}
					}
					else
					{
						npc.velocity.Y += 0.01f;
						if (npc.velocity.Y > 0.3f)
						{
							npc.ai[0] = -1f;
						}
					}
					int tileX = (int)npc.Center.X / 16;
					int tileY = (int)npc.Center.Y / 16;
					if (Framing.GetTileSafely(tileX, tileY - 1).LiquidAmount > 128)
					{
						if (Framing.GetTileSafely(tileX, tileY + 1).HasTile)
						{
							npc.ai[0] = -1f;
						}
						else if (Framing.GetTileSafely(tileX, tileY + 2).HasTile)
						{
							npc.ai[0] = -1f;
						}
					}
					if (npc.velocity.Y > 0.4f || npc.velocity.Y < -0.4f)
					{
						npc.velocity.Y *= 0.95f;
					}
				}
			}
			else //not wet, frantically jump around
			{
				if (npc.velocity.Y == 0f)
				{
					if (Main.netMode != NetmodeID.MultiplayerClient)
					{
						npc.velocity.Y = Main.rand.Next(-50, -20) * 0.1f;
						npc.velocity.X = Main.rand.Next(-20, 20) * 0.1f;
						npc.netUpdate = true;
					}
				}
				npc.velocity.Y += 0.3f;
				if (npc.velocity.Y > 10f)
				{
					npc.velocity.Y = 10f;
				}
				npc.ai[0] = 1f;
			}
			npc.rotation = npc.velocity.Y * npc.direction * 0.1f;
			if (npc.rotation < -0.2f)
			{
				npc.rotation = -0.2f;
			}
			if (npc.rotation > 0.2f)
			{
				npc.rotation = 0.2f;
			}
		}

		//TODO turn into usable method later
		public static void PirateAI(Projectile Projectile)
		{
			//	//Pirate AI related:
			//public int State
			//{
			//	get => (int)Projectile.ai[0];
			//	set => Projectile.ai[0] = value;
			//}

			//public int Timer
			//{
			//	get => (int)Projectile.ai[1];
			//	set => Projectile.ai[1] = value;
			//}

			//public bool Idle => State == 0;

			//public bool Flying
			//{
			//	get => State == 1;
			//	set => State = value ? 1 : 0;
			//}

			//public bool Attacking
			//{
			//	get => State == 2;
			//	set => State = value ? 2 : 0;
			//}
			int Timer = 0;
			bool Idle = true;
			bool Flying = false;
			bool Attacking = false;
			////End Pirate AI

			Player player = Main.player[Projectile.owner];

			int startAttackRange = 800;
			float awayFromPlayerDistMax = 500f;
			float awayFromPlayerDistYMax = 300f;

			Vector2 destination = player.Center;
			destination.X -= (15 + player.width / 2) * player.direction;
			destination.X -= Projectile.minionPos * 25 * player.direction;

			Projectile.shouldFallThrough = player.Bottom.Y - 12f > Projectile.Bottom.Y;
			Projectile.friendly = false;
			int timerReset = 0;
			int attackFrameCount = 3;
			int nextTimerValue = 5 * attackFrameCount;
			int attackTarget = -1;

			bool defaultstate = Idle;

			static bool CustomEliminationCheck_Pirates(Entity otherEntity, int currentTarget) => true;

			if (defaultstate)
				Projectile.Minion_FindTargetInRange(startAttackRange, ref attackTarget, skipIfCannotHitWithOwnBody: true, CustomEliminationCheck_Pirates);

			if (Flying)
			{
				Projectile.tileCollide = false;
				float velChange = 0.2f;
				float toPlayerSpeed = 10f;
				int maxLen = 200;
				if (toPlayerSpeed < Math.Abs(player.velocity.X) + Math.Abs(player.velocity.Y))
					toPlayerSpeed = Math.Abs(player.velocity.X) + Math.Abs(player.velocity.Y);

				Vector2 toPlayer = player.Center - Projectile.Center;
				float len = toPlayer.Length();

				AssAI.TeleportIfTooFar(Projectile, player.Center);

				if (len < maxLen && player.velocity.Y == 0f && Projectile.Bottom.Y <= player.Bottom.Y && !Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height))
				{
					//Reset back from flying
					Flying = false;
					Projectile.netUpdate = true;
					if (Projectile.velocity.Y < -6f)
						Projectile.velocity.Y = -6f;
				}

				if (!(len < 60f))
				{
					toPlayer.Normalize();
					toPlayer *= toPlayerSpeed;
					if (Projectile.velocity.X < toPlayer.X)
					{
						Projectile.velocity.X += velChange;
						if (Projectile.velocity.X < 0f)
							Projectile.velocity.X += velChange * 1.5f;
					}

					if (Projectile.velocity.X > toPlayer.X)
					{
						Projectile.velocity.X -= velChange;
						if (Projectile.velocity.X > 0f)
							Projectile.velocity.X -= velChange * 1.5f;
					}

					if (Projectile.velocity.Y < toPlayer.Y)
					{
						Projectile.velocity.Y += velChange;
						if (Projectile.velocity.Y < 0f)
							Projectile.velocity.Y += velChange * 1.5f;
					}

					if (Projectile.velocity.Y > toPlayer.Y)
					{
						Projectile.velocity.Y -= velChange;
						if (Projectile.velocity.Y > 0f)
							Projectile.velocity.Y -= velChange * 1.5f;
					}
				}

				if (Projectile.velocity.X != 0f)
					Projectile.spriteDirection = -Math.Sign(Projectile.velocity.X);
			}

			if (Attacking && Timer < 0)
			{
				Projectile.friendly = false;
				Timer += 1;
				if (nextTimerValue >= 0)
				{
					Timer = 0;
					Attacking = false;
					Projectile.netUpdate = true;
					return;
				}
			}
			else if (Attacking)
			{
				//Attacking animation
				Projectile.spriteDirection = -Projectile.direction;
				Projectile.rotation = 0f;

				Projectile.friendly = true;
				int startAttackFrame = 12;
				bool hasJumpingAttackFrames = true;
				int attackFrameNumber = (int)((float)nextTimerValue - Timer) / (nextTimerValue / attackFrameCount);
				Projectile.frame = startAttackFrame + attackFrameNumber;
				if (hasJumpingAttackFrames && Projectile.velocity.Y != 0f)
					Projectile.frame += attackFrameCount;

				Projectile.velocity.Y += 0.4f;
				if (Projectile.velocity.Y > 10f)
					Projectile.velocity.Y = 10f;

				Timer -= 1;
				if (Timer <= 0f)
				{
					if (timerReset <= 0)
					{
						Timer = 0;
						Attacking = false;
						Projectile.netUpdate = true;
						return;
					}

					Timer = -timerReset;
				}
			}

			if (attackTarget >= 0)
			{
				float maxDistance = startAttackRange;
				float toTargetMaxDist = 20f;

				NPC npc = Main.npc[attackTarget];
				Vector2 targetCenter = npc.Center;
				destination = targetCenter;
				if (Projectile.IsInRangeOfMeOrMyOwner(npc, maxDistance, out float _, out float _, out bool _))
				{
					Projectile.shouldFallThrough = npc.Center.Y > Projectile.Bottom.Y;

					bool flag11 = Projectile.velocity.Y == 0f;
					if (Projectile.wet && Projectile.velocity.Y > 0f && !Projectile.shouldFallThrough)
						flag11 = true;

					if (targetCenter.Y < Projectile.Center.Y - 30f && flag11)
					{
						float num25 = (targetCenter.Y - Projectile.Center.Y) * -1f;
						float num26 = 0.4f;
						float velY = (float)Math.Sqrt(num25 * 2f * num26);
						if (velY > 26f)
							velY = 26f;

						Projectile.velocity.Y = -velY;
					}

					if (Vector2.Distance(Projectile.Center, destination) < toTargetMaxDist)
					{
						float len = Projectile.velocity.Length();
						if (len > 10f)
							Projectile.velocity /= len / 10f;

						Attacking = true;
						Timer = nextTimerValue;
						Projectile.netUpdate = true;
						Projectile.direction = (targetCenter.X - Projectile.Center.X > 0f).ToDirectionInt();
					}
				}
			}

			if (Idle && attackTarget < 0)
			{
				if (player.rocketDelay2 > 0)
				{
					Flying = true;
					Projectile.netUpdate = true;
				}

				Vector2 toPlayer = player.Center - Projectile.Center;
				if (toPlayer.Length() > 2000f)
				{
					Projectile.Center = player.Center;
				}
				else if (toPlayer.Length() > awayFromPlayerDistMax || Math.Abs(toPlayer.Y) > awayFromPlayerDistYMax)
				{
					Flying = true;
					Projectile.netUpdate = true;
					if (Projectile.velocity.Y > 0f && toPlayer.Y < 0f)
						Projectile.velocity.Y = 0f;

					if (Projectile.velocity.Y < 0f && toPlayer.Y > 0f)
						Projectile.velocity.Y = 0f;
				}
			}

			if (Idle)
			{
				if (attackTarget < 0)
				{
					if (Projectile.Distance(player.Center) > 60f && Projectile.Distance(destination) > 60f && Math.Sign(destination.X - player.Center.X) != Math.Sign(Projectile.Center.X - player.Center.X))
						destination = player.Center;

					Rectangle rect = Utils.CenteredRectangle(destination, Projectile.Size);
					for (int i = 0; i < 20; i++)
					{
						if (Collision.SolidCollision(rect.TopLeft(), rect.Width, rect.Height))
							break;

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
							destination = Projectile.Center;
					}

					Vector2 fromDestToPlayer = player.Center - destination;
					if (fromDestToPlayer.Length() > awayFromPlayerDistMax || Math.Abs(fromDestToPlayer.Y) > awayFromPlayerDistYMax)
					{
						Rectangle rect2 = Utils.CenteredRectangle(player.Center, Projectile.Size);
						Vector2 fromPlayerToDest = destination - player.Center;
						Vector2 topLeft = rect2.TopLeft();
						for (float i = 0f; i < 1f; i += 0.05f)
						{
							Vector2 newTopLeft = rect2.TopLeft() + fromPlayerToDest * i;
							if (Collision.SolidCollision(rect2.TopLeft() + fromPlayerToDest * i, rect2.Width, rect2.Height))
								break;

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

				if (attackTarget != -1)
				{
					velXChange = 0.4f; //1f
					velXChangeMargin = 5f; //8f
					velXChangeMax = 5f; //8f
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
							Projectile.velocity.X -= velXChange;
						else
							Projectile.velocity.X -= velXChangeSmall;
					}
					else
					{
						xOff = 1;
						if (Projectile.velocity.X < velXChangeMargin)
							Projectile.velocity.X += velXChange;
						else
							Projectile.velocity.X += velXChangeSmall;
					}
				}
				else
				{
					Projectile.velocity.X *= 0.9f;
					if (Math.Abs(Projectile.velocity.X) < velXChange * 2f)
						Projectile.velocity.X = 0f;
				}

				bool tryJumping = Math.Abs(toDestination.X) >= 64f || (toDestination.Y <= -48f && Math.Abs(toDestination.X) >= 8f);
				if (xOff != 0 && tryJumping)
				{
					int x = (int)Projectile.Center.X / 16;
					int y = (int)Projectile.position.Y / 16;
					x += xOff;
					x += (int)Projectile.velocity.X;
					for (int j = y; j < y + Projectile.height / 16 + 1; j++)
					{
						if (WorldGen.SolidTile(x, j))
							canJumpOverTiles = true;
					}
				}

				Collision.StepUp(ref Projectile.position, ref Projectile.velocity, Projectile.width, Projectile.height, ref Projectile.stepSpeed, ref Projectile.gfxOffY);
				float nextVelocityY = Utils.GetLerpValue(0f, 100f, toDestination.Y, clamped: true) * Utils.GetLerpValue(-2f, -6f, Projectile.velocity.Y, clamped: true);
				if (Projectile.velocity.Y == 0f && canJumpOverTiles)
				{
					for (int k = 0; k < 3; k++)
					{
						int num42 = (int)(Projectile.Center.X) / 16;
						if (k == 0)
							num42 = (int)Projectile.position.X / 16;

						if (k == 2)
							num42 = (int)(Projectile.Right.X) / 16;

						int num43 = (int)(Projectile.Bottom.Y) / 16;
						if (!WorldGen.SolidTile(num42, num43) && !Main.tile[num42, num43].IsHalfBlock && Main.tile[num42, num43].Slope <= 0 && (!TileID.Sets.Platforms[Main.tile[num42, num43].TileType] || !Main.tile[num42, num43].HasTile || Main.tile[num42, num43].IsActuated))
							continue;

						try
						{
							num42 = (int)(Projectile.Center.X) / 16;
							num43 = (int)(Projectile.Center.Y) / 16;
							num42 += xOff;
							num42 += (int)Projectile.velocity.X;
							if (!WorldGen.SolidTile(num42, num43 - 1) && !WorldGen.SolidTile(num42, num43 - 2))
								Projectile.velocity.Y = -5.1f;
							else if (!WorldGen.SolidTile(num42, num43 - 2))
								Projectile.velocity.Y = -7.1f;
							else if (WorldGen.SolidTile(num42, num43 - 5))
								Projectile.velocity.Y = -11.1f;
							else if (WorldGen.SolidTile(num42, num43 - 4))
								Projectile.velocity.Y = -10.1f;
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
							Projectile.velocity.Y = -6f;
						else if (height < 80f)
							Projectile.velocity.Y = -7f;
						else if (height < 100f)
							Projectile.velocity.Y = -8f;
						else if (height < 120f)
							Projectile.velocity.Y = -9f;
						else if (height < 140f)
							Projectile.velocity.Y = -10f;
						else if (height < 160f)
							Projectile.velocity.Y = -11f;
						else if (height < 190f)
							Projectile.velocity.Y = -12f;
						else if (height < 210f)
							Projectile.velocity.Y = -13f;
						else if (height < 270f)
							Projectile.velocity.Y = -14f;
						else if (height < 310f)
							Projectile.velocity.Y = -15f;
						else
							Projectile.velocity.Y = -16f;
					}

					if (Projectile.wet && nextVelocityY == 0f)
						Projectile.velocity.Y *= 2f;
				}

				if (Projectile.velocity.X > velXChangeMax)
					Projectile.velocity.X = velXChangeMax;

				if (Projectile.velocity.X < -velXChangeMax)
					Projectile.velocity.X = -velXChangeMax;

				if (Projectile.velocity.X < 0f)
					Projectile.direction = -1;

				if (Projectile.velocity.X > 0f)
					Projectile.direction = 1;

				if (Projectile.velocity.X == 0f)
					Projectile.direction = (player.Center.X > Projectile.Center.X).ToDirectionInt();

				if (Projectile.velocity.X > velXChange && xOff == 1)
					Projectile.direction = 1;

				if (Projectile.velocity.X < -velXChange && xOff == -1)
					Projectile.direction = -1;

				Projectile.spriteDirection = -Projectile.direction;

				Projectile.velocity.Y += 0.4f + nextVelocityY * 1f;
				if (Projectile.velocity.Y > 10f)
				{
					Projectile.velocity.Y = 10f;
				}
			}

			#region default animations
			/*
            if (Flying)
            {
                //Flying animation
                if (pirate)
                {
                    Projectile.frameCounter++;
                    if (Projectile.frameCounter > 3)
                    {
                        Projectile.frame++;
                        Projectile.frameCounter = 0;
                    }

                    if ((Projectile.frame < 10) | (Projectile.frame > 13))
                        Projectile.frame = 10;

                    Projectile.rotation = Projectile.velocity.X * 0.1f;
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

                        if (Projectile.frame >= 4)
                            Projectile.frame = 0;
                    }
                    else
                    {
                        Projectile.frame = 0;
                        Projectile.frameCounter = 0;
                    }
                }
                else if (Projectile.velocity.Y != 0f)
                {
                    Projectile.frameCounter = 0;
                    Projectile.frame = 14;
                }
            }
            */
			#endregion
		}

		//TODO turn into usable method later
		public static void VampFrogAI(Projectile Projectile)
		{
			//	//VampFrog AI related:
			//public int State
			//{
			//	get => (int)Projectile.ai[0];
			//	set => Projectile.ai[0] = value;
			//}

			//public int Timer
			//{
			//	get => (int)Projectile.ai[1];
			//	set => Projectile.ai[1] = value;
			//}

			//public bool Idle => State == 0;

			//public bool Flying
			//{
			//	get => State == 1;
			//	set => State = value ? 1 : 0;
			//}

			//public bool Attacking
			//{
			//	get => State == 2;
			//	set => State = value ? 2 : 0;
			//}
			int Timer = 0;
			bool Idle = true;
			bool Flying = false;
			bool Attacking = false;
			////End VampFrog AI

			Player player = Main.player[Projectile.owner];

			int startAttackRange = 800;
			float awayFromPlayerDistMax = 500f;
			float awayFromPlayerDistYMax = 300f;

			Vector2 destination = player.Center;
			destination.X -= (35 + player.width / 2) * player.direction;
			destination.X -= Projectile.minionPos * 40 * player.direction;

			Projectile.shouldFallThrough = player.Bottom.Y - 12f > Projectile.Bottom.Y;
			Projectile.friendly = false;
			int timerReset = 0;
			int attackFrameCount = 4;
			int nextTimerValue = 5 * attackFrameCount;
			int attackTarget = -1;

			Projectile.friendly = true;
			timerReset = 60;

			bool defaultstate = Idle;

			static bool CustomEliminationCheck_Pirates(Entity otherEntity, int currentTarget) => true;

			if (defaultstate)
				Projectile.Minion_FindTargetInRange(startAttackRange, ref attackTarget, skipIfCannotHitWithOwnBody: true, CustomEliminationCheck_Pirates);

			if (Flying)
			{
				Projectile.tileCollide = false;
				float velChange = 0.2f;
				float toPlayerSpeed = 10f;
				int maxLen = 200;
				if (toPlayerSpeed < Math.Abs(player.velocity.X) + Math.Abs(player.velocity.Y))
					toPlayerSpeed = Math.Abs(player.velocity.X) + Math.Abs(player.velocity.Y);

				Vector2 toPlayer = player.Center - Projectile.Center;
				float len = toPlayer.Length();

				AssAI.TeleportIfTooFar(Projectile, player.Center);

				if (len < maxLen && player.velocity.Y == 0f && Projectile.Bottom.Y <= player.Bottom.Y && !Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height))
				{
					//Reset back from flying
					Flying = false;
					Projectile.netUpdate = true;
					if (Projectile.velocity.Y < -6f)
						Projectile.velocity.Y = -6f;
				}

				if (!(len < 60f))
				{
					toPlayer.Normalize();
					toPlayer *= toPlayerSpeed;
					if (Projectile.velocity.X < toPlayer.X)
					{
						Projectile.velocity.X += velChange;
						if (Projectile.velocity.X < 0f)
							Projectile.velocity.X += velChange * 1.5f;
					}

					if (Projectile.velocity.X > toPlayer.X)
					{
						Projectile.velocity.X -= velChange;
						if (Projectile.velocity.X > 0f)
							Projectile.velocity.X -= velChange * 1.5f;
					}

					if (Projectile.velocity.Y < toPlayer.Y)
					{
						Projectile.velocity.Y += velChange;
						if (Projectile.velocity.Y < 0f)
							Projectile.velocity.Y += velChange * 1.5f;
					}

					if (Projectile.velocity.Y > toPlayer.Y)
					{
						Projectile.velocity.Y -= velChange;
						if (Projectile.velocity.Y > 0f)
							Projectile.velocity.Y -= velChange * 1.5f;
					}
				}

				if (Projectile.velocity.X != 0f)
					Projectile.spriteDirection = -Math.Sign(Projectile.velocity.X);
			}

			if (Attacking && Timer < 0)
			{
				Projectile.friendly = false;
				Timer += 1;
				if (nextTimerValue >= 0)
				{
					Timer = 0;
					Attacking = false;
					Projectile.netUpdate = true;
					return;
				}
			}
			else if (Attacking)
			{
				//Attacking animation
				Projectile.spriteDirection = -Projectile.direction;
				Projectile.rotation = 0f;

				float num22 = ((float)nextTimerValue - Timer) / (float)nextTimerValue;
				if ((double)num22 > 0.25 && (double)num22 < 0.75)
					Projectile.friendly = true;

				int num23 = (int)(num22 * 5f);
				if (num23 > 2)
					num23 = 4 - num23;

				if (Projectile.velocity.Y != 0f)
					Projectile.frame = 21 + num23;
				else
					Projectile.frame = 18 + num23;

				if (Projectile.velocity.Y == 0f)
					Projectile.velocity.X *= 0.8f;

				Projectile.velocity.Y += 0.4f;
				if (Projectile.velocity.Y > 10f)
					Projectile.velocity.Y = 10f;

				Timer -= 1;
				if (Timer <= 0f)
				{
					if (timerReset <= 0)
					{
						Timer = 0;
						Attacking = false;
						Projectile.netUpdate = true;
						return;
					}

					Timer = -timerReset;
				}
			}

			if (attackTarget >= 0)
			{
				float maxDistance = startAttackRange;
				float toTargetMaxDist = 50f;

				NPC npc = Main.npc[attackTarget];
				Vector2 targetCenter = npc.Center;
				destination = targetCenter;
				if (Projectile.IsInRangeOfMeOrMyOwner(npc, maxDistance, out float _, out float _, out bool _))
				{
					Projectile.shouldFallThrough = npc.Center.Y > Projectile.Bottom.Y;

					bool flag11 = Projectile.velocity.Y == 0f;
					if (Projectile.wet && Projectile.velocity.Y > 0f && !Projectile.shouldFallThrough)
						flag11 = true;

					if (targetCenter.Y < Projectile.Center.Y - 30f && flag11)
					{
						float num25 = (targetCenter.Y - Projectile.Center.Y) * -1f;
						float num26 = 0.4f;
						float velY = (float)Math.Sqrt(num25 * 2f * num26);
						if (velY > 26f)
							velY = 26f;

						Projectile.velocity.Y = -velY;
					}

					if (Vector2.Distance(Projectile.Center, destination) < toTargetMaxDist)
					{
						float len = Projectile.velocity.Length();
						if (len > 10f)
							Projectile.velocity /= len / 10f;

						Attacking = true;
						Timer = nextTimerValue;
						Projectile.netUpdate = true;
						Projectile.direction = (targetCenter.X - Projectile.Center.X > 0f).ToDirectionInt();
					}
				}

				int dir = 1;
				if (targetCenter.X - Projectile.Center.X < 0f)
					dir = -1;

				destination.X += 20 * -dir;
			}

			if (Idle && attackTarget < 0)
			{
				if (player.rocketDelay2 > 0)
				{
					Flying = true;
					Projectile.netUpdate = true;
				}

				Vector2 toPlayer = player.Center - Projectile.Center;
				if (toPlayer.Length() > 2000f)
				{
					Projectile.Center = player.Center;
				}
				else if (toPlayer.Length() > awayFromPlayerDistMax || Math.Abs(toPlayer.Y) > awayFromPlayerDistYMax)
				{
					Flying = true;
					Projectile.netUpdate = true;
					if (Projectile.velocity.Y > 0f && toPlayer.Y < 0f)
						Projectile.velocity.Y = 0f;

					if (Projectile.velocity.Y < 0f && toPlayer.Y > 0f)
						Projectile.velocity.Y = 0f;
				}
			}

			if (Idle)
			{
				if (attackTarget < 0)
				{
					if (Projectile.Distance(player.Center) > 60f && Projectile.Distance(destination) > 60f && Math.Sign(destination.X - player.Center.X) != Math.Sign(Projectile.Center.X - player.Center.X))
						destination = player.Center;

					Rectangle rect = Utils.CenteredRectangle(destination, Projectile.Size);
					for (int i = 0; i < 20; i++)
					{
						if (Collision.SolidCollision(rect.TopLeft(), rect.Width, rect.Height))
							break;

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
							destination = Projectile.Center;
					}

					Vector2 fromDestToPlayer = player.Center - destination;
					if (fromDestToPlayer.Length() > awayFromPlayerDistMax || Math.Abs(fromDestToPlayer.Y) > awayFromPlayerDistYMax)
					{
						Rectangle rect2 = Utils.CenteredRectangle(player.Center, Projectile.Size);
						Vector2 fromPlayerToDest = destination - player.Center;
						Vector2 topLeft = rect2.TopLeft();
						for (float i = 0f; i < 1f; i += 0.05f)
						{
							Vector2 newTopLeft = rect2.TopLeft() + fromPlayerToDest * i;
							if (Collision.SolidCollision(rect2.TopLeft() + fromPlayerToDest * i, rect2.Width, rect2.Height))
								break;

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

				if (attackTarget != -1)
				{
					velXChange = 0.7f;
					velXChangeMargin = 6f;
					velXChangeMax = 6f;
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
							Projectile.velocity.X -= velXChange;
						else
							Projectile.velocity.X -= velXChangeSmall;
					}
					else
					{
						xOff = 1;
						if (Projectile.velocity.X < velXChangeMargin)
							Projectile.velocity.X += velXChange;
						else
							Projectile.velocity.X += velXChangeSmall;
					}

					bool shouldJumpOverTiles = true;

					if (attackTarget == -1)
						shouldJumpOverTiles = false;

					if (shouldJumpOverTiles)
						canJumpOverTiles = true;
				}
				else
				{
					Projectile.velocity.X *= 0.9f;
					if (Math.Abs(Projectile.velocity.X) < velXChange * 2f)
						Projectile.velocity.X = 0f;
				}

				bool tryJumping = Math.Abs(toDestination.X) >= 64f || (toDestination.Y <= -48f && Math.Abs(toDestination.X) >= 8f);
				if (xOff != 0 && tryJumping)
				{
					int x = (int)Projectile.Center.X / 16;
					int y = (int)Projectile.position.Y / 16;
					x += xOff;
					x += (int)Projectile.velocity.X;
					for (int j = y; j < y + Projectile.height / 16 + 1; j++)
					{
						if (WorldGen.SolidTile(x, j))
							canJumpOverTiles = true;
					}
				}

				Collision.StepUp(ref Projectile.position, ref Projectile.velocity, Projectile.width, Projectile.height, ref Projectile.stepSpeed, ref Projectile.gfxOffY);
				float nextVelocityY = Utils.GetLerpValue(0f, 100f, toDestination.Y, clamped: true) * Utils.GetLerpValue(-2f, -6f, Projectile.velocity.Y, clamped: true);
				if (Projectile.velocity.Y == 0f && canJumpOverTiles)
				{
					for (int k = 0; k < 3; k++)
					{
						int num42 = (int)(Projectile.Center.X) / 16;
						if (k == 0)
							num42 = (int)Projectile.position.X / 16;

						if (k == 2)
							num42 = (int)(Projectile.Right.X) / 16;

						int num43 = (int)(Projectile.Bottom.Y) / 16;
						if (!WorldGen.SolidTile(num42, num43) && !Main.tile[num42, num43].IsHalfBlock && Main.tile[num42, num43].Slope <= 0 && (!TileID.Sets.Platforms[Main.tile[num42, num43].TileType] || !Main.tile[num42, num43].HasTile || Main.tile[num42, num43].IsActuated))
							continue;

						try
						{
							num42 = (int)(Projectile.Center.X) / 16;
							num43 = (int)(Projectile.Center.Y) / 16;
							num42 += xOff;
							num42 += (int)Projectile.velocity.X;
							if (!WorldGen.SolidTile(num42, num43 - 1) && !WorldGen.SolidTile(num42, num43 - 2))
								Projectile.velocity.Y = -5.1f;
							else if (!WorldGen.SolidTile(num42, num43 - 2))
								Projectile.velocity.Y = -7.1f;
							else if (WorldGen.SolidTile(num42, num43 - 5))
								Projectile.velocity.Y = -11.1f;
							else if (WorldGen.SolidTile(num42, num43 - 4))
								Projectile.velocity.Y = -10.1f;
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
							Projectile.velocity.Y = -6f;
						else if (height < 80f)
							Projectile.velocity.Y = -7f;
						else if (height < 100f)
							Projectile.velocity.Y = -8f;
						else if (height < 120f)
							Projectile.velocity.Y = -9f;
						else if (height < 140f)
							Projectile.velocity.Y = -10f;
						else if (height < 160f)
							Projectile.velocity.Y = -11f;
						else if (height < 190f)
							Projectile.velocity.Y = -12f;
						else if (height < 210f)
							Projectile.velocity.Y = -13f;
						else if (height < 270f)
							Projectile.velocity.Y = -14f;
						else if (height < 310f)
							Projectile.velocity.Y = -15f;
						else
							Projectile.velocity.Y = -16f;
					}

					if (Projectile.wet && nextVelocityY == 0f)
						Projectile.velocity.Y *= 2f;
				}

				if (Projectile.velocity.X > velXChangeMax)
					Projectile.velocity.X = velXChangeMax;

				if (Projectile.velocity.X < -velXChangeMax)
					Projectile.velocity.X = -velXChangeMax;

				if (Projectile.velocity.X < 0f)
					Projectile.direction = -1;

				if (Projectile.velocity.X > 0f)
					Projectile.direction = 1;

				if (Projectile.velocity.X == 0f)
					Projectile.direction = (player.Center.X > Projectile.Center.X).ToDirectionInt();

				if (Projectile.velocity.X > velXChange && xOff == 1)
					Projectile.direction = 1;

				if (Projectile.velocity.X < -velXChange && xOff == -1)
					Projectile.direction = -1;

				Projectile.spriteDirection = -Projectile.direction;

				Projectile.velocity.Y += 0.4f + nextVelocityY * 1f;
				if (Projectile.velocity.Y > 10f)
				{
					Projectile.velocity.Y = 10f;
				}
			}

			#region default animations
			/*
            if (Flying)
            {
                //Flying animation
                int frameSpeed = 3;
                if (++Projectile.frameCounter >= frameSpeed * 4)
                    Projectile.frameCounter = 0;

                Projectile.frame = 14 + Projectile.frameCounter / frameSpeed;
                Projectile.rotation = Projectile.velocity.X * 0.15f;
            }
            else
            {
                Projectile.rotation = 0f;
                if (Projectile.velocity.Y == 0f)
                {
                    if (Projectile.velocity.X == 0f)
                    {
                        int frameSpeed = 4;
                        if (++Projectile.frameCounter >= 7 * frameSpeed && Main.rand.Next(50) == 0)
                            Projectile.frameCounter = 0;

                        int nextFrame = Projectile.frameCounter / frameSpeed;
                        if (nextFrame >= 4)
                            nextFrame = 6 - nextFrame;

                        if (nextFrame < 0)
                            nextFrame = 0;

                        Projectile.frame = 1 + nextFrame;
                    }
                    else if (Math.Abs(Projectile.velocity.X) >= 0.5f)
                    {
                        Projectile.frameCounter += (int)Math.Abs(Projectile.velocity.X);
                        Projectile.frameCounter++;
                        int num47 = 15;
                        int num48 = 8;
                        if (Projectile.frameCounter >= num48 * num47)
                            Projectile.frameCounter = 0;

                        int num49 = Projectile.frameCounter / num47;
                        Projectile.frame = num49 + 5;
                    }
                    else
                    {
                        Projectile.frame = 0;
                        Projectile.frameCounter = 0;
                    }
                }
                else if (Projectile.velocity.Y != 0f)
                {
                    if (Projectile.velocity.Y < 0f)
                    {
                        if (Projectile.frame > 9 || Projectile.frame < 5)
                        {
                            Projectile.frame = 5;
                            Projectile.frameCounter = 0;
                        }

                        if (++Projectile.frameCounter >= 1 && Projectile.frame < 9)
                        {
                            Projectile.frame++;
                            Projectile.frameCounter = 0;
                        }
                    }
                    else
                    {
                        if (Projectile.frame > 13 || Projectile.frame < 9)
                        {
                            Projectile.frame = 9;
                            Projectile.frameCounter = 0;
                        }

                        if (++Projectile.frameCounter >= 2 && Projectile.frame < 11)
                        {
                            Projectile.frame++;
                            Projectile.frameCounter = 0;
                        }
                    }
                }
            }
            */
			#endregion
		}
	}

	public enum WormType : byte
	{
		None = 0,
		Head = 1,
		Body1 = 2,
		Body2 = 3,
		Tail = 4
	}
}
