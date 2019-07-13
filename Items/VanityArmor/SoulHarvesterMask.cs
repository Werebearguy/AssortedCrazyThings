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
            item.width = 30;
            item.height = 22;
            item.rare = -11;
            item.value = 0;
            item.vanity = true;
            item.maxStack = 1;
        }
    }
}
