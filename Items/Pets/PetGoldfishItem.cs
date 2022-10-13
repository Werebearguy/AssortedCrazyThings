using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
	public class PetGoldfishItem : SimplePetItemBase
	{
		public override int PetType => ModContent.ProjectileType<PetGoldfishProj>();

		public override int BuffType => ModContent.BuffType<PetGoldfishBuff>();

		public override void SafeSetDefaults()
		{
			Item.value = Item.sellPrice(copper: 10);
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.FishStatue, 1).AddTile(TileID.DemonAltar).Register();
		}
	}

	public class PetGoldfishItem_AoMM : SimplePetItemBase_AoMM<PetGoldfishItem>
	{
		public override int BuffType => ModContent.BuffType<PetGoldfishBuff_AoMM>();
	}
}
