using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Accessories.Useful
{
    class SigilOfEmergency : AssItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sigil of Emergency");
            Tooltip.SetDefault("Summons a temporary minion to help you upon reaching critical health" +
                "\nIncreases your max number of minions");
            
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(6, 6));
            ItemID.Sets.AnimatesAsSoul[Item.type] = true;

            ItemID.Sets.ItemNoGravity[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 28;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = -11;
            Item.accessory = true;
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
