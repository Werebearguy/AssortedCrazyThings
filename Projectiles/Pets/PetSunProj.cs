using AssortedCrazyThings.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Pets
{
    public class PetSunProj : ModProjectile
    {
        public override string Texture
        {
            get
            {
                return "AssortedCrazyThings/Projectiles/Pets/PetSunProj_0"; //temp
            }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Personal Sun");
            Main.projFrames[projectile.type] = 1;
            Main.projPet[projectile.type] = true;
        }

        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.DD2PetGhost);
            projectile.aiStyle = -1;
            projectile.width = 62;
            projectile.height = 62;
            projectile.alpha = 0;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Lighting.AddLight(projectile.Center, Vector3.One);

            int texture = 0;
            if (Main.eclipse) //takes priority
            {
                texture = 2;
            }
            else if (Main.player[projectile.owner].head == 12)
            {
                texture = 1;
            }

            Texture2D image = AssortedCrazyThings.sunPetTextures[texture];

            Vector2 stupidOffset = new Vector2(projectile.width / 2, projectile.height - 28f);
            Vector2 drawPos = projectile.position - Main.screenPosition + stupidOffset;

            spriteBatch.Draw(image, drawPos, image.Bounds, Color.White, 0f, image.Bounds.Size() / 2, 1f, SpriteEffects.None, 0f);
            return false;
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            PetPlayer modPlayer = player.GetModPlayer<PetPlayer>(mod);
            if (player.dead)
            {
                modPlayer.PetSun = false;
            }
            if (modPlayer.PetSun)
            {
                projectile.timeLeft = 2;

                AssAI.FlickerwickPetAI(projectile, lightPet: false, lightDust: false, offsetX: 20f, offsetY: -32f);
            }
        }
    }
}
