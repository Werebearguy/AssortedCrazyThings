using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Pets
{
    public class CursedSkull : ModProjectile
    {
        public override string Texture
        {
            get
            {
                return "AssortedCrazyThings/Projectiles/Pets/CursedSkull_0"; //temp
            }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cursed Skull");
            Main.projFrames[projectile.type] = 3;
            Main.projPet[projectile.type] = true;
            drawOriginOffsetY = 2;
        }

        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.ZephyrFish);
            aiType = ProjectileID.ZephyrFish;
        }

        public override bool PreAI()
        {
            Player player = Main.player[projectile.owner];
            player.zephyrfish = false; // Relic from aiType
            return true;
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            PetPlayer modPlayer = player.GetModPlayer<PetPlayer>(mod);
            if (player.dead)
            {
                modPlayer.CursedSkull = false;
            }
            if (modPlayer.CursedSkull)
            {
                projectile.timeLeft = 2;
            }
        }

        public override void PostAI()
        {
            if (projectile.frame >= 3) projectile.frame = 0;

            if (projectile.Center.X - Main.player[projectile.owner].Center.X > 0f)
            {
                projectile.spriteDirection = 1;
            }
            else
            {
                projectile.spriteDirection = -1;
            }

            PetPlayer mPlayer = Main.player[projectile.owner].GetModPlayer<PetPlayer>(mod);
            Main.projectileTexture[projectile.type] = mod.GetTexture("Projectiles/Pets/CursedSkull_" + mPlayer.cursedSkullType);
        }
    }
}
