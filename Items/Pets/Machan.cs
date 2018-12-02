using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Harblesnargits_Mod_01.Items.Pets
{
	public class Machan : ModItem
		{
			public override void SetStaticDefaults()
				{
					DisplayName.SetDefault("Cute Stuffed Dragon");
					Tooltip.SetDefault("Summons a cute dragon to follow you.");
				}
			public override void SetDefaults()
				{
					item.CloneDefaults(ItemID.ZephyrFish);
					item.shoot = mod.ProjectileType("Machan");
					item.buffType = mod.BuffType("Machan");
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