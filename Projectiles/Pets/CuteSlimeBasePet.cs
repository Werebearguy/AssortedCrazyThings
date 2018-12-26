using AssortedCrazyThings.Items.PetAccessories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Pets
{
    public class CuteSlimeBasePet : ModProjectile
    {
        public const int Projwidth = 28;
        public const int Projheight = 32;
        public const int Texwidth = 28;
        public const int Texheight = 52;

        public override bool PreAI()
        {
            Player player = Main.player[projectile.owner];
            player.lizard = false;
            return true;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            AssGlobalProjectile gProjectile = projectile.GetGlobalProjectile<AssGlobalProjectile>(mod);
            for (byte slotNumber = 1; slotNumber < 5; slotNumber++) //0 is None, reserved
            {
                uint slimeAccessory = gProjectile.GetAccessory(slotNumber);
                if(slimeAccessory != 0)
                {
                    Texture2D texture = AssortedCrazyThings.slimeAccessoryTextures[slimeAccessory];
                    Rectangle frameLocal = new Rectangle(0, 0, texture.Width, texture.Height / 10);
                    frameLocal.Y = projectile.frame * Texheight;
                    SpriteEffects effect = projectile.spriteDirection != 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
                    Vector2 drawOrigin = new Vector2(Texwidth * 0.5f, Texheight * 0.5f);
                    Vector2 stupidOffset = Vector2.Zero;
                    Vector2 drawPos = Vector2.Zero;

                    if (slotNumber == (byte)SlotType.Body)
                    {
                        stupidOffset = new Vector2(-2f, -0.0f + drawOriginOffsetY); // new Vector2(-0.5f, -7.7f);
                        drawPos = projectile.position - Main.screenPosition + drawOrigin + stupidOffset;
                        spriteBatch.Draw(texture, drawPos, new Rectangle?(frameLocal), Color.White, projectile.rotation, frameLocal.Size() / 2, projectile.scale, effect, 0f);
                    }
                    if (slotNumber == (byte)SlotType.Hat)
                    {
                        stupidOffset = new Vector2(-2f, -0.7f + drawOriginOffsetY - 7f); // new Vector2(-0.5f, -7.7f);
                        drawPos = projectile.position - Main.screenPosition + drawOrigin + stupidOffset;
                        spriteBatch.Draw(texture, drawPos, new Rectangle?(frameLocal), Color.White, projectile.rotation, frameLocal.Size() / 2, projectile.scale, effect, 0f);
                    }
                }
            }
        }
    }
}
