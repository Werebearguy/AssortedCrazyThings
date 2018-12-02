using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Harblesnargits_Mod_01.Items
{
	public class ChunkysEye : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Chunky's Eye");
			Tooltip.SetDefault("Find Meatball's Eye and combine the two at a Demon Altar.");
		}
		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.Silk);
			item.rare = -11;
		}
	}
}