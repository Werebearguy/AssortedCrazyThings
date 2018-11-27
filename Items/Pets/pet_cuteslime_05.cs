using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Harblesnargits_Mod_01.Items.Pets
{
	public class pet_cuteslime_05 : ModItem
		{
			public override void SetStaticDefaults()
				{
					DisplayName.SetDefault("Bottled Slime");
					Tooltip.SetDefault("Summons a friendly Cute Slime to follow you.");
				}
			public override void SetDefaults()
				{
					item.CloneDefaults(ItemID.LizardEgg);
					item.shoot = mod.ProjectileType("pet_cuteslime_05");
					item.buffType = mod.BuffType("pet_cuteslime_05");
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