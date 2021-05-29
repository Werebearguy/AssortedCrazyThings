using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Accessories.Useful
{
    [AutoloadEquip(EquipType.Wings)]
    public class AnomalousWings : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Anomalous Wings");
            Tooltip.SetDefault("Allows slowfall"
                + "\nAllows quick travel in water");
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 28;
            Item.value = Item.sellPrice(gold: 1);
            Item.rare = -11;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (player.wet || player.honeyWet)
            {
                player.wingTimeMax = int.MaxValue; //180
                player.ignoreWater = true;
                player.accFlipper = true;
            }
        }

        //that thing is straight up not working
        public override void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising,
                ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
        {
            if (player.wet || player.honeyWet)
            {
                //ascentWhenFalling = 0.85f;
                //ascentWhenRising = 0.15f;
                //maxCanAscendMultiplier = 1f;
                //maxAscentMultiplier = 0.5f;
                //constantAscend = 1f;
            }
            else
            {
                ascentWhenFalling = 0.85f;
                ascentWhenRising = 1f;
                maxCanAscendMultiplier = 0f;
                maxAscentMultiplier = 0.001f;
                constantAscend = 1f;
            }

        }

        //that thing is straight up not working
        public override void HorizontalWingSpeeds(Player player, ref float speed, ref float acceleration)
        {
            //speed = 4f;
            //acceleration *= 0.5f;
        }
    }
}
