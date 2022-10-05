using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
	[LegacyName("LifelikeMechanicalFrog")]
	public class LifelikeMechanicalFrogItem : SimplePetItemBase
	{
		public override int PetType => ModContent.ProjectileType<LifelikeMechanicalFrogProj>();

		public override int BuffType => ModContent.BuffType<LifelikeMechanicalFrogBuff>();

		public override void SafeSetDefaults()
		{
			Item.value = Item.sellPrice(copper: 10);
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.Frog, 1).AddTile(TileID.Anvils).Register();
		}
	}

	public class LifelikeMechanicalFrogItem_AoMM : SimplePetItemBase_AoMM<LifelikeMechanicalFrogItem>
	{
		public override int BuffType => ModContent.BuffType<LifelikeMechanicalFrogBuff_AoMM>();
	}
}
