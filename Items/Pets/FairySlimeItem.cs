using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
	[Content(ContentType.FriendlyNPCs)]
	public class FairySlimeItem : SimplePetItemBase
	{
		public override int PetType => ModContent.ProjectileType<FairySlimeProj>();

		public override int BuffType => ModContent.BuffType<FairySlimeBuff>();

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bottled Fairy Slime");
			Tooltip.SetDefault("Summons a friendly Fairy Slime to follow you");
		}

		public override void SafeSetDefaults()
		{
			Item.rare = -11;
			Item.value = Item.sellPrice(copper: 10);
		}
	}
}
