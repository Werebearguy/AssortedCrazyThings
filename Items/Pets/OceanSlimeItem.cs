using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
	[Content(ContentType.HostileNPCs)]
	public class OceanSlimeItem : SimplePetItemBase
	{
		public override int PetType => ModContent.ProjectileType<OceanSlimeProj>();

		public override int BuffType => ModContent.BuffType<OceanSlimeBuff>();

		public override void SafeSetDefaults()
		{
			Item.value = Item.sellPrice(copper: 10);
		}
	}

	[Content(ContentType.AommSupport | ContentType.HostileNPCs)]
	public class OceanSlimeItem_AoMM : SimplePetItemBase_AoMM<OceanSlimeItem>
	{
		public override int BuffType => ModContent.BuffType<OceanSlimeBuff_AoMM>();
	}
}
