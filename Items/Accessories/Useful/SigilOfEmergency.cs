using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Accessories.Useful
{
    class SigilOfEmergency : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sigil of Emergency");
            Tooltip.SetDefault("Teleports you home when health is dangerously low");
        }

        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 22;
            item.value = Item.sellPrice(0, 1, 0, 0);
            item.rare = -11;
            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (1.1f * player.statLife < player.statLifeMax)
            {
                player.GetModPlayer<AssPlayer>().tempSoulMinion = true;
            }
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.RecallPotion, 5);
            recipe.AddIngredient(mod.ItemType<CaughtDungeonSoulAwakened>(), 10);
            recipe.AddIngredient(ItemID.Bone, 50);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
