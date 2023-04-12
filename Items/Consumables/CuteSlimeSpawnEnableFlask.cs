using AssortedCrazyThings.Buffs;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Consumables
{
	[Content(ContentType.CuteSlimes)]
	public class CuteSlimeSpawnEnableFlask : AssItem
	{
		public override void SetStaticDefaults()
		{
			ItemID.Sets.DrinkParticleColors[Item.type] = new Color[3] {
				new Color(13, 106, 137),
				new Color(10, 176, 230),
				new Color(146, 229, 255)
			};

			Item.ResearchUnlockCount = 10;
		}

		//TODO 1.4.4 use override + config check
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			string tooltip = "You will see Cute Slimes more often for a short time";
			if (ContentConfig.Instance.CuteSlimesPotionOnly)
			{
				tooltip = "Allows you to see Cute Slimes for a short time";
			}
			tooltips.Add(new TooltipLine(Mod, "Tooltip", tooltip));
		}

		public override void SetDefaults()
		{
			//item.CloneDefaults(ItemID.Silk);
			Item.width = 20;
			Item.height = 28;
			Item.useStyle = ItemUseStyleID.DrinkLiquid;
			Item.useAnimation = 17;
			Item.useTime = 17;
			Item.useTurn = true;
			Item.UseSound = SoundID.Item3;
			Item.maxStack = Item.CommonMaxStack;
			Item.consumable = true;
			Item.buffTime = 5 * 60 * 60;
			Item.buffType = ModContent.BuffType<CuteSlimeSpawnEnableBuff>();
			Item.rare = 0;
			Item.value = Item.sellPrice(copper: 20);
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.Ale, 1).AddIngredient(ItemID.Gel, 1).AddTile(TileID.Kegs).Register();
		}
	}
}
