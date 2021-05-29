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
            ItemID.Sets.ItemNoGravity[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.ZephyrFish);
            frame2CounterCount = 8.0;
            animatedTextureSelect = 0;

            Item.width = 26;
            Item.height = 28;
            Item.shoot = ModContent.ProjectileType<CompanionDungeonSoulPetProj2>();
            Item.buffType = ModContent.BuffType<CompanionDungeonSoulPetBuff2>();
            Item.rare = -11;

            Item.value = Item.sellPrice(silver: 50);
        }

        //hardmode recipe
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<CompanionDungeonSoulPetItem>(), 1).AddTile(TileID.CrystalBall).Register();
        }
    }
}
