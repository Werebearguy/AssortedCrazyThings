using AssortedCrazyThings.Items.PetAccessories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
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

        public override void PostAI()
        {
            if(projectile.velocity.Y != 0.1f) projectile.rotation = projectile.velocity.X * 0.01f;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            PetAccessoryProj gProjectile = projectile.GetGlobalProjectile<PetAccessoryProj>(mod);
            for (byte slotNumber = 1; slotNumber < 5; slotNumber++) //0 is None, reserved
            {
                uint slimeAccessory = gProjectile.GetAccessory(slotNumber);
                if(slimeAccessory != 0)
                {
                    Texture2D texture = AssortedCrazyThings.slimeAccessoryTextures[slimeAccessory];
                    Rectangle frameLocal = new Rectangle(0, projectile.frame * Texheight, texture.Width, texture.Height / 10);
                    SpriteEffects effect = projectile.spriteDirection != 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
                    Vector2 drawOrigin = new Vector2(Texwidth * 0.5f, Texheight * 0.5f);
                    Vector2 stupidOffset = AssortedCrazyThings.slimeAccessoryOffsets[slimeAccessory] + new Vector2(0f, projectile.gfxOffY);


                    //fix for legacy slimes
                    if(Array.IndexOf(AssortedCrazyThings.slimePetLegacy, projectile.type) != -1)
                    {
                        if (slotNumber != (byte)SlotType.Hat) //everything besides head
                        {
                            if (projectile.frame > 2 && projectile.frame < 6)
                            {
                                stupidOffset += new Vector2(-2f * projectile.spriteDirection, 0f);
                            }
                            else
                            {
                                if (projectile.spriteDirection == 1)
                                {
                                    stupidOffset += new Vector2(-4f, 0f);
                                }
                            }
                        }
                        else //hat
                        {
                            if (projectile.frame < 6)
                            {
                                stupidOffset += new Vector2(-2f * projectile.spriteDirection, 2f);

                                if (projectile.frame > 2)
                                {
                                    stupidOffset += new Vector2(-1f * projectile.spriteDirection, 0f);
                                }

                                if (projectile.spriteDirection == -1)
                                {
                                    stupidOffset += new Vector2(-4f, 0f);
                                }
                            }
                            else
                            {
                                stupidOffset += new Vector2(-2f * projectile.spriteDirection, 0f);
                                if (projectile.spriteDirection == -1)
                                {
                                    stupidOffset += new Vector2(-4f, 0f);
                                }
                            }
                        }
                    }

                    stupidOffset += new Vector2(0f, drawOriginOffsetY + (-7.5f * projectile.scale + 7.5f)); // new Vector2(-0.5f, -7.7f);
                    Vector2 drawPos = projectile.position - Main.screenPosition + drawOrigin + stupidOffset;
                    spriteBatch.Draw(texture, drawPos, new Rectangle?(frameLocal), drawColor, projectile.rotation, frameLocal.Size() / 2, projectile.scale, effect, 0f);
                }
            }
        }
    }
}
