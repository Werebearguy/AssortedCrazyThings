using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items
{
    public class MeatballsEye : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Meatball's Eye");
            Tooltip.SetDefault("'Find Chunky's Eye and combine the two at a Demon Altar'");
        }
        public override void SetDefaults()
        {
            item.maxStack = 999;
            item.width = 22;
            item.height = 22;
            item.value = 1000;
            item.rare = -11;
        }
    }
}
