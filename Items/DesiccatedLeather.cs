using Terraria;

namespace AssortedCrazyThings.Items
{

	[Content(ContentType.Bosses)]
	public class DesiccatedLeather : AssItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Desiccated Leather");
			Tooltip.SetDefault("'It's dry, sticky, and smells horrible'");

			Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 4;
		}
		public override void SetDefaults()
		{
			Item.maxStack = 999;
			Item.width = 22;
			Item.height = 22;
			Item.rare = 3;
			Item.value = Item.sellPrice(silver: 50);
		}
	}
}
