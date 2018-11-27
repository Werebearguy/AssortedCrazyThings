using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Harblesnargits_Mod_01.Items.Pets
{
	public class pet_cuteslime_09 : ModItem
	{
		public override void SetStaticDefaults()
			{
				DisplayName.SetDefault("Bottled Christmas Slime");
				Tooltip.SetDefault("Summons a friendly Cute Christmas Slime to follow you.");
			}
		public override void SetDefaults()
			{
				item.CloneDefaults(ItemID.LizardEgg);
				item.shoot = mod.ProjectileType("pet_cuteslime_09");
				item.buffType = mod.BuffType("pet_cuteslime_09");
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