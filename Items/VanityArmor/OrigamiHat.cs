using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.VanityArmor
{
    [AutoloadEquip(EquipType.Head)]
    public class OrigamiHat : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Origami Hat");
        }
        public override void SetDefaults()
        {
            //item.CloneDefaults(ItemID.BallaHat);
            Item.width = 24;
            Item.height = 20;
            Item.rare = -11;
            Item.value = 0;
            Item.vanity = true;
            Item.maxStack = 1;
        }
    }
}
