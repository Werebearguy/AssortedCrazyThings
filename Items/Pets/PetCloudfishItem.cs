using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
	public class PetCloudfishItem : SimplePetItemBase
	{
		public override int PetType => ModContent.ProjectileType<PetCloudfishProj>();

		public override int BuffType => ModContent.BuffType<PetCloudfishBuff>();

		public override void SafeSetDefaults()
		{
			Item.width = 32;
			Item.height = 32;

			Item.value = Item.sellPrice(copper: 4); //Cost of 1 bottle
		}

		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient(ItemID.Cloudfish)
				.AddIngredient(ItemID.Bottle)
				.AddCondition(Condition.NearWater)
				//By hand
				.Register();
		}
	}

	public class PetCloudfishItem_AoMM : SimplePetItemBase_AoMM<PetCloudfishItem>
	{
		public override int BuffType => ModContent.BuffType<PetCloudfishBuff_AoMM>();
	}
}
