using Terraria.GameContent.Creative;
using Terraria.ID;

namespace AssortedCrazyThings.Items
{
	[Content(ContentType.Bosses)]
	public class AntiqueKey : AssItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Antique Key");
			Tooltip.SetDefault("'An old key that looks to fit the lock on the nearby cage...'");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.GoldenKey);
			Item.width = 18;
			Item.height = 28;
			Item.rare = 3;
		}
	}
}
