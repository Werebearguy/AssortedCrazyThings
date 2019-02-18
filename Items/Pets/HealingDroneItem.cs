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
            Tooltip.SetDefault("Summons a Healing Drone to follow you and heal when injured");
        }

        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.ZephyrFish);
            item.shoot = mod.ProjectileType("HealingDroneProj");
            item.buffType = mod.BuffType("HealingDroneBuff");
            item.width = 30;
            item.height = 28;
            item.rare = -11;
            item.value = Item.sellPrice(copper: 10); //TODO based on recipe
        }

        public override void UseStyle(Player player)
        {
            if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
            {
                player.AddBuff(item.buffType, 3600, true);
            }
        }

        //TODO
        public override void AddRecipes()
        {
            //ModRecipe recipe = new ModRecipe(mod);
            //recipe.AddIngredient(ItemID.Frog, 1);
            //recipe.AddTile(TileID.Anvils);
            //recipe.SetResult(this);
            //recipe.AddRecipe();
        }
    }
}
