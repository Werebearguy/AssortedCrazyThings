using AssortedCrazyThings.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Minions.CompanionDungeonSouls
{
    public abstract class CompanionDungeonSoulMinionBase : ModProjectile
    {
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
            Main.projFrames[projectile.type] = 8;
            Main.projPet[projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[projectile.type] = true;
            ProjectileID.Sets.Homing[projectile.type] = true;
        }

        public sealed override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.Spazmamini);
            projectile.width = 18; //14
            projectile.height = 28; //24
            projectile.aiStyle = -1;
            projectile.friendly = true;
            projectile.minion = true; //only determines the damage type
            projectile.minionSlots = 0.5f;
            projectile.penetrate = -1;
            projectile.tileCollide = false;

            projectile.usesIDStaticNPCImmunity = true;
            projectile.idStaticNPCHitCooldown = 8;

            dustColor = 0;

            MoreSetDefaults();
        }

        public virtual void MoreSetDefaults()
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

        //public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        //{
        //    //if (target.immune[projectile.owner] == 10)
        //    //{
        //    //    projectile.usesLocalNPCImmunity = true;
        //    //    projectile.localNPCImmunity[target.whoAmI] = 10;
        //    //    target.immune[projectile.owner] = 8; //0
        //    //    //immunity frame now 8 instead of 10 ticks long
        //    //}
        //}

        private void Draw()
        {
            if (AI_STATE == STATE_DASH)
            {
                projectile.rotation = projectile.velocity.X * 0.05f;
            }
            else
            {
                projectile.rotation = 0;
            }

            projectile.frameCounter++;
            if (projectile.frameCounter >= 4)
            {
                projectile.frame++;
                projectile.frameCounter = 0;
            }
            if (projectile.frame >= Main.projFrames[projectile.type])
            {
                projectile.frame = 0;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            return false;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            float sinY = -10f;
            if (Main.hasFocus)  //here since we override the AI, we can use the projectiles own frame and frameCounter in Draw()
            {
                Draw();
                sincounter = sincounter > 120 ? 0 : sincounter + 1;
                sinY = (float)((Math.Sin((sincounter / 120f) * MathHelper.TwoPi) - 1) * 10);
            }

            lightColor = projectile.GetAlpha(lightColor) * 0.99f; //1f is opaque
            lightColor.R = Math.Max(lightColor.R, (byte)200); //100 for dark
            lightColor.G = Math.Max(lightColor.G, (byte)200);
            lightColor.B = Math.Max(lightColor.B, (byte)200);

            //the one that spawns on hit via SigilOfEmergency
            if (projectile.minionSlots == 0f && projectile.timeLeft < 120)
            {
                lightColor = projectile.GetAlpha(lightColor) * (projectile.timeLeft / 120f);
            }

            Lighting.AddLight(projectile.Center, new Vector3(0.15f, 0.15f, 0.35f));

            SpriteEffects effects = SpriteEffects.None;
            Texture2D image = mod.GetTexture("Projectiles/Minions/CompanionDungeonSouls/" + Name);// Main.projectileTexture[projectile.type];

            AssPlayer mPlayer = projectile.GetOwner().GetModPlayer<AssPlayer>();
            if (mPlayer.soulSaviorArmor && projectile.minionSlots == 1f)
            {
                image = mod.GetTexture("Projectiles/Minions/CompanionDungeonSouls/" + Name + "_Empowered");
            }
            Rectangle bounds = new Rectangle
            {
                X = 0,
                Y = projectile.frame,
                Width = image.Bounds.Width,
                Height = image.Bounds.Height / Main.projFrames[projectile.type]
            };
            bounds.Y *= bounds.Height; //cause proj.frame only contains the frame number

            //Generate visual dust
            if (Main.rand.NextFloat() < 0.02f)
            {
                Vector2 position = new Vector2(projectile.position.X + projectile.width / 2, projectile.position.Y + projectile.height / 2 + sinY);
                Dust dust = Dust.NewDustPerfect(position, 135, new Vector2(Main.rand.NextFloat(-0.3f, 0.3f), Main.rand.NextFloat(-1.5f, -1f)), 200, Color.LightGray, 1f);
                dust.noGravity = false;
                dust.noLight = true;
                dust.fadeIn = Main.rand.NextFloat(0.8f, 1.1f);

                if (dustColor != 0)
                {
                    dust.shader = GameShaders.Armor.GetSecondaryShader((byte)GameShaders.Armor.GetShaderIdFromItemId(dustColor), projectile.GetOwner());
                }
            }

            //Dust upon spawning
            if (projectile.localAI[0] < 60)
            {
                Vector2 position = new Vector2(projectile.position.X + projectile.width / 2, projectile.position.Y + projectile.height / 3 + sinY);
                for (int i = 0; i < 1; i++)
                {
                    if (Main.rand.NextFloat() < (60 - projectile.localAI[0]) / 360f)
                    {
                        Dust dust = Dust.NewDustPerfect(position, 135, new Vector2(Main.rand.NextFloat(-0.3f, 0.3f), Main.rand.NextFloat(-1.5f, -1f)), 200, Color.LightGray, (60 - projectile.localAI[0]) / 60f + 1f);
                        dust.noGravity = false;
                        dust.noLight = true;
                        dust.fadeIn = Main.rand.NextFloat(0.2f);

                        if (dustColor != 0)
                        {
                            dust.shader = GameShaders.Armor.GetSecondaryShader((byte)GameShaders.Armor.GetShaderIdFromItemId(dustColor), projectile.GetOwner());
                        }
                    }
                }
                projectile.localAI[0]++;
            }

            Vector2 stupidOffset = new Vector2(projectile.width / 2, projectile.height - 10f + sinY);

            spriteBatch.Draw(image, projectile.position - Main.screenPosition + stupidOffset, bounds, lightColor, projectile.rotation, bounds.Size() / 2, projectile.scale, effects, 0f);
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            AssPlayer mPlayer = projectile.GetOwner().GetModPlayer<AssPlayer>();
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
                return projectile.ai[0];
            }
            set
            {
                projectile.ai[0] = value;
            }
        }

        public override void AI()
        {
            //AI_STATE == 0 : no target found, or found and approaching
            //AI_STATE == 1 : noclipping to player
            //AI_STATE == 2 : target found, dashing (includes delay after dash)

            Player player = projectile.GetOwner();
            AssPlayer mPlayer = player.GetModPlayer<AssPlayer>();
            if (player.dead)
            {
                mPlayer.soulMinion = false;
            }

            if (isTemp)
            {
                projectile.minionSlots = 0f;
                projectile.timeLeft = 600; //10 seconds
                isTemp = false;
            }

            if (player.dead && projectile.minionSlots == 0f)
            {
                projectile.timeLeft = 0; //kill temporary soul when dead
            }

            if (mPlayer.soulMinion && (projectile.minionSlots == 0.5f || projectile.minionSlots == 1f)) //if spawned naturally they will have 0.5f
            {
                projectile.timeLeft = 2;
            }

            float distanceFromTarget = defdistanceFromTarget;

            float overlapVelo = 0.04f; //0.05
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                //fix overlap with other minions
                if (i != projectile.whoAmI && Main.projectile[i].active && Main.projectile[i].owner == projectile.owner && Math.Abs(projectile.position.X - Main.projectile[i].position.X) + Math.Abs(projectile.position.Y - Main.projectile[i].position.Y) < projectile.width)
                {
                    if (projectile.position.X < Main.projectile[i].position.X) projectile.velocity.X = projectile.velocity.X - overlapVelo;
                    else projectile.velocity.X = projectile.velocity.X + overlapVelo;

                    if (projectile.position.Y < Main.projectile[i].position.Y) projectile.velocity.Y = projectile.velocity.Y - overlapVelo;
                    else projectile.velocity.Y = projectile.velocity.Y + overlapVelo;
                }
            }
            bool flag23 = false;
            if (AI_STATE == STATE_DASH) //attack mode

            {
                projectile.friendly = true;
                projectile.ai[1] += 1f;
                projectile.extraUpdates = 1;

                if (projectile.ai[1] > defdashDelay) //40f
                {
                    projectile.ai[1] = 1f;
                    AI_STATE = STATE_MAIN;
                    projectile.extraUpdates = 0;
                    projectile.numUpdates = 0;
                    projectile.netUpdate = true;
                }
                else
                {
                    flag23 = true;
                }
            }

            if (!flag23)
            {
                Vector2 targetCenter = projectile.position;
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
                            float between = Vector2.Distance(npc.Center, projectile.Center);
                            if (((Vector2.Distance(projectile.Center, targetCenter) > between && between < distanceFromTarget) || !foundTarget) &&
                                //EITHER HE CAN SEE IT, OR THE TARGET IS (default case: 14) TILES AWAY BUT THE MINION IS INSIDE A TILE
                                //makes it so the soul can still attack if it dashed "through tiles"
                                (Collision.CanHitLine(projectile.position, projectile.width, projectile.height, npc.position, npc.width, npc.height) ||
                                (between < defdistanceAttackNoclip/* && Collision.SolidCollision(projectile.position, projectile.width, projectile.height)*/)))
                            {
                                distanceFromTarget = between;
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
                    projectile.friendly = true;
                    //Main.NewText(projectile.ai[1] + " " + Main.time);
                    distanceNoclip = defdistancePlayerFarAwayWhenHasTarget;
                }
                if (Vector2.Distance(player.Center, projectile.Center) > distanceNoclip) //go to player
                {
                    AI_STATE = STATE_NOCLIP;
                    projectile.tileCollide = false; //true
                    projectile.netUpdate = true;
                }
                if (foundTarget && AI_STATE == STATE_MAIN)//idek
                {
                    Vector2 distanceToTargetVector = targetCenter - projectile.Center;
                    float distanceToTarget = distanceToTargetVector.Length();
                    distanceToTargetVector.Normalize();
                    //Main.NewText(distanceToTarget);
                    if (distanceToTarget > defdistanceToEnemyBeforeCanDash) //200f //approach distance to enemy
                    {
                        //if its far away from it
                        //Main.NewText("first " + Main.time);
                        distanceToTargetVector *= veloFactorToEnemy;
                        projectile.velocity = (projectile.velocity * (accFactorToEnemy - 1) + distanceToTargetVector) / accFactorToEnemy;
                    }
                    else //slowdown after a dash
                    {
                        //if its close to the enemy
                        //Main.NewText("second " + distanceToTarget);
                        distanceToTargetVector *= 0f - veloFactorAfterDash;
                        projectile.velocity = (projectile.velocity * (accFactorAfterDash - 1) + distanceToTargetVector) / accFactorAfterDash;
                    }
                }
                else //!(foundTarget && AI_STATE == STATE_MAIN)
                {
                    projectile.friendly = false;
                    float veloIdle = defveloIdle; //6f

                    Vector2 distanceToPlayerVector = player.Center - projectile.Center + new Vector2(0f, defplayerFloatHeight); //at what height it floats above player
                    float distanceToPlayer = distanceToPlayerVector.Length();
                    if (distanceToPlayer > defplayerCatchUpIdle) //8f
                    {
                        veloIdle = defveloCatchUpIdle; //8f
                    }
                    if (AI_STATE == STATE_NOCLIP) //noclipping
                    {
                        veloIdle = defveloNoclip; //15f
                    }
                    if (distanceToPlayer < defbackToIdleFromNoclipping && AI_STATE == STATE_NOCLIP && !Collision.SolidCollision(projectile.position, projectile.width, projectile.height))
                    {
                        AI_STATE = STATE_MAIN;
                        projectile.netUpdate = true;
                    }
                    if (distanceToPlayer > 2000f) //teleport to player it distance too big
                    {
                        projectile.position = player.Center;
                        projectile.netUpdate = true;
                    }
                    if (distanceToPlayer > 70f) //the immediate range around the player (when it passively floats about)
                    {
                        distanceToPlayerVector.Normalize();
                        distanceToPlayerVector *= veloIdle;
                        float accIdle = 100f; //41f
                        projectile.velocity = (projectile.velocity * (accIdle - 1) + distanceToPlayerVector) / accIdle;
                    }
                    else if (projectile.velocity.X == 0f && projectile.velocity.Y == 0f)
                    {
                        projectile.velocity.X = -0.15f;
                        projectile.velocity.Y = -0.05f;
                    }
                }

                if (projectile.ai[1] > 0f)
                {
                    //projectile.ai[1] += 1f;
                    projectile.ai[1] += Main.rand.Next(1, 4);
                }

                if (projectile.ai[1] > defdashDelay)
                {
                    projectile.ai[1] = 0f;
                    projectile.netUpdate = true;
                }

                if (AI_STATE == STATE_MAIN)
                {
                    if ((projectile.ai[1] == 0f & foundTarget) && distanceFromTarget < defstartDashRange) //500f //DASH HERE YEEEEEEE
                    {
                        projectile.ai[1] = 1f;
                        if (Main.myPlayer == projectile.owner)
                        {
                            Vector2 targetVeloOffset = Main.npc[targetIndex].velocity;

                            AI_STATE = STATE_DASH;
                            Vector2 value20 = targetCenter + targetVeloOffset * 5 - projectile.Center;
                            value20.Normalize();
                            projectile.velocity = value20 * defdashIntensity; //8f
                            projectile.netUpdate = true;
                        }
                    }
                }
            }
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            fallThrough = true;
            return false; //true
        }
    }
}