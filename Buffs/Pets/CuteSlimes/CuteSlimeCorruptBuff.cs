using AssortedCrazyThings.Projectiles.Pets.CuteSlimes;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets.CuteSlimes
{
	public class CuteSlimeCorruptBuff : CuteSlimeBuffBase
	{
		public override int PetType => ModContent.ProjectileType<CuteSlimeCorruptProj>();

		public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().CuteSlimeCorrupt;

		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Cute Corrupt Slime");
			Description.SetDefault("A cute corrupt slime is following you");
		}
	}
}
