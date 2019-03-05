using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items
{
    public class TinyFrogCrown : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tiny Prince's Tiny Crown");
            Tooltip.SetDefault("'Give your Lifelike Mechanical Frog a Princelike Tiny Crown'");
        }

        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.Silk);
            item.width = 18;
            item.height = 16;
            item.maxStack = 1;
            item.rare = -11;
            item.useAnimation = 16;
            item.useTime = 16;
            item.UseSound = SoundID.Item1;
            item.consumable = false;
            item.value = Item.sellPrice(silver:12);
        }

        public override void AddRecipes()
        {
            //migration recipes in VanitySlector
        }
    }
}
