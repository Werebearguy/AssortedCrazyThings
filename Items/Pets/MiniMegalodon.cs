using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Harblesnargits_Mod_01.Items.Pets
{
	public class MiniMegalodon : ModItem
		{
			public override void SetStaticDefaults()
				{
					DisplayName.SetDefault("Megalodon Tooth");
					Tooltip.SetDefault("Summons a friendly Mini Megalodon that flies with you.");
				}
			public override void SetDefaults()
				{
					item.CloneDefaults(ItemID.ZephyrFish);
					item.shoot = mod.ProjectileType("MiniMegalodon");
					item.buffType = mod.BuffType("MiniMegalodon");
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