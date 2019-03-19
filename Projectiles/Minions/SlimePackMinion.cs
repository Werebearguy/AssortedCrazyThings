using AssortedCrazyThings.Projectiles.Pets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Terraria;
using Terraria.ID;

namespace AssortedCrazyThings.Projectiles.Minions
{
    //check this file for more info vvvvvvvv
    public class SlimePackMinion : BabySlimeBase
    {
        public const int DefDamage = 26;
        public const float DefKnockback = 4f; //same as slime staff x 2
        public const float SpikedIncrease = 1.4f;

        protected byte TotalNumberOfThese = 17; //17 for basic, 16 for advanced

        protected string SlimeType = ""; 

        public override string Texture
        {
            get
            {
                return "AssortedCrazyThings/Projectiles/Minions/SlimePackMinions/SlimeMinion" + SlimeType + "_0"; //use fixed texture
            }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Slime Pack Minion");
            Main.projFrames[projectile.type] = 6;
            Main.projPet[projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[projectile.type] = true;
            ProjectileID.Sets.Homing[projectile.type] = true;
            drawOffsetX = -10;
            drawOriginOffsetY = -2;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 8;
        }

        public override void MoreSetDefaults()
        {
            //used to set dimensions (if necessary) //also use to set projectile.minion
            projectile.width = 32;
            projectile.height = 30;

            projectile.minion = true;

            shootSpikes = false;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            //AssUtils.Print("send netupdate " + PickedTexture + " " + ShootTimer);
            writer.Write((byte)PickedTexture);
            //writer.Write((byte)ShootTimer);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            PickedTexture = reader.ReadByte();
            //ShootTimer = reader.ReadByte();
            //AssUtils.Print("recv netupdate " + PickedTexture + " " + ShootTimer);
        }

        public override bool PreAI()
        {
            AssPlayer modPlayer = Main.player[projectile.owner].GetModPlayer<AssPlayer>(mod);
            if (Main.player[projectile.owner].dead)
            {
                modPlayer.slimePackMinion = false;
            }
            if (modPlayer.slimePackMinion)
            {
                projectile.timeLeft = 2;
            }
            
            if (Main.netMode != NetmodeID.Server && Main.myPlayer == projectile.owner)
            {
                if (!HasTexture)
                {
                    byte tex = (byte)Main.rand.Next(TotalNumberOfThese);
                    PickedTexture = tex;
                    projectile.netUpdate = true;
                }
            }

            return true;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            //Rainbow is _5, Illuminant is _15
            if (HasTexture)
            {
                Texture2D image = mod.GetTexture("Projectiles/Minions/SlimePackMinions/SlimeMinion" + SlimeType + "_" + PickedTexture);
                Rectangle bounds = new Rectangle
                {
                    X = 0,
                    Y = projectile.frame,
                    Width = image.Bounds.Width,
                    Height = image.Bounds.Height / 6
                };
                bounds.Y *= bounds.Height; //cause proj.frame only contains the frame number
                Vector2 stupidOffset = new Vector2(0f, projectile.gfxOffY); //gfxoffY is for when the projectile is on a slope or half brick
                SpriteEffects effect = projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
                Vector2 drawOrigin = new Vector2(projectile.width * 0.5f, projectile.height * 0.5f);
                Vector2 drawPos = projectile.position - Main.screenPosition + drawOrigin + stupidOffset;

                if (PickedTexture == 5 && SlimeType != "Assorted")
                {
                    double cX = projectile.Center.X + drawOffsetX;
                    double cY = projectile.Center.Y + drawOriginOffsetY;
                    lightColor = Lighting.GetColor((int)(cX / 16), (int)(cY / 16), Main.DiscoColor * 1.2f);
                }

                Color color = lightColor * ((255 - projectile.alpha) / 255f);

                if (SlimeType == "Assorted" && (PickedTexture == 0 ||
                    PickedTexture == 5 ||
                    PickedTexture == 6 ||
                    PickedTexture == 7 ||
                    PickedTexture == 10 ||
                    PickedTexture == 11)) color.A = 255;

                if (PickedTexture == 3 && SlimeType != "Assorted") //pinky
                {
                    drawPos.Y += 7f;
                    projectile.scale = 0.5f;
                }

                spriteBatch.Draw(image, drawPos, bounds, color, projectile.rotation, bounds.Size() / 2, projectile.scale, effect, 0f);
            }
            return false;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            if (HasTexture)
            {
                if ((PickedTexture == 15 && SlimeType != "Assorted") || PickedTexture == 12 && SlimeType == "Assorted")
                {
                    Texture2D image = mod.GetTexture("Projectiles/Minions/SlimePackMinions/SlimeMinion" + SlimeType + "_" + PickedTexture + "_Glowmask");
                    Rectangle bounds = new Rectangle
                    {
                        X = 0,
                        Y = projectile.frame,
                        Width = image.Bounds.Width,
                        Height = image.Bounds.Height / 6
                    };
                    bounds.Y *= bounds.Height; //cause proj.frame only contains the frame number

                    SpriteEffects effect = projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
                    Vector2 drawOrigin = new Vector2(projectile.width * 0.5f, projectile.height * 0.5f);
                    Vector2 stupidOffset = new Vector2(0f, projectile.gfxOffY); //gfxoffY is for when the projectile is on a slope or half brick

                    if (PickedTexture == 15 && SlimeType != "Assorted") //illuminant slime
                    {
                        for (int k = projectile.oldPos.Length - 1; k >= 0; k--)
                        {
                            //the higher the k, the older the position
                            //Length is implicitely set in TrailCacheLength up there
                            //start from half the length so the origninal sprite isnt super blurred
                            Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + stupidOffset;
                            Color color = projectile.GetAlpha(Color.White) * ((projectile.oldPos.Length - k) / (1f * projectile.oldPos.Length)) * ((255 - 80) / 255f);
                            color.A = (byte)(80 * ((projectile.oldPos.Length - k) / (1f * projectile.oldPos.Length)));
                            spriteBatch.Draw(image, drawPos, bounds, color, projectile.oldRot[k], bounds.Size() / 2, projectile.scale, effect, 0f);
                        }
                    }
                    else if (PickedTexture == 12 && SlimeType == "Assorted")
                    {
                        Vector2 drawPos = projectile.position - Main.screenPosition + drawOrigin + stupidOffset;
                        spriteBatch.Draw(image, drawPos, bounds, lightColor, projectile.rotation, bounds.Size() / 2, projectile.scale, effect, 0f);
                    }
                }
            }
        }
    }

    public class SlimePackSpikedMinion : SlimePackMinion
    {
        public override void MoreSetDefaults()
        {
            //used to set dimensions (if necessary) //also use to set projectile.minion
            projectile.width = 32;
            projectile.height = 30;

            projectile.minion = true;

            SlimeType = "Spiked";
            TotalNumberOfThese = 16;

            shootSpikes = true;
        }
    }

    public class SlimePackAssortedMinion : SlimePackMinion
    {
        public override void SetStaticDefaults()
        {
            //could've left it out but removed the trailing cache thing
            DisplayName.SetDefault("Slime Pack Minion");
            Main.projFrames[projectile.type] = 6;
            Main.projPet[projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[projectile.type] = true;
            ProjectileID.Sets.Homing[projectile.type] = true;
            drawOffsetX = -10;
            drawOriginOffsetY = -2;
        }

        public override void MoreSetDefaults()
        {
            //used to set dimensions (if necessary) //also use to set projectile.minion
            projectile.width = 32;
            projectile.height = 30;

            projectile.minion = true;

            SlimeType = "Assorted";
            TotalNumberOfThese = 13;

            shootSpikes = false;
        }
    }
}
