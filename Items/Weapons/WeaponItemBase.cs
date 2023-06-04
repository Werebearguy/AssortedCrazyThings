namespace AssortedCrazyThings.Items.Weapons
{
	[Content(ContentType.Weapons)]
	public abstract class WeaponItemBase : AssItem
	{
		public sealed override void SetStaticDefaults()
		{
			SafeSetStaticDefaults();
		}

		public virtual void SafeSetStaticDefaults()
		{

		}
	}
}
