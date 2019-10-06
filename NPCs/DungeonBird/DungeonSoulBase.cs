using AssortedCrazyThings.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs.DungeonBird
{
    //this class also contains the NPC classes at the very bottom

    public abstract class DungeonSoulBase : ModNPC
    {
        protected double frameCount;
        protected float fadeAwayMax;
        public static int SoulActiveTime = NPC.activeTime * 5;

        public static int wid = 34; //24
        public static int hei = 38;

        public override void SetDefaults()
        {
            //adjust stats here to match harvester hitbox 1:1, then do findframes in postdraw
            npc.width = wid; //42 //16
            npc.height = hei; //52 //24
            npc.npcSlots = 0f; //takes 1/10 npc slots out of 200 when alive
            npc.chaseable = false;
            npc.dontCountMe = true;
            npc.dontTakeDamageFromHostiles = true;
            npc.dontTakeDamage = true;
            npc.friendly = true;
            npc.noGravity = true;
            npc.damage = 0;
            npc.defense = 0;
            npc.lifeMax = 5;
            npc.scale = 1f;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.value = 0f;
            npc.aiStyle = -1;
            aiType = -1;// NPCID.ToxicSludge;
            animationType = -1;// NPCID.ToxicSludge;
            npc.color = new Color(0, 0, 0, 50);
            npc.timeLeft = SoulActiveTime;
            npc.direction = 1;
            MoreSetDefaults();
        }

        public virtual void MoreSetDefaults()
        {

        }

        public static readonly short offsetYPeriod = 120;

        public static void SetTimeLeft(NPC npcto, NPC npcfrom)
        {
            if (!npcfrom.Equals(npcto))
            {
                //type check since souls might despawn and index changes
                if (npcfrom.active && (AssWorld.harvesterTypes[0] == npcfrom.type || AssWorld.harvesterTypes[1] == npcfrom.type) && npcto.timeLeft > HarvesterBase.EatTimeConst)
                {
                    npcto.timeLeft = HarvesterBase.EatTimeConst;
                    npcto.netUpdate = true;
                }
            }
        }

        public float AI_State
        {
            get
            {
                return npc.ai[0];
            }
            set
            {
                npc.ai[0] = value;
            }
        }

        public float AI_Local_Timer
        {
            get
            {
                return npc.localAI[0];
            }
            set
            {
                npc.localAI[0] = value;
            }
        }

        public static void KillInstantly(NPC npc)
        {
            npc.life = 0;
            npc.active = false;
            npc.netUpdate = true;
        }

        public override bool CheckActive()
        {
            //manually decrease timeleft
            return false;
        }

        protected int HarvesterTarget()
        {
            int tar = 200;
            for (short j = 0; j < 200; j++)
            {
                if (Main.npc[j].active && (AssWorld.harvesterTypes[0] == Main.npc[j].type || AssWorld.harvesterTypes[1] == Main.npc[j].type))
                {
                    tar = j;
                    break;
                }
            }
            return tar;
        }

        protected Entity GetTarget()
        {
            int res = HarvesterTarget();
            if (res != 200) return Main.npc[res];
            else return npc;
        }

        protected bool IsTargetActive()
        {
            return !GetTarget().Equals(npc);
        }

        public override void FindFrame(int frameHeight)
        {
            if (AI_State == 0)
            {
                npc.frameCounter++;
                if (npc.frameCounter >= frameCount)
                {
                    npc.frame.Y += frameHeight;
                    npc.frameCounter = 0;
                    if (npc.frame.Y > 3 * frameHeight)
                    {
                        npc.frame.Y = 0;
                    }
                }
            }
            else if (AI_State == 1)
            {
                if (npc.velocity.Y > 0) //dropping down
                {
                    npc.frame.Y = frameHeight * 4;
                }
                else if ((npc.velocity.Y == 0 || npc.velocity.Y < 2f && npc.velocity.Y > 0f) && npc.velocity.X == 0)
                {
                    npc.frameCounter++;
                    if (npc.frameCounter <= 8.0)
                    {
                        npc.frame.Y = frameHeight * 5;
                    }
                    else if (npc.frameCounter <= 16.0)
                    {
                        npc.frame.Y = frameHeight * 6;
                    }
                    else if (npc.frameCounter <= 24.0)
                    {
                        npc.frame.Y = frameHeight * 7;
                    }
                    else if (npc.frameCounter <= 32.0)
                    {
                        npc.frame.Y = frameHeight * 6;
                    }
                    else
                    {
                        npc.frameCounter = 0;
                    }
                }
            }
            else
            {
                npc.frame.Y = 0;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            return false;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            lightColor = npc.GetAlpha(lightColor) * 0.99f; //1f is opaque
            lightColor.R = Math.Max(lightColor.R, (byte)200); //100 for dark
            lightColor.G = Math.Max(lightColor.G, (byte)200);
            lightColor.B = Math.Max(lightColor.B, (byte)200);

            Texture2D image = Main.npcTexture[npc.type];
            Rectangle bounds = new Rectangle
            {
                X = 0,
                Y = npc.frame.Y,
                Width = image.Bounds.Width,
                Height = image.Bounds.Height / Main.npcFrameCount[npc.type]
            };

            float sinY = 0;
            if (Main.netMode != NetmodeID.Server)
            {
                if (AI_State == 0)
                {
                    AI_Local_Timer = AI_Local_Timer > offsetYPeriod ? 0 : AI_Local_Timer + 1;
                    sinY = (float)((Math.Sin((AI_Local_Timer / offsetYPeriod) * MathHelper.TwoPi) - 1) * 10);
                }
                else if (AI_State == 1)
                {
                    if (AI_Local_Timer != 0.25f * offsetYPeriod && AI_Local_Timer != 1.25f * offsetYPeriod) //zero at 1/4 and 5/4 PI
                    {
                        AI_Local_Timer++;
                        sinY = (float)((Math.Sin((AI_Local_Timer / offsetYPeriod) * MathHelper.TwoPi) - 1) * 10);
                    }
                    else
                    {
                        sinY = 0;
                    }
                }
            }

            if (npc.timeLeft <= fadeAwayMax && (AI_State == 0))
            {
                lightColor = npc.GetAlpha(lightColor) * (npc.timeLeft / (float)fadeAwayMax);
            }

            Vector2 stupidOffset = new Vector2(wid / 2, (hei - 10f) + sinY);

            if (AI_State == 1)
            {
                if ((npc.velocity.Y == 0 || npc.velocity.Y < 2f && npc.velocity.Y > 0f) && npc.velocity.X == 0)
                {
                    lightColor = Color.White * 0.05f; //draw really weak
                }
            }
            SpriteEffects effects = SpriteEffects.None;

            spriteBatch.Draw(image, npc.position - Main.screenPosition + stupidOffset, bounds, lightColor, npc.rotation, bounds.Size() / 2, npc.scale, effects, 0f);
        }

        public override void AI()
        {
            npc.scale = 1f;
            Entity tar = GetTarget();
            NPC tarnpc = new NPC();
            if (tar is NPC)
            {
                tarnpc = (NPC)tar;
            }

            npc.noTileCollide = false;

            if (AI_State == 0)
            {
                npc.noGravity = true;
                //npc.position - new Vector2(10f, 4f), npc.width + 20, npc.height + 4

                //concider only the bottom half of the hitbox, a bit wider (minus a small bit below)
                //if (Collision.SolidCollision(npc.position + new Vector2(-10f, npc.height / 2), npc.width + 20, npc.height / 2 -2))

                if (Collision.SolidCollision(npc.position/* + new Vector2(-10f, 0f)*/, npc.width/* + 20*/, npc.height/* + 2*/))
                {
                    Vector2 between = tarnpc.Center + new Vector2(0f, -4f) - npc.Center;
                    if (IsTargetActive() && between.Length() > 100f)
                    {
                        npc.noTileCollide = true;
                        float factor = 2f; //2f
                        int acc = 100; //4
                        Vector2 between2 = between;
                        between2.Normalize();
                        between2 *= factor;
                        if (npc.velocity.Length() < 1.5f) npc.velocity = (npc.velocity * (acc - 1) + between2) / acc;

                        if (between.Length() < 105f)
                        {
                            npc.netUpdate = true;
                        }
                        return;
                    }
                }
                else
                {
                    npc.noTileCollide = false;
                    npc.velocity *= 0.075f;
                }
            }

            //go into "eaten" mode
            if (!tarnpc.Equals(npc))
            {
                if (npc.getRect().Intersects(tarnpc.getRect()) && AI_State == 0 && !Collision.SolidCollision(npc.position, npc.width, npc.height/* + 2*/)/* && tarnpc.velocity.Y <= 0*/) // tarnpc.velocity.Y <= 0 for only when it jumps
                {
                    AI_State = 1;
                    npc.velocity.Y = 1f;
                }
            }

            //lock into place when harvester and soul hitbox collide
            if (AI_State == 1 && npc.velocity.Y != 0)
            {
                float betweenX = tarnpc.Center.X - npc.Center.X;
                if (betweenX > 2f || betweenX < -2f)
                {
                    float factor = 4f; //2f
                    int acc = 4; //4
                    betweenX = betweenX / Math.Abs(betweenX);
                    betweenX *= factor;
                    npc.velocity.X = (npc.velocity.X * (acc - 1) + betweenX) / acc;
                    npc.noTileCollide = false;
                }
                else
                {
                    npc.velocity.X = 0;
                }
                npc.velocity.Y += 0.08f; //0.06
            }
            else if (AI_State == 1 && (npc.velocity.Y == 0 || npc.velocity.Y < 2f && npc.velocity.Y > 0f))
            {
                npc.velocity.X = 0;
            }

            --npc.timeLeft;
            if (npc.timeLeft < 0)
            {
                KillInstantly(npc);
            }
        }
    }

    //the one the harvester hunts for
    public class DungeonSoul : DungeonSoulBase
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dungeon Soul");
            Main.npcFrameCount[npc.type] = 8;
        }

        public override void MoreSetDefaults()
        {
            frameCount = 8.0;
            Main.npcCatchable[ModContent.NPCType<DungeonSoul>()] = true;
            npc.catchItem = (short)ModContent.ItemType<CaughtDungeonSoul>();

            fadeAwayMax = HarvesterBase.EatTimeConst;
        }
    }

    //the one that gets converted into
    public class DungeonSoulFreed : DungeonSoulBase
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dungeon Soul");
            Main.npcFrameCount[npc.type] = 8;
        }

        public override void MoreSetDefaults()
        {
            frameCount = 4.0;
            Main.npcCatchable[ModContent.NPCType<DungeonSoulFreed>()] = true;
            npc.catchItem = (short)ModContent.ItemType<CaughtDungeonSoulFreed>();

            npc.timeLeft = 3600;
            fadeAwayMax = 3600;
        }

        public override bool PreAI()
        {
            //only if awakened
            if (npc.ai[2] != 0)
            {
                AI_Local_Timer = npc.ai[2];
                npc.ai[2] = 0;
            }
            npc.noTileCollide = false;
            npc.velocity *= 0.95f;

            --npc.timeLeft;
            if (npc.timeLeft < 0)
            {
                KillInstantly(npc);
            }
            return false;
        }
    }
}
