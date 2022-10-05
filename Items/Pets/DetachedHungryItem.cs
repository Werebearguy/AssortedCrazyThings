using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
	[LegacyName("DetachedHungry")]
	public class DetachedHungryItem : SimplePetItemBase
	{
		public override int PetType => ModContent.ProjectileType<DetachedHungryProj>();

		public override int BuffType => ModContent.BuffType<DetachedHungryBuff>();

		public override void SafeSetDefaults()
		{
			Item.value = Item.sellPrice(gold: 1);
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.HellstoneBar, 4).AddIngredient(ItemID.RottenChunk, 10).AddTile(TileID.DemonAltar).Register();
		}
	}

	public class DetachedHungryItem_AoMM : SimplePetItemBase_AoMM<DetachedHungryItem>
	{
		public override int BuffType => ModContent.BuffType<DetachedHungryBuff_AoMM>();
	}
}
