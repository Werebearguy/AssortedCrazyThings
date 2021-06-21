using Terraria.ModLoader;

namespace AssortedCrazyThings.Items
{
    [Autoload]
    public class MeatballsEye : AssItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Meatball's Eye");
            Tooltip.SetDefault("'Find Chunky's Eye and combine the two at a Demon Altar'");
        }
        public override void SetDefaults()
        {
            Item.maxStack = 999;
            Item.width = 22;
            Item.height = 22;
            Item.value = 1000;
            Item.rare = -11;
        }
    }
}
