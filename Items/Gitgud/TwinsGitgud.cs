using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Gitgud
{
	public class TwinsGitgud : ModItem
	{
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Metal Contact Lens");
            Tooltip.SetDefault("Consolation Prize"
                + "\n15% reduced damage taken from The Twins"
                + "\nImmunity to 'Cursed Inferno' while The Twins are alive"
                + "\n[c/E180CE:'git gud']");
		}

		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.Silk);
            item.width = 32;
            item.height = 32;
            item.value = Item.sellPrice(copper: 1);
            item.rare = -1;
            item.maxStack = 1;
            item.accessory = true;
		}

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<GitGudPlayer>(mod).twinsGitgud = true;
        }
    }
}
