using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.VanityArmor
{

    [Content(ContentType.Boss)]
    [AutoloadEquip(EquipType.Head)]
    public class SoulHarvesterMask : AssItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Soul Harvester Mask");
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 28;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(silver: 75);
            Item.vanity = true;
            Item.maxStack = 1;
        }
    }
}
