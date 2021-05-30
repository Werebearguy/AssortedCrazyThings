using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs.DungeonBird
{
    public abstract class HarvesterTalon : ModNPC
    {
        public override string Texture
        {
            get
            {
                return "AssortedCrazyThings/NPCs/DungeonBird/HarvesterTalon";
            }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault(Harvester.name);
            Main.npcFrameCount[NPC.type] = 1;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White * ((255 - NPC.alpha) / 255f);
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

            NPC.boss = true;
            NPC.noGravity = true;
            NPC.width = 40; //38
            NPC.height = 30; //42
            NPC.aiStyle = -1;
            NPC.damage = Harvester.talonDamage;
            NPC.defense = 28;
            NPC.lifeMax = 1337;
            NPC.scale = 1f;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.alpha = 255;
            NPC.buffImmune[BuffID.Poisoned] = true;
            NPC.buffImmune[BuffID.OnFire] = true;
            NPC.buffImmune[BuffID.Confused] = true;
            NPC.dontTakeDamage = true;
            NPC.dontCountMe = true;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            return false;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            if (AssWorld.harvesterIndex != -1)
            {
                Texture2D texture = Mod.GetTexture("NPCs/DungeonBird/HarvesterChain").Value;
                //Main.chain21Texture
                Vector2 center = new Vector2(NPC.Center.X, NPC.Center.Y);
                NPC body = Main.npc[AssWorld.harvesterIndex];
                float num22 = body.Center.X - center.X;
                float num23 = body.Center.Y - center.Y;
                num23 -= -Harvester.TalonOffsetY + 20f; //has to result to 7f

                //num22 = ((npc.type != AssortedCrazyThings.harvesterTalonLeft) ? (num22 + aaaHarvester3.TalonOffsetRightX - 12f) : (num22 + aaaHarvester3.TalonOffsetLeftX + 14f)); //66f, -70f

                num22 = ((NPC.type != AssortedCrazyThings.harvesterTalonLeft) ? (num22 + Harvester.TalonOffsetRightX) : (num22 + Harvester.TalonOffsetLeftX)); //66f, -70f
                num22 = (NPC.spriteDirection == 1) ? num22 + (Harvester.TalonDirectionalOffset + 6) : num22 - (Harvester.TalonDirectionalOffset + 6);


                SpriteEffects effect = (NPC.spriteDirection == 1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

                bool flag6 = true;
                while (flag6)
                {
                    float num24 = (float)Math.Sqrt(num22 * num22 + num23 * num23);
                    if (num24 < 38f) //16
                    {
                        flag6 = false;
                    }
                    else
                    {
                        num24 = 38f / num24; //16
                        num22 *= num24;
                        num23 *= num24;
                        center.X += num22;
                        center.Y += num23;
                        num22 = body.Center.X - center.X;
                        num23 = body.Center.Y - center.Y;
                        num23 -= -Harvester.TalonOffsetY + 20f; //7f
                        num22 = (NPC.type != AssortedCrazyThings.harvesterTalonLeft) ? (num22 + Harvester.TalonOffsetRightX) : (num22 + Harvester.TalonOffsetLeftX); //66f, -70f
                        num22 = (NPC.spriteDirection == 1) ? num22 + (Harvester.TalonDirectionalOffset + 6) : num22 - (Harvester.TalonDirectionalOffset + 6);

                        if (Main.rand.NextBool(8))
                        {
                            Dust dust;
                            dust = Dust.NewDustPerfect(new Vector2(center.X, center.Y), 135, new Vector2(Main.rand.NextFloat(-3f, 3f), Main.rand.NextFloat(-3f, 3f)), 26, new Color(255, 255, 255), Main.rand.NextFloat(1f, 1.6f));
                            dust.noLight = true;
                            dust.noGravity = true;
                            dust.fadeIn = Main.rand.NextFloat(0.5f, 1.5f);
                        }
                        Main.spriteBatch.Draw(texture, center - Main.screenPosition + new Vector2(0f, NPC.gfxOffY + NPC.height / 2), new Rectangle(0, 0, texture.Width, texture.Height), Color.White, 0f, texture.Size() / 2, 1f, effect, 0f);
                    }
                }

                texture = Mod.GetTexture("NPCs/DungeonBird/HarvesterTalon").Value;
                spriteBatch.Draw(texture, NPC.Center - Main.screenPosition + new Vector2(0f, NPC.gfxOffY), new Rectangle(0, 0, texture.Width, texture.Height), Color.White, 0f, texture.Size() / 2, 1f, effect, 0f);
            }
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            if (Main.rand.NextFloat() >= 0.5f)
            {
                target.AddBuff(BuffID.Slow, 120, false); //2 seconds, 50% chance
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

        public float AI_Timer
        {
            get
            {
                return NPC.ai[1];
            }
            set
            {
                NPC.ai[1] = value;
            }
        }

        public float RetractCounter
        {
            get
            {
                return NPC.ai[2];
            }
            set
            {
                NPC.ai[2] = value;
            }
        }

        public override bool CheckDead()
        {
            NPC.boss = false; //To get rid of the default death message
            return base.CheckDead();
        }

        public override void AI()
        {
            if (AssWorld.harvesterIndex < 0)
            {
                NPC.StrikeNPCNoInteraction(9999, 0f, 0);
                NPC.netUpdate = true;
            }
            else
            {
                NPC body = Main.npc[AssWorld.harvesterIndex];
                NPC.target = body.target;
                if (NPC.target < 0 || NPC.target >= Main.maxPlayers) return;
                Player target = Main.player[NPC.target];

                NPC.gfxOffY = Harvester.sinY;
                NPC.spriteDirection = body.spriteDirection;

                if (NPC.alpha > 0)
                {
                    NPC.alpha -= 5;
                    if (NPC.alpha < 4)
                    {
                        NPC.alpha = 0;
                        NPC.netUpdate = true;
                    }
                    NPC.scale = 1f;
                    AI_Timer = 0f;
                }

                if (AI_State == 0f)
                {
                    NPC.noTileCollide = true;
                    float num691 = 14f; //14f
                    if (NPC.life < NPC.lifeMax / 2)
                    {
                        num691 += 3f; //3f
                    }
                    if (NPC.life < NPC.lifeMax / 4)
                    {
                        num691 += 3f; //3f
                    }
                    if (body.life < body.lifeMax)
                    {
                        num691 += 8f; //8f
                    }
                    float betweenSelfAndBodyX = body.Center.X - NPC.Center.X;
                    float betweenSelfAndBodyY = body.Center.Y - NPC.Center.Y;
                    betweenSelfAndBodyY -= -Harvester.TalonOffsetY;
                    betweenSelfAndBodyX = (NPC.type != AssortedCrazyThings.harvesterTalonLeft) ? (betweenSelfAndBodyX + Harvester.TalonOffsetRightX) : (betweenSelfAndBodyX + Harvester.TalonOffsetLeftX);
                    float len = (float)Math.Sqrt(betweenSelfAndBodyX * betweenSelfAndBodyX + betweenSelfAndBodyY * betweenSelfAndBodyY);
                    float somevar = 12f;
                    if (len < somevar + num691)
                    {
                        RetractCounter = 0f;
                        NPC.velocity.X = betweenSelfAndBodyX;
                        NPC.velocity.Y = betweenSelfAndBodyY;
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
                                if ((NPC.type == AssortedCrazyThings.harvesterTalonLeft && NPC.Center.X + test > target.Center.X) || (NPC.type == AssortedCrazyThings.harvesterTalonRight && NPC.Center.X - test < target.Center.X))
                                {
                                    AI_Timer = 0f;
                                    AI_State = 1f;
                                    NPC.netUpdate = true;
                                }
                                else
                                {
                                    AI_Timer = 0f;
                                }
                            }
                        }
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
                        //end new

                        len = num691 / len;
                        NPC.velocity.X = betweenSelfAndBodyX * len * retractFactor; //both 1f
                        NPC.velocity.Y = betweenSelfAndBodyY * len * retractFactor;
                    }
                }
                else if (AI_State == 1f)
                {
                    //Punch toward target
                    NPC.noTileCollide = true;
                    NPC.collideX = false;
                    NPC.collideY = false;
                    float speed = 12f;
                    //new
                    if (body.life < body.lifeMax / 2)
                    {
                        speed += 4f;
                    }
                    if (body.life < body.lifeMax / 4)
                    {
                        speed += 4f;
                    }
                    float speedX = target.Center.X - NPC.Center.X;
                    float speedY = target.Center.Y - NPC.Center.Y;
                    float len = (float)Math.Sqrt(speedX * speedX + speedY * speedY);
                    len = speed / len;
                    NPC.velocity.X = speedX * len;
                    NPC.velocity.Y = speedY * len;
                    AI_State = 2f;
                }
                else if (AI_State == 2f)
                {
                    //fly through air/whatever and check if it hit tiles
                    //fist has  40 width 30 height
                    //talon has 38 width 42 height
                    if (Math.Abs(NPC.velocity.X) > Math.Abs(NPC.velocity.Y))
                    {
                        if (NPC.velocity.X > 0f && NPC.Center.X > target.Center.X)
                        {
                            NPC.noTileCollide = false;
                        }
                        if (NPC.velocity.X < 0f && NPC.Center.X < target.Center.X)
                        {
                            NPC.noTileCollide = false;
                        }
                    }
                    else
                    {
                        if (NPC.velocity.Y > 0f && NPC.Center.Y > target.Center.Y)
                        {
                            NPC.noTileCollide = false;
                        }
                        if (NPC.velocity.Y < 0f && NPC.Center.Y < target.Center.Y)
                        {
                            NPC.noTileCollide = false;
                        }
                    }
                    Vector2 vector84 = new Vector2(NPC.Center.X, NPC.Center.Y);
                    float num699 = body.Center.X - vector84.X;
                    float num700 = body.Center.Y - vector84.Y;
                    num699 += body.velocity.X;
                    num700 += body.velocity.Y;
                    num700 -= -Harvester.TalonOffsetY;
                    num699 = (NPC.type != AssortedCrazyThings.harvesterTalonLeft) ? (num699 + Harvester.TalonOffsetRightX) : (num699 + Harvester.TalonOffsetLeftX);
                    float num701 = (float)Math.Sqrt(num699 * num699 + num700 * num700);
                    if (body.life < body.lifeMax)
                    {
                        NPC.knockBackResist = 0f;
                        if (num701 > 700f || NPC.collideX || NPC.collideY || Collision.SolidCollision(NPC.position, NPC.width, NPC.height + 8)) //if collides with tiles or far away, go back to 0 and do the retreat code
                        {
                            NPC.noTileCollide = true;
                            NPC.netUpdate = true;
                            AI_State = 0f;
                        }
                    }
                    else
                    {
                        bool flag41 = NPC.justHit;

                        if ((num701 > 600f || NPC.collideX || NPC.collideY || Collision.SolidCollision(NPC.position, NPC.width, NPC.height + 8)) | flag41)
                        {
                            NPC.noTileCollide = true;
                            AI_State = 0f;
                            NPC.netUpdate = true;
                        }
                    }
                }
            }
        }
    }

    public class HarvesterTalonLeft : HarvesterTalon
    {

    }

    public class HarvesterTalonRight : HarvesterTalon
    {

    }
}