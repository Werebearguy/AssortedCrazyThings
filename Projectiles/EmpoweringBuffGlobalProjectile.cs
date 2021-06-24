using AssortedCrazyThings.Base;
using Terraria;
using Terraria.ID;

namespace AssortedCrazyThings.Projectiles
{
    [Content(ContentType.Bosses)]
    public class EmpoweringBuffGlobalProjectile : AssGlobalProjectile
    {
        //public override bool InstancePerEntity => false;

        public override void ModifyHitNPC(Projectile projectile, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if ((projectile.minion || ProjectileID.Sets.MinionShot[projectile.type]) && projectile.friendly && projectile.damage > 0)
            {
                AssPlayer mPlayer = projectile.GetOwner().GetModPlayer<AssPlayer>();
                if (mPlayer.empoweringBuff)
                {
                    damage += (int)(damage * mPlayer.step * 0.25f);
                }
            }
        }
    }
}
