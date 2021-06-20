using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Armor
{
    [AutoloadEquip(EquipType.Legs)]
    public class SoulSaviorRobe : AssItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Soul Savior Robe");
            Tooltip.SetDefault("Increases minion damage by 10%"
                + "\nIncreases your max number of minions");
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 14;
            Item.value = Item.sellPrice(gold: 2, silver: 60);
            Item.rare = -11;
            Item.defense = 12;
        }

        public override void UpdateEquip(Player player)
        {
            player.maxMinions++;
            player.GetDamage(DamageClass.Summon) += 0.1f;
        }

        public override void EquipFrameEffects(Player player, EquipType type)
        {
            player.shoe = 0;
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<DesiccatedLeather>(), 1).AddIngredient(ItemID.Ectoplasm, 3).AddIngredient(ModContent.ItemType<CaughtDungeonSoulFreed>(), 12).AddTile(TileID.MythrilAnvil).Register();
        }
    }
}
