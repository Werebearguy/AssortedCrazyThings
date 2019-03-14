using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Minions
{
    public class SlimePackMinionSpikeProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Slime Pack Minion Spike");
            Main.projFrames[projectile.type] = 16;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 8;
            ProjectileID.Sets.MinionShot[projectile.type] = true;
        }

        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.WoodenArrowFriendly);
            projectile.aiStyle = 1;
            projectile.height = 6;
            projectile.width = 6;
            projectile.penetrate = 1;
            projectile.tileCollide = true;
            projectile.friendly = true;
            projectile.hostile = false;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Collision.HitTiles(projectile.position + projectile.velocity, projectile.velocity, projectile.width, projectile.height);
            return true;
        }

        public byte PickedTexture
        {
            get
            {
                return (byte)projectile.ai[1];
            }
            set
            {
                projectile.ai[1] = value;
            }
        }

        public override void PostAI()
        {
            //projectile.ai[1] used as color picker
            if (projectile.ai[0] < 2)
            {
                Main.PlaySound(SoundID.Item17, projectile.position);
            }

            if (Main.rand.NextFloat() < 0.1f)
            {
                Vector2 randVelo = new Vector2(Main.rand.NextFloat() - 0.5f, Main.rand.NextFloat() - 0.5f) + Vector2.Normalize(projectile.velocity);
                Dust dust = Dust.NewDustPerfect(projectile.Center, 16, randVelo, 120, new Color(0, 0, 0), 0.9f);
            }

        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            SpriteEffects effects = SpriteEffects.None;
            Rectangle bounds = new Rectangle();
            bounds.X = 0;
            bounds.Width = Main.projectileTexture[projectile.type].Width;
            bounds.Height = Main.projectileTexture[projectile.type].Height / Main.projFrames[projectile.type];
            if (PickedTexture > 15) PickedTexture = 15; //protection against prince slime
            bounds.Y = PickedTexture * bounds.Height;
            Vector2 stupidOffset = new Vector2(0f, 0f);

            if (PickedTexture == 5)
            {
                double cX = projectile.Center.X + drawOffsetX;
                double cY = projectile.Center.Y + drawOriginOffsetY;
                lightColor = Lighting.GetColor((int)(cX / 16), (int)(cY / 16), Main.DiscoColor * 1.2f);
            }

            spriteBatch.Draw(Main.projectileTexture[projectile.type], projectile.position - Main.screenPosition + stupidOffset, bounds, lightColor, projectile.rotation, bounds.Size() / 2, projectile.scale, effects, 0f);

            return false;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            if (PickedTexture == 15)
            {
                Texture2D image = mod.GetTexture("Projectiles/Minions/SlimePackMinionSpikeProj_Glowmask");
                SpriteEffects effects = SpriteEffects.None;

                for (int k = projectile.oldPos.Length - 1; k >= 0; k--)
                {
                    Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition;
                    Color color = projectile.GetAlpha(Color.White) * ((projectile.oldPos.Length - k) / (1f * projectile.oldPos.Length));
                    spriteBatch.Draw(image, drawPos, image.Bounds, color, projectile.rotation, image.Bounds.Size() / 2, projectile.scale, effects, 0f);
                    //Main.NewText(projectile.oldRot[k]);
                }
            }
        }
    }
}
