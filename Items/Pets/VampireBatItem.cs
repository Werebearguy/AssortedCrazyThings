using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
	[LegacyName("VampireBat")]
	public class VampireBatItem : SimplePetItemBase
	{
		public override int PetType => ModContent.ProjectileType<VampireBatProj>();

		public override int BuffType => ModContent.BuffType<VampireBatBuff>();

		public override void SafeSetDefaults()
		{
			Item.value = Item.sellPrice(silver: 20);
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.BrokenBatWing, 2).AddIngredient(ItemID.SoulofNight, 10).AddTile(TileID.DemonAltar).Register();
		}
	}

	public class VampireBatItem_AoMM : SimplePetItemBase_AoMM<VampireBatItem>
	{
		public override int BuffType => ModContent.BuffType<VampireBatBuff_AoMM>();
	}
}
