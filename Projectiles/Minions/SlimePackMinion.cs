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
        public const int DefDamage = 36;

        private const byte TotalNumberOfThese = 33;

        public override string Texture
        {
            get
            {
                return "AssortedCrazyThings/Projectiles/Minions/SlimePackMinions/SlimeMinion_0"; //use fixed texture
            }
        }

        public byte texture = 0;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Slime Pack Minion");
            Main.projFrames[projectile.type] = 6;
            Main.projPet[projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[projectile.type] = true;
            ProjectileID.Sets.Homing[projectile.type] = true;
            drawOffsetX = -8;
            drawOriginOffsetY = -2;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 8;
        }

        public override void MoreSetDefaults()
        {
            //used to set dimensions (if necessary) //also use to set projectile.minion
            projectile.width = 34;
            projectile.height = 30;

            projectile.minion = true;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write((byte)texture);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            texture = reader.ReadByte();
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
            
            //since default state is 0, textures will use [1 to TotalNumberOfThese] as indices, then it will substract 1 in pre-draw
            if (Main.netMode != NetmodeID.Server)
            {
                if (texture == 0)
                {
                    byte tex = (byte)Main.rand.Next(1, TotalNumberOfThese + 1);
                    texture = tex;
                    projectile.netUpdate = true;
                }
            }

            return true;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            //Rainbow is _10 and _11, Illuminant is _30 and _31
            if (!(texture == 0))
            {
                Texture2D image = mod.GetTexture("Projectiles/Minions/SlimePackMinions/SlimeMinion_" + (texture - 1));
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

                if ((texture - 1) == 10 || (texture - 1) == 11)
                {
                    double cX = projectile.Center.X + drawOffsetX;
                    double cY = projectile.Center.Y + drawOriginOffsetY;
                    drawColor = Lighting.GetColor((int)(cX / 16), (int)(cY / 16), Main.DiscoColor * 1.2f);
                }

                Color color = drawColor * ((255 - projectile.alpha) / 255f);

                spriteBatch.Draw(image, drawPos, bounds, color, projectile.rotation, bounds.Size() / 2, projectile.scale, effect, 0f);
            }
            return false;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            if (!(texture == 0))
            {
                if ((texture - 1) == 30 || (texture - 1) == 31)
                {
                    Texture2D image = mod.GetTexture("Projectiles/Minions/SlimePackMinions/SlimeMinion_" + (texture - 1) + "_Glowmask");
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
                    //the higher the k, the older the position
                    //Length is implicitely set in TrailCacheLength up there
                    //start from half the length so the origninal sprite isnt super blurred
                    for (int k = projectile.oldPos.Length - 1; k >= 0; k--)
                    {
                        Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
                        Color color = projectile.GetAlpha(Color.White) * ((projectile.oldPos.Length - k) / (1f * projectile.oldPos.Length));
                        spriteBatch.Draw(image, drawPos, bounds, color, projectile.oldRot[k], bounds.Size() / 2, projectile.scale, effect, 0f);
                    }
                }
            }
        }
    }
}
