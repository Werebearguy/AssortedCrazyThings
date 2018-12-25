using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using AssortedCrazyThings.Buffs;
using AssortedCrazyThings.Projectiles.Pets;

namespace AssortedCrazyThings.Items.Pets
{
	public class CuteSlimeRainbow : ModItem
	{
		public override void SetStaticDefaults()
			{
				DisplayName.SetDefault("Bottled Slime");
				Tooltip.SetDefault("Summons a friendly Cute Slime to follow you.");
			}
		public override void SetDefaults()
			{
				item.CloneDefaults(ItemID.LizardEgg);
				item.shoot = mod.ProjectileType<CuteSlimeRainbowPet>();

                item.buffType = mod.BuffType<CuteSlimeRainbowBuff>();

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