using AssortedCrazyThings.Buffs;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
    public class CompanionDungeonSoulPetItem2 : CaughtDungeonSoulBase
    {
        public override string Texture
        {
            get
            {
                return "AssortedCrazyThings/Items/Pets/CompanionDungeonSoulPetItem"; //use fixed texture
            }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Companion Soul");
            Tooltip.SetDefault("Summons a friendly Soul to light your way"
                               + "\nPet slot");
            ItemID.Sets.ItemNoGravity[item.type] = true;
        }

        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.ZephyrFish);
            frame2CounterCount = 8.0;
            animatedTextureSelect = 0;

            item.width = 26;
            item.height = 28;
            item.shoot = ModContent.ProjectileType<CompanionDungeonSoulPetProj2>();
            item.buffType = ModContent.BuffType<CompanionDungeonSoulPetBuff2>();
            item.rare = -11;

            item.value = Item.sellPrice(silver: 50);
        }

        //hardmode recipe
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<CompanionDungeonSoulPetItem>(), 1);
            recipe.AddTile(TileID.CrystalBall);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
