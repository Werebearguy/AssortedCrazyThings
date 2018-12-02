using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

<<<<<<< HEAD:Items/MeatballsEye.cs
namespace AssortedCrazyThings.Items
=======
namespace Harblesnargits_Mod_01.Items
>>>>>>> 2b1e982462604937bebde9cef41c390f73703722:Items/MeatballsEye.cs
{
	public class MeatballsEye : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Meatball's Eye");
			Tooltip.SetDefault("Find Chunky's Eye and combine the two at a Demon Altar.");
		}
		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.Silk);
			item.rare = -11;
		}
	}
}