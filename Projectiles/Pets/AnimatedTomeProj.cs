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
            Main.projFrames[Projectile.type] = 4;
            Main.projPet[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.BabyHornet);
            Projectile.width = 22;
            Projectile.height = 18;
            Projectile.aiStyle = -1;
        }

        public override void AI()
        {
            Player player = Projectile.GetOwner();
            PetPlayer modPlayer = player.GetModPlayer<PetPlayer>();
            if (player.dead)
            {
                modPlayer.AnimatedTome = false;
            }
            if (modPlayer.AnimatedTome)
            {
                Projectile.timeLeft = 2;
            }
            AssAI.ZephyrfishAI(Projectile);
            AssAI.ZephyrfishDraw(Projectile);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            PetPlayer mPlayer = Projectile.GetOwner().GetModPlayer<PetPlayer>();
            SpriteEffects effects = Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            Texture2D image = Mod.GetTexture("Projectiles/Pets/AnimatedTomeProj_" + mPlayer.animatedTomeType).Value;
            Rectangle bounds = new Rectangle
            {
                X = 0,
                Y = Projectile.frame,
                Width = image.Bounds.Width,
                Height = image.Bounds.Height / Main.projFrames[Projectile.type]
            };
            bounds.Y *= bounds.Height; //cause proj.frame only contains the frame number

            Vector2 stupidOffset = new Vector2(Projectile.width / 2 - Projectile.direction * 3f, Projectile.height / 2 + Projectile.gfxOffY);

            Main.spriteBatch.Draw(image, Projectile.position - Main.screenPosition + stupidOffset, bounds, lightColor, Projectile.rotation, bounds.Size() / 2, Projectile.scale, effects, 0f);

            return false;
        }
    }
}
