using Terraria;

namespace AssortedCrazyThings.Items.Accessories
{
	[Content(ContentType.Accessories)]
	public abstract class AccessoryBase : AssItem
	{
		public sealed override void SetStaticDefaults()
		{
			Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1; //All accessories have 1

			SafeSetStaticDefaults();
		}

		public virtual void SafeSetStaticDefaults()
		{

		}

		public sealed override void SetDefaults()
		{
			SafeSetDefaults();

			Item.accessory = true;
		}

		public virtual void SafeSetDefaults()
		{

		}
	}
}
