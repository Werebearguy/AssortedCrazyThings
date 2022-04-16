using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
	[Content(ContentType.DroppedPets)]
	public class GobletItem : SimplePetItemBase
	{
		public override int PetType => ModContent.ProjectileType<GobletProj>();

		public override int BuffType => ModContent.BuffType<GobletBuff>();

		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Goblet Battle Standard");
			Tooltip.SetDefault("Summons a tiny goblin to follow you");
		}

		public override void SafeSetDefaults()
		{
			Item.value = Item.sellPrice(silver: 10);
		}
	}
}
