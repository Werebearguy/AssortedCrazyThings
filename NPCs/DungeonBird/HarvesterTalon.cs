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
            Main.npcFrameCount[npc.type] = 1;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White * ((255 - npc.alpha) / 255f);
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

            npc.boss = true;
            npc.noGravity = true;
            npc.width = 40; //38
            npc.height = 30; //42
            npc.aiStyle = -1;
            npc.damage = Harvester.talonDamage;
            npc.defense = 28;
            npc.lifeMax = 1337;
            npc.scale = 1f;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.alpha = 255;
            npc.buffImmune[BuffID.Poisoned] = true;
            npc.buffImmune[BuffID.OnFire] = true;
            npc.buffImmune[BuffID.Confused] = true;
            npc.dontTakeDamage = true;
            npc.dontCountMe = true;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            return false;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            if (AssWorld.harvesterIndex != -1)
            {
                Texture2D texture = mod.GetTexture("NPCs/DungeonBird/HarvesterChain");
                //Main.chain21Texture
                Vector2 center = new Vector2(npc.Center.X, npc.Center.Y);
                NPC body = Main.npc[AssWorld.harvesterIndex];
                float num22 = body.Center.X - center.X;
                float num23 = body.Center.Y - center.Y;
                num23 -= -Harvester.TalonOffsetY + 20f; //has to result to 7f

                //num22 = ((npc.type != AssWorld.harvesterTalonLeft) ? (num22 + aaaHarvester3.TalonOffsetRightX - 12f) : (num22 + aaaHarvester3.TalonOffsetLeftX + 14f)); //66f, -70f

                num22 = ((npc.type != AssWorld.harvesterTalonLeft) ? (num22 + Harvester.TalonOffsetRightX) : (num22 + Harvester.TalonOffsetLeftX)); //66f, -70f
                num22 = (npc.spriteDirection == 1) ? num22 + (Harvester.TalonDirectionalOffset + 6) : num22 - (Harvester.TalonDirectionalOffset + 6);


                SpriteEffects effect = (npc.spriteDirection == 1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

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
                        num22 = (npc.type != AssWorld.harvesterTalonLeft) ? (num22 + Harvester.TalonOffsetRightX) : (num22 + Harvester.TalonOffsetLeftX); //66f, -70f
                        num22 = (npc.spriteDirection == 1) ? num22 + (Harvester.TalonDirectionalOffset + 6) : num22 - (Harvester.TalonDirectionalOffset + 6);

                        if (Main.rand.NextBool(8))
                        {
                            Dust dust;
                            dust = Dust.NewDustPerfect(new Vector2(center.X, center.Y), 135, new Vector2(Main.rand.NextFloat(-3f, 3f), Main.rand.NextFloat(-3f, 3f)), 26, new Color(255, 255, 255), Main.rand.NextFloat(1f, 1.6f));
                            dust.noLight = true;
                            dust.noGravity = true;
                            dust.fadeIn = Main.rand.NextFloat(0.5f, 1.5f);
                        }
                        spriteBatch.Draw(texture, center - Main.screenPosition + new Vector2(0f, npc.gfxOffY + npc.height / 2), new Rectangle(0, 0, texture.Width, texture.Height), Color.White, 0f, texture.Size() / 2, 1f, effect, 0f);
                    }
                }

                texture = mod.GetTexture("NPCs/DungeonBird/HarvesterTalon");
                spriteBatch.Draw(texture, npc.Center - Main.screenPosition + new Vector2(0f, npc.gfxOffY), new Rectangle(0, 0, texture.Width, texture.Height), Color.White, 0f, texture.Size() / 2, 1f, effect, 0f);
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
                return npc.ai[0];
            }
            set
            {
                npc.ai[0] = value;
            }
        }

        public float AI_Timer
        {
            get
            {
                return npc.ai[1];
            }
            set
            {
                npc.ai[1] = value;
            }
        }

        public float RetractCounter
        {
            get
            {
                return npc.ai[2];
            }
            set
            {
                npc.ai[2] = value;
            }
        }

        public override bool CheckDead()
        {
            npc.boss = false; //To get rid of the default death message
            return base.CheckDead();
        }

        public override void AI()
        {
            if (AssWorld.harvesterIndex < 0)
            {
                npc.StrikeNPCNoInteraction(9999, 0f, 0);
                npc.netUpdate = true;
            }
            else
            {
                NPC body = Main.npc[AssWorld.harvesterIndex];
                npc.target = body.target;
                if (npc.target < 0 || npc.target >= Main.maxPlayers) return;
                Player target = Main.player[npc.target];

                npc.gfxOffY = Harvester.sinY;
                npc.spriteDirection = body.spriteDirection;

                if (npc.alpha > 0)
                {
                    npc.alpha -= 5;
                    if (npc.alpha < 4)
                    {
                        npc.alpha = 0;
                        npc.netUpdate = true;
                    }
                    npc.scale = 1f;
                    AI_Timer = 0f;
                }

                if (AI_State == 0f)
                {
                    npc.noTileCollide = true;
                    float num691 = 14f; //14f
                    if (npc.life < npc.lifeMax / 2)
                    {
                        num691 += 3f; //3f
                    }
                    if (npc.life < npc.lifeMax / 4)
                    {
                        num691 += 3f; //3f
                    }
                    if (body.life < body.lifeMax)
                    {
                        num691 += 8f; //8f
                    }
                    float betweenSelfAndBodyX = body.Center.X - npc.Center.X;
                    float betweenSelfAndBodyY = body.Center.Y - npc.Center.Y;
                    betweenSelfAndBodyY -= -Harvester.TalonOffsetY;
                    betweenSelfAndBodyX = (npc.type != AssWorld.harvesterTalonLeft) ? (betweenSelfAndBodyX + Harvester.TalonOffsetRightX) : (betweenSelfAndBodyX + Harvester.TalonOffsetLeftX);
                    float len = (float)Math.Sqrt(betweenSelfAndBodyX * betweenSelfAndBodyX + betweenSelfAndBodyY * betweenSelfAndBodyY);
                    float somevar = 12f;
                    if (len < somevar + num691)
                    {
                        RetractCounter = 0f;
                        npc.velocity.X = betweenSelfAndBodyX;
                        npc.velocity.Y = betweenSelfAndBodyY;
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
                            npc.TargetClosest();
                            target = Main.player[npc.target];
                            //test is 100f
                            float test = Harvester.Wid / 2; //its for checking which (or both) talons to shoot, so the left one has also range to the right 100 in

                            //new
                            float x = target.Center.X - npc.Center.X;
                            float y = target.Center.Y - npc.Center.Y;
                            float toPlayer = (float)Math.Sqrt(x * x + y * y);

                            if (toPlayer < 500f && npc.BottomLeft.Y < target.BottomLeft.Y) //distance where it is allowed to swing at player
                            {
                                //end new
                                if ((npc.type == AssWorld.harvesterTalonLeft && npc.Center.X + test > target.Center.X) || (npc.type == AssWorld.harvesterTalonRight && npc.Center.X - test < target.Center.X))
                                {
                                    AI_Timer = 0f;
                                    AI_State = 1f;
                                    npc.netUpdate = true;
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
                        npc.velocity.X = betweenSelfAndBodyX * len * retractFactor; //both 1f
                        npc.velocity.Y = betweenSelfAndBodyY * len * retractFactor;
                    }
                }
                else if (AI_State == 1f)
                {
                    //Punch toward target
                    npc.noTileCollide = true;
                    npc.collideX = false;
                    npc.collideY = false;
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
                    float speedX = target.Center.X - npc.Center.X;
                    float speedY = target.Center.Y - npc.Center.Y;
                    float len = (float)Math.Sqrt(speedX * speedX + speedY * speedY);
                    len = speed / len;
                    npc.velocity.X = speedX * len;
                    npc.velocity.Y = speedY * len;
                    AI_State = 2f;
                }
                else if (AI_State == 2f)
                {
                    //fly through air/whatever and check if it hit tiles
                    //fist has  40 width 30 height
                    //talon has 38 width 42 height
                    if (Math.Abs(npc.velocity.X) > Math.Abs(npc.velocity.Y))
                    {
                        if (npc.velocity.X > 0f && npc.Center.X > target.Center.X)
                        {
                            npc.noTileCollide = false;
                        }
                        if (npc.velocity.X < 0f && npc.Center.X < target.Center.X)
                        {
                            npc.noTileCollide = false;
                        }
                    }
                    else
                    {
                        if (npc.velocity.Y > 0f && npc.Center.Y > target.Center.Y)
                        {
                            npc.noTileCollide = false;
                        }
                        if (npc.velocity.Y < 0f && npc.Center.Y < target.Center.Y)
                        {
                            npc.noTileCollide = false;
                        }
                    }
                    Vector2 vector84 = new Vector2(npc.Center.X, npc.Center.Y);
                    float num699 = body.Center.X - vector84.X;
                    float num700 = body.Center.Y - vector84.Y;
                    num699 += body.velocity.X;
                    num700 += body.velocity.Y;
                    num700 -= -Harvester.TalonOffsetY;
                    num699 = (npc.type != AssWorld.harvesterTalonLeft) ? (num699 + Harvester.TalonOffsetRightX) : (num699 + Harvester.TalonOffsetLeftX);
                    float num701 = (float)Math.Sqrt(num699 * num699 + num700 * num700);
                    if (body.life < body.lifeMax)
                    {
                        npc.knockBackResist = 0f;
                        if (num701 > 700f || npc.collideX || npc.collideY || Collision.SolidCollision(npc.position, npc.width, npc.height + 8)) //if collides with tiles or far away, go back to 0 and do the retreat code
                        {
                            npc.noTileCollide = true;
                            npc.netUpdate = true;
                            AI_State = 0f;
                        }
                    }
                    else
                    {
                        bool flag41 = npc.justHit;

                        if ((num701 > 600f || npc.collideX || npc.collideY || Collision.SolidCollision(npc.position, npc.width, npc.height + 8)) | flag41)
                        {
                            npc.noTileCollide = true;
                            AI_State = 0f;
                            npc.netUpdate = true;
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