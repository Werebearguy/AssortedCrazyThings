using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
	[Content(ContentType.FriendlyNPCs)]
	public class HornedSlimeItem : SimplePetItemBase
	{
		public override int PetType => ModContent.ProjectileType<HornedSlimeProj>();

		public override int BuffType => ModContent.BuffType<HornedSlimeBuff>();

		public override void SafeSetDefaults()
		{
			Item.value = Item.sellPrice(copper: 10);
		}
	}

	[Content(ContentType.AommSupport | ContentType.FriendlyNPCs)]
	public class HornedSlimeItem_AoMM : SimplePetItemBase_AoMM<HornedSlimeItem>
	{
		public override int BuffType => ModContent.BuffType<HornedSlimeBuff_AoMM>();
	}
}
