using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
	[Content(ContentType.DroppedPets)]
	public class CuteLamiaPetItem : SimplePetItemBase
	{
		public override int PetType => ModContent.ProjectileType<CuteLamiaPetProj>();

		public override int BuffType => ModContent.BuffType<CuteLamiaPetBuff>();

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shaking Snake Pot");
			Tooltip.SetDefault("Summons a small, friendly snake to follow you");
		}

		public override void SafeSetDefaults()
		{
			Item.rare = -11;
			Item.value = Item.sellPrice(copper: 10);
		}
	}
}
