using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Minions
{
    public class CompanionDungeonSoul : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Companion Soul");
            Main.projFrames[projectile.type] = 2; //4
            Main.projPet[projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[projectile.type] = false;
            ProjectileID.Sets.Homing[projectile.type] = true;
            ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true; //This is necessary for right-click targeting
        }

        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.Spazmamini);
            projectile.width = 14;
            projectile.height = 24;
            aiType = ProjectileID.Spazmamini;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }

        public override bool PreAI()
        {
            Player player = Main.player[projectile.owner];
            player.twinsMinion = false; // Relic from aiType
            return true;
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            AssPlayer modPlayer = player.GetModPlayer<AssPlayer>(mod);
            if (player.dead)
            {
                modPlayer.soulArmorMinions = false;
            }
            if (modPlayer.soulArmorMinions)
            {
                projectile.timeLeft = 2;
            }
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            fallThrough = true;
            return true;
        }
    }
}