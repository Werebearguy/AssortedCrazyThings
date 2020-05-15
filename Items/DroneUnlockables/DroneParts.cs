using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.DroneUnlockables
{
    public class DroneParts : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Drone Parts");
            Tooltip.SetDefault("'These parts could be repurposed...'");
        }

        public override void SetDefaults()
        {
            item.maxStack = 999;
            item.rare = -11;
            item.width = 26;
            item.height = 24;
            item.value = Item.sellPrice(silver: 50);
        }
    }
}
