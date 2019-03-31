using AssortedCrazyThings.Buffs;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
    public class DrumstickElementalItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Magical Drumstick");
            Tooltip.SetDefault("Summons a delicious Drumstick Elemental to follow you");
        }

        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.ZephyrFish);
            item.shoot = mod.ProjectileType<DrumstickElementalProj>();
            item.buffType = mod.BuffType<DrumstickElementalBuff>();
            item.rare = -11;
            item.value = Item.sellPrice(silver: 7, copper: 50);
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
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Duck);
            recipe.AddTile(TileID.CookingPots);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
