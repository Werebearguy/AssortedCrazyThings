using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs.Harvester
{
	[Content(ContentType.Bosses)]
	public abstract class HarvesterTalon : AssNPC
	{
		public override string Texture
		{
			get
			{
				return "AssortedCrazyThings/NPCs/Harvester/HarvesterTalon";
			}
		}

		/// <summary>
		/// Used for offset/directional calculations
		/// </summary>
		public virtual bool RightTalon => false;

		public const int ChainFrameCount = 6;
		public const int ChainFrameSpeed = 6;
		public static Asset<Texture2D> ChainAsset;
		public static Asset<Texture2D> OutlineAsset;

		public override void Load()
		{
			ChainAsset = Mod.Assets.Request<Texture2D>("NPCs/Harvester/HarvesterBossChain");
			OutlineAsset = ModContent.Request<Texture2D>(Texture + "_Outline");
		}

		public override void Unload()
		{
			ChainAsset = null;
			OutlineAsset = null;
		}

		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[NPC.type] = 1;

			NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
			{
				Hide = true //Hides this NPC from the Bestiary
			};
			NPCID.Sets.NPCBestiaryDrawOffset[NPC.type] = value;

			NPCID.Sets.SpecificDebuffImmunity[NPC.type][BuffID.Poisoned] = true;
			NPCID.Sets.SpecificDebuffImmunity[NPC.type][BuffID.OnFire] = true;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			if (!HasParent)
			{
				return base.GetAlpha(lightColor);
			}

			NPC body = Main.npc[ParentWhoAmI];
			return body.GetAlpha(lightColor);
		}

		public override void SetDefaults()
		{
			/*else if (type == 247 || type == 248)
                    {
                        noGravity = true;
                        width = 40;
                        height = 30;
                        aiStyle = 47;
                        damage = 59;
                        defense = 28;
                        lifeMax = 7000;
                        HitSound = SoundID.NPCHit4;
                        DeathSound = SoundID.NPCDeath14;
                        alpha = 255;
                        buffImmune[20] = true;
                        buffImmune[24] = true;
                    }*/

			NPC.boss = true; //TODO Why is it true?
			NPC.noGravity = true;
			NPC.noTileCollide = true;
			NPC.width = 40; //38 //latest 40
			NPC.height = 42; //42//latest 30
			NPC.aiStyle = -1;
			NPC.damage = HarvesterBoss.talonDamage;
			NPC.defense = 28;
			NPC.lifeMax = 1337;
			NPC.scale = 1f;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.alpha = 255;
			NPC.dontTakeDamage = true;
			NPC.dontCountMe = true;
			NPC.SpawnWithHigherTime(30);
		}

		public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
		{
			NPC.lifeMax = 1337;
			NPC.damage = (int)(NPC.damage * 1.1f);
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			return false;
		}

		public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			if (!HasParent || !Visible)
			{
				return;
			}

			NPC body = Main.npc[ParentWhoAmI];
			if (body.ModNPC is not HarvesterBoss harvester)
			{
				return;
			}

			Vector2 center = NPC.Center;
			float x = body.Center.X - center.X;
			float y = body.Center.Y - center.Y;
			y -= -harvester.talonOffsetY + 20f; //has to result to 7f

			x += GetOffset(harvester); //66f, -70f
			x += NPC.spriteDirection * (HarvesterBoss.talonDirectionalOffset + 6);

			SpriteEffects effect = (NPC.spriteDirection == 1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

			Texture2D texture = ChainAsset.Value;
			bool keepDrawing = true;
			int step = 0;
			while (keepDrawing)
			{
				step++;
				float len = (float)Math.Sqrt(x * x + y * y);
				float between = 38f;
				if (len < between) //16
				{
					keepDrawing = false;
				}
				else
				{
					len = between / len; //16
					x *= len;
					y *= len;
					center.X += x;
					center.Y += y;
					x = body.Center.X - center.X;
					y = body.Center.Y - center.Y;
					y -= -harvester.talonOffsetY + 20f;
					x += GetOffset(harvester);
					x += NPC.spriteDirection * (HarvesterBoss.talonDirectionalOffset + 0);

					if (Main.rand.NextBool(8))
					{
						Dust dust = Dust.NewDustPerfect(center, 135, new Vector2(Main.rand.NextFloat(-3f, 3f), Main.rand.NextFloat(-3f, 3f)), 26, new Color(255, 255, 255), Main.rand.NextFloat(1f, 1.6f));
						dust.noLight = true;
						dust.noGravity = true;
						dust.fadeIn = Main.rand.NextFloat(0.5f, 1.5f);
					}

					int frameY = (((int)NPC.frameCounter / ChainFrameSpeed) + step) % ChainFrameCount;
					Rectangle frame = texture.Frame(1, ChainFrameCount, frameY: frameY);
					spriteBatch.Draw(texture, center - screenPos + new Vector2(0f, NPC.gfxOffY + NPC.height / 2), frame, Color.White * NPC.Opacity, 0f, frame.Size() / 2, 1f, effect, 0f);
				}
			}

			texture = OutlineAsset.Value;

			int spriteOffset = -NPC.spriteDirection * HarvesterBoss.talonDirectionalOffset;
			Vector2 drawPos = NPC.Center - screenPos + new Vector2(spriteOffset, NPC.gfxOffY);
			Color color = new Color(195, 247, 255);

			int fadeIn = 10;
			int fadeDuration = 10;
			int fadeOut = 12;
			int finalTime = fadeIn + fadeDuration;
			if (OutlineFadeTimer < fadeIn)
			{
				color *= (float)OutlineFadeTimer / fadeIn;
			}
			else if (OutlineFadeTimer > finalTime)
			{
				int fadeOutTimer = OutlineFadeTimer - finalTime;
				if (fadeOutTimer < fadeOut)
				{
					color *= 1f - (float)fadeOutTimer / fadeOut;
				}
				else
				{
					color *= 0f;
				}
			}

			spriteBatch.Draw(texture, drawPos + new Vector2(0, 0.5f), NPC.frame, color, NPC.rotation, texture.Size() / 2, NPC.scale, effect, 0f);

			Main.instance.LoadNPC(NPC.type);
			texture = TextureAssets.Npc[NPC.type].Value;
			color = NPC.GetAlpha(drawColor);
			spriteBatch.Draw(texture, drawPos, NPC.frame, color, NPC.rotation, texture.Size() / 2, NPC.scale, effect, 0f);
		}

		public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
		{
			target.AddBuff(BuffID.Slow, 120, false); //2 seconds, 100% chance
		}

		public const float State_Seek_Retract = 0f;
		public const float State_Punch = 1f;
		public const float State_Punching = 2f;

		public ref float AI_State => ref NPC.ai[0];

		public ref float AI_Timer => ref NPC.ai[1];

		public ref float RetractCounter => ref NPC.ai[2];

		public int ParentWhoAmI
		{
			get => (int)NPC.ai[3] - 1;
			set => NPC.ai[3] = value + 1;
		}

		public bool HasParent => ParentWhoAmI >= 0 && ParentWhoAmI < Main.maxNPCs;

		public bool Idle
		{
			get => NPC.localAI[0] == 0f;
			set => NPC.localAI[0] = value ? 0f : 1f;
		}

		public bool LastTickVisible
		{
			get => NPC.localAI[1] == 1f;
			set => NPC.localAI[1] = value ? 1f : 0f;
		}

		public bool CurrentTickVisible
		{
			get => NPC.localAI[2] == 1f;
			set => NPC.localAI[2] = value ? 1f : 0f;
		}

		public bool Visible => LastTickVisible && CurrentTickVisible;

		public int OutlineFadeTimer
		{
			get => (int)NPC.localAI[3];
			set => NPC.localAI[3] = value;
		}

		public override bool CheckDead()
		{
			NPC.boss = false; //To get rid of the default death message
			return base.CheckDead();
		}

		public override void FindFrame(int frameHeight)
		{
			//Used just for chain anim
			NPC.frameCounter++;
		}

		public override void AI()
		{
			if (!HasParent)
			{
				NPC.life = 0;
				NPC.active = false;
				NPC.netUpdate = true;
				return;
			}

			NPC body = Main.npc[ParentWhoAmI];
			if (!body.active || body.type != AssortedCrazyThings.harvester)
			{
				NPC.life = 0;
				NPC.active = false;
				NPC.netUpdate = true;
				return;
			}

			HarvesterBoss harvester = body.ModNPC as HarvesterBoss;

			NPC.target = body.target;
			if (NPC.target < 0 || NPC.target >= Main.maxPlayers) return;
			Player target = Main.player[NPC.target];

			CheckVisible(harvester);

			NPC.gfxOffY = body.gfxOffY;
			NPC.spriteDirection = body.spriteDirection;
			NPC.direction = body.direction;
			NPC.alpha = body.alpha;

			//TODO has unused tile collision code and manual retract detection (fallback)

			if (NPC.alpha > 0)
			{
				//NPC.alpha -= 5;
				//if (NPC.alpha < 4)
				//{
				//    NPC.alpha = 0;
				//    NPC.netUpdate = true;
				//}
				NPC.scale = 1f;
				AI_Timer = 0f;
			}

			if (AI_State != State_Seek_Retract)
			{
				Idle = false;
			}

			if (AI_State == State_Seek_Retract)
			{
				//NPC.noTileCollide = true;

				if (NPC.Hitbox.Intersects(body.Hitbox))
				{
					//If in idle/retract state, and fully retracted (aka colliding with body hitbox), fix position, so that direction changes wont have the talons move left/right
					NPC.Center = body.Center + new Vector2(GetOffset(harvester), harvester.talonOffsetY);
					NPC.velocity *= 0f;
					NPC.netOffset = body.netOffset;
					Idle = true;
					return;
				}

				float speed = 18f; //14f
				if (NPC.life < NPC.lifeMax / 2)
				{
					speed += 3f; //3f
				}
				if (NPC.life < NPC.lifeMax / 4)
				{
					speed += 3f; //3f
				}
				if (body.life < body.lifeMax)
				{
					speed += 8f; //8f
				}

				Vector2 toBody = body.Center - NPC.Center;
				toBody.Y += harvester.talonOffsetY;
				toBody.X += GetOffset(harvester);

				float betweenSelfAndBodyX = body.Center.X - NPC.Center.X;
				float betweenSelfAndBodyY = body.Center.Y - NPC.Center.Y;
				betweenSelfAndBodyY -= -harvester.talonOffsetY;
				betweenSelfAndBodyX += GetOffset(harvester);
				float len = (float)Math.Sqrt(betweenSelfAndBodyX * betweenSelfAndBodyX + betweenSelfAndBodyY * betweenSelfAndBodyY);
				float somevar = 12f;
				if (len < somevar + speed && harvester.AI_State == HarvesterBoss.State_Bombing)
				{
					RetractCounter = 0f;
					NPC.velocity.X = betweenSelfAndBodyX;
					NPC.velocity.Y = betweenSelfAndBodyY;
					//Handled by harvester
					/*
                    AI_Timer += 1f;
                    //new
                    if (body.life < body.lifeMax / 2)
                    {
                        AI_Timer += 1f;
                    }
                    if (body.life < body.lifeMax / 4)
                    {
                        AI_Timer += 1f;
                    }
                    if (AI_Timer >= 60f)
                    {
                        NPC.TargetClosest();
                        target = Main.player[NPC.target];
                        //test is 100f
                        float test = Harvester.Wid / 2; //its for checking which (or both) talons to shoot, so the left one has also range to the right 100 in

                        //new
                        float x = target.Center.X - NPC.Center.X;
                        float y = target.Center.Y - NPC.Center.Y;
                        float toPlayer = (float)Math.Sqrt(x * x + y * y);

                        if (toPlayer < 500f && NPC.BottomLeft.Y < target.BottomLeft.Y) //distance where it is allowed to swing at player
                        {
                            //end new
                            if ((!RightTalon && NPC.Center.X + test > target.Center.X) || (RightTalon && NPC.Center.X - test < target.Center.X))
                            {
                                AI_Timer = 0f;
                                AI_State = State_Punch;
                                NPC.netUpdate = true;
                            }
                            else
                            {
                                AI_Timer = 0f;
                            }
                        }
                    }
                    */
				}
				else //retract
				{
					//new
					RetractCounter += 1f;
					float retractFactor = 0.5f + RetractCounter / 100f;
					if (body.life < body.lifeMax / 2)
					{
						retractFactor += 0.25f;
					}
					if (body.life < body.lifeMax / 4)
					{
						retractFactor += 0.25f;
					}

					//Also scale retract speed with velocity
					float velo = body.velocity.Length();

					retractFactor += velo / 80f;

					//end new

					if (len > speed)
					{
						len = speed / len;
					}
					NPC.velocity.X = betweenSelfAndBodyX * len * retractFactor; //both 1f
					NPC.velocity.Y = betweenSelfAndBodyY * len * retractFactor;
				}
			}
			else if (AI_State == State_Punch)
			{
				//Punch toward moving direction diagonally
				//NPC.noTileCollide = true;
				NPC.collideX = false;
				NPC.collideY = false;
				float speed = 17f;
				//new
				if (body.life <= body.lifeMax / 2)
				{
					speed += 2f;
				}
				if (body.life <= body.lifeMax / 4)
				{
					speed += 4f;
				}
				Vector2 directionTo = new Vector2(NPC.direction, 1f); //1/1 ratio
				directionTo.Normalize();

				NPC.velocity = directionTo * speed;
				AI_State = State_Punching;
			}
			else if (AI_State == State_Punching)
			{
				//fly through air/whatever and check if it hit tiles
				//fist has  40 width 30 height
				//talon has 38 width 42 height
				//if (Math.Abs(NPC.velocity.X) > Math.Abs(NPC.velocity.Y))
				//{
				//    if (NPC.velocity.X > 0f && NPC.Center.X > target.Center.X)
				//    {
				//        NPC.noTileCollide = false;
				//    }
				//    if (NPC.velocity.X < 0f && NPC.Center.X < target.Center.X)
				//    {
				//        NPC.noTileCollide = false;
				//    }
				//}
				//else
				//{
				//    if (NPC.velocity.Y > 0f && NPC.Center.Y > target.Center.Y)
				//    {
				//        NPC.noTileCollide = false;
				//    }
				//    if (NPC.velocity.Y < 0f && NPC.Center.Y < target.Center.Y)
				//    {
				//        NPC.noTileCollide = false;
				//    }
				//}

				Vector2 toBody = body.Center + body.velocity - NPC.Center;
				toBody.Y += harvester.talonOffsetY;
				toBody.X += GetOffset(harvester);
				float distSQ = toBody.LengthSquared();
				if (body.life < body.lifeMax)
				{
					NPC.knockBackResist = 0f;
					if (distSQ > 700f * 700f /*|| NPC.collideX || NPC.collideY || Collision.SolidCollision(NPC.position, NPC.width, NPC.height + 8)*/) //if collides with tiles or far away, go back to 0 and do the retreat code
					{
						//NPC.noTileCollide = true;
						NPC.netUpdate = true;
						AI_State = State_Seek_Retract;
					}
				}
				else
				{
					if ((distSQ > 600f * 600f /*|| NPC.collideX || NPC.collideY || Collision.SolidCollision(NPC.position, NPC.width, NPC.height + 8)*/) | NPC.justHit)
					{
						//NPC.noTileCollide = true;
						NPC.netUpdate = true;
						AI_State = State_Seek_Retract;
					}
				}
			}
		}

		private void CheckVisible(HarvesterBoss harvester)
		{
			CurrentTickVisible = LastTickVisible;
			LastTickVisible = harvester.AI_Animation == HarvesterBoss.Animation_Bombing;

			if (Visible)
			{
				OutlineFadeTimer++;
			}
			else
			{
				OutlineFadeTimer = 0;
			}
		}

		private float GetOffset(HarvesterBoss harvester)
		{
			return RightTalon ? harvester.talonOffsetRightX : harvester.talonOffsetLeftX;
		}
	}

	public class HarvesterTalonLeft : HarvesterTalon
	{
	}

	public class HarvesterTalonRight : HarvesterTalon
	{
		public override bool RightTalon => true;
	}
}
