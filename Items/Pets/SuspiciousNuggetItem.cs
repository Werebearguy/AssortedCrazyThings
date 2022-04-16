using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.Enums;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
	public class SuspiciousNuggetItem : SimplePetItemBase
	{
		public override int PetType => ModContent.ProjectileType<SuspiciousNuggetProj>();

		public override int BuffType => ModContent.BuffType<SuspiciousNuggetBuff>();

		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Suspicious Nugget");
			Tooltip.SetDefault("Summons a Suspicious Nugget to follow you");
		}

		public override void SafeSetDefaults()
		{
			Item.SetShopValues(ItemRarityColor.Orange3, Item.buyPrice(50)); //10 times more expensive then cube
		}
	}
}
