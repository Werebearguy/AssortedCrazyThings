using Terraria.UI;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.UI.Chat;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameInput;
using AssortedCrazyThings.NPCs.DungeonBird;
using AssortedCrazyThings.Base;
using AssortedCrazyThings.NPCs;

namespace AssortedCrazyThings.UI
{
    class HoverNPCUI : UIState
    {
        internal static bool visible = false;
        internal static string drawString = "";
        internal static Color drawColor = Color.White;

        private string AlmostVanillaBehavior()
        {
            //works on dummies for some reason
            Player player = Main.LocalPlayer;
            string ret = "";
            Rectangle rectangle = new Rectangle((int)((float)Main.mouseX + Main.screenPosition.X), (int)((float)Main.mouseY + Main.screenPosition.Y), 1, 1);
            if (player.gravDir == -1f)
            {
                rectangle.Y = (int)Main.screenPosition.Y + Main.screenHeight - Main.mouseY;
            }

            float num = Main.mouseTextColor / 255f;

            for (int k = 0; k < 200; k++)
            {
                //LoadNPC(Main.npc[k].type); //idk why
                if (Main.npc[k].active)
                {
                    Rectangle npcrect = new Rectangle((int)Main.npc[k].Bottom.X - Main.npc[k].frame.Width / 2, (int)Main.npc[k].Bottom.Y - Main.npc[k].frame.Height, Main.npc[k].frame.Width, Main.npc[k].frame.Height);
                    if (Main.npc[k].type >= 87 && Main.npc[k].type <= 92) //Wyvern
                    {
                        npcrect = new Rectangle((int)((double)Main.npc[k].position.X + (double)Main.npc[k].width * 0.5 - 32.0), (int)((double)Main.npc[k].position.Y + (double)Main.npc[k].height * 0.5 - 32.0), 64, 64);
                    }
                    if (rectangle.Intersects(npcrect)) //mouse cursor inside hitbox
                    {
                        drawColor = new Color((byte)(255 * num), (byte)(255 * num), (byte)(255 * num), Main.mouseTextColor);
                        player.showItemIcon = false;
                        ret = Main.npc[k].GivenOrTypeName;
                        int num2 = k;
                        if (Main.npc[k].realLife >= 0)
                        {
                            num2 = Main.npc[k].realLife;
                        }
                        if (Main.npc[num2].lifeMax > 1 && !Main.npc[num2].dontTakeDamage)
                        {
                            ret = ret + ": " + Main.npc[num2].life + "/" + Main.npc[num2].lifeMax;
                        }
                        break;
                    }
                }
            }
            return ret;
        }

        private string Custom()
        {
            Player player = Main.LocalPlayer;
            string ret = "";
            Rectangle rectangle = new Rectangle((int)((float)Main.mouseX + Main.screenPosition.X), (int)((float)Main.mouseY + Main.screenPosition.Y), 1, 1);
            if (player.gravDir == -1f)
            {
                rectangle.Y = (int)Main.screenPosition.Y + Main.screenHeight - Main.mouseY;
            }

            float num = Main.mouseTextColor / 255f;

            for (int k = 0; k < 200; k++)
            {
                if (Main.npc[k].active)
                {
                    Rectangle npcrect = new Rectangle((int)Main.npc[k].Bottom.X - Main.npc[k].frame.Width / 2, (int)Main.npc[k].Bottom.Y - Main.npc[k].frame.Height, Main.npc[k].frame.Width, Main.npc[k].frame.Height);
                    if (rectangle.Intersects(npcrect)) //mouse cursor inside hitbox
                    {
                        drawColor = new Color((byte)(35 * num), (byte)(200f * num), (byte)(254f * num), Main.mouseTextColor);
                        if ((Main.npc[k].type == AssUtils.Instance.NPCType<DungeonSoul>() ||
                           Main.npc[k].type == AssUtils.Instance.NPCType<DungeonSoulFreed>()) && Main.npc[k].ai[0] == 0)
                        {
                            ret = "Catch it with a net";
                        }
                        else if (Main.npc[k].type == AssUtils.Instance.NPCType<Harvester1>())
                        {
                            //can technically also take life-1
                            //HarvesterBase m = (HarvesterBase)Main.npc[k].modNPC;
                            //ret = "Souls eaten: " + m.soulsEaten + "/" + 15; //m.maxSoulsEaten

                            ret = "Souls eaten: " + (Main.npc[k].life - 1) + "/" + 15;
                        }
                        else if (Main.npc[k].type == AssUtils.Instance.NPCType<Harvester2>())
                        {
                            ret = "Souls eaten: " + (Main.npc[k].life - 1 + 5) + "/" + 15;
                        }

                        if (Main.npc[k].type == AssUtils.Instance.NPCType<ChunkysEye>() || Main.npc[k].type == AssUtils.Instance.NPCType<MeatballsEye>())
                        {
                            drawColor = Color.White * num;
                            ret = "Catch it with a net";
                        }
                        break;
                    }
                }
            }
            return ret;
        }

        //Update
        public override void Update(GameTime gameTime)
        {
            //if (!visible) return;
            if (Main.hoverItemName != "") return;
            base.Update(gameTime);

            drawString = "";

            int lastMouseXbak = Main.lastMouseX;
            int lastMouseYbak = Main.lastMouseY;
            int mouseXbak = Main.mouseX;
            int mouseYbak = Main.mouseY;
            int lastscreenWidthbak = Main.screenWidth;
            int lastscreenHeightbak = Main.screenHeight;

            PlayerInput.SetZoom_Unscaled();
            PlayerInput.SetZoom_MouseInWorld();

            //do stuff
            //drawString = AlmostVanillaBehavior();
            drawString = Custom();

            Main.lastMouseX = lastMouseXbak;
            Main.lastMouseY = lastMouseYbak;
            Main.mouseX = mouseXbak;
            Main.mouseY = mouseYbak;
            Main.screenWidth = lastscreenWidthbak;
            Main.screenHeight = lastscreenHeightbak;
        }

        //Draw
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            //if (!visible) return;
            if (Main.hoverItemName != "") return;
            base.DrawSelf(spriteBatch);

            if (drawString != "") Main.LocalPlayer.showItemIcon = false;
            Vector2 mousePos = new Vector2(Main.mouseX, Main.mouseY);
            mousePos.X += 10;
            mousePos.Y += 10;
            if (Main.ThickMouse)
            {
                mousePos.X += 6;
                mousePos.Y += 6;
            }

            Vector2 vector = Main.fontMouseText.MeasureString(drawString);

            if (mousePos.X + vector.X + 4f > Main.screenWidth)
            {
                mousePos.X = (int)((float)Main.screenWidth - vector.X - 4f);
            }
            if (mousePos.Y + vector.Y + 4f > Main.screenHeight)
            {
                mousePos.Y = (int)(Main.screenHeight - vector.Y - 4f);
            }

            ChatManager.DrawColorCodedStringWithShadow(spriteBatch, Main.fontMouseText, drawString, mousePos + new Vector2(0, 22), drawColor, 0, Vector2.Zero, Vector2.One);
        }
    }
}
