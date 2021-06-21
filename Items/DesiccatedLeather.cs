using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items
{

    [Content(ContentType.Boss)]
    public class DesiccatedLeather : AssItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Desiccated Leather");
            Tooltip.SetDefault("'It's dry, sticky, and smells horrible'");
        }
        public override void SetDefaults()
        {
            Item.maxStack = 999;
            Item.width = 22;
            Item.height = 22;
            Item.rare = -11;
            Item.value = Item.sellPrice(silver: 50);
        }
    }
}
