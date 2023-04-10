using Terraria;

namespace AssortedCrazyThings.Projectiles.Pets.CuteSlimes
{
	public class CuteSlimeBlueProj : CuteSlimeBaseProj
	{
		public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().CuteSlimeBlue;

		public override void SafeSetStaticDefaults()
		{
			// DisplayName.SetDefault("Cute Blue Slime");
		}

		public override void SafeSetDefaults()
		{
			Projectile.alpha = 75;
		}
	}
}
