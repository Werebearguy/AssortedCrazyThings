using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs
{
	[Content(ContentType.PlaceablesFunctional)]
	public class WyvernCampfireGlobalNPC : AssGlobalNPC
	{
		public override bool AppliesToEntity(NPC entity, bool lateInstantiation)
		{
			return lateInstantiation && entity.type >= NPCID.WyvernHead && entity.type <= NPCID.WyvernTail;
		}

		public bool sentWyvernPacket = false;

		public override bool InstancePerEntity => true;


		public const short fadeTimer = 254;
		public short fadeTimerCount = 0;

		private void KillInstantly(NPC npc)
		{
			// These 3 lines instantly kill the npc without showing damage numbers, dropping loot, or playing DeathSound. Use this for instant deaths
			npc.life = 0;
			//npc.HitEffect();
			npc.active = false;
			npc.netUpdate = true;
			SoundEngine.PlaySound(SoundID.NPCDeath16, npc.position); // plays a fizzle sound
		}

		private bool SlowDown(ref NPC npc)
		{
			//returns true if after SlowDown it still moves, false if it stands still
			bool isMoving = !(npc.velocity.X == 0 && npc.velocity.Y == 0);
			if (isMoving)
			{
				if (npc.velocity.X > 4f || npc.velocity.X < -4f)
				{
					npc.velocity.X *= 0.9f;
				}
				else
				{
					if (npc.velocity.X > 0.3f)
					{
						npc.velocity.X -= 0.5f;
					}
					else if (npc.velocity.X < -0.3f)
					{
						npc.velocity.X += 0.5f;
					}
				}

				if (npc.velocity.Y > 4f || npc.velocity.Y < -4f)
				{
					npc.velocity.Y *= 0.9f;
				}
				else
				{
					if (npc.velocity.Y > 0.3f)
					{
						npc.velocity.Y -= 0.5f;
					}
					else if (npc.velocity.Y < -0.3f)
					{
						npc.velocity.Y += 0.5f;
					}
				}

				if ((npc.velocity.X > 0f && npc.velocity.X < 0.3f) || (npc.velocity.X < 0f && npc.velocity.X > -0.3f))
				{
					npc.velocity.X = 0f;
				}
				if ((npc.velocity.Y > 0f && npc.velocity.Y < 0.3f) || (npc.velocity.Y < 0f && npc.velocity.Y > -0.3f))
				{
					npc.velocity.Y = 0f;
				}
				//Main.NewText("X" + npc.velocity.X);
				//Main.NewText("Y" + npc.velocity.Y);

				//update isMoving again
				isMoving = !(npc.velocity.X == 0 && npc.velocity.Y == 0);
			}

			return isMoving;
		}

		private void QuickWyvernDust(Vector2 pos, Color color, float fadeIn, float chance)
		{
			if (Main.rand.NextFloat() < chance)
			{
				int type = 15;
				Dust dust = Dust.NewDustDirect(pos, 4, 4, type, 0f, 0f, 120, color, 2f);
				dust.position = pos;
				dust.velocity = new Vector2(Main.rand.NextFloat(-7, 7), Main.rand.NextFloat(-7, 7));
				dust.fadeIn = fadeIn; //3f
				dust.noLight = true;
				dust.noGravity = true;
			}
		}

		public Vector2 RotToNormal(float rotation)
		{
			return new Vector2((float)Math.Sin(rotation), (float)-Math.Cos(rotation));
		}

		public void FadeAway(ref short fadeTimerCount, ref NPC npc)
		{
			//14 segments each 42 coordinates apart == 580x580 rect around center of wyvern head
			Rectangle rect = new Rectangle((int)npc.Center.X - 580, (int)npc.Center.Y - 580, 2 * 580, 2 * 580);
			fadeTimerCount++;
			for (short j = 0; j < Main.maxNPCs; j++)
			{
				NPC other = Main.npc[j];
				if (other.active && (other.type > NPCID.WyvernHead && other.type <= NPCID.WyvernTail)/* && NPCID.WyvernHead != Main.npc[j].type*/)
				{
					if (rect.Intersects(other.Hitbox))
					{
						QuickWyvernDust(other.Center, Color.White, (float)(fadeTimerCount / 50f), 0.5f);
						other.GetGlobalNPC<WyvernCampfireGlobalNPC>().fadeTimerCount = fadeTimerCount;
					}
				}
			}

			//dust infront of the head (because it so long)
			Vector2 normal = 60 * RotToNormal(npc.rotation);
			QuickWyvernDust(npc.Center + normal, Color.White, (float)(fadeTimerCount / 50f), 0.5f);
		}

		public override Color? GetAlpha(NPC npc, Color drawColor)
		{
			if (fadeTimerCount > 0)
			{
				if (npc.type >= NPCID.WyvernHead && npc.type <= NPCID.WyvernTail)
				{
					return drawColor * ((float)(fadeTimer - fadeTimerCount) / 255f);
				}
			}
			return base.GetAlpha(npc, drawColor);
		}

		public override bool PreAI(NPC npc)
		{
			if (Main.netMode != NetmodeID.Server)
			{
				//this section of code doesn't run on the server anyway cause wyvernCampfire only gets set on LocalPlayer
				if (npc.type == NPCID.WyvernHead && Main.player[(int)Player.FindClosest(npc.position, npc.width, npc.height)].GetModPlayer<AssPlayer>().wyvernCampfire)
				{
					if (!sentWyvernPacket && Main.netMode == NetmodeID.MultiplayerClient)
					{
						sentWyvernPacket = true;
						ModPacket packet = Mod.GetPacket();
						packet.Write((byte)AssMessageType.WyvernCampfireKill);
						packet.Write(npc.whoAmI);
						packet.Send();
					}
					else if (Main.netMode == NetmodeID.SinglePlayer)
					{
						if (!SlowDown(ref npc))
						{
							if (fadeTimerCount <= fadeTimer)
							{
								FadeAway(ref fadeTimerCount, ref npc);
							}
							else
							{
								fadeTimerCount = 0;
								KillInstantly(npc);
							}
							return false;
						}
						else
						{
							fadeTimerCount = 0;
						}
					}
					//id 87 == head -> id 92 == tail
				}
			}
			return true;
		}
	}
}
