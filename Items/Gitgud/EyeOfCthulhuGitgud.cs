using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Gitgud
{
	public class EyeOfCthulhuGitgud : ModItem
	{
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Broken Lens");
            Tooltip.SetDefault("Consolation Prize"
                + "\n15% reduced damage taken from Eye of Cthulhu"
                + "\n[c/E180CE:'git gud']");
		}

		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.Silk);
            item.width = 16;
            item.height = 20;
            item.value = Item.sellPrice(copper: 1);
            item.rare = -1;
            item.maxStack = 1;
            item.accessory = true;
		}

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<GitGudPlayer>(mod).eyeOfCthulhuGitgud = true;
        }
    }
}
