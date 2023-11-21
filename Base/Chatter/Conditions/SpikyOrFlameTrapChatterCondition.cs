using Terraria;
using Terraria.ID;

namespace AssortedCrazyThings.Base.Chatter.Conditions
{
	public class SpikyOrFlameTrapChatterCondition : ChatterCondition
	{
		protected override bool Check(ChatterSource source, IChatterParams param)
		{
			if (param is not PlayerHurtByTrapChatterParams p || p.Projectile is not Projectile proj)
			{
				return false;
			}

			return proj.type == ProjectileID.SpikyBallTrap || proj.type == ProjectileID.FlamesTrap;
		}
	}
}
