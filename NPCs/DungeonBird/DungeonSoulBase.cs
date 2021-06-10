using AssortedCrazyThings.Base;
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
        protected int frameSpeed;
        protected float fadeAwayMax;
        public static int SoulActiveTime = NPC.activeTime * 5;

        public static int wid = 34; //24
        public static int hei = 38;

        public sealed override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dungeon Soul");
            Main.npcFrameCount[NPC.type] = 6;
            Main.npcCatchable[NPC.type] = true;

            //Hide both souls
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            {
                Hide = true //Hides this NPC from the Bestiary
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);

            SafeSetStaticDefaults();
        }

        public virtual void SafeSetStaticDefaults()
        {

        }

        public override void SetDefaults()
        {
            //adjust stats here to match harvester hitbox 1:1, then do findframes in postdraw
            NPC.width = wid; //42 //16
            NPC.height = hei; //52 //24
            NPC.npcSlots = 0f;
            NPC.chaseable = false;
            NPC.dontCountMe = true;
            NPC.dontTakeDamageFromHostiles = true;
            NPC.dontTakeDamage = true;
            NPC.friendly = true;
            NPC.noGravity = true;
            NPC.damage = 0;
            NPC.defense = 0;
            NPC.lifeMax = 5;
            NPC.scale = 1f;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.value = 0f;
            NPC.aiStyle = -1;
            AIType = -1;// NPCID.ToxicSludge;
            AnimationType = -1;// NPCID.ToxicSludge;
            NPC.color = new Color(0, 0, 0, 50);
            NPC.timeLeft = SoulActiveTime;
            NPC.direction = 1;
            SafeSetDefaults();
        }

        public virtual void SafeSetDefaults()
        {

        }

        public static readonly short offsetYPeriod = 120;

        public static void SetTimeLeft(NPC npcto, NPC npcfrom)
        {
            if (!npcfrom.Equals(npcto))
            {
                //type check since souls might despawn and index changes
                if (npcfrom.active && (AssortedCrazyThings.harvesterTypes[0] == npcfrom.type || AssortedCrazyThings.harvesterTypes[1] == npcfrom.type) && npcto.timeLeft > HarvesterBase.EatTimeConst)
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
                return NPC.ai[0];
            }
            set
            {
                NPC.ai[0] = value;
            }
        }

        public float AI_Local_Timer
        {
            get
            {
                return NPC.localAI[0];
            }
            set
            {
                NPC.localAI[0] = value;
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
            for (short j = 0; j < Main.maxNPCs; j++)
            {
                NPC other = Main.npc[j];
                if (other.active && (AssortedCrazyThings.harvesterTypes[0] == Main.npc[j].type || AssortedCrazyThings.harvesterTypes[1] == Main.npc[j].type))
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
            else return NPC;
        }

        protected bool IsTargetActive()
        {
            return !GetTarget().Equals(NPC);
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.LoopAnimation(frameHeight, frameSpeed);
            //if (AI_State == 0)
            //{
            //    NPC.frameCounter++;
            //    if (NPC.frameCounter >= frameCount)
            //    {
            //        NPC.frame.Y += frameHeight;
            //        NPC.frameCounter = 0;
            //        if (NPC.frame.Y >= 6 * frameHeight)
            //        {
            //            NPC.frame.Y = 0;
            //        }
            //    }
            //}
            //else if (AI_State == 1)
            //{
            //    if (NPC.velocity.Y > 0) //dropping down
            //    {
            //        NPC.frame.Y = frameHeight * 4;
            //    }
            //    else if ((NPC.velocity.Y == 0 || NPC.velocity.Y < 2f && NPC.velocity.Y > 0f) && NPC.velocity.X == 0)
            //    {
            //        NPC.frameCounter++;
            //        if (NPC.frameCounter <= 8.0)
            //        {
            //            NPC.frame.Y = frameHeight * 5;
            //        }
            //        else if (NPC.frameCounter <= 16.0)
            //        {
            //            NPC.frame.Y = frameHeight * 6;
            //        }
            //        else if (NPC.frameCounter <= 24.0)
            //        {
            //            NPC.frame.Y = frameHeight * 7;
            //        }
            //        else if (NPC.frameCounter <= 32.0)
            //        {
            //            NPC.frame.Y = frameHeight * 6;
            //        }
            //        else
            //        {
            //            NPC.frameCounter = 0;
            //        }
            //    }
            //}
            //else
            //{
            //    NPC.frame.Y = 0;
            //}
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            return false;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            drawColor = NPC.GetAlpha(drawColor) * 0.99f; //1f is opaque
            drawColor.R = Math.Max(drawColor.R, (byte)200); //100 for dark
            drawColor.G = Math.Max(drawColor.G, (byte)200);
            drawColor.B = Math.Max(drawColor.B, (byte)200);

            Texture2D image = Terraria.GameContent.TextureAssets.Npc[NPC.type].Value;
            Rectangle bounds = new Rectangle
            {
                X = 0,
                Y = NPC.frame.Y,
                Width = image.Bounds.Width,
                Height = image.Bounds.Height / Main.npcFrameCount[NPC.type]
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

            if (NPC.timeLeft <= fadeAwayMax && (AI_State == 0))
            {
                drawColor = NPC.GetAlpha(drawColor) * (NPC.timeLeft / (float)fadeAwayMax);
            }

            Vector2 stupidOffset = new Vector2(wid / 2, (hei - 10f) + sinY);

            if (AI_State == 1)
            {
                if ((NPC.velocity.Y == 0 || NPC.velocity.Y < 2f && NPC.velocity.Y > 0f) && NPC.velocity.X == 0)
                {
                    drawColor = Color.White * 0.05f; //draw really weak
                }
            }
            SpriteEffects effects = SpriteEffects.None;

            spriteBatch.Draw(image, NPC.position - screenPos + stupidOffset, bounds, drawColor, NPC.rotation, bounds.Size() / 2, NPC.scale, effects, 0f);
        }

        public override void AI()
        {
            --NPC.timeLeft;
            if (NPC.timeLeft < 0)
            {
                KillInstantly(NPC);
            }

            NPC.noTileCollide = false;
            NPC.scale = 1f;

            Entity tar = GetTarget();
            if (!(tar is NPC tarnpc))
            {
                return;
            }

            if (AI_State == 0)
            {
                NPC.noGravity = true;
                //npc.position - new Vector2(10f, 4f), npc.width + 20, npc.height + 4

                //concider only the bottom half of the hitbox, a bit wider (minus a small bit below)
                //if (Collision.SolidCollision(npc.position + new Vector2(-10f, npc.height / 2), npc.width + 20, npc.height / 2 -2))

                if (Collision.SolidCollision(NPC.position/* + new Vector2(-10f, 0f)*/, NPC.width/* + 20*/, NPC.height/* + 2*/))
                {
                    Vector2 between = tarnpc.Center + new Vector2(0f, -4f) - NPC.Center;
                    if (IsTargetActive() && between.Length() > 100f)
                    {
                        NPC.noTileCollide = true;
                        float factor = 2f; //2f
                        int acc = 100; //4
                        Vector2 between2 = between;
                        between2.Normalize();
                        between2 *= factor;
                        if (NPC.velocity.Length() < 1.5f) NPC.velocity = (NPC.velocity * (acc - 1) + between2) / acc;

                        if (between.Length() < 105f)
                        {
                            NPC.netUpdate = true;
                        }
                        return;
                    }
                }
                else
                {
                    NPC.noTileCollide = false;
                    NPC.velocity *= 0.075f;
                }
            }

            //go into "eaten" mode
            if (!tarnpc.Equals(NPC))
            {
                if (NPC.getRect().Intersects(tarnpc.getRect()) && AI_State == 0 && !Collision.SolidCollision(NPC.position, NPC.width, NPC.height/* + 2*/)/* && tarnpc.velocity.Y <= 0*/) // tarnpc.velocity.Y <= 0 for only when it jumps
                {
                    AI_State = 1;
                    NPC.velocity.Y = 1f;
                }
            }

            //lock into place when harvester and soul hitbox collide
            if (AI_State == 1 && NPC.velocity.Y != 0)
            {
                float betweenX = tarnpc.Center.X - NPC.Center.X;
                if ((betweenX > 2f || betweenX < -2f) && betweenX != 0f)
                {
                    float factor = 4f; //2f
                    int acc = 4; //4
                    betweenX /= Math.Abs(betweenX);
                    betweenX *= factor;
                    NPC.velocity.X = (NPC.velocity.X * (acc - 1) + betweenX) / acc;
                    NPC.noTileCollide = false;
                }
                else
                {
                    NPC.velocity.X = 0;
                }
                NPC.velocity.Y += 0.08f; //0.06
            }
            else if (AI_State == 1 && (NPC.velocity.Y == 0 || NPC.velocity.Y < 2f && NPC.velocity.Y > 0f))
            {
                NPC.velocity.X = 0;
            }
        }
    }

    //the one the harvester hunts for
    public class DungeonSoul : DungeonSoulBase
    {
        public override void SafeSetDefaults()
        {
            frameSpeed = 6;
            NPC.catchItem = (short)ModContent.ItemType<CaughtDungeonSoul>();

            fadeAwayMax = HarvesterBase.EatTimeConst;
        }
    }

    //the one that gets converted into
    public class DungeonSoulFreed : DungeonSoulBase
    {
        public override void SafeSetDefaults()
        {
            frameSpeed = 4;
            NPC.catchItem = (short)ModContent.ItemType<CaughtDungeonSoulFreed>();

            NPC.timeLeft = 3600;
            fadeAwayMax = 3600;
        }

        public override bool PreAI()
        {
            //only if awakened
            if (NPC.ai[2] != 0)
            {
                AI_Local_Timer = NPC.ai[2];
                NPC.ai[2] = 0;
            }
            NPC.noTileCollide = false;
            NPC.velocity *= 0.95f;

            --NPC.timeLeft;
            if (NPC.timeLeft < 0)
            {
                KillInstantly(NPC);
            }
            return false;
        }
    }
}
