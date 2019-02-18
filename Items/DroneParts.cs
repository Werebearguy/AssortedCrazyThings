using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items
{
	public class DroneParts : ModItem
	{
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Drone Parts");
			Tooltip.SetDefault("'Maybe the steampunker can help you with that'");
		}
		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.Silk);
			item.rare = -12;
            item.value = Item.sellPrice(silver: 50);
        }
	}
}
