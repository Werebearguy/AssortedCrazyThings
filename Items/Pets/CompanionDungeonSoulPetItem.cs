using AssortedCrazyThings.Buffs;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
    public class CompanionDungeonSoulPetItem : CaughtDungeonSoulBase
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Companion Soul");
            Tooltip.SetDefault("Summons a friendly Soul to light your way"
                               + "\nLight pet slot");
            ItemID.Sets.ItemNoGravity[item.type] = true;
        }

        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.ZephyrFish);
            frame2CounterCount = 8.0;
            animatedTextureSelect = 0;

            item.width = 26;
            item.height = 28;
            item.shoot = ModContent.ProjectileType<CompanionDungeonSoulPetProj>();
            item.buffType = ModContent.BuffType<CompanionDungeonSoulPetBuff>();
            item.rare = -11;

            item.value = Item.sellPrice(silver: 50);
        }

        //hardmode recipe
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<CaughtDungeonSoulFreed>(), 1);
            recipe.AddIngredient(ModContent.ItemType<DesiccatedLeather>(), 1);
            recipe.AddIngredient(ItemID.Bone, 2);
            recipe.AddTile(TileID.CrystalBall);
            recipe.SetResult(this);
            recipe.AddRecipe();

            recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<CompanionDungeonSoulPetItem2>(), 1);
            recipe.AddTile(TileID.CrystalBall);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
