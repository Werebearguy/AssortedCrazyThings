using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
	[Content(ContentType.Bosses)]
	public class PetHarvesterItem : SimplePetItemBase
	{
		public override int PetType => ModContent.ProjectileType<PetHarvesterProj>();

		public override int BuffType => ModContent.BuffType<PetHarvesterBuff>();

		public override void SafeSetStaticDefaults()
		{
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(6, 6));
			ItemID.Sets.AnimatesAsSoul[Item.type] = true;
		}

		public override void SafeSetDefaults()
		{
			Item.noUseGraphic = true;
			Item.rare = ItemRarityID.Master;
			Item.master = true;
			Item.value = Item.sellPrice(0, 5);
		}
	}

	[Content(ContentType.AommSupport | ContentType.Bosses)]
	public class PetHarvesterItem_AoMM : SimplePetItemBase_AoMM<PetHarvesterItem>
	{
		public override void EvenSaferSetStaticDefaults()
		{
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(6, 6));
			ItemID.Sets.AnimatesAsSoul[Item.type] = true;
		}

		public override int BuffType => ModContent.BuffType<PetHarvesterBuff_AoMM>();
	}
}
