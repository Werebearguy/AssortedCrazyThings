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
            Main.projFrames[Projectile.type] = 4;
            Main.projPet[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.ZephyrFish);
            Projectile.aiStyle = -1;
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.tileCollide = false;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D image = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;

            Rectangle bounds = new Rectangle();
            bounds.X = 0;
            bounds.Width = image.Bounds.Width;
            bounds.Height = image.Bounds.Height / Main.projFrames[Projectile.type];
            bounds.Y = Projectile.frame * bounds.Height;

            SpriteEffects effects = Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            Vector2 eyeCenter = new Vector2(0f, 12f);

            Vector2 stupidOffset = new Vector2(Projectile.width / 2, Projectile.height / 2);
            Vector2 drawPos = Projectile.position - Main.screenPosition + stupidOffset;
            Vector2 drawOrigin = bounds.Size() / 2;

            Main.spriteBatch.Draw(image, drawPos, bounds, lightColor, Projectile.rotation, drawOrigin - eyeCenter, 1f, effects, 0f);

            //Draw Eye

            image = Mod.GetTexture("Projectiles/Pets/TrueObservingEyeProj_Eye").Value;

            Vector2 between = Projectile.GetOwner().Center - (Projectile.position + stupidOffset);
            //between.Length(): 94 is "idle", 200 is very fast following
            //28.5f = 200f / 7f
            float magnitude = Utils.Clamp(between.Length() / 28.5f, 1.3f, 7f);

            between.Normalize();
            between *= magnitude;

            drawPos += between;
            drawOrigin = image.Bounds.Size() / 2;
            Main.spriteBatch.Draw(image, drawPos, image.Bounds, lightColor, Projectile.rotation, drawOrigin, 1f, effects, 0f);

            return false;
        }

        public override void AI()
        {
            Player player = Projectile.GetOwner();
            PetPlayer modPlayer = player.GetModPlayer<PetPlayer>();
            if (player.dead)
            {
                modPlayer.TrueObservingEye = false;
            }
            if (modPlayer.TrueObservingEye)
            {
                Projectile.timeLeft = 2;
            }
            AssAI.FlickerwickPetAI(Projectile, lightPet: false, lightDust: false, reverseSide: true, vanityPet: true, veloSpeed: 0.5f, offsetX: 20f, offsetY: -60f);
            AssAI.FlickerwickPetDraw(Projectile, 6, 8);
        }
    }
}
