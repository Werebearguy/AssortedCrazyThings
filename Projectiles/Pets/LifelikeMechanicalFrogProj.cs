using AssortedCrazyThings.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Pets
{
    public class LifelikeMechanicalFrogProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lifelike Mechanical Frog");
            Main.projFrames[Projectile.type] = 8;
            Main.projPet[Projectile.type] = true;
            DrawOriginOffsetY = 1;
        }

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.Bunny);
            AIType = ProjectileID.Bunny;
            Projectile.width = 18;
            Projectile.height = 20;
        }

        public override bool PreAI()
        {
            Player player = Projectile.GetOwner();
            player.bunny = false; // Relic from AIType
            return true;
        }

        public override void AI()
        {
            Player player = Projectile.GetOwner();
            PetPlayer modPlayer = player.GetModPlayer<PetPlayer>();
            if (player.dead)
            {
                modPlayer.LifelikeMechanicalFrog = false;
            }
            if (modPlayer.LifelikeMechanicalFrog)
            {
                Projectile.timeLeft = 2;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            PetPlayer mPlayer = Projectile.GetOwner().GetModPlayer<PetPlayer>();
            SpriteEffects effects = Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            Texture2D image = Mod.GetTexture("Projectiles/Pets/LifelikeMechanicalFrogProj" + (mPlayer.mechFrogCrown == 1 ? "Crown" : "")).Value;
            Rectangle bounds = new Rectangle
            {
                X = 0,
                Y = Projectile.frame,
                Width = image.Bounds.Width,
                Height = image.Bounds.Height / Main.projFrames[Projectile.type]
            };
            bounds.Y *= bounds.Height; //cause proj.frame only contains the frame number

            Vector2 stupidOffset = new Vector2(Projectile.width / 2, Projectile.height / 2 + 1f + Projectile.gfxOffY);

            Main.EntitySpriteDraw(image, Projectile.position - Main.screenPosition + stupidOffset, bounds, lightColor, Projectile.rotation, bounds.Size() / 2, Projectile.scale, effects, 0);

            return false;
        }
    }
}
