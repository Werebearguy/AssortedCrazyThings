using Terraria;
using Terraria.DataStructures;
using Terraria.ID;

namespace AssortedCrazyThings.Items.Accessories.Useful
{
	[Content(ContentType.Bosses)]
	public abstract class SigilItemBase : AccessoryBase
	{
		public sealed override void SafeSetStaticDefaults()
		{
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(6, 6));
			ItemID.Sets.AnimatesAsSoul[Item.type] = true;

			EvenSaferSetStaticDefaults();
		}

		public virtual void EvenSaferSetStaticDefaults()
		{

		}

		public override void SafeSetDefaults()
		{
			Item.width = 26;
			Item.height = 28;
			Item.value = Item.sellPrice(0, 1, 0, 0);
			Item.rare = 3;
		}
	}
}
