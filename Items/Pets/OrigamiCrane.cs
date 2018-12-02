using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
	public class OrigamiCrane : ModItem
		{
			public override void SetStaticDefaults()
				{
					DisplayName.SetDefault("Origami Crane");
					Tooltip.SetDefault("Summons an Origami Crane that follows you.");
				}
			public override void SetDefaults()
				{
					item.CloneDefaults(ItemID.ZephyrFish);
					item.shoot = mod.ProjectileType("OrigamiCrane");
					item.buffType = mod.BuffType("OrigamiCrane");
					item.rare = -11;
				}
			public override void UseStyle(Player player)
				{
					if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
						{
							player.AddBuff(item.buffType, 3600, true);
						}
				}
		}
}