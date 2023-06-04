using Terraria;

namespace AssortedCrazyThings.Projectiles.Pets.CuteSlimes
{
	public class CuteSlimeCrimsonJungleProj : CuteSlimeBaseProj
	{
		public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().CuteSlimeCrimsonJungle;

		public override void SafeSetDefaults()
		{
			Projectile.scale = 1.2f;
			Projectile.alpha = 75;
			DrawOriginOffsetY = -14;
		}
	}
}
