using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
	public class CuteGastropod : ModItem
		{
			public override void SetStaticDefaults()
				{
					DisplayName.SetDefault("Mysterious Pod");
					Tooltip.SetDefault("Summons a friendly Cute Gastropod to follow you.");
				}
			public override void SetDefaults()
				{
					item.CloneDefaults(ItemID.ZephyrFish);
					item.shoot = mod.ProjectileType("CuteGastropod");
					item.buffType = mod.BuffType("CuteGastropod");
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