using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
    public class DocileDemonEyeGreen : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Unconscious Demon Eye");
            Tooltip.SetDefault("Summons a docile green Demon Eye to follow you."
                + "\nLegacy Appearance, use 'Docile Demon Eye' instead."
                + "\nThis version of the pet will be discontinued in the next update.");
        }

        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.ZephyrFish);
            item.shoot = mod.ProjectileType("DocileDemonEyeGreen");
            item.buffType = mod.BuffType("DocileDemonEyeGreen");
            item.rare = -11;
            item.value = Item.sellPrice(silver: 10);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.BlackLens, 1);
            recipe.AddIngredient(ItemID.Lens, 2);
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
