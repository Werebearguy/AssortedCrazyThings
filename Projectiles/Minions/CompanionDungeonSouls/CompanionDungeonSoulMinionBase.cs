using AssortedCrazyThings.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.GameContent;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Minions.CompanionDungeonSouls
{
	[Content(ContentType.Bosses)]
	public abstract class CompanionDungeonSoulMinionBase : AssProjectile
	{
		private float sinY;
		private int sincounter;
		public int dustColor;
		//more like an initializer (set minionSlots and timeLeft accordingly)
		public bool isTemp = false;


		//SetDefaults stuff
		public float defdistanceFromTarget;// = 700f;
		public float defdistancePlayerFarAway;// = 800f;
		public float defdistancePlayerFarAwayWhenHasTarget;// = 1200f;
		public float defdistanceToEnemyBeforeCanDash;// = 20f; //20f
		public float defplayerFloatHeight;// = -60f; //-60f
		public float defplayerCatchUpIdle;// = 300f; //300f
		public float defbackToIdleFromNoclipping;// = 150f; //150f
		public float defdashDelay;// = 40f; //time it stays in the "dashing" state after a dash, he dashes when he is in state 0 aswell
		public float defdistanceAttackNoclip; //defdashDelay * 5; only for prewol version
		public float defstartDashRange;// = defdistanceToEnemyBeforeCanDash + 10f; //30f
		public float defdashIntensity;// = 4f; //4f

		public float veloFactorToEnemy;// = 6f; //8f
		public float accFactorToEnemy;// = 16f; //41f

		public float veloFactorAfterDash;// = 8f; //4f
		public float accFactorAfterDash;// = 41f; //41f

		public float defveloIdle;// = 1f;
		public float defveloCatchUpIdle;// = 8f;
		public float defveloNoclip;// = 12f;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Companion Soul");
			Main.projFrames[Projectile.type] = 6;
			Main.projPet[Projectile.type] = true;
			ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
			ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
		}

		public sealed override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.Spazmamini);
			Projectile.width = 14;
			Projectile.height = 24;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Summon;
			Projectile.minion = true;
			Projectile.minionSlots = 0.5f;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;

			Projectile.usesIDStaticNPCImmunity = true;
			Projectile.idStaticNPCHitCooldown = 8;

			dustColor = 0;

			SafeSetDefaults();
		}

		public virtual void SafeSetDefaults()
		{

		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write((bool)isTemp);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			isTemp = reader.ReadBoolean();
		}

		public override bool? CanCutTiles()
		{
			return false;
		}

		public override bool MinionContactDamage()
		{
			return true;
		}

		private void Draw()
		{
			if (AI_STATE == STATE_DASH)
			{
				Projectile.rotation = Projectile.velocity.X * 0.05f;
			}
			else
			{
				Projectile.rotation = 0;
			}

			Projectile.LoopAnimation(4);
		}

		public override bool PreDraw(ref Color lightColor)
		{
			return false;
		}

		public override void PostDraw(Color lightColor)
		{
			lightColor = Projectile.GetAlpha(lightColor) * 0.99f; //1f is opaque
			lightColor.R = Math.Max(lightColor.R, (byte)200); //100 for dark
			lightColor.G = Math.Max(lightColor.G, (byte)200);
			lightColor.B = Math.Max(lightColor.B, (byte)200);

			//the one that spawns on hit via SigilOfEmergency
			if (Projectile.minionSlots == 0f && Projectile.timeLeft < 120)
			{
				lightColor = Projectile.GetAlpha(lightColor) * (Projectile.timeLeft / 120f);
			}

			SpriteEffects effects = SpriteEffects.None;
			Texture2D image = TextureAssets.Projectile[Projectile.type].Value;

			AssPlayer mPlayer = Projectile.GetOwner().GetModPlayer<AssPlayer>();
			Rectangle bounds = image.Frame(1, Main.projFrames[Projectile.type], frameY: Projectile.frame);
			Vector2 stupidOffset = new Vector2(Projectile.width / 2, Projectile.height - 10f + sinY);
			Vector2 origin = bounds.Size() / 2;

			if (mPlayer.soulSaviorArmor && Projectile.minionSlots == 1f)
			{
				//Texture2D empoweredImage = Mod.Assets.Request<Texture2D>("Projectiles/Minions/CompanionDungeonSouls/CompanionDungeonSoul_Empowered").Value;
				Texture2D empoweredImage = ModContent.Request<Texture2D>(Texture + "_Empowered").Value;

				float addScale = (float)((Math.Sin((sincounter / 240f) * MathHelper.TwoPi) + 0.5f) * 0.1f);
				Color color = Color.White * (0.1f + 3 * addScale);
				Main.EntitySpriteDraw(empoweredImage, Projectile.position - Main.screenPosition + stupidOffset, bounds, color, Projectile.rotation, origin, Projectile.scale + 0.1f + addScale, effects, 0);
			}

			Main.EntitySpriteDraw(image, Projectile.position - Main.screenPosition + stupidOffset, bounds, lightColor, Projectile.rotation, origin, Projectile.scale, effects, 0);
		}

		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			AssPlayer mPlayer = Projectile.GetOwner().GetModPlayer<AssPlayer>();
			if (mPlayer.soulSaviorArmor)
			{
				damage = (int)(1.3f * damage);
			}
		}

		private const float STATE_MAIN = 0f;
		private const float STATE_NOCLIP = 1f;
		private const float STATE_DASH = 2f;

		public float AI_STATE
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

		public override void AI()
		{
			//AI_STATE == 0 : no target found, or found and approaching
			//AI_STATE == 1 : noclipping to player
			//AI_STATE == 2 : target found, dashing (includes delay after dash)

			Player player = Projectile.GetOwner();
			AssPlayer mPlayer = player.GetModPlayer<AssPlayer>();
			if (player.dead)
			{
				mPlayer.soulMinion = false;
			}

			if (isTemp)
			{
				Projectile.minionSlots = 0f;
				Projectile.timeLeft = 600; //10 seconds
				isTemp = false;
			}

			if (player.dead && Projectile.minionSlots == 0f)
			{
				Projectile.timeLeft = 0; //kill temporary soul when dead
			}

			if (mPlayer.soulMinion && (Projectile.minionSlots == 0.5f || Projectile.minionSlots == 1f)) //if spawned naturally they will have 0.5f
			{
				Projectile.timeLeft = 2;
			}

			float distanceFromTargetSQ = defdistanceFromTarget * defdistanceFromTarget;

			float overlapVelo = 0.04f; //0.05
			for (int i = 0; i < Main.maxProjectiles; i++)
			{
				//fix overlap with other minions
				Projectile other = Main.projectile[i];
				if (i != Projectile.whoAmI && other.active && other.owner == Projectile.owner && Math.Abs(Projectile.position.X - other.position.X) + Math.Abs(Projectile.position.Y - other.position.Y) < Projectile.width)
				{
					if (Projectile.position.X < other.position.X) Projectile.velocity.X = Projectile.velocity.X - overlapVelo;
					else Projectile.velocity.X = Projectile.velocity.X + overlapVelo;

					if (Projectile.position.Y < other.position.Y) Projectile.velocity.Y = Projectile.velocity.Y - overlapVelo;
					else Projectile.velocity.Y = Projectile.velocity.Y + overlapVelo;
				}
			}
			bool flag23 = false;
			if (AI_STATE == STATE_DASH) //attack mode

			{
				Projectile.friendly = true;
				Projectile.ai[1] += 1f;
				Projectile.extraUpdates = 1;

				if (Projectile.ai[1] > defdashDelay) //40f
				{
					Projectile.ai[1] = 1f;
					AI_STATE = STATE_MAIN;
					Projectile.extraUpdates = 0;
					Projectile.numUpdates = 0;
					Projectile.netUpdate = true;
				}
				else
				{
					flag23 = true;
				}
			}

			if (!flag23)
			{
				Vector2 targetCenter = Projectile.position;
				bool foundTarget = false;
				//if (AI_STATE != STATE_NOCLIP)
				//{
				//    projectile.tileCollide = false; //true
				//}
				//if (projectile.tileCollide && WorldGen.SolidTile(Framing.GetTileSafely((int)projectile.Center.X / 16, (int)projectile.Center.Y / 16)))
				//{
				//    projectile.tileCollide = false;
				//}

				//only target closest NPC if that NPC is some range (200f) maybe

				//NPC ownerMinionAttackTargetNPC3 = projectile.OwnerMinionAttackTargetNPC;
				//if (ownerMinionAttackTargetNPC3 != null && ownerMinionAttackTargetNPC3.CanBeChasedBy(this))
				//{
				//    float between = Vector2.Distance(ownerMinionAttackTargetNPC3.Center, projectile.Center);
				//    if (((Vector2.Distance(projectile.Center, vector40) > between && between < distance1) || !foundTarget) && 
				//        Collision.CanHitLine(projectile.position, projectile.width, projectile.height, ownerMinionAttackTargetNPC3.position, ownerMinionAttackTargetNPC3.width, ownerMinionAttackTargetNPC3.height))
				//    {
				//        distance1 = between;
				//        vector40 = ownerMinionAttackTargetNPC3.Center;
				//        foundTarget = true;
				//    }
				//}
				int targetIndex = -1;
				if (!foundTarget)
				{
					for (int j = 0; j < Main.maxNPCs; j++)
					{
						NPC npc = Main.npc[j];
						if (npc.CanBeChasedBy())
						{
							float betweenSQ = Projectile.DistanceSQ(npc.Center);
							if (((Projectile.DistanceSQ(targetCenter) > betweenSQ && betweenSQ < distanceFromTargetSQ) || !foundTarget) &&
								//EITHER HE CAN SEE IT, OR THE TARGET IS (default case: 14) TILES AWAY BUT THE MINION IS INSIDE A TILE
								//makes it so the soul can still attack if it dashed "through tiles"
								(Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, npc.position, npc.width, npc.height) ||
								(betweenSQ < defdistanceAttackNoclip/* && Collision.SolidCollision(projectile.position, projectile.width, projectile.height)*/)))
							{
								distanceFromTargetSQ = betweenSQ;
								targetCenter = npc.Center;
								targetIndex = j;
								foundTarget = true;
							}
						}
					}
				}
				float distanceNoclip = defdistancePlayerFarAway;
				if (foundTarget)
				{
					Projectile.friendly = true;
					//Main.NewText(projectile.ai[1] + " " + Main.time);
					distanceNoclip = defdistancePlayerFarAwayWhenHasTarget;
				}
				if (Vector2.Distance(player.Center, Projectile.Center) > distanceNoclip) //go to player
				{
					AI_STATE = STATE_NOCLIP;
					Projectile.tileCollide = false; //true
					Projectile.netUpdate = true;
				}
				if (foundTarget && AI_STATE == STATE_MAIN)//idek
				{
					Vector2 distanceToTargetVector = targetCenter - Projectile.Center;
					float distanceToTarget = distanceToTargetVector.Length();
					distanceToTargetVector.Normalize();
					//Main.NewText(distanceToTarget);
					if (distanceToTarget > defdistanceToEnemyBeforeCanDash) //200f //approach distance to enemy
					{
						//if its far away from it
						//Main.NewText("first " + Main.time);
						distanceToTargetVector *= veloFactorToEnemy;
						Projectile.velocity = (Projectile.velocity * (accFactorToEnemy - 1) + distanceToTargetVector) / accFactorToEnemy;
					}
					else //slowdown after a dash
					{
						//if its close to the enemy
						//Main.NewText("second " + distanceToTarget);
						distanceToTargetVector *= 0f - veloFactorAfterDash;
						Projectile.velocity = (Projectile.velocity * (accFactorAfterDash - 1) + distanceToTargetVector) / accFactorAfterDash;
					}
				}
				else //!(foundTarget && AI_STATE == STATE_MAIN)
				{
					Projectile.friendly = false;
					float veloIdle = defveloIdle; //6f

					Vector2 distanceToPlayerVector = player.Center - Projectile.Center + new Vector2(0f, defplayerFloatHeight); //at what height it floats above player
					float distanceToPlayer = distanceToPlayerVector.Length();
					if (distanceToPlayer > defplayerCatchUpIdle) //8f
					{
						veloIdle = defveloCatchUpIdle; //8f
					}
					if (AI_STATE == STATE_NOCLIP) //noclipping
					{
						veloIdle = defveloNoclip; //15f
					}
					if (distanceToPlayer < defbackToIdleFromNoclipping && AI_STATE == STATE_NOCLIP && !Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height))
					{
						AI_STATE = STATE_MAIN;
						Projectile.netUpdate = true;
					}
					if (distanceToPlayer > 2000f) //teleport to player it distance too big
					{
						Projectile.position = player.Center;
						Projectile.netUpdate = true;
					}
					if (distanceToPlayer > 70f) //the immediate range around the player (when it passively floats about)
					{
						distanceToPlayerVector.Normalize();
						distanceToPlayerVector *= veloIdle;
						float accIdle = 100f; //41f
						Projectile.velocity = (Projectile.velocity * (accIdle - 1) + distanceToPlayerVector) / accIdle;
					}
					else if (Projectile.velocity.X == 0f && Projectile.velocity.Y == 0f)
					{
						Projectile.velocity.X = -0.15f;
						Projectile.velocity.Y = -0.05f;
					}
				}

				if (Projectile.ai[1] > 0f)
				{
					//projectile.ai[1] += 1f;
					Projectile.ai[1] += Main.rand.Next(1, 4);
				}

				if (Projectile.ai[1] > defdashDelay)
				{
					Projectile.ai[1] = 0f;
					Projectile.netUpdate = true;
				}

				if (AI_STATE == STATE_MAIN)
				{
					if ((Projectile.ai[1] == 0f & foundTarget) && distanceFromTargetSQ < defstartDashRange * defstartDashRange) //500f //DASH HERE YEEEEEEE
					{
						Projectile.ai[1] = 1f;
						if (Main.myPlayer == Projectile.owner)
						{
							Vector2 targetVeloOffset = Main.npc[targetIndex].velocity;

							AI_STATE = STATE_DASH;
							Vector2 value20 = targetCenter + targetVeloOffset * 5 - Projectile.Center;
							value20.Normalize();
							Projectile.velocity = value20 * defdashIntensity; //8f
							Projectile.netUpdate = true;
						}
					}
				}
			}

			Draw();

			Lighting.AddLight(Projectile.Center, new Vector3(0.15f, 0.15f, 0.35f));

			sincounter = sincounter > 120 ? 0 : sincounter + 1;
			sinY = (float)((Math.Sin((sincounter / 120f) * MathHelper.TwoPi) - 1) * 10);

			//Generate visual dust
			if (Main.rand.NextFloat() < 0.02f)
			{
				Vector2 position = new Vector2(Projectile.position.X + Projectile.width / 2, Projectile.position.Y + Projectile.height / 2 + sinY);
				Dust dust = Dust.NewDustPerfect(position, 135, new Vector2(Main.rand.NextFloat(-0.3f, 0.3f), Main.rand.NextFloat(-1.5f, -1f)), 200, Color.LightGray, 1f);
				dust.noGravity = false;
				dust.noLight = true;
				dust.fadeIn = Main.rand.NextFloat(0.8f, 1.1f);

				if (dustColor != 0)
				{
					dust.shader = GameShaders.Armor.GetSecondaryShader((byte)GameShaders.Armor.GetShaderIdFromItemId(dustColor), player);
				}
			}

			//Dust upon spawning
			if (Projectile.localAI[0] < 60)
			{
				Projectile.localAI[0]++;
				Vector2 position = new Vector2(Projectile.position.X + Projectile.width / 2, Projectile.position.Y + Projectile.height / 3 + sinY);

				if (Main.rand.NextFloat() < (60 - Projectile.localAI[0]) / 360f)
				{
					Dust dust = Dust.NewDustPerfect(position, 135, new Vector2(Main.rand.NextFloat(-0.3f, 0.3f), Main.rand.NextFloat(-1.5f, -1f)), 200, Color.LightGray, (60 - Projectile.localAI[0]) / 60f + 1f);
					dust.noGravity = false;
					dust.noLight = true;
					dust.fadeIn = Main.rand.NextFloat(0.2f);

					if (dustColor != 0)
					{
						dust.shader = GameShaders.Armor.GetSecondaryShader((byte)GameShaders.Armor.GetShaderIdFromItemId(dustColor), player);
					}
				}
			}
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			fallThrough = true;
			return false; //true
		}
	}
}
