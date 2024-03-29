using Terraria;

namespace AssortedCrazyThings.Items
{

	[Content(ContentType.Bosses)]
	public class DesiccatedLeather : AssItem
	{
		public override void SetStaticDefaults()
		{
			Item.ResearchUnlockCount = 4;
		}
		public override void SetDefaults()
		{
			Item.maxStack = Item.CommonMaxStack;
			Item.width = 22;
			Item.height = 22;
			Item.rare = 3;
			Item.value = Item.sellPrice(silver: 50);
		}
	}
}
