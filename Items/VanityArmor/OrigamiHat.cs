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
            item.width = 24;
            item.height = 20;
            item.rare = -11;
            item.value = 0;
            item.vanity = true;
            item.maxStack = 1;
        }
	}
}
