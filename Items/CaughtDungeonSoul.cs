using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items
{
	public class CaughtDungeonSoul : ModItem
	{
        private int sincounter;
        private int frame2Counter;
        private int frame2;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dungeon Soul");
            Tooltip.SetDefault("A soul caught by a net.");
            // ticksperframe, frameCount
            //Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(5, 4));
            //ItemID.Sets.AnimatesAsSoul[item.type] = true;
            ItemID.Sets.ItemIconPulse[item.type] = true;
            ItemID.Sets.ItemNoGravity[item.type] = true;
        }

        public override void SetDefaults()
        {
            Item refItem = new Item();
            refItem.SetDefaults(ItemID.SoulofSight);
            item.width = 14;// refItem.width;
            item.height = 24;//refItem.height; //24
            item.maxStack = 999;
            item.value = 100;
            item.rare = -11;
            item.color = Color.White;
        }

        public void FindFrame(int frameHeight)
        {
            frame2Counter++;
            if (frame2Counter <= 8.0)
            {
                frame2 = 0;
            }
            else if (frame2Counter <= 16.0)
            {
                frame2 = frameHeight * 1;
            }
            else if (frame2Counter <= 24.0)
            {
                frame2 = frameHeight * 2;
            }
            else if (frame2Counter <= 32.0)
            {
                frame2 = frameHeight * 3;
            }
            else
            {
                frame2Counter = 0;
            }
        }

        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            return false;
        }

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            FindFrame(item.height);
            Lighting.AddLight(item.Center, new Vector3(0.15f, 0.15f, 0.35f));

            lightColor.R = Math.Max(lightColor.R, (byte)200); //100 for dark
            lightColor.G = Math.Max(lightColor.G, (byte)200);
            lightColor.B = Math.Max(lightColor.B, (byte)200);
            lightColor.A = 255; //255 is opaque

            SpriteEffects effects = SpriteEffects.None;
            Texture2D image = mod.GetTexture("Items/CaughtSoulAnimated");
            Rectangle bounds = new Rectangle
            {
                X = 0,
                Y = frame2,
                Width = image.Bounds.Width,
                Height = (int)(image.Bounds.Height / 4)
            };

            float sinY = 0;
            sincounter = sincounter > 120 ? 0 : sincounter + 1;
            sinY = (float)((Math.Sin((sincounter / 120f) * 2 * Math.PI) - 1) * 10);

            Vector2 stupidOffset = new Vector2(item.width / 2, (item.height - 10f) + sinY);

            spriteBatch.Draw(image, item.position - Main.screenPosition + stupidOffset, bounds, lightColor, rotation, bounds.Size() / 2, scale, effects, 0f);
        }
    }
}