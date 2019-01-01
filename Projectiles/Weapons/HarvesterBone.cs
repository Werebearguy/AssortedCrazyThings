using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Weapons
{
    //used in BaseHarvester, same as bone, just applies slow
    public class HarvesterBone : ModProjectile
    {
        public override string Texture
        {
            get
            {
                return "Terraria/Projectile_" + ProjectileID.SkeletonBone;
            }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Harvester Bone");
        }

        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.SkeletonBone);
        }

        public override void ModifyHitPlayer(Player target, ref int damage, ref bool crit)
        {
            if (Main.rand.NextFloat() >= 0.5f)
            {
                target.AddBuff(BuffID.Slow, 90); //1 1/2 seconds, 50% chance
            }
        }
    }
}
