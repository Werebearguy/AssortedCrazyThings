using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Accessories.Useful
{
	[LegacyName("SigilOfEmergency")]
	public class SigilOfTheBeak : SigilItemBase
	{
		public static readonly int PreHMDamage = 30;
		public static readonly int HMDamage = 50;

		public override void SafeSetDefaults()
		{
			base.SafeSetDefaults();

			Item.width = 26;
			Item.height = 28;
			Item.value = Item.sellPrice(0, 1, 0, 0);
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			AssPlayer assPlayer = player.GetModPlayer<AssPlayer>();
			assPlayer.sigilOfTheBeak = Item;
			assPlayer.sigilOfTheBeakDamage = PreHMDamage;
		}
	}
}
