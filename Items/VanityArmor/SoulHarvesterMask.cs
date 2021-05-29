using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.VanityArmor
{
    [AutoloadEquip(EquipType.Head)]
    public class SoulHarvesterMask : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Soul Harvester Mask");
        }
        public override void SetDefaults()
        {
            //item.CloneDefaults(ItemID.BallaHat);
            Item.width = 30;
            Item.height = 22;
            Item.rare = -11;
            Item.value = 0;
            Item.vanity = true;
            Item.maxStack = 1;
        }
    }
}
