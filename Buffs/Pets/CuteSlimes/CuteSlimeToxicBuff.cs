using AssortedCrazyThings.Projectiles.Pets.CuteSlimes;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets.CuteSlimes
{
	public class CuteSlimeToxicBuff : CuteSlimeBuffBase
	{
		public override int PetType => ModContent.ProjectileType<CuteSlimeToxicProj>();

		public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().CuteSlimeToxic;

		public override void SafeSetDefaults()
		{
			DisplayName.SetDefault("Cute Toxic Slime");
			Description.SetDefault("A cute toxic slime is following you");
		}
	}
}
