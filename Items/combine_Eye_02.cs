using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Harblesnargits_Mod_01.Items
{
	public class combine_Eye_02 : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Meatball's Eye");
			Tooltip.SetDefault("Find his brother and reunite them.");
		}
		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.Silk);
			item.rare = -11;
		}
	}
}