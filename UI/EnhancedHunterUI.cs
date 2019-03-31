using AssortedCrazyThings.NPCs.DungeonBird;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.UI;

namespace AssortedCrazyThings.UI
{
    class EnhancedHunterUI : UIState
    {
        //Is the UI visible?
        internal static bool visible = false;

        internal static List<int> type;

        internal static List<int> bossHeadIndex;

        internal static List<Vector2> drawPos;

        internal static List<float> drawRotation;

        internal static List<bool> drawLOS;

        internal static List<Color> drawColor;

        internal static Texture2D arrowTexture; //<- = null in Mod.Unload()

        internal static int[] blacklistNPCs;

        public override void OnInitialize()
        {
            type = new List<int>();
            bossHeadIndex = new List<int>();
            drawPos = new List<Vector2>();
            drawRotation = new List<float>();
            drawLOS = new List<bool>();
            drawColor = new List<Color>();
            arrowTexture = AssUtils.Instance.GetTexture("UI/UIArrow");

            blacklistNPCs = new int[]
            {
                NPCID.Bee,
                NPCID.BeeSmall,
                NPCID.Creeper,
                NPCID.ChaosBall,
                //NPCID.EaterofWorldsBody,
				//NPCID.EaterofWorldsTail,
                NPCID.Golem,
                NPCID.GolemFistLeft,
                NPCID.GolemFistRight,
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
                NPCID.TheHungry,
                NPCID.TheHungryII,
                NPCID.VileSpit,
                NPCID.WallofFleshEye,
                NPCID.WaterSphere,
                AssUtils.Instance.NPCType<Harvester1>(),
                AssUtils.Instance.NPCType<Harvester2>()
            };

            Array.Sort(blacklistNPCs);
        }

