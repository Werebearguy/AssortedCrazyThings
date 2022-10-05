using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
	[LegacyName("DocileDemonEye")]
	public class DocileDemonEyeItem : SimplePetItemBase
	{
		public override int PetType => ModContent.ProjectileType<DocileDemonEyeProj>();

		public override int BuffType => ModContent.BuffType<DocileDemonEyeBuff>();

		public override void SafeSetDefaults()
		{
			Item.width = 34;
			Item.height = 22;
			Item.value = Item.sellPrice(silver: 10);
		}

		public override void AddRecipes()
		{
			//regular recipe, dont delete
			CreateRecipe(1).AddIngredient(ItemID.BlackLens, 1).AddIngredient(ItemID.Lens, 1).AddTile(TileID.DemonAltar).Register();
		}
	}

	public class DocileDemonEyeItem_AoMM : SimplePetItemBase_AoMM<DocileDemonEyeItem>
	{
		public override int BuffType => ModContent.BuffType<DocileDemonEyeBuff_AoMM>();
	}
}
