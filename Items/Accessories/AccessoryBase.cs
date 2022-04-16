using Terraria;

namespace AssortedCrazyThings.Items.Accessories
{
	[Content(ContentType.Accessories)]
	public abstract class AccessoryBase : AssItem
	{
		public sealed override void SetDefaults()
		{
			SafeSetDefaults();

			Item.accessory = true;
			Item.canBePlacedInVanityRegardlessOfConditions = true;
		}

		public virtual void SafeSetDefaults()
		{

		}
	}
}
