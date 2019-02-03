using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Armor
{
    [AutoloadEquip(EquipType.Legs)]
    public class SoulSaviorRobe : ModItem
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
            item.width = 24;
            item.height = 14;
            item.value = Item.sellPrice(gold: 2, silver: 60);
            item.rare = -11;
            item.defense = 12;
        }

        public override void UpdateEquip(Player player)
        {
            player.maxMinions++;
            player.minionDamage += 0.1f;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType<DesiccatedLeather>(), 1);
            recipe.AddIngredient(ItemID.Ectoplasm, 3);
            recipe.AddIngredient(mod.ItemType<CaughtDungeonSoulFreed>(), 12);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
