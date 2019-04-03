using AssortedCrazyThings.NPCs.DungeonBird;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.UI;

namespace AssortedCrazyThings.UI
{
    class HarvesterEdgeUI : UIState
    {
        //Is the UI visible? (used internally)
        internal static bool visible = false;

        internal static List<Vector2> drawPos;

        internal static Texture2D texture; //<- = null in Mod.Unload()

        internal static int[] typeList;

        internal static List<int> type;

        public override void OnInitialize()
        {
            texture = AssUtils.Instance.GetTexture("NPCs/DungeonBird/Harvester2Head");

            typeList = new int[] { AssUtils.Instance.NPCType<Harvester1>(), AssUtils.Instance.NPCType<Harvester2>() };
            drawPos = new List<Vector2>();
            type = new List<int>();
        }

        private bool SetDrawPos(bool drawAll = false)
        {
            type.Clear();
            drawPos.Clear();
            //find first occurence of suitable boss to display
            for (int k = 0; k < 200; k++)
            {
                NPC npc = Main.npc[k];
                if (npc.active && Array.IndexOf(typeList, npc.type) != -1)
                {
                    int ltype = npc.type;
                    Vector2 ldrawPos = Vector2.Zero;

                    //when to draw the icon, has to be PendingResolutionWidth/Height because ScreenWidth/Height doesn't work in this case

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
                        Vector2 between = npc.Center - Main.LocalPlayer.Center;
                        //you can also save a rotation here via Atan2 and then draw an arrow or similar

                        if (between.X == 0f) between.X = 0.0001f; //protection against division by zero
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

                        type.Add(ltype);
                        drawPos.Add(ldrawPos);

                        if (!drawAll) break;
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
            visible = SetDrawPos(drawAll: true);
        }

        //Draw
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);

            if (visible)
            {
                for (int i = 0; i < type.Count; i++)
                {
                    int ltype = type[i];
                    Vector2 ldrawPos = drawPos[i];

                    int tempheight = texture.Height/* / Main.npcFrameCount[ltype]*/;
                    //adjust pos if outside of screen, more padding
                    if (ldrawPos.X >= Main.screenWidth - texture.Width) ldrawPos.X = Main.screenWidth - texture.Width;
                    if (ldrawPos.X <= texture.Width) ldrawPos.X = texture.Width;
                    if (ldrawPos.Y >= Main.screenHeight - tempheight) ldrawPos.Y = Main.screenHeight - tempheight;
                    if (ldrawPos.Y <= tempheight) ldrawPos.Y = tempheight;

                    int finalWidth = texture.Width;
                    int finalHeight = tempheight;
                    Rectangle outputRect = new Rectangle((int)ldrawPos.X - (finalWidth / 2), (int)ldrawPos.Y - (finalHeight / 2), finalWidth, finalHeight);
                    //outputWeaponRect.Inflate(10, 10);
                    //spriteBatch.Draw(tex, outputWeaponRect, Color.White);
                    Color color = Color.White * 0.78f;
                    spriteBatch.Draw(texture, outputRect, new Rectangle(0, 0, texture.Width, tempheight), Color.White);
                }
            }
        }
    }
}
