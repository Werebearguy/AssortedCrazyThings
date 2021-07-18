using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent;

namespace AssortedCrazyThings.Projectiles.Minions
{
    [Content(ContentType.Weapons)]
    public class SlimePackMinionSpike : AssProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Slime Pack Minion Spike");
            Main.projFrames[Projectile.type] = 16;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
            ProjectileID.Sets.MinionShot[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.WoodenArrowFriendly);
            Projectile.aiStyle = 1;
            Projectile.height = 12;
            Projectile.width = 12;
            Projectile.penetrate = 1;
            Projectile.tileCollide = true;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Summon;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
            return true;
        }

        public byte PickedTexture
        {
            get
            {
                return (byte)Projectile.ai[1];
            }
            set
            {
                Projectile.ai[1] = value;
            }
        }

        private Color ColorFromTexture(byte tex)
        {
            return tex switch
            {
                0 => new Color(70, 70, 70),
                1 => new Color(90, 140, 255),
                2 => new Color(10, 180, 40),
                3 => new Color(255, 30, 90),
                4 => new Color(220, 70, 255),
                5 => new Color(110, 110, 110),
                6 => new Color(255, 90, 60),
                7 => new Color(150, 150, 10),
                8 => new Color(160, 70, 22),
                9 => new Color(210, 140, 100),
                10 => new Color(200, 250, 255),
                11 => new Color(80, 80, 140),
                12 => new Color(70, 50, 140),
                13 => new Color(100, 100, 200),
                14 => new Color(200, 99, 100),
                15 => new Color(255, 50, 230),
                _ => default(Color),
            };
        }

        public override void PostAI()
        {
            //projectile.ai[1] used as color picker
            if (Projectile.ai[0] < 2) //Uses Arrow AI decrement of ai[0] to play a sound
            {
                SoundEngine.PlaySound(SoundID.Item17, Projectile.Center);
            }

            if (Main.rand.NextFloat() < 0.2f)
            {
                Vector2 randVelo = new Vector2(Main.rand.NextFloat(0.5f) - 0.25f, Main.rand.NextFloat(0.5f) - 0.25f) + Vector2.Normalize(Projectile.velocity) * 0.5f;
                Dust dust = Dust.NewDustPerfect(Projectile.Center, 16, randVelo, 120, ColorFromTexture((byte)Projectile.ai[1]), 0.7f);
                dust.fadeIn = 0f;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (PickedTexture > 15) PickedTexture = 15; //protection against prince slime
            SpriteEffects effects = SpriteEffects.None;
            Texture2D image = TextureAssets.Projectile[Projectile.type].Value;
            Rectangle bounds = image.Frame(1, Main.projFrames[Projectile.type], frameY: PickedTexture);
            Vector2 stupidOffset = new Vector2(Projectile.width * 0.5f, Projectile.height * 0.5f);

            if (PickedTexture == 5)
            {
                double cX = Projectile.Center.X + DrawOffsetX;
                double cY = Projectile.Center.Y + DrawOriginOffsetY;
                lightColor = Lighting.GetColor((int)(cX / 16), (int)(cY / 16), Main.DiscoColor * 1.2f);
            }

            Main.EntitySpriteDraw(image, Projectile.position - Main.screenPosition + stupidOffset, bounds, lightColor, Projectile.rotation, bounds.Size() / 2, Projectile.scale, effects, 0);

            return false;
        }

        public override void PostDraw(Color lightColor)
        {
            if (PickedTexture == 15)
            {
                Texture2D image = Mod.Assets.Request<Texture2D>("Projectiles/Minions/SlimePackMinionSpike_Glowmask").Value;
                SpriteEffects effects = SpriteEffects.None;

                for (int k = Projectile.oldPos.Length - 1; k >= 0; k--)
                {
                    Vector2 stupidOffset = new Vector2(Projectile.width * 0.5f, Projectile.height * 0.5f);
                    Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + stupidOffset;
                    Color color = Projectile.GetAlpha(Color.White) * ((Projectile.oldPos.Length - k) / (1f * Projectile.oldPos.Length));
                    Main.EntitySpriteDraw(image, drawPos, image.Bounds, color, Projectile.rotation, image.Bounds.Size() / 2, Projectile.scale, effects, 0);
                }
            }
        }
    }
}
