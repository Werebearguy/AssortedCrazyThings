using Terraria;

namespace AssortedCrazyThings.Projectiles.Pets.CuteSlimes
{
	public class CuteSlimeIceProj : CuteSlimeBaseProj
	{
		public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().CuteSlimeIce;

		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Cute Ice Slime");
		}

		public override void SafeSetDefaults()
		{
			Projectile.alpha = 75;
		}
	}
}
