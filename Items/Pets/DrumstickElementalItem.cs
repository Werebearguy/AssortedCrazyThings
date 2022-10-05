using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
	public class DrumstickElementalItem : SimplePetItemBase
	{
		public override int PetType => ModContent.ProjectileType<DrumstickElementalProj>();

		public override int BuffType => ModContent.BuffType<DrumstickElementalBuff>();

		public override void SafeSetDefaults()
		{
			Item.value = Item.sellPrice(silver: 7, copper: 50);
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.Duck).AddTile(TileID.CookingPots).Register();
			CreateRecipe(1).AddIngredient(ItemID.MallardDuck).AddTile(TileID.CookingPots).Register();
		}
	}

	public class DrumstickElementalItem_AoMM : SimplePetItemBase_AoMM<DrumstickElementalItem>
	{
		public override int BuffType => ModContent.BuffType<DrumstickElementalBuff_AoMM>();
	}
}
