using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
	public class TorchGodLightPetItem : SimplePetItemBase
	{
		public override int PetType => ModContent.ProjectileType<TorchGodLightPetProj>();

		public override int BuffType => ModContent.BuffType<TorchGodLightPetBuff>();

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Godly Torch");
			Tooltip.SetDefault("Summons a godly torch to follow you\n" +
				"Automatically places your normal torches with 'Smart Cursor' active");
		}

		public override void SafeSetDefaults()
		{
			Item.rare = -11;
			Item.value = Item.sellPrice(gold: 2);
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.TorchGodsFavor)
				.AddIngredient(ItemID.Torch, 999)
				.AddTile(TileID.DemonAltar)
				.Register();
		}
	}
}
