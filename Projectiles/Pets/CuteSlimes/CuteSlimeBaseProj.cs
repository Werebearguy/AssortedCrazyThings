using AssortedCrazyThings.Base;
using AssortedCrazyThings.Items.PetAccessories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Pets.CuteSlimes
{
    public abstract class CuteSlimeBaseProj : ModProjectile
    {
        private const string PetAccessoryFolder = "AssortedCrazyThings/Items/PetAccessories/";
        private const string NoHair = "NoHair";
        private const string Draw = "_Draw";

        public const int Projwidth = 28;
        public const int Projheight = 32;

        protected int frame2Counter = 0;
        protected int frame2 = 0;

        public override bool PreAI()
        {
            Player player = Projectile.GetOwner();
            player.lizard = false;
            return true;
        }

        public override void PostAI()
        {
            //readjusting the animation
            if (Projectile.ai[0] != 0f) //frame 6 to 9 flying
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
                if (Projectile.velocity.Y == 0.1f)
                {
                    if (Projectile.velocity.X == 0f)
                    {
                        frame2 = 0; //0 //idle frame
                        frame2Counter = 0;
                    }
                    else if (Math.Abs(Projectile.velocity.X) > 0.1)
                    {
                        frame2Counter += (int)(Math.Abs(Projectile.velocity.X) * 0.25f);
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
                    if (Projectile.velocity.Y < 0f)
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

            //0.1f because vanilla sets it to 0.1f when on the ground
            if (Projectile.velocity.Y != 0.1f) Projectile.rotation = Projectile.velocity.X * 0.01f;
        }

        public override bool PreDraw(ref Color drawColor)
        {
            DrawAccessories(drawColor, preDraw: true);

            DrawBaseSprite(drawColor);

            DrawAccessories(drawColor);
            return false;
        }

        /// <summary>
        /// Draws the base sprite. Picks the NoHair variant if needed
        /// </summary>
        private void DrawBaseSprite(Color drawColor)
        {
            PetPlayer pPlayer = Projectile.GetOwner().GetModPlayer<PetPlayer>();
            //check if it wears a "useNoHair" hat, then if it does, change the texture to that,
            //otherwise use default one
            bool useNoHair = false;
            PetAccessory petAccessoryHat = pPlayer.GetAccessoryInSlot((byte)SlotType.Hat);
            SlimePet sPet = SlimePets.GetPet(Projectile.type);

            if (petAccessoryHat != null &&
                petAccessoryHat.UseNoHair &&
                sPet.HasNoHair) //if it has a NoHair tex
            {
                useNoHair = true;
            }

            bool drawPreAddition = true;
            bool drawPostAddition = true;

            PetAccessory petAccessory;
            //handle if pre/post additions are drawn based on the slimePet(Pre/Post)AdditionSlot
            for (byte slotNumber = 1; slotNumber < 5; slotNumber++)
            {
                petAccessory = pPlayer.GetAccessoryInSlot(slotNumber);

                if (petAccessory != null)
                {
                    if (sPet.PreAdditionSlot == slotNumber) drawPreAddition = false;
                    if (sPet.PostAdditionSlot == slotNumber) drawPostAddition = false;
                }
            }

            bool drawnPreDraw = drawPreAddition ? MorePreDrawBaseSprite(drawColor, useNoHair) : true; //do a pre-draw for rainbow and dungeon

            if (drawnPreDraw)
            {
                //Draw NoHair if necessary, otherwise regular sprite
                Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
                if (useNoHair) //only if not legacy
                {
                    texture = ModContent.GetTexture(texture.Name + NoHair).Value;
                }
                Rectangle frameLocal = new Rectangle(0, frame2 * texture.Height / 10, texture.Width, texture.Height / 10);
                SpriteEffects effect = Projectile.spriteDirection != 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
                Vector2 drawOrigin = new Vector2(Projwidth * 0.5f, (texture.Height / 10) * 0.5f);
                Vector2 stupidOffset = new Vector2(Projectile.type == ModContent.ProjectileType<CuteSlimePinkNewProj>() ? -8f : 0f, Projectile.gfxOffY + DrawOriginOffsetY);
                Vector2 drawPos = Projectile.position - Main.screenPosition + drawOrigin + stupidOffset;
                Color color = Projectile.GetAlpha(drawColor);
                //color = drawColor * ((255f - projectile.alpha) / 255f);

                Main.spriteBatch.Draw(texture, drawPos, frameLocal, color, Projectile.rotation, frameLocal.Size() / 2, Projectile.scale, effect, 0f);
            }

            if (drawPostAddition) MorePostDrawBaseSprite(drawColor, useNoHair); //used for xmas bow, lava horn, princess crown and illuminant afterimage
        }

        /// <summary>
        /// Draw the pet specific PreDraw behind the base sprite
        /// </summary>
        public virtual bool MorePreDrawBaseSprite(Color drawColor, bool useNoHair)
        {
            return true;
        }

        /// <summary>
        /// Draw the pet specific PostDraw infront of the base sprite
        /// </summary>
        public virtual void MorePostDrawBaseSprite(Color drawColor, bool useNoHair)
        {

        }

        /// <summary>
        /// Draws the pet vanity accessories (behind or infront of the base sprite)
        /// </summary>
        private void DrawAccessories(Color drawColor, bool preDraw = false)
        {
            PetPlayer pPlayer = Projectile.GetOwner().GetModPlayer<PetPlayer>();
            SlimePet sPet = SlimePets.GetPet(Projectile.type);
            PetAccessory petAccessory;

            string textureString;
            string colorString;
            string drawString;
            sbyte altTextureNumber;
            Texture2D texture;
            Rectangle frameLocal;
            SpriteEffects effect;
            Vector2 drawOrigin;
            Vector2 stupidOffset;
            Color color;
            Vector2 originOffset;
            Vector2 drawPos;

            for (byte slotNumber = 1; slotNumber < 5; slotNumber++) //0 is None, reserved
            {
                petAccessory = pPlayer.GetAccessoryInSlot(slotNumber);

                if (petAccessory != null &&
                    (preDraw || !petAccessory.PreDraw) &&
                    !sPet.IsSlotTypeBlacklisted[slotNumber])
                {
                    textureString = PetAccessoryFolder + petAccessory.Name;
                    colorString = petAccessory.HasAlts ? petAccessory.AltTextureSuffixes[petAccessory.Color] : "";

                    drawString = Draw;

                    altTextureNumber = petAccessory.PetVariations[SlimePets.slimePets.IndexOf(Projectile.type)];
                    if (altTextureNumber > 0) //change texture if not -1 and not 0
                    {
                        drawString += altTextureNumber;
                    }
                    else if (altTextureNumber == -1)
                    {
                        continue;
                    }
                    texture = ModContent.GetTexture(textureString + colorString + drawString).Value;

                    frameLocal = new Rectangle(0, frame2 * (Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Height() / Main.projFrames[Projectile.type]), texture.Width, texture.Height / 10);

                    //get necessary properties and parameters for draw
                    effect = Projectile.spriteDirection != 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
                    drawOrigin = new Vector2(Projwidth * 0.5f, (texture.Height / 10) * 0.5f);
                    stupidOffset = new Vector2(Projectile.type == ModContent.ProjectileType<CuteSlimePinkNewProj>() ? -8f : 0f, DrawOriginOffsetY + Projectile.gfxOffY);
                    color = drawColor * ((255 - petAccessory.Alpha) / 255f);

                    originOffset = -petAccessory.Offset;
                    originOffset.X *= Math.Sign(Projectile.spriteDirection);

                    drawPos = Projectile.position - Main.screenPosition + drawOrigin + stupidOffset;
                    Main.spriteBatch.Draw(texture, drawPos, frameLocal, color, Projectile.rotation, frameLocal.Size() / 2 + originOffset, Projectile.scale, effect, 0f);
                }
            }
        }
    }
}
