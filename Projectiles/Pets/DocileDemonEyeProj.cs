using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Pets
{
    public class DocileDemonEyeProj : ModProjectile
    {
        public const byte TotalNumberOfThese = 12;

        public override string Texture
        {
            get
            {
                return "AssortedCrazyThings/Projectiles/Pets/DocileDemonEyeProj_0"; //temp
            }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Docile Demon Eye");
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

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            PetPlayer modPlayer = player.GetModPlayer<PetPlayer>(mod);
            if (player.dead)
            {
                modPlayer.DocileDemonEye = false;
            }
            if (modPlayer.DocileDemonEye)
            {
                projectile.timeLeft = 2;
            }
        }

        public override void PostAI()
        {
            PetPlayer mPlayer = Main.player[projectile.owner].GetModPlayer<PetPlayer>(mod);
            Main.projectileTexture[projectile.type] = mod.GetTexture("Projectiles/Pets/DocileDemonEyeProj_" + mPlayer.petEyeType);
        }
    }
}
