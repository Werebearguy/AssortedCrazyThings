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
            drawOffsetX = -12;
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

        public override void PostAI()
        {
            PetPlayer mPlayer = Main.player[projectile.owner].GetModPlayer<PetPlayer>(mod);
            Main.projectileTexture[projectile.type] = mod.GetTexture("Projectiles/Pets/YoungWyvern_" + mPlayer.youngWyvernType);
        }
    }
}
