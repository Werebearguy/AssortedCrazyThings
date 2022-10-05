using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
	public class NumberMuncherItem : SimplePetItemBase
	{
		public override int PetType => ModContent.ProjectileType<NumberMuncherProj>();

		public override int BuffType => ModContent.BuffType<NumberMuncherBuff>();

		public override void SafeSetDefaults()
		{
			Item.value = Item.sellPrice(0, 0, 1, 20); //two statues
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.AlphabetStatue1, 2)
				.AddTile(TileID.Bookcases)
				.Register();
		}
	}

	public class NumberMuncherItem_AoMM : SimplePetItemBase_AoMM<NumberMuncherItem>
	{
		public override int BuffType => ModContent.BuffType<NumberMuncherBuff_AoMM>();
	}
}
