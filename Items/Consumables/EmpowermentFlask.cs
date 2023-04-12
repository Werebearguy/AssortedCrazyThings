using AssortedCrazyThings.Buffs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Consumables
{
	[Content(ContentType.Bosses)]
	public class EmpowermentFlask : AssItem
	{
		public override void SetStaticDefaults()
		{

			ItemID.Sets.DrinkParticleColors[Item.type] = new Color[3] {
				new Color(13, 106, 137),
				new Color(10, 176, 230),
				new Color(146, 229, 255)
			};

			Item.ResearchUnlockCount = 20;
		}

		public override void SetDefaults()
		{
			Item.width = 20;
			Item.height = 30;
			Item.useStyle = ItemUseStyleID.DrinkLiquid;
			Item.useAnimation = 17;
			Item.useTime = 17;
			Item.useTurn = true;
			Item.UseSound = SoundID.Item3;
			Item.maxStack = Item.CommonMaxStack;
			Item.consumable = true;
			Item.buffTime = 2 * 60 * 60;
			Item.buffType = ModContent.BuffType<EmpoweringBuff>();
			Item.rare = 2;
			Item.value = Item.sellPrice(silver: 2);
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.BottledWater, 1).AddIngredient(ModContent.ItemType<CaughtDungeonSoulFreed>(), 3).AddIngredient(ItemID.Bone, 10).AddTile(TileID.Bottles).Register();
		}
	}
}
