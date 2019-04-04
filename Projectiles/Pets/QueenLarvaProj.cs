using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Pets
{
    public class QueenLarvaProj : ModProjectile
    {
        private int sincounter;

        public override string Texture
        {
            get
            {
                return "AssortedCrazyThings/Projectiles/Pets/QueenLarvaProj_0"; //temp
            }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Queen Bee Larva");
            Main.projFrames[projectile.type] = 4;
            Main.projPet[projectile.type] = true;
        }

        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.DD2PetGhost);
            projectile.aiStyle = -1;
            projectile.width = 28;
            projectile.height = 34;
            projectile.alpha = 0;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            PetPlayer mPlayer = Main.player[projectile.owner].GetModPlayer<PetPlayer>(mod);
            Texture2D image = mod.GetTexture("Projectiles/Pets/QueenLarvaProj_" + mPlayer.queenLarvaType);
            Rectangle bounds = new Rectangle();
            bounds.X = 0;
            bounds.Width = image.Bounds.Width;
            bounds.Height = (image.Bounds.Height / Main.projFrames[projectile.type]);
            bounds.Y = projectile.frame * bounds.Height;

            float sinY = 0;
            sincounter = sincounter > 150 ? 0 : sincounter + 1;
            sinY = (float)((Math.Sin((sincounter / 150f) * 2 * Math.PI) - 1) * 2);

            Vector2 stupidOffset = new Vector2(projectile.width / 2, (projectile.height - 20f) + sinY);
            Vector2 drawPos = projectile.position - Main.screenPosition + stupidOffset;

            spriteBatch.Draw(image, drawPos, bounds, lightColor, 0f, bounds.Size() / 2, 1f, SpriteEffects.None, 0f);
            return false;
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            PetPlayer modPlayer = player.GetModPlayer<PetPlayer>(mod);
            if (player.dead)
            {
                modPlayer.QueenLarva = false;
            }
            if (modPlayer.QueenLarva)
            {
                projectile.timeLeft = 2;

                AssAI.FlickerwickPetAI(projectile, lightPet: false, lightDust: false, reverseSide: true, offsetX: 16f, offsetY: 10f);

                AssAI.FlickerwickPetDraw(projectile, frameCounterMaxFar: 4, frameCounterMaxClose: 10);
            }
        }
    }
}
