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
            Player player = Main.player[projectile.owner];
            player.hornet = false; // Relic from aiType
            return true;
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            PetPlayer modPlayer = player.GetModPlayer<PetPlayer>(mod);
            if (player.dead)
            {
                modPlayer.YoungHarpy = false;
            }
            if (modPlayer.YoungHarpy)
            {
                projectile.timeLeft = 2;
            }
        }

        public override void PostAI()
        {
            PetPlayer mPlayer = Main.player[projectile.owner].GetModPlayer<PetPlayer>(mod);
            Main.projectileTexture[projectile.type] = mod.GetTexture("Projectiles/Pets/YoungHarpy_" + mPlayer.youngHarpyType);
        }
    }
}
