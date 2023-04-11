using Terraria;

namespace AssortedCrazyThings.Items.Accessories
{
	[Content(ContentType.Accessories)]
	public abstract class AccessoryBase : AssItem
	{
		public sealed override void SetStaticDefaults()
		{
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
