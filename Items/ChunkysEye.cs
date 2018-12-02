using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

<<<<<<< HEAD:Items/ChunkysEye.cs
namespace AssortedCrazyThings.Items
=======
namespace Harblesnargits_Mod_01.Items
>>>>>>> 2b1e982462604937bebde9cef41c390f73703722:Items/ChunkysEye.cs
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