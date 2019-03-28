using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Gitgud
{
	public class GreenThumb : ModItem
	{
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Green Thumb");
            Tooltip.SetDefault("Consolation Prize"
                + "\n15% reduced damage taken from Plantera"
                + "\nImmunity to poison while Plantera is alive"
                + "\n[c/E180CE:'git gud']");
		}

        public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.Silk);
            item.width = 32;
            item.height = 30;
            item.value = Item.sellPrice(copper: 1);
            item.rare = -1;
            item.maxStack = 1;
            item.accessory = true;
		}

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<GitGudPlayer>(mod).planteraGitgud = true;
        }
    }
}
