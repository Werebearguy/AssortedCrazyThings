using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Harblesnargits_Mod_01.Items.Pets
{
	public class SmallMegalodon : ModItem
		{
			public override void SetStaticDefaults()
				{
					DisplayName.SetDefault("Megalodon Tooth");
					Tooltip.SetDefault("Summons a friendly Small Megalodon that flies with you.");
				}
			public override void SetDefaults()
				{
					item.CloneDefaults(ItemID.ZephyrFish);
					item.shoot = mod.ProjectileType("SmallMegalodon");
					item.buffType = mod.BuffType("SmallMegalodon");
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