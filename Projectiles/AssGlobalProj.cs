using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles
{
    public class AssGlobalProj : GlobalProjectile
    {
        public AssGlobalProj()
        {

        }

        public override bool InstancePerEntity
        {
            get
            {
                return true;
            }
        }

        public override void ModifyHitNPC(Projectile projectile, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if(projectile.minion && projectile.friendly && projectile.damage > 0)
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