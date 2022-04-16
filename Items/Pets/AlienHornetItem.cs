using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
	[LegacyName("AlienHornet")]
	public class AlienHornetItem : SimplePetItemBase
	{
		public override int PetType => ModContent.ProjectileType<AlienHornetProj>();

		public override int BuffType => ModContent.BuffType<AlienHornetBuff>();

		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Vortex Nectar");
			Tooltip.SetDefault("Summons a friendly Alien Hornet to follow you");
		}

		public override void SafeSetDefaults()
		{
			Item.value = Item.sellPrice(gold: 5);
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.Nectar, 1).AddIngredient(ItemID.FragmentVortex, 10).AddTile(TileID.DemonAltar).Register();
		}
	}
}
