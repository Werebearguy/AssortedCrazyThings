using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Pets
{
    public class EnchantedSword : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Enchanted Sword");
            Main.projFrames[projectile.type] = 8;
            Main.projPet[projectile.type] = true;
        }

        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.TikiSpirit);
            aiType = ProjectileID.TikiSpirit;
            projectile.width = 26;
            projectile.height = 50;
        }

        public override bool PreAI()
        {
            Player player = Main.player[projectile.owner];
            return true;
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            PetPlayer modPlayer = player.GetModPlayer<PetPlayer>(mod);
            if (player.dead)
            {
                modPlayer.EnchantedSword = false;
            }
            if (modPlayer.EnchantedSword)
            {
                projectile.timeLeft = 2;
            }
            AssAI.TeleportIfTooFar(projectile, player.MountedCenter);
        }
    }
}
