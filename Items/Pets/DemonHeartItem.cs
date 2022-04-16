using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
	[LegacyName("DemonHeart")]
	public class DemonHeartItem : SimplePetItemBase
	{
		public override int PetType => ModContent.ProjectileType<DemonHeartProj>();

		public override int BuffType => ModContent.BuffType<DemonHeartBuff>();

		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Demon Heart");
			Tooltip.SetDefault("Summons a friendly Demon Heart to follow you");
		}

		public override void SafeSetDefaults()
		{
			Item.rare = ItemRarityID.Expert;
			Item.value = Item.sellPrice(gold: 2);
			Item.expert = true;
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.DemonHeart, 1).AddTile(TileID.DemonAltar).Register();
		}
	}
}
