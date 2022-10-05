using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
	[Content(ContentType.FriendlyNPCs)]
	public class JoyousSlimeItem : SimplePetItemBase
	{
		public override int PetType => ModContent.ProjectileType<JoyousSlimeProj>();

		public override int BuffType => ModContent.BuffType<JoyousSlimeBuff>();

		public override void SafeSetDefaults()
		{
			Item.value = Item.sellPrice(copper: 10);
		}
	}

	[Content(ContentType.AommSupport | ContentType.FriendlyNPCs)]
	public class JoyousSlimeItem_AoMM : SimplePetItemBase_AoMM<JoyousSlimeItem>
	{
		public override int BuffType => ModContent.BuffType<JoyousSlimeBuff_AoMM>();
	}
}
