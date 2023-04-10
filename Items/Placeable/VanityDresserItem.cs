using AssortedCrazyThings.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Placeable
{
	[Content(ContentType.PlaceablesFunctional | ContentType.DroppedPets | ContentType.OtherPets, needsAllToFilter: true)]
	public class VanityDresserItem : PlaceableItem<VanityDresserTile>
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Costume Dresser");
			/* Tooltip.SetDefault("Left Click to change your Pet's appearance"
				 + "\nRight Click to change your Light Pet's appearance"); */

			Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.DefaultToPlaceableTile(TileType);
			Item.width = 34;
			Item.height = 26;
			Item.maxStack = 99;
			Item.rare = 1;
			Item.value = Item.sellPrice(silver: 10);
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.Dresser).AddIngredient(ModContent.ItemType<VanitySelector>()).Register();
		}
	}
}
