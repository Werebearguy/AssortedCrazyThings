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

        protected int frame2Counter = 0;
        protected int frame2 = 0;

        public override bool PreAI()
        {
            Player player = Main.player[projectile.owner];
            player.lizard = false;
            return true;
        }

        public override void PostAI()
        {
            if(projectile.ai[0] != 0f) //frame 6 to 9 flying
            {
                frame2Counter += 3;
                if (frame2Counter > 6)
                {
                    frame2++;
                    frame2Counter = 0;
                }
                if (frame2 <= 5 || frame2 > 9)
                {
                    frame2 = 6;
                }
            }
            else //frame 1 to 4 walking
            {
                if (projectile.velocity.Y == 0.1f)
                {
                    if (projectile.velocity.X == 0f)
                    {
                        frame2 = 1; //0
                        frame2Counter = 0;
                    }
                    else if (Math.Abs(projectile.velocity.X) > 0.1)
                    {
                        frame2Counter += (int)(Math.Abs(projectile.velocity.X) * 0.25f);
                        frame2Counter++;
                        if (frame2Counter > 6)
                        {
                            frame2++;
                            frame2Counter = 0;
                        }
                        if (frame2 > 4) //5
                        {
                            frame2 = 1; //0
                        }
                    }
                    else
                    {
                        frame2 = 1; //0
                        frame2Counter = 0;
                    }
                }
                else //frame 6 to 9 flying
                {
                    frame2Counter++;
                    if (projectile.velocity.Y < 0f)
                    {
                        frame2Counter += 2;
                    }
                    if (frame2Counter > 6)
                    {
                        frame2++;
                        frame2Counter = 0;
                    }
                    if (frame2 > 9)
                    {
                        frame2 = 6;
                    }
                    if (frame2 < 6)
                    {
                        frame2 = 6;
                    }
                }
            }

            if (projectile.velocity.Y != 0.1f) projectile.rotation = projectile.velocity.X * 0.01f;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            if (!AssortedCrazyThings.slimePetLegacy.Contains(projectile.type)) //if not a legacy slime
            {
                DrawAccessories(spriteBatch, drawColor, preDraw: true);
            }

            DrawBaseSprite(spriteBatch, drawColor);
            return false;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            if(!AssortedCrazyThings.slimePetLegacy.Contains(projectile.type)) //if not a legacy slime
            {
                DrawAccessories(spriteBatch, drawColor);
            }
        }

        private void DrawBaseSprite(SpriteBatch spriteBatch, Color drawColor)
        {
            PetPlayer mPlayer = Main.player[projectile.owner].GetModPlayer<PetPlayer>();
            //check if it wears a "useNoHair" hat, then if it does, change the texture to that,
            //otherwise use default one
            bool useNoHair = false;
            uint slimeAccessoryHat = mPlayer.GetAccessory((byte)SlotType.Hat);
            if (slimeAccessoryHat != 0 &&
                PetAccessory.UseNoHair[slimeAccessoryHat] &&
                //Array.IndexOf(AssortedCrazyThings.slimePetNoHair, projectile.type) != -1 && //if it has a NoHair tex
                AssortedCrazyThings.slimePetNoHair.Contains(projectile.type) && //if it has a NoHair tex
                !AssortedCrazyThings.slimePetLegacy.Contains(projectile.type)) //if its not legacy
            {
                useNoHair = true;
            }

            if (MoreDrawBaseSprite(spriteBatch, drawColor, useNoHair)) //do a pre-draw for the rainbow slimes
            {
                Texture2D texture = Main.projectileTexture[projectile.type];
                if (useNoHair) //only if not legacy
                {
                    //because texture name is absolute but GetTexture takes the relative path, we only take the name as reference
                    //and construct the rest
                    string[] texNameList = texture.Name.Split(new string[] { "/" }, 10, StringSplitOptions.RemoveEmptyEntries);
                    texture = mod.GetTexture("Projectiles/Pets/" + texNameList[texNameList.Length - 1] + "NoHair");
                }
                Rectangle frameLocal = new Rectangle(0, frame2 * Texheight, texture.Width, texture.Height / 10);
                SpriteEffects effect = projectile.spriteDirection != 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
                Vector2 drawOrigin = new Vector2(Texwidth * 0.5f, Texheight * 0.5f);
                Vector2 stupidOffset = new Vector2(0f, projectile.gfxOffY + drawOriginOffsetY);
                Vector2 drawPos = projectile.position - Main.screenPosition + drawOrigin + stupidOffset;
                Color color = drawColor * ((255f - projectile.alpha) / 255f);

                //AssUtils.ShowDustAtPos(135, drawPos + Main.screenPosition);

                spriteBatch.Draw(texture, drawPos, new Rectangle?(frameLocal), color, projectile.rotation, frameLocal.Size() / 2, projectile.scale, effect, 0f);
            }
        }

        public virtual bool MoreDrawBaseSprite(SpriteBatch spriteBatch, Color drawColor, bool useNoHair)
        {
            return true;
        }

        private void DrawAccessories(SpriteBatch spriteBatch, Color drawColor, bool preDraw = false)
        {
            PetPlayer mPlayer = Main.player[projectile.owner].GetModPlayer<PetPlayer>();

            for (byte slotNumber = 1; slotNumber < 5; slotNumber++) //0 is None, reserved
            {
                //slimeAccessory is the indexed number of the accessory (from 0 to 255)
                
                uint slimeAccessory = mPlayer.GetAccessory(slotNumber);

                if ((preDraw || !PetAccessory.PreDraw[slimeAccessory]) && slimeAccessory != 0)
                {
                    Texture2D texture = PetAccessory.Texture[slimeAccessory];

                    int altTextureNumber = PetAccessory.AltTexture[slimeAccessory, mPlayer.petColor];
                        
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

                    Rectangle frameLocal = new Rectangle(0, frame2 * Texheight, texture.Width, texture.Height / 10);

                    //get necessary properties and parameters for draw
                    SpriteEffects effect = projectile.spriteDirection != 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
                    Vector2 drawOrigin = new Vector2(Texwidth * 0.5f, Texheight * 0.5f);
                    Vector2 stupidOffset = new Vector2(0f, drawOriginOffsetY + projectile.gfxOffY);
                    Color color = drawColor * ((255 - PetAccessory.Alpha[slimeAccessory]) / 255f);

                    //AssUtils.ShowDustAtPos(136, projectile.position + drawOrigin + stupidOffset);

                    Vector2 originOffset = - PetAccessory.Offset[slimeAccessory];
                    if (projectile.spriteDirection == -1)
                    {
                        originOffset.X = -originOffset.X;
                    }
                    
                    Vector2 drawPos = projectile.position - Main.screenPosition + drawOrigin + stupidOffset;
                    spriteBatch.Draw(texture, drawPos, new Rectangle?(frameLocal), color, projectile.rotation, frameLocal.Size() / 2 + originOffset, projectile.scale, effect, 0f);
                }
            }
        }

        //add a new color in alphabetic order (same in PetAccessoryClass.AddAltTextures, in the signature and in the intArray inside
        public enum PetColor : byte
        {
            Black,
            Blue,
            Corrupt,
            Crimson,
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
