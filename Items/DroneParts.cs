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
            //TODO tooltip
            Tooltip.SetDefault("'You could make use of this'");
        }

        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.Silk);
            item.rare = -11;
            item.width = 26;
            item.height = 24;
            item.value = Item.sellPrice(silver: 50);
        }
    }
}
