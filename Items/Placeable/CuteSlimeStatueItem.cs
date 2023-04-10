using AssortedCrazyThings.Tiles;
using Terraria;
using Terraria.ID;

namespace AssortedCrazyThings.Items.Placeable
{
	//TODO RegularCuteSlimes config
	[Content(ContentType.PlaceablesFunctional | ContentType.CuteSlimes)]
	public class CuteSlimeStatueItem : PlaceableItem<CuteSlimeStatueTile>
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Cute Slime Statue");
			// Tooltip.SetDefault("You can't catch statue spawned creatures");
			
			Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.DefaultToPlaceableTile(TileType);
			Item.width = 24;
			Item.height = 32;
			Item.maxStack = 99;
			Item.rare = 0;
			Item.value = Item.sellPrice(0, 0, 0, 60);
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.Gel, 200).AddIngredient(ItemID.StoneBlock, 50).AddRecipeGroup(AssRecipes.RegularCuteSlimesGroup, 1).AddTile(TileID.HeavyWorkBench).Register();
		}
	}
}
