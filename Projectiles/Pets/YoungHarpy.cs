using AssortedCrazyThings.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Pets
{
    public class YoungHarpy : ModProjectile
    {
        public override string Texture
        {
            get
            {
                return "AssortedCrazyThings/Projectiles/Pets/YoungHarpy_0"; //temp
            }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Young Harpy");
            Main.projFrames[projectile.type] = 4;
            Main.projPet[projectile.type] = true;
        }

        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.BabyHornet);
            aiType = ProjectileID.BabyHornet;
        }

        public override bool PreAI()
        {
            Player player = projectile.GetOwner();
            player.hornet = false; // Relic from aiType
            return true;
        }

        public override void AI()
        {
            Player player = projectile.GetOwner();
            PetPlayer modPlayer = player.GetModPlayer<PetPlayer>();
            if (player.dead)
            {
                modPlayer.YoungHarpy = false;
            }
            if (modPlayer.YoungHarpy)
            {
                projectile.timeLeft = 2;
            }
            AssAI.TeleportIfTooFar(projectile, player.MountedCenter);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            PetPlayer mPlayer = projectile.GetOwner().GetModPlayer<PetPlayer>();
            SpriteEffects effects = projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            Texture2D image = mod.GetTexture("Projectiles/Pets/YoungHarpy_" + mPlayer.youngHarpyType);
            Rectangle bounds = new Rectangle
            {
                X = 0,
                Y = projectile.frame,
                Width = image.Bounds.Width,
                Height = image.Bounds.Height / Main.projFrames[projectile.type]
            };
            bounds.Y *= bounds.Height; //cause proj.frame only contains the frame number

            Vector2 stupidOffset = new Vector2(projectile.width / 2, projectile.height / 2 + projectile.gfxOffY);

            spriteBatch.Draw(image, projectile.position - Main.screenPosition + stupidOffset, bounds, lightColor, projectile.rotation, bounds.Size() / 2, projectile.scale, effects, 0f);

            return false;
        }
    }
}
