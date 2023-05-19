using Terraria;

namespace AssortedCrazyThings.Projectiles.Pets.CuteSlimes
{
	public class CuteSlimePinkIceProj : CuteSlimeBaseProj
	{
		public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().CuteSlimePinkIce;

		public override void SafeSetDefaults()
		{
			Projectile.scale = 1.2f;
			Projectile.alpha = 75;
			DrawOriginOffsetY = -14;
		}
	}
}
