using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Pets
{
    public class DocileMechanicalLaserEyePurple : ModProjectile
    {
        public override string Texture
        {
            get
            {
                return "AssortedCrazyThings/Projectiles/Pets/DocileDemonEyeWarning";
            }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Docile Laser Eye");
            Main.projFrames[projectile.type] = 2;
            Main.projPet[projectile.type] = true;
        }

        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.BabyEater);
            aiType = ProjectileID.BabyEater;
        }

        public override bool PreAI()
        {
            Player player = Main.player[projectile.owner];
            player.eater = false; // Relic from aiType
            return true;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            SpriteEffects effects = SpriteEffects.None;
            Texture2D image = mod.GetTexture("Projectiles/Pets/DocileDemonEyeWarning");

            Vector2 stupidOffset = new Vector2(projectile.width / 2, projectile.height / 2);

            spriteBatch.Draw(image, projectile.position - Main.screenPosition + stupidOffset, image.Bounds, Color.White, projectile.rotation, image.Bounds.Size() / 2, projectile.scale, effects, 0f);

            return false;
        }

        public override void AI()
        {
            projectile.frame = 0;
            projectile.rotation = 0f;
            Player player = Main.player[projectile.owner];
            PetPlayer modPlayer = player.GetModPlayer<PetPlayer>(mod);
            if (player.dead)
            {
                modPlayer.DocileMechanicalLaserEyePurple = false;
            }
            if (modPlayer.DocileMechanicalLaserEyePurple)
            {
                projectile.timeLeft = 2;
            }
        }
    }
}
