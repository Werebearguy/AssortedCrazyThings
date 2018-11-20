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
            DisplayName.SetDefault("This is supposed to be green, not orange");
		}
		
        public override void SetDefaults()
        {
			projectile.CloneDefaults(ProjectileID.EyeFire);
			projectile.aiStyle = 23;
			projectile.friendly = true;
			projectile.hostile = false;
        }
		
    }
}