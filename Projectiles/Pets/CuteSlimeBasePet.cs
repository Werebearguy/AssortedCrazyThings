using AssortedCrazyThings.Items.PetAccessories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Pets
{
    public abstract class CuteSlimeBasePet : ModProjectile
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

        private void DrawBaseSprite(SpriteBatch spriteBatch, Color drawColor)
        {
            //check if it wears a "useNoHair" hat, then if it does, change the texture to that,
            //otherwise use default one
            PetAccessoryProj gProjectile = projectile.GetGlobalProjectile<PetAccessoryProj>(mod);
            bool useNoHair = false;
            //PetAccessory.UseNoHair[slimeAccessory] && (slotNumber == (byte)SlotType.Hat)
            uint slimeAccessoryHat = gProjectile.GetAccessory((byte)SlotType.Hat);
            if (slimeAccessoryHat != 0 &&
                PetAccessory.UseNoHair[slimeAccessoryHat] &&
                Array.IndexOf(AssortedCrazyThings.slimePetNoHair, projectile.type) != -1 && //if it has a NoHair tex
                Array.IndexOf(AssortedCrazyThings.slimePetLegacy, projectile.type) == -1) //if its not legacy
            {
                useNoHair = true;
            }

            if(MoreDrawBaseSprite(spriteBatch, drawColor, useNoHair)) //do a pre-draw for the rainbow slimes
            {
                Texture2D texture = Main.projectileTexture[projectile.type];
                if (useNoHair) //only if not legacy
                {
                    //because texture name is absolute but GetTexture takes the relative path, we only take the name as reference
                    //and construct the rest
                    string[] texNameList = texture.Name.Split(new string[] { "/" }, 10, StringSplitOptions.RemoveEmptyEntries);
                    texture = mod.GetTexture("Projectiles/Pets/" + texNameList[texNameList.Length - 1] + "NoHair");
                }
                Rectangle frameLocal = new Rectangle(0, projectile.frame * Texheight, texture.Width, texture.Height / 10);
                SpriteEffects effect = projectile.spriteDirection != 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
                Vector2 drawOrigin = new Vector2(Texwidth * 0.5f, Texheight * 0.5f);
                Vector2 stupidOffset = new Vector2(0f, projectile.gfxOffY + drawOriginOffsetY);
                Vector2 drawPos = projectile.position - Main.screenPosition + drawOrigin + stupidOffset;
                spriteBatch.Draw(texture, drawPos, new Rectangle?(frameLocal), drawColor, projectile.rotation, frameLocal.Size() / 2, projectile.scale, effect, 0f);
            }
        }

        public virtual bool MoreDrawBaseSprite(SpriteBatch spriteBatch, Color drawColor, bool useNoHair)
        {
            return true;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            if (Array.IndexOf(AssortedCrazyThings.slimePetLegacy, projectile.type) == -1) //if not a legacy slime
            {
                DrawAccessories(spriteBatch, drawColor, preDraw: true);
            }

            DrawBaseSprite(spriteBatch, drawColor);
            return false;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            if(Array.IndexOf(AssortedCrazyThings.slimePetLegacy, projectile.type) == -1) //if not a legacy slime
            {
                DrawAccessories(spriteBatch, drawColor);
            }
        }

        private void DrawAccessories(SpriteBatch spriteBatch, Color drawColor, bool preDraw = false)
        {
            PetAccessoryProj gProjectile = projectile.GetGlobalProjectile<PetAccessoryProj>(mod);
            for (byte slotNumber = 1; slotNumber < 5; slotNumber++) //0 is None, reserved
            {
                //slimeAccessory is the indexed number of the accessory (from 0 to 255)
                uint slimeAccessory = gProjectile.GetAccessory(slotNumber);
                if ((preDraw || !PetAccessory.PreDraw[slimeAccessory]) && slimeAccessory != 0)
                {
                    Texture2D texture = PetAccessory.Texture[slimeAccessory];

                    //if(slotNumber == (byte)SlotType.Hat)
                    //{
                        int altTextureNumber = PetAccessory.AltTexture[slimeAccessory, gProjectile.GetColor()];
                        
                        if (altTextureNumber != -1 && altTextureNumber != 0) //change texture if not -1 and not -0
                        {
                            //because texture name is absolute but GetTexture takes the relative path, we only take the name as reference
                            //and construct the rest
                            string[] texNameList = texture.Name.Split(new string[] { "/" }, 10, StringSplitOptions.RemoveEmptyEntries);
                            texture = mod.GetTexture("Items/PetAccessories/" + texNameList[texNameList.Length - 1] + altTextureNumber);
                        }
                        else if (altTextureNumber == -1)
                        {
                            continue;
                        }
                        //else if 0: normal behavior
                    //}


                    Rectangle frameLocal = new Rectangle(0, projectile.frame * Texheight, texture.Width, texture.Height / 10);

                    //get necessary properties and parameters for draw
                    SpriteEffects effect = projectile.spriteDirection != 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
                    Vector2 drawOrigin = new Vector2(Texwidth * 0.5f, Texheight * 0.5f);
                    Vector2 stupidOffset = PetAccessory.Offset[slimeAccessory] + new Vector2(0f, projectile.gfxOffY);
                    Color color = drawColor * ((255 - PetAccessory.Alpha[slimeAccessory]) / 255f);

                    //here, pick the sprite according to whats specified in the slimeAccessory
                    //some kinda map

                    //fix for legacy slimes
                    if (Array.IndexOf(AssortedCrazyThings.slimePetLegacy, projectile.type) != -1)
                    {
                        /*
                            if (slotNumber == (byte)SlotType.Carried)
                            {
                                //stupidOffset.X += -2f;
                                if (projectile.spriteDirection == 1 && projectile.scale <= 1f)
                                {
                                    stupidOffset.X += -2f;
                                }

                                stupidOffset.Y += -2f;
                                if (projectile.frame > 2 && projectile.frame < 6)
                                {
                                    stupidOffset += new Vector2(2f, -2f);
                                }
                            }

                            if (slotNumber == (byte)SlotType.Body || slotNumber == (byte)SlotType.Misc)
                            {
                                if (projectile.frame > 2 && projectile.frame < 6)
                                {
                                    stupidOffset += new Vector2(-4f * projectile.spriteDirection, 0f);
                                }
                                else
                                {
                                    if (projectile.spriteDirection == 1)
                                    {
                                        stupidOffset += new Vector2(-4f, 0f);
                                    }
                                }
                            }

                            if (slotNumber == (byte)SlotType.Hat)
                            {
                                if (projectile.spriteDirection == -1)
                                {
                                    //stupidOffset += new Vector2(-4f, 0f);
                                }

                                if (projectile.type == AssortedCrazyThings.slimePetLegacy[5]) //rainbow slime fix
                                {
                                    if (projectile.spriteDirection == -1)
                                    {
                                        //stupidOffset += new Vector2(4f, 0f);
                                    }
                                }

                                if (projectile.frame < 6)
                                {

                                    stupidOffset += new Vector2(-2f * projectile.spriteDirection, 2f);

                                    if (projectile.frame > 2)
                                    {
                                        stupidOffset += new Vector2(-1f * projectile.spriteDirection, 0f);
                                    }
                                }
                                else
                                {
                                    //-4f
                                    stupidOffset += new Vector2(-3f * projectile.spriteDirection, 2f);
                                }
                            }
                        */
                    }

                    if (slotNumber == (byte)SlotType.Carried)
                    {
                        float handsOffsetX = -22f * projectile.scale + 22f;
                        if (PetAccessory.Offset[slimeAccessory].X <= -6f)
                        {
                            handsOffsetX = -(2.5f * projectile.scale) + 3.5f;
                        }
                        float handsOffsetY = (projectile.scale < 1) ? 2.5f * projectile.scale - 2.5f : 10f * projectile.scale - 10f;
                        stupidOffset.X += handsOffsetX;
                        stupidOffset.Y += handsOffsetY;
                        //if (projectile.frame > 2 && projectile.frame < 6) //hands "bounce"
                        //{
                        //    // += -4f
                        //    stupidOffset.X += -4f + handsOffsetX / 4f;
                        //}

                        if (projectile.spriteDirection == -1)
                        {
                            stupidOffset.X -= 2 * stupidOffset.X;
                        }
                    }

                    if (slotNumber == (byte)SlotType.Hat)
                    {
                        if (projectile.scale == 0.6f) //hack
                        {
                            stupidOffset.Y -= 1f;
                        }
                        if (PetAccessory.Offset[slimeAccessory].Y != 0f)
                        {
                            if (projectile.scale == 1.2f)
                            {
                                stupidOffset.Y += 2f;
                            }
                            stupidOffset.Y += (1f - projectile.scale) * 16f;
                        }
                    }

                    if (projectile.scale == 0.6f) //hack
                    {
                        stupidOffset.Y += -7.5f * 2.25f * projectile.scale + 7.5f; //2f
                    }

                    stupidOffset += new Vector2(0f, drawOriginOffsetY + (-7.5f * projectile.scale + 7.5f));
                    Vector2 drawPos = projectile.position - Main.screenPosition + drawOrigin + stupidOffset;
                    spriteBatch.Draw(texture, drawPos, new Rectangle?(frameLocal), color, projectile.rotation, frameLocal.Size() / 2, projectile.scale, effect, 0f);
                }
            }
        }

        public enum PetColor : byte
        {
            Black,
            Blue,
            Green,
            Pink,
            Purple,
            Rainbow,
            Red,
            Xmas,
            Yellow
        }
    }
}
