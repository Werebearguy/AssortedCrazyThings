using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Weapons
{
    public class PocketSand : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Surprise");
        }

        public override void SetDefaults()
        {
			projectile.CloneDefaults(ProjectileID.PurificationPowder);
			projectile.aiStyle = 6;
			projectile.friendly = true;
			projectile.hostile = false;
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (Main.rand.NextFloat() >= .66f)
            {
                target.AddBuff(BuffID.Confused, 120); //two full seconds, 33% chance
            }
        }
    }
}
