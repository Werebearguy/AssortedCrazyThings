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

		public override void SafeSetDefaults()
		{
			Item.value = Item.sellPrice(copper: 10);
		}
	}

	[Content(ContentType.AommSupport | ContentType.FriendlyNPCs)]
	public class FairySlimeItem_AoMM : SimplePetItemBase_AoMM<FairySlimeItem>
	{
		public override int BuffType => ModContent.BuffType<FairySlimeBuff_AoMM>();
	}
}
