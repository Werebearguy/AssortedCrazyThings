using Terraria;

namespace AssortedCrazyThings.Items.Accessories.Useful
{
	public class SigilOfEmergency : SigilItemBase
	{
		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Sigil of Emergency");
			Tooltip.SetDefault("Summons a temporary minion to help you upon reaching critical health" +
				"\nIncreases your max number of minions");
		}

		public override void SafeSetDefaults()
		{
			Item.width = 26;
			Item.height = 28;
			Item.value = Item.sellPrice(0, 1, 0, 0);
			Item.rare = -11;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			//4
			if (4 * player.statLife < player.statLifeMax2)
			{
				player.GetModPlayer<AssPlayer>().tempSoulMinion = Item;
			}
			player.maxMinions++;
		}
	}
}
