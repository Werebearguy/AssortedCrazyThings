using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Weapons
{
    [Autoload]
    public class SpazmatismFire : AssProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("'Handle with care'");
        }

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.EyeFire);
            Projectile.aiStyle = 23;
            Projectile.friendly = true;
            Projectile.hostile = false;
            //adding this makes the game select the AI that is meant for the Spazmatism cursed fire ai
            AIType = ProjectileID.EyeFire;
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (Main.rand.NextFloat() >= .66f)
            {
                target.AddBuff(BuffID.CursedInferno, 180); //three full seconds, 33% chance
            }
        }
    }
}
