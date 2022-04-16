using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
	public class GhostMartianItem : SimplePetItemBase
	{
		public override int PetType => ModContent.ProjectileType<GhostMartianProj>();

		public override int BuffType => ModContent.BuffType<GhostMartianBuff>();

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("K-II Meter");
			Tooltip.SetDefault("Summons a vengeful invader's ghost to follow you");
		}

		public override void SafeSetDefaults()
		{
			Item.noUseGraphic = true;
			Item.rare = -11;
			Item.value = Item.sellPrice(copper: 10);
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.MartianConduitPlating, 25)
				.AddCondition(Recipe.Condition.InGraveyardBiome)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}
}
