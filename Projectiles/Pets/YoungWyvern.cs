using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Pets
{
    public class YoungWyvern : ModProjectile
    {
        public override string Texture
        {
            get
            {
                return "AssortedCrazyThings/Projectiles/Pets/YoungWyvern_0"; //temp
            }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Young Wyvern");
            Main.projFrames[projectile.type] = 11;
            Main.projPet[projectile.type] = true;
        }

        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.BlackCat);
            aiType = ProjectileID.BlackCat;
        }

        public override bool PreAI()
        {
            Player player = Main.player[projectile.owner];
            player.blackCat = false; // Relic from aiType
            return true;
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            PetPlayer modPlayer = player.GetModPlayer<PetPlayer>(mod);
            if (player.dead)
            {
                modPlayer.YoungWyvern = false;
            }
            if (modPlayer.YoungWyvern)
            {
                projectile.timeLeft = 2;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            PetPlayer mPlayer = Main.player[projectile.owner].GetModPlayer<PetPlayer>(mod);
            SpriteEffects effects = projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            Texture2D image = mod.GetTexture("Projectiles/Pets/YoungWyvern_" + mPlayer.youngWyvernType);
            Rectangle bounds = new Rectangle
            {
                X = 0,
                Y = projectile.frame,
                Width = image.Bounds.Width,
                Height = image.Bounds.Height / 11
            };
            bounds.Y *= bounds.Height; //cause proj.frame only contains the frame number
            
            Vector2 stupidOffset = new Vector2(30, 23f + projectile.gfxOffY);

            spriteBatch.Draw(image, projectile.position - Main.screenPosition + stupidOffset, bounds, lightColor, projectile.rotation, bounds.Size() / 2, projectile.scale, effects, 0f);

            return false;
        }
    }
}
