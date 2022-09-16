using AssortedCrazyThings.Projectiles.Pets.CuteSlimes;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets.CuteSlimes
{
	public class CuteSlimeBlueBuff : CuteSlimeBuffBase
	{
		public override int PetType => ModContent.ProjectileType<CuteSlimeBlueProj>();

		public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().CuteSlimeBlue;

		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Cute Blue Slime");
			Description.SetDefault("A cute blue slime is following you");
		}
	}
}
