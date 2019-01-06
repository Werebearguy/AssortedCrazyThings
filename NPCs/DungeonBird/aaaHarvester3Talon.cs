using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs.DungeonBird
{
    public abstract class aaaHarvester3Talon : ModNPC
    {
        public override string Texture
        {
            get
            {
                return "AssortedCrazyThings/NPCs/DungeonBird/aaaHarvester3_talon"; //temp
            }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("aaaHarvester3Talon");
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

            npc.noGravity = true;
            npc.width = 40; //38
            npc.height = 30; //42
            npc.aiStyle = -1;
            npc.damage = aaaHarvester3.talonDamage;
            npc.defense = 28;
            npc.lifeMax = 1337;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.alpha = 255;
            npc.buffImmune[BuffID.Poisoned] = true;
            npc.buffImmune[BuffID.OnFire] = true;

            npc.buffImmune[BuffID.Confused] = true;
            npc.dontTakeDamage = true;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            if (AssWorld.harvesterIndex != -1)
            {
                Texture2D texture = mod.GetTexture("NPCs/DungeonBird/aaaHarvester3_" + "chain");
                //Main.chain21Texture
                Vector2 center = new Vector2(npc.Center.X, npc.Center.Y);
                /*
                 *         aaaHarvester3.TalonOffsetLeftX = -84;
                           aaaHarvester3.TalonOffsetRightX = 78;
                           aaaHarvester3.TalonOffsetY = -9;
                 */
                float num22 = Main.npc[AssWorld.harvesterIndex].Center.X - center.X;
                float num23 = Main.npc[AssWorld.harvesterIndex].Center.Y - center.Y;
                num23 -= -aaaHarvester3.TalonOffsetY + 20f; //has to result to 7f

                //num22 = ((npc.type != AssWorld.harvesterTalonLeft) ? (num22 + aaaHarvester3.TalonOffsetRightX - 12f) : (num22 + aaaHarvester3.TalonOffsetLeftX + 14f)); //66f, -70f

                num22 = ((npc.type != AssWorld.harvesterTalonLeft) ? (num22 + aaaHarvester3.TalonOffsetRightX) : (num22 + aaaHarvester3.TalonOffsetLeftX)); //66f, -70f
                num22 = (npc.spriteDirection == 1) ? num22 + (aaaHarvester3.TalonDirectionalOffset + 6) : num22 - (aaaHarvester3.TalonDirectionalOffset + 6);

                bool flag6 = true;
                while (flag6)
                {
                    float num24 = (float)Math.Sqrt((double)(num22 * num22 + num23 * num23));
                    if (num24 < 48f) //16
                    {
                        flag6 = false;
                    }
                    else
                    {
                        num24 = 48f / num24; //16
                        num22 *= num24;
                        num23 *= num24;
                        center.X += num22;
                        center.Y += num23;
                        num22 = Main.npc[AssWorld.harvesterIndex].Center.X - center.X;
                        num23 = Main.npc[AssWorld.harvesterIndex].Center.Y - center.Y;
                        num23 -= -aaaHarvester3.TalonOffsetY + 20f; //7f
                        num22 = ((npc.type != AssWorld.harvesterTalonLeft) ? (num22 + aaaHarvester3.TalonOffsetRightX) : (num22 + aaaHarvester3.TalonOffsetLeftX)); //66f, -70f
                        num22 = (npc.spriteDirection == 1) ? num22 + (aaaHarvester3.TalonDirectionalOffset + 6) : num22 - (aaaHarvester3.TalonDirectionalOffset + 6);

                        if (Main.rand.NextBool(8))
                        {
                            Dust dust;
                            dust = Dust.NewDustPerfect(new Vector2(center.X, center.Y), 135, new Vector2(Main.rand.NextFloat(-3f, 3f), Main.rand.NextFloat(-3f, 3f)), 26, new Color(255, 255, 255), Main.rand.NextFloat(1f, 1.6f));
                            dust.noLight = true;
                            dust.noGravity = true;
                            dust.fadeIn = Main.rand.NextFloat(0.5f, 1.5f);
                        }

                        SpriteEffects effect = (npc.spriteDirection == 1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
                        Color color6 = Lighting.GetColor((int)center.X / 16, (int)(center.Y / 16f));
                        spriteBatch.Draw(texture, center - Main.screenPosition + new Vector2(0f, npc.gfxOffY + npc.height / 2), new Rectangle(0, 0, texture.Width, texture.Height), Color.White, 0f, new Vector2((float)texture.Width * 0.5f, (float)texture.Height * 0.5f), 1f, effect, 0f);
                    }
                }
            }
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            if (Main.rand.NextFloat() >= 0.5f)
            {
                target.AddBuff(BuffID.Slow, 300); //5 seconds, 50% chance
            }
        }

        public override void AI()
        {
            if (AssWorld.harvesterIndex < 0)
            {
                npc.StrikeNPCNoInteraction(9999, 0f, 0);
            }
            else
            {
                npc.gfxOffY = aaaHarvester3.sinY;
                npc.spriteDirection = Main.npc[AssWorld.harvesterIndex].spriteDirection;

                if (npc.alpha > 0)
                {
                    npc.alpha -= 5;
                    if (npc.alpha < 0)
                    {
                        npc.alpha = 0;
                    }
                    npc.ai[1] = 0f;
                }

                if (npc.ai[0] == 0f)
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
                    if (Main.npc[AssWorld.harvesterIndex].life < Main.npc[AssWorld.harvesterIndex].lifeMax)
                    {
                        num691 += 8f; //8f
                    }
                    Vector2 vector82 = new Vector2(npc.Center.X, npc.Center.Y);
                    float betweenSelfAndBodyX = Main.npc[AssWorld.harvesterIndex].Center.X - vector82.X;
                    float betweenSelfAndBodyY = Main.npc[AssWorld.harvesterIndex].Center.Y - vector82.Y;
                    betweenSelfAndBodyY -= -aaaHarvester3.TalonOffsetY;
                    betweenSelfAndBodyX = ((npc.type != AssWorld.harvesterTalonLeft) ? (betweenSelfAndBodyX + aaaHarvester3.TalonOffsetRightX) : (betweenSelfAndBodyX + aaaHarvester3.TalonOffsetLeftX));
                    float len = (float)Math.Sqrt((double)(betweenSelfAndBodyX * betweenSelfAndBodyX + betweenSelfAndBodyY * betweenSelfAndBodyY));
                    float somevar = 12f;
                    if (len < somevar + num691)
                    {
                        //if (npc.type == AssWorld.harvesterTalonLeft)
                        //{
                        //    Main.NewText("aaa " + len);
                        //    Main.NewText("XXYXY " + betweenSelfAndBodyX);
                        //}
                        //npc.rotation = 0f;
                        npc.velocity.X = betweenSelfAndBodyX;
                        npc.velocity.Y = betweenSelfAndBodyY;
                        npc.ai[1] += 1f;
                        //if (npc.life < npc.lifeMax / 2)
                        //{
                        //    npc.ai[1] += 1f;
                        //}
                        //if (npc.life < npc.lifeMax / 4)
                        //{
                        //    npc.ai[1] += 1f;
                        //}
                        //new
                        if (Main.npc[AssWorld.harvesterIndex].life < Main.npc[AssWorld.harvesterIndex].lifeMax / 2)
                        {
                            npc.ai[1] += 1f;
                        }
                        if (Main.npc[AssWorld.harvesterIndex].life < Main.npc[AssWorld.harvesterIndex].lifeMax / 4)
                        {
                            npc.ai[1] += 1f;
                        }
                        //if (Main.npc[AssWorld.harvesterIndex].life < Main.npc[AssWorld.harvesterIndex].lifeMax)
                        //{
                        //    npc.ai[1] += 10f;
                        //}
                        if (npc.ai[1] >= 60f)
                        {
                            //if (npc.type == AssWorld.harvesterTalonLeft) Main.NewText("bbbbbb");
                            npc.TargetClosest();
                            //test is 100f
                            float test = aaaHarvester3.Wid / 2; //its for checking which (or both) talons to shoot, so the left one has also range to the right 100 in

                            //new
                            float x = Main.player[npc.target].Center.X - npc.Center.X;
                            float y = Main.player[npc.target].Center.Y - npc.Center.Y;
                            float toPlayer = (float)Math.Sqrt((double)(x * x + y * y));

                            if(toPlayer < 500f && npc.BottomLeft.Y < Main.player[npc.target].BottomLeft.Y) //distance where it is allowed to swing at player
                            {
                                //end new
                                if ((npc.type == AssWorld.harvesterTalonLeft && npc.Center.X + test > Main.player[npc.target].Center.X) || (npc.type == AssWorld.harvesterTalonRight && npc.Center.X - test < Main.player[npc.target].Center.X))
                                {
                                    npc.ai[1] = 0f;
                                    npc.ai[0] = 1f;
                                }
                                else
                                {
                                    npc.ai[1] = 0f;
                                }
                            }
                        }
                    }
                    else //retract
                    {
                        //new
                        float retractFactor = 0.5f;
                        if (Main.npc[AssWorld.harvesterIndex].life < Main.npc[AssWorld.harvesterIndex].lifeMax / 2)
                        {
                            retractFactor += 0.25f;
                        }
                        if (Main.npc[AssWorld.harvesterIndex].life < Main.npc[AssWorld.harvesterIndex].lifeMax / 4)
                        {
                            retractFactor += 0.25f;
                        }





                        //end new

                        len = num691 / len;
                        npc.velocity.X = betweenSelfAndBodyX * len * retractFactor; //both 1f
                        npc.velocity.Y = betweenSelfAndBodyY * len * retractFactor;
                        //npc.rotation = (float)Math.Atan2(0.0 - (double)npc.velocity.Y, 0.0 - (double)npc.velocity.X);
                        //if (npc.type == AssWorld.harvesterTalonLeft)
                        //{
                        //    npc.rotation = (float)Math.Atan2((double)npc.velocity.Y, (double)npc.velocity.X);
                        //}
                    }
                }
                else if (npc.ai[0] == 1f)
                {
                    //Punch toward target
                    npc.noTileCollide = true;
                    npc.collideX = false;
                    npc.collideY = false;
                    float num695 = 12f;
                    //if (npc.life < npc.lifeMax / 2)
                    //{
                    //    num695 += 4f;
                    //}
                    //if (npc.life < npc.lifeMax / 4)
                    //{
                    //    num695 += 4f;
                    //}
                    //new
                    if (Main.npc[AssWorld.harvesterIndex].life < Main.npc[AssWorld.harvesterIndex].lifeMax / 2)
                    {
                        num695 += 4f;
                    }
                    if (Main.npc[AssWorld.harvesterIndex].life < Main.npc[AssWorld.harvesterIndex].lifeMax / 4)
                    {
                        num695 += 4f;
                    }
                    //if (Main.npc[AssWorld.harvesterIndex].life < Main.npc[AssWorld.harvesterIndex].lifeMax)
                    //{
                    //    num695 += 10f;
                    //}
                    Vector2 vector83 = new Vector2(npc.Center.X, npc.Center.Y);
                    float num696 = Main.player[npc.target].Center.X - vector83.X;
                    float num697 = Main.player[npc.target].Center.Y - vector83.Y;
                    float num698 = (float)Math.Sqrt((double)(num696 * num696 + num697 * num697));
                    num698 = num695 / num698;
                    npc.velocity.X = num696 * num698;
                    npc.velocity.Y = num697 * num698;
                    npc.ai[0] = 2f;
                    //npc.rotation = (float)Math.Atan2((double)npc.velocity.Y, (double)npc.velocity.X);
                    //if (npc.type == AssWorld.harvesterTalonLeft)
                    //{
                    //    npc.rotation = (float)Math.Atan2(0.0 - (double)npc.velocity.Y, 0.0 - (double)npc.velocity.X);
                    //}
                }
                else if (npc.ai[0] == 2f)
                {
                    //fly through air/whatever and check if it hit tiles
                    //fist has  40 width 30 height
                    //talon has 38 width 42 height
                    if (Math.Abs(npc.velocity.X) > Math.Abs(npc.velocity.Y))
                    {
                        if (npc.velocity.X > 0f && npc.Center.X > Main.player[npc.target].Center.X)
                        {
                            npc.noTileCollide = false;
                        }
                        if (npc.velocity.X < 0f && npc.Center.X < Main.player[npc.target].Center.X)
                        {
                            npc.noTileCollide = false;
                        }
                    }
                    else
                    {
                        if (npc.velocity.Y > 0f && npc.Center.Y > Main.player[npc.target].Center.Y)
                        {
                            npc.noTileCollide = false;
                        }
                        if (npc.velocity.Y < 0f && npc.Center.Y < Main.player[npc.target].Center.Y)
                        {
                            npc.noTileCollide = false;
                        }
                    }
                    Vector2 vector84 = new Vector2(npc.Center.X, npc.Center.Y);
                    float num699 = Main.npc[AssWorld.harvesterIndex].Center.X - vector84.X;
                    float num700 = Main.npc[AssWorld.harvesterIndex].Center.Y - vector84.Y;
                    num699 += Main.npc[AssWorld.harvesterIndex].velocity.X;
                    num700 += Main.npc[AssWorld.harvesterIndex].velocity.Y;
                    num700 -= -aaaHarvester3.TalonOffsetY;
                    num699 = ((npc.type != AssWorld.harvesterTalonLeft) ? (num699 + aaaHarvester3.TalonOffsetRightX) : (num699 + aaaHarvester3.TalonOffsetLeftX));
                    float num701 = (float)Math.Sqrt((double)(num699 * num699 + num700 * num700));
                    if (Main.npc[AssWorld.harvesterIndex].life < Main.npc[AssWorld.harvesterIndex].lifeMax)
                    {
                        npc.knockBackResist = 0f;
                        if (num701 > 700f || npc.collideX || npc.collideY || Collision.SolidCollision(npc.position, npc.width, npc.height + 8)) //if collides with tiles or far away, go back to 0 and do the retreat code
                        {
                            npc.noTileCollide = true;
                            npc.ai[0] = 0f;
                        }
                    }
                    else
                    {
                        bool flag41 = npc.justHit;
                        //check for if the head is on half health, then set flag to false
                        //if (flag41)
                        //{
                        //    int num2;
                        //    for (int num702 = 0; num702 < 200; num702 = num2 + 1)
                        //    {
                        //        if (Main.npc[num702].active && Main.npc[num702].type == headType)
                        //        {
                        //            if (Main.npc[num702].life < Main.npc[num702].lifeMax / 2)
                        //            {
                        //                if (npc.knockBackResist == 0f)
                        //                {
                        //                    flag41 = false;
                        //                }
                        //                npc.knockBackResist = 0f;
                        //            }
                        //            break;
                        //        }
                        //        num2 = num702;
                        //    }
                        //}
                        if ((num701 > 600f || npc.collideX || npc.collideY || Collision.SolidCollision(npc.position, npc.width, npc.height + 8)) | flag41)
                        {
                            npc.noTileCollide = true;
                            npc.ai[0] = 0f;
                        }
                    }
                }
                else if (npc.ai[0] == 3f)
                {
                    //?????? no transition in and no transition out
                    npc.noTileCollide = true;
                    float num703 = 12f;
                    float num704 = 0.4f;
                    Vector2 vector85 = new Vector2(npc.Center.X, npc.Center.Y);
                    float num705 = Main.player[npc.target].Center.X - vector85.X;
                    float num706 = Main.player[npc.target].Center.Y - vector85.Y;
                    float num707 = (float)Math.Sqrt((double)(num705 * num705 + num706 * num706));
                    num707 = num703 / num707;
                    num705 *= num707;
                    num706 *= num707;
                    if (npc.velocity.X < num705)
                    {
                        npc.velocity.X = npc.velocity.X + num704;
                        if (npc.velocity.X < 0f && num705 > 0f)
                        {
                            npc.velocity.X = npc.velocity.X + num704 * 2f;
                        }
                    }
                    else if (npc.velocity.X > num705)
                    {
                        npc.velocity.X = npc.velocity.X - num704;
                        if (npc.velocity.X > 0f && num705 < 0f)
                        {
                            npc.velocity.X = npc.velocity.X - num704 * 2f;
                        }
                    }
                    if (npc.velocity.Y < num706)
                    {
                        npc.velocity.Y = npc.velocity.Y + num704;
                        if (npc.velocity.Y < 0f && num706 > 0f)
                        {
                            npc.velocity.Y = npc.velocity.Y + num704 * 2f;
                        }
                    }
                    else if (npc.velocity.Y > num706)
                    {
                        npc.velocity.Y = npc.velocity.Y - num704;
                        if (npc.velocity.Y > 0f && num706 < 0f)
                        {
                            npc.velocity.Y = npc.velocity.Y - num704 * 2f;
                        }
                    }
                    //npc.rotation = (float)Math.Atan2((double)npc.velocity.Y, (double)npc.velocity.X);
                    //if (npc.type == AssWorld.harvesterTalonLeft)
                    //{
                    //    npc.rotation = (float)Math.Atan2(0.0 - (double)npc.velocity.Y, 0.0 - (double)npc.velocity.X);
                    //}
                }
            }
        }
    }

    public class aaaHarvester3Left : aaaHarvester3Talon
    {

    }

    public class aaaHarvester3Right : aaaHarvester3Talon
    {

    }
}