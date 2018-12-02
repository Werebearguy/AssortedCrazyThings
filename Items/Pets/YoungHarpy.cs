using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
	public class YoungHarpy : ModItem
		{
			public override void SetStaticDefaults()
				{
					DisplayName.SetDefault("Clump of Down Feathers");
					Tooltip.SetDefault("Summons a friendly Harpy to follow you.");
				}
			public override void SetDefaults()
				{
					item.CloneDefaults(ItemID.ZephyrFish);
					item.shoot = mod.ProjectileType("YoungHarpy");
					item.buffType = mod.BuffType("YoungHarpy");
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