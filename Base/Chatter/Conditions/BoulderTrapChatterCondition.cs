using Terraria;
using Terraria.ID;

namespace AssortedCrazyThings.Base.Chatter.Conditions
{
	public class BoulderTrapChatterCondition : ChatterCondition
	{
		protected override bool Check(ChatterSource source, IChatterParams param)
		{
			if (param is not PlayerHurtByTrapChatterParams p || p.Projectile is not Projectile proj)
			{
				return false;
			}

			return proj.type == ProjectileID.Boulder || proj.type == ProjectileID.BouncyBoulder || proj.type == ProjectileID.RollingCactus || proj.type == ProjectileID.LifeCrystalBoulder || proj.type == ProjectileID.MiniBoulder || proj.type == ProjectileID.MoonBoulder;
		}
	}
}
