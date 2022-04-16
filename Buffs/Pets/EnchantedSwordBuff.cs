using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets
{
	public class EnchantedSwordBuff : SimplePetBuffBase
	{
		public override int PetType => ModContent.ProjectileType<EnchantedSwordProj>();

		public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().EnchantedSword;

		public override void SafeSetDefaults()
		{
			DisplayName.SetDefault("Enchanted Sword");
			Description.SetDefault("The Enchanted Sword watches over you");
		}
	}
}
