using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
    public class HealingDroneItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Healing Drone");
            Tooltip.SetDefault("Legacy Item, discontinued"
                + "\nCraft the item into the non-legacy version");
        }

        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.ZephyrFish);
            item.shoot = mod.ProjectileType("HealingDroneProj");
            item.buffType = mod.BuffType("HealingDroneBuff");
            item.width = 30;
            item.height = 28;
            item.rare = -11;
            item.value = Item.sellPrice(gold: 3, silver: 75);
        }

        public override void UseStyle(Player player)
        {
            if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
            {
                player.AddBuff(item.buffType, 3600, true);
            }
        }

        public override void AddRecipes()
        {
            //TODO Migration recipe
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType<DroneParts>(), 1);
            recipe.AddIngredient(ItemID.WireBulb, 1);
            recipe.AddIngredient(ItemID.LifeCrystal, 1);
            recipe.AddIngredient(ItemID.Wire, 5);
            recipe.AddIngredient(ItemID.Cog, 5);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
