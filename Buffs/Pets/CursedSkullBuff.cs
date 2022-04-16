using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets
{
	public class CursedSkullBuff : SimplePetBuffBase
	{
		public override int PetType => ModContent.ProjectileType<CursedSkullProj>();

		public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().CursedSkull;

		public override void SafeSetDefaults()
		{
			DisplayName.SetDefault("Cursed Skull");
			Description.SetDefault("It won't curse you, I promise");
		}
	}
}
