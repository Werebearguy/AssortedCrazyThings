using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items
{
    public class DemonEyeContactCase : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Demon Eye Contact Case");
            Tooltip.SetDefault("Use to switch your docile Demon Eye pet's appearance");
        }

        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.Silk);
            item.width = 28;
            item.height = 28;
            item.maxStack = 1;
            item.rare = -11;
            item.useAnimation = 16;
            item.useTime = 16;
            item.UseSound = SoundID.Item1;
            item.consumable = false;
            item.value = Item.sellPrice(silver: 11);
        }

        public override void AddRecipes()
        {
            //migration recipes in VanitySlector
        }
    }
}
