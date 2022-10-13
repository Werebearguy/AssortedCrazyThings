using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
	public class DynamiteBunnyItem : SimplePetItemBase
	{
		public override int PetType => ModContent.ProjectileType<DynamiteBunnyProj>();

		public override int BuffType => ModContent.BuffType<DynamiteBunnyBuff>();

		public override void SafeSetDefaults()
		{
			Item.width = 14;
			Item.height = 32;

			Item.value = Item.sellPrice(silver: 4);
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.Dynamite);
			recipe.AddIngredient(ItemID.Carrot);
			recipe.AddTile(TileID.DemonAltar);
			recipe.Register();

			recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.ExplosiveBunny);
			recipe.AddTile(TileID.DemonAltar);
			recipe.Register();
		}
	}

	public class DynamiteBunnyItem_AoMM : SimplePetItemBase_AoMM<DynamiteBunnyItem>
	{
		public override int BuffType => ModContent.BuffType<DynamiteBunnyBuff_AoMM>();
	}
}
