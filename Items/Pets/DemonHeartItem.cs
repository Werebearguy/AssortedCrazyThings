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

	public class DemonHeartItem_AoMM : SimplePetItemBase_AoMM<DemonHeartItem>
	{
		public override int BuffType => ModContent.BuffType<DemonHeartBuff_AoMM>();
	}
}
