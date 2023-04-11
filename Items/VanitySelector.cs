using Terraria;
using Terraria.ID;

namespace AssortedCrazyThings.Items
{
	//Needs PlaceablesFunctional as it's an ingredient for VanityDresserItem
	[Content(ContentType.PlaceablesFunctional | ContentType.DroppedPets | ContentType.OtherPets, needsAllToFilter: true)]
	public class VanitySelector : AssItem
	{
		public override void SetDefaults()
		{
			Item.width = 32;
			Item.height = 32;
			Item.maxStack = 1;
			Item.rare = 1;
			Item.useAnimation = 16;
			Item.useTime = 16;
			Item.UseSound = SoundID.Item1;
			Item.consumable = false;
			Item.value = Item.sellPrice(silver: 10);
		}

		public override bool CanUseItem(Player player)
		{
			return false;
		}

		public override void AddRecipes()
		{
			//actual recipe here
			CreateRecipe(1).AddRecipeGroup("IronBar", 10).AddIngredient(ItemID.Silk, 50).AddTile(TileID.Anvils).Register();
		}
	}
}