        private bool SetDrawPos()
        {
            type.Clear();
            bossHeadIndex.Clear();
            drawPos.Clear();
            drawRotation.Clear();
            drawLOS.Clear();
            drawColor.Clear();
            for (int k = 0; k < 200; k++)
            {
                NPC npc = Main.npc[k];

                if (type.Count < 20 && //limit to 20 drawn at all times
                    npc.active && !npc.friendly && npc.damage > 0 && npc.lifeMax > 5 &&
                    !npc.dontCountMe && !npc.hide && !npc.dontTakeDamage &&
                    Array.BinarySearch(blacklistNPCs, npc.type) < 0 &&
                    !AssUtils.IsWormBodyOrTail(npc))
                {
                    Vector2 between = npc.Center - Main.LocalPlayer.Center;
                    //screen "radius" is 960, "diameter" is 1920
                    int diameter = 1300 * 3; //radar range, basically two screens wide

                    if (between.Length() < diameter / 2)
                    {

                        int ltype = npc.type;
                        Vector2 ldrawPos = Vector2.Zero;

                        //when to draw the icon, has to be PendingResolutionWidth/Height because ScreenWidth/Height doesn't work in this case

                        //rectangle at which the npc ISN'T rendered (so its sprite won't draw aswell as the NPC itself)

                        //independent of resolution, but scales with zoom factor

                        float zoomFactorX = 0.25f * AssortedCrazyThings.ZoomFactor.X;
                        float zoomFactorY = 0.25f * AssortedCrazyThings.ZoomFactor.Y;
                        //for some reason with small hitbox NPCs, it starts drawing closer to the player than it should when zoomed in too much
                        if (zoomFactorX > 0.175f) zoomFactorX = 0.175f;
                        if (zoomFactorY > 0.175f) zoomFactorY = 0.175f;

                        int rectPosX = (int)(Main.screenPosition.X + (Main.PendingResolutionWidth * zoomFactorX));
                        int rectPosY = (int)(Main.screenPosition.Y + (Main.PendingResolutionHeight * zoomFactorY));
                        int rectWidth = (int)(Main.PendingResolutionWidth * (1 - 2f * zoomFactorX));
                        int rectHeight = (int)(Main.PendingResolutionHeight * (1 - 2f * zoomFactorY));

                        //padding for npc height
                        Rectangle rectangle = new Rectangle(rectPosX - npc.width / 2,
                            rectPosY - npc.height / 2,
                            rectWidth + npc.width,
                            rectHeight + npc.height);

                        if (!rectangle.Intersects(npc.getRect()))
                        {
                            if (between.X == 0f) between.X = 0.0001f; //protection against division by zero
                            if (between.Y == 0f) between.Y = 0.0001f; //protection against NaN
                            float slope = between.Y / between.X;

                            Vector2 pad = new Vector2
                                (
                                (Main.screenWidth + npc.width) / 2,
                                (Main.screenHeight + npc.height) / 2
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
                            ldrawPos -= new Vector2(npc.width / 2, npc.height / 2);

                            //get boss head texture if it has one and use that instead of the NPC texture
                            int lbossHeadIndex = -1;
                            if (npc.GetBossHeadTextureIndex() >= 0 && npc.GetBossHeadTextureIndex() < Main.npcHeadBossTexture.Length)
                            {
                                lbossHeadIndex = npc.GetBossHeadTextureIndex();
                                if (AssortedCrazyThings.BossAssistLoadedWithRadar) continue; //Don't add the boss to the list of draws
                            }

                            //get color if NPC has any
                            drawColor.Add(npc.color);

                            type.Add(ltype);
                            bossHeadIndex.Add(lbossHeadIndex);
                            drawPos.Add(ldrawPos);
                            drawRotation.Add((float)Math.Atan2(between.Y, between.X));
                            drawLOS.Add(Collision.CanHitLine(Main.LocalPlayer.position, Main.LocalPlayer.width, Main.LocalPlayer.height, npc.position, npc.width, npc.height));
                        }
                    }
                }
            }
            return type.Count > 0;
        }

        //Update
        public override void Update(GameTime gameTime)
        {
            if (!visible) return;
            base.Update(gameTime);

            //do stuff
            SetDrawPos();
        }

        //Draw
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            if (!visible) return;
            base.DrawSelf(spriteBatch);

            for (int i = 0; i < type.Count; i++)
            {
                Vector2 ldrawPos = drawPos[i]; //contains npc.Center basically, but for screenpos
                Texture2D tex = Main.npcTexture[type[i]];

                //scale image down to max 64x64, only one of them needs to be max
                int tempWidth = tex.Width;
                int tempHeight = tex.Height / Main.npcFrameCount[type[i]];
                float scaleFactor = (float)64 / ((tempWidth > tempHeight) ? tempWidth : tempHeight);
                if(scaleFactor > 0.75f) //because when fully zoomed out, the texture isn't actually drawn in 1:1 scale onto the screen
                {
                    scaleFactor = 0.75f; //only scale down, don't scale up
                }
                int finalWidth = (int)(tempWidth * scaleFactor);
                int finalHeight = (int)(tempHeight * scaleFactor);

                //if it's a boss, draw the head texture instead, no scaling
                if(bossHeadIndex[i] != -1)
                {
                    tex = Main.npcHeadBossTexture[bossHeadIndex[i]];
                    tempWidth = tex.Width;
                    tempHeight = tex.Height;
                    finalWidth = tex.Width;
                    finalHeight = tex.Height;
                }
                int arrowPad = 10;

                //adjust pos if outside of screen, more padding for arrow
                if (ldrawPos.X >= Main.screenWidth - finalWidth - arrowPad) ldrawPos.X = Main.screenWidth - finalWidth - arrowPad;
                if (ldrawPos.X <= finalWidth + arrowPad) ldrawPos.X = finalWidth + arrowPad;
                if (ldrawPos.Y >= Main.screenHeight - finalHeight - arrowPad) ldrawPos.Y = Main.screenHeight - finalHeight - arrowPad;
                if (ldrawPos.Y <= finalHeight + arrowPad) ldrawPos.Y = finalHeight + arrowPad;

                ////adjust pos if outside of screen, more padding
                //if (ldrawPos.X >= Main.screenWidth - finalWidth * 1.25f) ldrawPos.X = Main.screenWidth - finalWidth * 1.25f;
                //if (ldrawPos.X <= finalWidth * 1.25f) ldrawPos.X = finalWidth * 1.25f;
                //if (ldrawPos.Y >= Main.screenHeight - finalHeight * 1.25f) ldrawPos.Y = Main.screenHeight - finalHeight * 1.25f;
                //if (ldrawPos.Y <= finalHeight * 1.25f) ldrawPos.Y = finalHeight * 1.25f;

                //create rect around center
                Rectangle outputRect = new Rectangle((int)ldrawPos.X - (finalWidth / 2), (int)ldrawPos.Y - (finalHeight / 2), finalWidth, finalHeight);

                //set color overlay if NPC has one
                Color color = Color.LightGray;
                if (drawColor[i] != default(Color))
                {
                    color = new Color(
                        Math.Max(drawColor[i].R - 25, 50),
                        Math.Max(drawColor[i].G - 25, 50),
                        Math.Max(drawColor[i].B - 25, 50),
                        Math.Max((byte)(drawColor[i].A * 1.5f), (byte)75));
                }
                color *= drawLOS[i] ? 0.75f : 0.5f;
                spriteBatch.Draw(tex, outputRect, new Rectangle(0, 0, tempWidth, tempHeight), color);

                //draw Arrow
                Vector2 stupidOffset = drawRotation[i].ToRotationVector2() * 24f;
                Vector2 drawPosArrow = ldrawPos + stupidOffset;
                color = drawLOS[i] ? Color.Green * 0.75f : Color.Red * 0.75f;
                color.A = 150;
                spriteBatch.Draw(arrowTexture, drawPosArrow, null, color, drawRotation[i], arrowTexture.Bounds.Size() / 2, 1f, SpriteEffects.None, 0f);
            }
        }
    }
}
