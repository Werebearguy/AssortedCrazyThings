using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Harblesnargits_Mod_01.Projectiles.Weapons
{
    public class proj_fire_01 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Handle with care");
        }

        public override void SetDefaults()
        {
			projectile.CloneDefaults(ProjectileID.EyeFire);
			projectile.aiStyle = 23;
			projectile.friendly = true;
			projectile.hostile = false;
            //adding this makes the game select the AI that is meant for the Spazmatism cursed fire ai
            aiType = ProjectileID.EyeFire;
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
