using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Pets
{
    public class SkeletronPrimeHandProj : ModProjectile
    {
        public override string Texture
        {
            get
            {
                return "AssortedCrazyThings/Projectiles/Pets/SkeletronPrimeHandProj_0"; //temp
            }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Skeletron Prime Pet Hand");
            Main.projFrames[projectile.type] = 2;
            Main.projPet[projectile.type] = true;
            drawOriginOffsetY = -8;
        }

        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.BabyEater);
            aiType = ProjectileID.BabyEater;
            //projectile.aiStyle = -1;
            projectile.width = 24;
            projectile.height = 32;
        }

        public override bool PreAI()
        {
            Player player = Main.player[projectile.owner];
            player.eater = false; // Relic from aiType
            return true;
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            PetPlayer modPlayer = player.GetModPlayer<PetPlayer>(mod);
            if (player.dead)
            {
                modPlayer.SkeletronPrimeHand = false;
            }
            if (modPlayer.SkeletronPrimeHand)
            {
                projectile.timeLeft = 2;
            }
        }

        public override void PostAI()
        {
            projectile.rotation = projectile.velocity.X * -0.08f;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            AssUtils.DrawSkeletronLikeArms(spriteBatch, "AssortedCrazyThings/Projectiles/Pets/SkeletronPrimeHand_Arm", projectile.Center, Main.player[projectile.owner].Center, selfPad: projectile.height / 2, centerPad: -20f, direction: 0);

            PetPlayer mPlayer = Main.player[projectile.owner].GetModPlayer<PetPlayer>(mod);
            Texture2D image = mod.GetTexture("Projectiles/Pets/SkeletronPrimeHandProj_" + mPlayer.skeletronPrimeHandType);
            Rectangle bounds = new Rectangle();
            bounds.X = 0;
            bounds.Width = image.Bounds.Width;
            bounds.Height = image.Bounds.Height / Main.projFrames[projectile.type];
            bounds.Y = projectile.frame * bounds.Height;

            Vector2 stupidOffset = new Vector2(projectile.width / 2, projectile.height);
            Vector2 drawPos = projectile.position - Main.screenPosition + stupidOffset;
            Vector2 drawOrigin = bounds.Size() / 2;
            drawOrigin.Y += projectile.height / 2;

            spriteBatch.Draw(image, drawPos, bounds, lightColor, projectile.rotation, drawOrigin, 1f, SpriteEffects.None, 0f);
            return false;
        }
    }
}
