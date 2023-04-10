using AssortedCrazyThings.Base;
using Terraria;
using Terraria.ID;

namespace AssortedCrazyThings.Projectiles
{
	[Content(ContentType.Bosses)]
	public class EmpoweringBuffGlobalProjectile : AssGlobalProjectile
	{
		//public override bool InstancePerEntity => false;

		public override void ModifyHitNPC(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers)
		{
			if ((projectile.minion || ProjectileID.Sets.MinionShot[projectile.type]) && projectile.friendly && projectile.damage > 0)
			{
				AssPlayer mPlayer = projectile.GetOwner().GetModPlayer<AssPlayer>();
				if (mPlayer.empoweringBuff)
				{
					modifiers.SourceDamage += mPlayer.empoweringStep * 0.25f;
				}
			}
		}
	}
}
