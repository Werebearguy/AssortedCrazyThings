using Terraria.UI;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.UI.Chat;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria.GameInput;
using AssortedCrazyThings.NPCs.DungeonBird;
using Terraria.ID;

namespace AssortedCrazyThings.UI
{
    class HarvesterEdgeUI : UIState
    {
        //Is the UI visible? (used internally)
        internal static bool visible = false;

        internal static List<Vector2> drawPos;

        internal static Dictionary<int, Texture2D> typeToTexture; //<- = null in Mod.Unload()

        internal static int[] typeList;

        internal static List<int> type;

        public override void OnInitialize()
        {
            typeToTexture = new Dictionary<int, Texture2D>
            {
                { AssUtils.Instance.NPCType<Harvester1>(), AssUtils.Instance.GetTexture("NPCs/DungeonBird/Harvester2Head") }, //1
                { AssUtils.Instance.NPCType<Harvester2>(), AssUtils.Instance.GetTexture("NPCs/DungeonBird/Harvester2Head") }
            };

            typeList = new List<int>(typeToTexture.Keys).ToArray();
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
                if(Main.npc[k].active && Array.IndexOf(typeList, Main.npc[k].type) != -1)
                {
                    int ltype = Main.npc[k].type;
                    Vector2 ldrawPos = Vector2.Zero; 

                    //when to draw the icon, has to be PendingResolutionWidth/Height because ScreenWidth/Height doesn't work in this case
                    Rectangle rectangle = new Rectangle(
                        (int)(Main.screenPosition.X - Main.npc[k].width),
                        (int)(Main.screenPosition.Y - Main.npc[k].height),
                        (int)(Main.PendingResolutionWidth + Main.npc[k].width),
                        (int)(Main.PendingResolutionHeight + Main.npc[k].height));

                    //rectangle.Inflate(-100, -100);

                    if (!rectangle.Intersects(Main.npc[k].getRect()))
                    {
                        Vector2 between = Main.npc[k].Center - Main.LocalPlayer.Center;
                        //you can also save a rotation here via Atan2 and then draw an arrow or similar

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

                        ldrawPos += new Vector2(pad.X, pad.Y);

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
                    Texture2D tex = typeToTexture[ltype];

                    int tempheight = tex.Height/* / Main.npcFrameCount[ltype]*/;
                    //adjust pos if outside of screen, more padding
                    if (ldrawPos.X >= Main.screenWidth - tex.Width) ldrawPos.X = Main.screenWidth - tex.Width;
                    if (ldrawPos.X <= tex.Width) ldrawPos.X = tex.Width;
                    if (ldrawPos.Y >= Main.screenHeight - tempheight) ldrawPos.Y = Main.screenHeight - tempheight;
                    if (ldrawPos.Y <= tempheight) ldrawPos.Y = tempheight;

                    int finalWidth = tex.Width / 2;
                    int finalHeight = tempheight / 2;
                    Rectangle outputWeaponRect = new Rectangle((int)ldrawPos.X - (finalWidth / 2), (int)ldrawPos.Y - (finalHeight / 2), finalWidth, finalHeight);
                    //outputWeaponRect.Inflate(10, 10);
                    //spriteBatch.Draw(tex, outputWeaponRect, Color.White);
                    Color color = Color.White * 0.78f;
                    spriteBatch.Draw(tex, outputWeaponRect, new Rectangle(0, 0, tex.Width, tempheight), Color.White);
                }
            }
        }
    }
}
