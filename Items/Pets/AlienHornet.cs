using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
    public class AlienHornet : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Vortex Nectar");
            Tooltip.SetDefault("Summons a friendly Alien Hornet to follow you.");
        }

        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.ZephyrFish);
            item.shoot = mod.ProjectileType("AlienHornet");
            item.buffType = mod.BuffType("AlienHornet");
            item.rare = -11;
            item.value = Item.sellPrice(copper: 10);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Nectar, 1);
            recipe.AddIngredient(ItemID.FragmentVortex, 10);
            recipe.AddTile(TileID.DemonAltar);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override void UseStyle(Player player)
        {
            if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
            {
                player.AddBuff(item.buffType, 3600, true);
            }
        }
    }
}
