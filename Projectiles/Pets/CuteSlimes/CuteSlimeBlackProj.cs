using Terraria;

namespace AssortedCrazyThings.Projectiles.Pets.CuteSlimes
{
	public class CuteSlimeBlackProj : CuteSlimeBaseProj
	{
		public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().CuteSlimeBlack;

		public override void SafeSetDefaults()
		{
			//Projectile.scale = 0.9f;
			Projectile.alpha = 75;
		}
	}
}
