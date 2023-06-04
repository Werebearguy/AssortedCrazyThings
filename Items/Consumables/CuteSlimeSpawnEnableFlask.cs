using AssortedCrazyThings.Buffs;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Consumables
{
	[Content(ContentType.CuteSlimes)]
	public class CuteSlimeSpawnEnableFlask : AssItem
	{
		public static LocalizedText SeeMoreOftenText { get; private set; }
		public static LocalizedText SeeText { get; private set; }

		public override LocalizedText Tooltip => LocalizedText.Empty;

		public override void SetStaticDefaults()
		{
			SeeMoreOftenText = this.GetLocalization("SeeMoreOften");
			SeeText = this.GetLocalization("See");

			ItemID.Sets.DrinkParticleColors[Item.type] = new Color[3] {
				new Color(13, 106, 137),
				new Color(10, 176, 230),
				new Color(146, 229, 255)
			};

			Item.ResearchUnlockCount = 10;
		}

		public override void SetDefaults()
		{
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

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			tooltips.Add(new TooltipLine(Mod, "Tooltip", (ContentConfig.Instance.CuteSlimesPotionOnly ? SeeText : SeeMoreOftenText).ToString()));
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.Ale, 1).AddIngredient(ItemID.Gel, 1).AddTile(TileID.Kegs).Register();
		}
	}
}
