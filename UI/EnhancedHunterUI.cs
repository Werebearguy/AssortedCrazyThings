using Terraria.UI;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;
using Terraria.ID;

namespace AssortedCrazyThings.UI
{
    class EnhancedHunterUI : UIState
    {
        //Is the UI visible?
        internal static bool visible = false;

        internal static List<int> type;

        internal static List<Vector2> drawPos;

        internal static List<float> drawRotation;

        internal static List<float> drawAlpha;

        internal static Texture2D arrowTexture; //<- = null in Mod.Unload()

        internal static int[] blacklistNPCs;

        public override void OnInitialize()
        {
            type = new List<int>();
            drawPos = new List<Vector2>();
            drawRotation = new List<float>();
            drawAlpha = new List<float>();
            arrowTexture = AssUtils.Instance.GetTexture("UI/UIArrow");

            blacklistNPCs = new int[]
            {
                NPCID.BeeSmall,
                NPCID.Creeper,
                NPCID.ChaosBall,
                //NPCID.EaterofWorldsBody,
				//NPCID.EaterofWorldsTail,
                NPCID.Golem,
				NPCID.GolemFistLeft,
				NPCID.GolemFistRight,
				NPCID.MartianSaucer,
				NPCID.MartianSaucerCannon,
				NPCID.MartianSaucerTurret,
				NPCID.MoonLordCore,
				NPCID.MoonLordHand,
				NPCID.MoonLordHead,
				NPCID.MoonLordFreeEye,
				NPCID.PlanterasHook,
				NPCID.PlanterasTentacle,
				NPCID.PrimeCannon,
				NPCID.PrimeLaser,
				NPCID.PrimeSaw,
				NPCID.PrimeVice,
                NPCID.Probe,
                NPCID.SkeletronHand,
				NPCID.SkeletronHead,
				//NPCID.TheDestroyerBody,
				//NPCID.TheDestroyerTail,
				NPCID.TheHungry,
				NPCID.TheHungryII,
                NPCID.VileSpit,
                //NPCID.WallofFlesh,
				NPCID.WallofFleshEye,
            };

            Array.Sort(blacklistNPCs);
        }

        private bool SetDrawPos()
        {
            type.Clear();
            drawPos.Clear();
            drawRotation.Clear();
            drawAlpha.Clear();
            for (int k = 0; k < 200; k++)
            {
                if (type.Count < 20 && //limit to 20 drawn at all times
                    Main.npc[k].active && !Main.npc[k].friendly && Main.npc[k].damage > 0 && Main.npc[k].lifeMax > 5 &&
                    !Main.npc[k].dontCountMe && !Main.npc[k].hide && !Main.npc[k].dontTakeDamage
                    && Array.BinarySearch(blacklistNPCs, Main.npc[k].type) < 0)
                {
                    Vector2 between = Main.npc[k].Center - Main.LocalPlayer.Center;
                    //screen "radius" is 960, "diameter" is 1920
                    int diameter = 1300 * 3; //radar range * 3

                    if (between.Length() < diameter / 2)
                    {

                        int ltype = Main.npc[k].type;
                        Vector2 ldrawPos = Vector2.Zero;

                        //rectangle at which the npc ISN'T rendered (so its sprite won't draw aswell as the NPC itself)
                        Rectangle rectangle = new Rectangle(
                            (int)(Main.screenPosition.X - Main.npc[k].width / 2),
                            (int)(Main.screenPosition.Y - Main.npc[k].height / 2),
                            (int)(Main.PendingResolutionWidth + Main.npc[k].width),
                            (int)(Main.PendingResolutionHeight + Main.npc[k].height));

                        //rectangle.Inflate(-100, -100);

                        if (!rectangle.Intersects(Main.npc[k].getRect()))
                        {
                            if (between.X == 0f) between.X = 0.0001f; //protection against division by zero
                            float slope = between.Y / between.X;

                            Vector2 pad = new Vector2
                                (
                                (Main.screenWidth + Main.npc[k].width) / 2,
                                (Main.screenHeight + Main.npc[k].height) / 2
                                );

                            //first iteration

                            if (between.Y > 0) //target below player
                            {
                                //use lower border which is positive
                                if (between.Y > pad.Y)
                                {
                                    ldrawPos.Y = pad.Y;
                                }
                                else
                                {
                                    ldrawPos.Y = between.Y;
                                }
                            }
                            else //target above player
                            {
                                //use upper border which is negative
                                if (between.Y < -pad.Y)
                                {
                                    ldrawPos.Y = -pad.Y;
                                }
                                else
                                {
                                    ldrawPos.Y = between.Y;
                                }
                            }
                            ldrawPos.X = ldrawPos.Y / slope;

                            //second iteration

                            if (ldrawPos.X > 0) //if x is outside the right edge
                            {
                                //use right border which is positive
                                if (ldrawPos.X > pad.X)
                                {
                                    ldrawPos.X = pad.X;
                                }
                            }
                            else if (ldrawPos.X <= 0) //if x is outside the left edge
                            {
                                //use left border which is negative
                                if (ldrawPos.X <= -pad.X)
                                {
                                    ldrawPos.X = -pad.X;
                                }
                            }
                            ldrawPos.Y = ldrawPos.X * slope;

                            //revert offset
                            ldrawPos += new Vector2(pad.X, pad.Y);

                            //since we were operating based on Center to Center, we need to put the drawPos back to position instead
                            ldrawPos -= new Vector2(Main.npc[k].width / 2, Main.npc[k].height / 2);

                            //rotation here for arrow


                            type.Add(ltype);
                            drawPos.Add(ldrawPos);
                            drawRotation.Add((float)Math.Atan2(between.Y, between.X));
                            drawAlpha.Add(Collision.CanHitLine(Main.LocalPlayer.position, Main.LocalPlayer.width, Main.LocalPlayer.height, Main.npc[k].position, Main.npc[k].width, Main.npc[k].height) ? 0.75f : 0.5f);
                        }
                    }
                }
            }
            return type.Count > 0;
        }

