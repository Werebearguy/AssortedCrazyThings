using Terraria;

namespace AssortedCrazyThings.Projectiles.Pets.CuteSlimes
{
	public class CuteSlimePinkProj : CuteSlimeBaseProj
	{
		public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().CuteSlimePink;

		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Cute Pink Slime");
		}

		public override void SafeSetDefaults()
		{
			Projectile.scale = 0.8f;
			Projectile.alpha = 75;
			DrawOriginOffsetY = -19;
		}
	}
}
