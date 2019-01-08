using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.VanityArmor
{
    [AutoloadEquip(EquipType.Head)]
    public class SoulHarvesterMask : ModItem
	{
		public override void SetStaticDefaults()
		{
            DisplayName.SetDefault("Soul Harvester Mask");
			//Tooltip.SetDefault("If you're reading this, the sprite is not final.");
		}
		public override void SetDefaults()
		{
			//item.CloneDefaults(ItemID.BallaHat);
            item.width = 28;
            item.height = 20;
            item.rare = -11;
            item.vanity = true;
            item.maxStack = 1;
		}
	}
}