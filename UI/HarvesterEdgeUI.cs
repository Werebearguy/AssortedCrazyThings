using AssortedCrazyThings.Base;
using AssortedCrazyThings.NPCs.DungeonBird;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace AssortedCrazyThings.UI
{
    class HarvesterEdgeUI : UIState
    {
        //Is the UI visible? (used internally)
        internal static bool visible = false;

        internal static List<Vector2> drawPos;

        internal static Asset<Texture2D> texture; //<- = null in Mod.Unload()

        internal static int[] typeList;

        internal static List<int> type;

        public override void OnInitialize()
        {
            texture = AssUtils.Instance.Assets.Request<Texture2D>("NPCs/DungeonBird/Harvester2Head");

            typeList = new int[] { ModContent.NPCType<Harvester1>(), ModContent.NPCType<Harvester2>() };
            drawPos = new List<Vector2>();
            type = new List<int>();
        }

        private bool SetDrawPos(bool drawAll = false)
        {
            type.Clear();
            drawPos.Clear();
            //find first occurence of suitable boss to display
            for (int k = 0; k < Main.maxNPCs; k++)
            {
                NPC npc = Main.npc[k];
                if (npc.active && Array.IndexOf(typeList, npc.type) != -1)
                {
                    int ltype = npc.type;
                    Vector2 ldrawPos = Vector2.Zero;

                    //when to draw the icon, has to be PendingResolutionWidth/Height because ScreenWidth/Height doesn't work in this case

                    float zoomFactorX = 0.25f * AssUISystem.ZoomFactor.X;
                    float zoomFactorY = 0.25f * AssUISystem.ZoomFactor.Y;
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

                        if (between.X == 0f) between.X = 0.0001f; //protection against division by zero
                        if (between.Y == 0f) between.Y = 0.0001f; //protection against NaN

                        if (Main.LocalPlayer.gravDir != 1f)
                        {
                            between.Y = -between.Y;
                        }
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
                var tex = texture.Value;
                for (int i = 0; i < type.Count; i++)
                {
                    int ltype = type[i];
                    Vector2 ldrawPos = drawPos[i];

                    int tempheight = tex.Height/* / Main.npcFrameCount[ltype]*/;
                    //adjust pos if outside of screen, more padding
                    if (ldrawPos.X >= Main.screenWidth - tex.Width) ldrawPos.X = Main.screenWidth - tex.Width;
                    if (ldrawPos.X <= tex.Width) ldrawPos.X = tex.Width;
                    if (ldrawPos.Y >= Main.screenHeight - tempheight) ldrawPos.Y = Main.screenHeight - tempheight;
                    if (ldrawPos.Y <= tempheight) ldrawPos.Y = tempheight;

                    int finalWidth = tex.Width;
                    int finalHeight = tempheight;
                    Rectangle outputRect = new Rectangle((int)ldrawPos.X - (finalWidth / 2), (int)ldrawPos.Y - (finalHeight / 2), finalWidth, finalHeight);
                    //outputWeaponRect.Inflate(10, 10);
                    //Main.spriteBatch.Draw(tex, outputWeaponRect, Color.White);
                    Color color = Color.White * 0.78f;
                    Main.spriteBatch.Draw(tex, outputRect, new Rectangle(0, 0, tex.Width, tempheight), Color.White);
                }
            }
        }
    }
}
