using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.VanityArmor
{
	[Content(ContentType.Bosses)]
	[AutoloadEquip(EquipType.Head)]
	public class SoulHarvesterMask : AssItem
	{
		public override void SetDefaults()
		{
			Item.width = 32;
			Item.height = 28;
			Item.rare = 1;
			Item.value = Item.sellPrice(silver: 75);
			Item.vanity = true;
		}
	}
}
