using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
	[Content(ContentType.DroppedPets)]
	public class FairySwarmItem : SimplePetItemBase
	{
		public override int PetType => ModContent.ProjectileType<FairySwarmProj>();

		public override int BuffType => ModContent.BuffType<FairySwarmBuff>();

		public override void SafeSetStaticDefaults()
		{
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 4));
			ItemID.Sets.AnimatesAsSoul[Item.type] = true;
		}

		public override void SafeSetDefaults()
		{
			Item.noUseGraphic = true;
			Item.value = Item.sellPrice(copper: 10);
		}
	}

	[Content(ContentType.AommSupport | ContentType.DroppedPets)]
	public class FairySwarmItem_AoMM : SimplePetItemBase_AoMM<FairySwarmItem>
	{
		public override void EvenSaferSetStaticDefaults()
		{
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 4));
			ItemID.Sets.AnimatesAsSoul[Item.type] = true;
		}

		public override int BuffType => ModContent.BuffType<FairySwarmBuff_AoMM>();
	}
}
