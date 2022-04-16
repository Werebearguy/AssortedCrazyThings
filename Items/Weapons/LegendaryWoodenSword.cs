using AssortedCrazyThings.Projectiles.Weapons;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Weapons
{
	[Content(ContentType.Weapons)]
	public class LegendaryWoodenSword : AssItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Legendary Wooden Sword");
		}

		public override void SetDefaults()
		{
			//Item.CloneDefaults(ItemID.IronShortsword);
			Item.damage = 8;
			Item.knockBack = 4f;
			Item.shootSpeed = 2.1f;
			Item.useStyle = 13;
			Item.useAnimation = 12;
			Item.useTime = 12;
			Item.width = 32;
			Item.height = 32;
			Item.UseSound = SoundID.Item1;
			Item.DamageType = DamageClass.Melee;
			Item.autoReuse = false;
			Item.noMelee = true;
			Item.noUseGraphic = true;

			Item.rare = -11;
			Item.value = Item.sellPrice(0, 0, 0, 10); //Woods have no sell value, just make this 10 copper cause why not
			Item.shoot = ModContent.ProjectileType<LegendaryWoodenSwordProj>();
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.Wood, 10)
				.AddIngredient(ItemID.BorealWood, 10)
				.AddIngredient(ItemID.PalmWood, 10)
				.AddIngredient(ItemID.RichMahogany, 10)
				.AddIngredient(ItemID.Wood, 10)
				.AddRecipeGroup("ACT:EvilWood", 10)
				.AddTile(TileID.WorkBenches)
				.Register();
		}
	}
}
