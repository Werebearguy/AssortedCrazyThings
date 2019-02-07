using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
    public class DocileDemonEye : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Docile Demon Eye");
            Tooltip.SetDefault("Summons a docile Demon Eye to follow you"
                + "\nChange its appearance with a Demon Eye Contact Case");
        }

        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.ZephyrFish);
            item.width = 34;
            item.height = 22;
            item.shoot = mod.ProjectileType("DocileDemonEyeProj");
            item.buffType = mod.BuffType("DocileDemonEyeBuff");
            item.rare = -11;
            item.value = Item.sellPrice(silver: 10);
        }

        public override void AddRecipes()
        {
            //regular recipe, dont delete
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.BlackLens, 1);
            recipe.AddIngredient(ItemID.Lens, 1);
            recipe.AddTile(TileID.DemonAltar);
            recipe.SetResult(this);
            recipe.AddRecipe();


            //legacy recipes
            ModRecipe recipe2 = new ModRecipe(mod);
            recipe2.AddIngredient(mod.ItemType<DocileDemonEyeGreen>());
            recipe2.AddTile(TileID.DemonAltar);
            recipe2.SetResult(this);
            recipe2.AddRecipe();

            ModRecipe recipe3 = new ModRecipe(mod);
            recipe3.AddIngredient(mod.ItemType<DocileDemonEyePurple>());
            recipe3.AddTile(TileID.DemonAltar);
            recipe3.SetResult(this);
            recipe3.AddRecipe();

            ModRecipe recipe4 = new ModRecipe(mod);
            recipe4.AddIngredient(mod.ItemType<DocileDemonEyeRed>());
            recipe4.AddTile(TileID.DemonAltar);
            recipe4.SetResult(this);
            recipe4.AddRecipe();

            ModRecipe recipe5 = new ModRecipe(mod);
            recipe5.AddIngredient(mod.ItemType<DocileFracturedEyeGreen>());
            recipe5.AddTile(TileID.DemonAltar);
            recipe5.SetResult(this);
            recipe5.AddRecipe();

            ModRecipe recipe6 = new ModRecipe(mod);
            recipe6.AddIngredient(mod.ItemType<DocileFracturedEyePurple>());
            recipe6.AddTile(TileID.DemonAltar);
            recipe6.SetResult(this);
            recipe6.AddRecipe();

            ModRecipe recipe7 = new ModRecipe(mod);
            recipe7.AddIngredient(mod.ItemType<DocileFracturedEyeRed>());
            recipe7.AddTile(TileID.DemonAltar);
            recipe7.SetResult(this);
            recipe7.AddRecipe();

            ModRecipe recipe8 = new ModRecipe(mod);
            recipe8.AddIngredient(mod.ItemType<DocileMechanicalEyeGreen>());
            recipe8.AddTile(TileID.DemonAltar);
            recipe8.SetResult(this);
            recipe8.AddRecipe();

            ModRecipe recipe9 = new ModRecipe(mod);
            recipe9.AddIngredient(mod.ItemType<DocileMechanicalEyePurple>());
            recipe9.AddTile(TileID.DemonAltar);
            recipe9.SetResult(this);
            recipe9.AddRecipe();

            ModRecipe recipe0 = new ModRecipe(mod);
            recipe0.AddIngredient(mod.ItemType<DocileMechanicalEyeRed>());
            recipe0.AddTile(TileID.DemonAltar);
            recipe0.SetResult(this);
            recipe0.AddRecipe();

            ModRecipe recipei = new ModRecipe(mod);
            recipei.AddIngredient(mod.ItemType<DocileMechanicalLaserEyeGreen>());
            recipei.AddTile(TileID.DemonAltar);
            recipei.SetResult(this);
            recipei.AddRecipe();

            ModRecipe recipeo = new ModRecipe(mod);
            recipeo.AddIngredient(mod.ItemType<DocileMechanicalLaserEyePurple>());
            recipeo.AddTile(TileID.DemonAltar);
            recipeo.SetResult(this);
            recipeo.AddRecipe();

            ModRecipe recipeu = new ModRecipe(mod);
            recipeu.AddIngredient(mod.ItemType<DocileMechanicalLaserEyeRed>());
            recipeu.AddTile(TileID.DemonAltar);
            recipeu.SetResult(this);
            recipeu.AddRecipe();
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
