using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.VanityArmor
{
    [AutoloadEquip(EquipType.Head)]
    public class PrettyMask : ModItem
	{
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Pretty Mask");
			Tooltip.SetDefault("Goes well with a red scarf, red gloves, and red boots");
        }
		public override void SetDefaults()
		{
			//item.CloneDefaults(ItemID.BallaHat);
            item.width = 26;
            item.height = 26;
            item.rare = -11;
            item.value = 0;
            item.vanity = true;
            item.maxStack = 1;
        }
	}
}