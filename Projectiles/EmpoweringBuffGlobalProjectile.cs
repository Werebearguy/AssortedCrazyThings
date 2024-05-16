using AssortedCrazyThings.Base;
using Terraria;

namespace AssortedCrazyThings.Projectiles
{
	[Content(ContentType.Bosses)]
	public class EmpoweringBuffGlobalProjectile : AssGlobalProjectile
	{
		public override void ModifyHitNPC(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers)
		{
			if (projectile.IsMinionOrSentryRelated)
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
