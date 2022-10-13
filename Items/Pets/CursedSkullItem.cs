using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
	[LegacyName("CursedSkull")]
	public class CursedSkullItem : SimplePetItemBase
	{
		public override int PetType => ModContent.ProjectileType<CursedSkullProj>();

		public override int BuffType => ModContent.BuffType<CursedSkullBuff>();

		public override void SafeSetDefaults()
		{
			Item.value = Item.sellPrice(copper: 10);
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.Bone, 10).AddTile(TileID.DemonAltar).Register();
		}
	}

	public class CursedSkullItem_AoMM : SimplePetItemBase_AoMM<CursedSkullItem>
	{
		public override int BuffType => ModContent.BuffType<CursedSkullBuff_AoMM>();
	}
}
