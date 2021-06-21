using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Accessories.Useful
{
    [Autoload]
    [AutoloadEquip(EquipType.Wings)]
    public class HarvesterWings : AssItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Harvester Wings");
            Tooltip.SetDefault("Allows flight and slow fall" +
            "\nIncreases your max number of minions" +
            "\n5% increased summon damage");

            ArmorIDs.Wing.Sets.Stats[Item.wingSlot] = new WingStats(95, 6.5f, 1.35f);
        }

        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 28;
            Item.value = Item.sellPrice(gold: 1);
            Item.rare = -11;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage(DamageClass.Summon) += 0.05f;
            player.maxMinions++;
        }

        public override void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising, ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
        {
            ascentWhenFalling = 0.25f;
            ascentWhenRising = 0.15f;
            maxCanAscendMultiplier = 0.4f;
            maxAscentMultiplier = 2f;
            constantAscend = 0.12f;
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.Bone, 25).AddIngredient(ItemID.SoulofFlight, 10).AddIngredient(ModContent.ItemType<CaughtDungeonSoulFreed>(), 10).AddIngredient(ModContent.ItemType<DesiccatedLeather>(), 2).AddTile(TileID.MythrilAnvil).Register();
        }
    }
}
