using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Armor
{
    [AutoloadEquip(EquipType.Body)]
    public class SoulSaviorPlate : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Soul Savior Breastplate");
            Tooltip.SetDefault("Increases minion damage by 10%"
                + "\nIncreases your max number of minions by 2");
        }

        public override void SetDefaults()
        {
            item.width = 28;
            item.height = 22;
            item.value = Item.sellPrice(gold: 3, silver: 70);
            item.rare = 2;
            item.defense = 18;
        }

        public override void UpdateEquip(Player player)
        {
            player.maxMinions += 2;
            player.minionDamage += 0.1f;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType<DesiccatedLeather>(), 1);
            recipe.AddIngredient(ItemID.Ectoplasm, 4);
            recipe.AddIngredient(mod.ItemType<CaughtDungeonSoulFreed>(), 24);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
