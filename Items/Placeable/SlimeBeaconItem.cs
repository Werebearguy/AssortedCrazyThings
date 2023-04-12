using AssortedCrazyThings.Tiles;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Placeable
{
	public class SlimeBeaconItem : PlaceableItem<SlimeBeaconTile>
	{
		public static LocalizedText NoMultiplayerText { get; private set; }

		public override void SetStaticDefaults()
		{
			NoMultiplayerText = this.GetLocalization("NoMultiplayer");
		}

		public override void SetDefaults()
		{
			Item.DefaultToPlaceableTile(TileType);
			Item.width = 28;
			Item.height = 28;
			Item.maxStack = Item.CommonMaxStack;
			Item.rare = 1;
			Item.value = Item.buyPrice(0, 10, 0, 0);
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			if (Main.netMode == NetmodeID.MultiplayerClient) tooltips.Add(new TooltipLine(Mod, nameof(NoMultiplayerText), NoMultiplayerText.ToString()));
		}
	}
}
