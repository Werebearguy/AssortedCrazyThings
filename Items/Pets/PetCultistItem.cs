using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
	[Content(ContentType.DroppedPets)]
	public class PetCultistItem : SimplePetItemBase
	{
		public override int PetType => ModContent.ProjectileType<PetCultistProj>();

		public override int BuffType => ModContent.BuffType<PetCultistBuff>();

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tiny Ominous Coin");
			Tooltip.SetDefault("Summons a tiny cultist to follow you and heal when injured");
		}

		public override void SafeSetDefaults()
		{
			Item.rare = -11;
			Item.value = Item.sellPrice(copper: 10);
		}
	}
}
