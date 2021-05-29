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
            Item.maxStack = 999;
            Item.rare = -11;
            Item.width = 26;
            Item.height = 24;
            Item.value = Item.sellPrice(silver: 50);
        }
    }
}
