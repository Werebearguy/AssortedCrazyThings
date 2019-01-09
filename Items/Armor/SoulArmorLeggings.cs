using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Armor
{
    [AutoloadEquip(EquipType.Legs)]
    public class SoulArmorLeggings : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Soul Savior's Garb");
            Tooltip.SetDefault("Soul Savior Garment"
                + "\n5% increased movement speed");
        }

        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 18;
            item.value = Item.sellPrice(silver: 60);
            item.rare = -11;
            item.defense = 6; //1 more than necro leggings
        }

        public override void UpdateEquip(Player player)
        {
            player.moveSpeed += 0.05f;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType<CaughtDungeonSoulAwakened>(), 10);
            recipe.AddIngredient(mod.ItemType<DesiccatedLeather>(), 10);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}