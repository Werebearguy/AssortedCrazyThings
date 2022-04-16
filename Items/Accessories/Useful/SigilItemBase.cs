using Terraria;
using Terraria.DataStructures;
using Terraria.ID;

namespace AssortedCrazyThings.Items.Accessories.Useful
{
	//TODO extend functionality regarding cooldown tooltips
	[Content(ContentType.Bosses)]
	public abstract class SigilItemBase : AccessoryBase
	{
		public sealed override void SetStaticDefaults()
		{
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(6, 6));
			ItemID.Sets.AnimatesAsSoul[Item.type] = true;

			ItemID.Sets.ItemNoGravity[Item.type] = true;

			SafeSetStaticDefaults();
		}

		public virtual void SafeSetStaticDefaults()
		{

		}

		public override void SafeSetDefaults()
		{
			Item.width = 26;
			Item.height = 28;
			Item.value = Item.sellPrice(0, 1, 0, 0);
			Item.rare = -11;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			//4
			if (4 * player.statLife < player.statLifeMax2)
			{
				player.GetModPlayer<AssPlayer>().tempSoulMinion = Item;
			}
			player.maxMinions++;
		}
	}
}
