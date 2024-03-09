using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets
{
	public class EnchantedSwordBuff : SimplePetBuffBase
	{
		public override void SafeSetStaticDefaults()
		{
			Main.vanityPet[Type] = false;
			Main.lightPet[Type] = true;
		}

		public override int PetType => ModContent.ProjectileType<EnchantedSwordProj>();

		public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().EnchantedSword;
	}
}
