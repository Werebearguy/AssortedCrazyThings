using AssortedCrazyThings.NPCs.DungeonBird;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items
{
    public abstract class CaughtDungeonSoulBase : ModItem
    {
        protected int animatedTextureSelect;
        private int sincounter;
        protected double frame2CounterCount;
        private int frame2Counter;
        private int frame2;

        public override bool CloneNewInstances
        {
            get
            {
                return true;
            }
        }

        public override void SetDefaults()
        {
            item.width = 14;
            item.height = 24;
            item.maxStack = 999;
            item.value = 0;
            item.rare = -11;
            item.color = Color.White;

            MoreSetDefaults();
        }

        public virtual void MoreSetDefaults()
        {

        }

        public void Draw()
        {
            frame2Counter++;
            if (frame2Counter >= frame2CounterCount)
            {
                frame2++;
                frame2Counter = 0;
                if (frame2 > 3)
                {
                    frame2 = 0;
                }
            }
        }

        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            return false;
        }

        //draw only in world, not in inventory
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            float sinY = -10f;
            if (Main.hasFocus)
            {
                Draw();
                sincounter = sincounter > 120 ? 0 : sincounter + 1;
                sinY = (float)((Math.Sin((sincounter / 120f) * 2 * Math.PI) - 1) * 10);
            }

            lightColor = item.GetAlpha(lightColor) * 0.78f; //1f is opaque
            lightColor.R = Math.Max(lightColor.R, (byte)200); //100 for dark
            lightColor.G = Math.Max(lightColor.G, (byte)200);
            lightColor.B = Math.Max(lightColor.B, (byte)200);

            Lighting.AddLight(item.Center, new Vector3(0.15f, 0.15f, 0.35f));

            SpriteEffects effects = SpriteEffects.None;
            Texture2D image = AssortedCrazyThings.animatedTextureArray[animatedTextureSelect];
            Rectangle bounds = new Rectangle
            {
                X = 0,
                Y = frame2,
                Width = image.Bounds.Width,
                Height = (int)(image.Bounds.Height / 4)
            };
            bounds.Y *= bounds.Height; //cause proj.frame only contains the frame number

            Vector2 stupidOffset = new Vector2(item.width / 2, (item.height - 10f) + sinY);

            spriteBatch.Draw(image, item.position - Main.screenPosition + stupidOffset, bounds, lightColor * 0.78f, rotation, bounds.Size() / 2, scale, effects, 0f);
        }
    }
}
