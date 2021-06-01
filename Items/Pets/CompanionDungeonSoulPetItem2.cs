using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
    public class CompanionDungeonSoulPetItem2 : CompanionDungeonSoulPetItem
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
            base.SetStaticDefaults();
            Tooltip.SetDefault("Summons a friendly Soul to light your way"
                               + "\nPet slot");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.shoot = ModContent.ProjectileType<CompanionDungeonSoulPetProj2>();
            Item.buffType = ModContent.BuffType<CompanionDungeonSoulPetBuff2>();
        }

        //hardmode recipe
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<CompanionDungeonSoulPetItem>(), 1).AddTile(TileID.CrystalBall).Register();
        }
    }
}
