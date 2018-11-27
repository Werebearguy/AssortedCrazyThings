using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Harblesnargits_Mod_01.Items.Pets
{
	public class pet_harpy_01 : ModItem
		{
			public override void SetStaticDefaults()
				{
					DisplayName.SetDefault("PET HARPY ITEM 01");
					Tooltip.SetDefault("Summons a friendly Harpy to follow you.");
				}
			public override void SetDefaults()
				{
					item.CloneDefaults(ItemID.ZephyrFish);
					item.shoot = mod.ProjectileType("pet_harpy_01");
					item.buffType = mod.BuffType("pet_harpy_01");
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