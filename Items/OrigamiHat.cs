using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Harblesnargits_Mod_01.Items
{
    [AutoloadEquip(EquipType.Head)]
    public class OrigamiHat : ModItem
	{
		public override void SetStaticDefaults()
		{
			//Tooltip.SetDefault("Find Meatball's Eye and combine the two at a Demon Altar.");
		}
		public override void SetDefaults()
		{
			//item.CloneDefaults(ItemID.BallaHat);
            item.width = 24;
            item.height = 20;
            item.rare = -11;
            item.vanity = true;
		}
	}
}