using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items
{
	public class CaughtSoul : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Soul");
			Tooltip.SetDefault("A soul caught by a net.");
		}
		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.Silk);
			item.rare = -11;
		}
	}
}