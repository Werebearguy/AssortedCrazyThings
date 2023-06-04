using Terraria;

namespace AssortedCrazyThings.Items.DroneUnlockables
{
	[Content(ContentType.Weapons)]
	public class DroneParts : AssItem
	{
		public override void SetDefaults()
		{
			Item.maxStack = Item.CommonMaxStack;
			Item.rare = 4;
			Item.width = 26;
			Item.height = 24;
			Item.value = Item.sellPrice(silver: 50);
		}
	}
}
