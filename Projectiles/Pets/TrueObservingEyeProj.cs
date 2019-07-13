using AssortedCrazyThings.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Pets
{
    public class TrueObservingEyeProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("True Observing Eye");
            Main.projFrames[projectile.type] = 4;
            Main.projPet[projectile.type] = true;
        }

        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.ZephyrFish);
            projectile.aiStyle = -1;
            projectile.width = 12;
            projectile.height = 12;
            projectile.tileCollide = false;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D image = Main.projectileTexture[projectile.type];

            Rectangle bounds = new Rectangle();
            bounds.X = 0;
            bounds.Width = image.Bounds.Width;
            bounds.Height = image.Bounds.Height / Main.projFrames[projectile.type];
            bounds.Y = projectile.frame * bounds.Height;

            SpriteEffects effects = projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            Vector2 eyeCenter = new Vector2(0f, 12f);

            Vector2 stupidOffset = new Vector2(projectile.width / 2, projectile.height / 2);
            Vector2 drawPos = projectile.position - Main.screenPosition + stupidOffset;
            Vector2 drawOrigin = bounds.Size() / 2;

            spriteBatch.Draw(image, drawPos, bounds, lightColor, projectile.rotation, drawOrigin - eyeCenter, 1f, effects, 0f);

            //Draw Eye

            image = mod.GetTexture("Projectiles/Pets/TrueObservingEyeProj_Eye");

            Vector2 between = Main.player[projectile.owner].Center - (projectile.position + stupidOffset);
            //between.Length(): 94 is "idle", 200 is very fast following
            //28.5f = 200f / 7f
            float magnitude = Utils.Clamp(between.Length() / 28.5f, 1.3f, 7f);

            between.Normalize();
            between *= magnitude;

            drawPos += between;
            drawOrigin = image.Bounds.Size() / 2;
            spriteBatch.Draw(image, drawPos, image.Bounds, lightColor, projectile.rotation, drawOrigin, 1f, effects, 0f);

            return false;
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            PetPlayer modPlayer = player.GetModPlayer<PetPlayer>(mod);
            if (player.dead)
            {
                modPlayer.TrueObservingEye = false;
            }
            if (modPlayer.TrueObservingEye)
            {
                projectile.timeLeft = 2;
            }
            AssAI.FlickerwickPetAI(projectile, lightPet: false, lightDust: false, reverseSide: true, vanityPet: true, veloSpeed: 0.5f, offsetX: 20f, offsetY: -60f);
            AssAI.FlickerwickPetDraw(projectile, 6, 8);
        }
    }
}
