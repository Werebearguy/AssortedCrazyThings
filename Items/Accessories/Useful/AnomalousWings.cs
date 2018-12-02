using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Accessories.Useful
{
	[AutoloadEquip(EquipType.Wings)]
	public class AnomalousWings : ModItem
		{
			public override void SetStaticDefaults()
				{
					DisplayName.SetDefault("Anomalous Wings");
					Tooltip.SetDefault("Allows slowfall"
						+ "\nAllows quick travel in water");
				}
			public override void SetDefaults()
				{
					item.width = 22;
					item.height = 28;
					item.value = 0;
					item.rare = -11;
					item.accessory = true;
				}
			public override void UpdateAccessory(Player player, bool hideVisual)
				{
					player.wingTimeMax = 180;
					player.ignoreWater = true;
				}
			public override void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising,
				ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
				{
					ascentWhenFalling = 0.85f;
					ascentWhenRising = 0.05f;
					maxCanAscendMultiplier = 0f;
					maxAscentMultiplier = 0.001f;
					constantAscend = 0f;
				}
			public override void HorizontalWingSpeeds(Player player, ref float speed, ref float acceleration)
				{
					speed = 4f;
					acceleration *= 0.5f;
				}
		}
}