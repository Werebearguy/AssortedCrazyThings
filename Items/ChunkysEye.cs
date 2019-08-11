using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items
{
    public class ChunkysEye : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Chunky's Eye");
            Tooltip.SetDefault("'Find Meatball's Eye and combine the two at a Demon Altar'");
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