        //Update
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            //do stuff
            SetDrawPos();
        }

        //Draw
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);

            for (int i = 0; i < type.Count; i++)
            {
                //Main.NewText("rot " + drawRotation[i] + " " + drawRotation[i].ToRotationVector2());
                Vector2 ldrawPos = drawPos[i]; //contains npc.Center basically, but for screenpos
                Texture2D tex = Main.npcTexture[type[i]];

                //scale image down to max 64x64, only one of them needs to be max
                int tempWidth = tex.Width;
                int tempHeight = tex.Height / Main.npcFrameCount[type[i]];
                float scaleFactor = (float)64 / ((tempWidth > tempHeight) ? tempWidth : tempHeight);
                if(scaleFactor > 0.75f)
                {
                    scaleFactor = 0.75f; //only scale down, don't scale up
                }
                int finalWidth = (int)(tempWidth * scaleFactor);
                int finalHeight = (int)(tempHeight * scaleFactor);

                //Main.NewText(scaleFactor);
                //Main.NewText("tex : " + tempWidth + "; " + tempHeight);
                //Main.NewText("fin : " + finalWidth + "; " + finalHeight);
                //adjust pos if outside of screen, more padding
                if (ldrawPos.X >= Main.screenWidth - finalWidth * 1.5f) ldrawPos.X = Main.screenWidth - finalWidth * 1.5f;
                if (ldrawPos.X <= finalWidth * 1.5f) ldrawPos.X = finalWidth * 1.5f;
                if (ldrawPos.Y >= Main.screenHeight - finalHeight * 1.25f) ldrawPos.Y = Main.screenHeight - finalHeight * 1.25f;
                if (ldrawPos.Y <= finalHeight * 1.25f) ldrawPos.Y = finalHeight * 1.25f;

                //int finalWidth = tex.Width;
                //int finalHeight = tempHeight;
                //create rect around center
                Rectangle outputRect = new Rectangle((int)ldrawPos.X - (finalWidth / 2), (int)ldrawPos.Y - (finalHeight / 2), finalWidth, finalHeight);
                //Main.NewText("rect: " + outputRect);
                //outputWeaponRect.Inflate(10, 10);
                //spriteBatch.Draw(tex, outputWeaponRect, Color.White);
                Color color = Color.LightGray * drawAlpha[i];
                spriteBatch.Draw(tex, outputRect, new Rectangle(0, 0, tempWidth, tempHeight), color);


                Vector2 stupidOffset = drawRotation[i].ToRotationVector2() * 32f;
                Vector2 drawPosArrow = ldrawPos + stupidOffset;
                color = drawAlpha[i] == 0.5f ? Color.Red * 0.75f : Color.Green * 0.75f;
                spriteBatch.Draw(arrowTexture, drawPosArrow, null, color, drawRotation[i], arrowTexture.Bounds.Size() / 2, 1f, SpriteEffects.None, 0f);
            }
        }
    }
}
