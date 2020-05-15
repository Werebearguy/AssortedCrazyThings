using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items
{
    public class DesiccatedLeather : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Desiccated Leather");
            Tooltip.SetDefault("'It's dry, sticky, and smells horrible'");
        }
        public override void SetDefaults()
        {
            item.maxStack = 999;
            item.width = 22;
            item.height = 22;
            item.rare = -11;
            item.value = Item.sellPrice(silver: 50);
        }
    }
}
