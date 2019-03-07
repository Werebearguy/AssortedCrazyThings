using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles
{
    public class AssGlobalProj : GlobalProjectile
    {
        public override bool InstancePerEntity
        {
            get
            {
                return true;
            }
        }

        public override void ModifyHitNPC(Projectile projectile, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if((projectile.minion || ProjectileID.Sets.MinionShot[projectile.type]) && projectile.friendly && projectile.damage > 0)
            {
                AssPlayer mPlayer = Main.player[projectile.owner].GetModPlayer<AssPlayer>();
                if (mPlayer.empoweringBuff)
                {
                    damage += (int)(damage * mPlayer.step * 0.25f);
                }
            }
        }
    }
}
