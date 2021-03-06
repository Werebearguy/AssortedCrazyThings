using AssortedCrazyThings.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Pets
{
    public class AnimatedTomeProj : ModProjectile
    {
        public override string Texture
        {
            get
            {
                return "AssortedCrazyThings/Projectiles/Pets/AnimatedTomeProj_0"; //temp
            }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Animated Tome");
            Main.projFrames[projectile.type] = 4;
            Main.projPet[projectile.type] = true;
        }

        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.BabyHornet);
            projectile.width = 22;
            projectile.height = 18;
            projectile.aiStyle = -1;
        }

        public override void AI()
        {
            Player player = projectile.GetOwner();
            PetPlayer modPlayer = player.GetModPlayer<PetPlayer>();
            if (player.dead)
            {
                modPlayer.AnimatedTome = false;
            }
            if (modPlayer.AnimatedTome)
            {
                projectile.timeLeft = 2;
            }
            AssAI.ZephyrfishAI(projectile);
            AssAI.ZephyrfishDraw(projectile);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            PetPlayer mPlayer = projectile.GetOwner().GetModPlayer<PetPlayer>();
            SpriteEffects effects = projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            Texture2D image = mod.GetTexture("Projectiles/Pets/AnimatedTomeProj_" + mPlayer.animatedTomeType);
            Rectangle bounds = new Rectangle
            {
                X = 0,
                Y = projectile.frame,
                Width = image.Bounds.Width,
                Height = image.Bounds.Height / Main.projFrames[projectile.type]
            };
            bounds.Y *= bounds.Height; //cause proj.frame only contains the frame number

            Vector2 stupidOffset = new Vector2(projectile.width / 2 - projectile.direction * 3f, projectile.height / 2 + projectile.gfxOffY);

            spriteBatch.Draw(image, projectile.position - Main.screenPosition + stupidOffset, bounds, lightColor, projectile.rotation, bounds.Size() / 2, projectile.scale, effects, 0f);

            return false;
        }
    }
}
