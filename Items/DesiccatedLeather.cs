using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items
{
	public class DesiccatedLeather : ModItem
	{
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Desiccated Leather");
			Tooltip.SetDefault("It's dry, sticky, and smells horrible.");
		}
		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.Silk);
			item.rare = -11;
            item.value = Item.sellPrice(silver: 50);
        }
	}
}
