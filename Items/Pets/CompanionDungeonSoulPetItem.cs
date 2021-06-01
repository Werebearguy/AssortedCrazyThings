using AssortedCrazyThings.Buffs.Pets;
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
            ItemID.Sets.ItemNoGravity[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.ZephyrFish);
            frame2CounterCount = 8.0;
            animatedTextureSelect = 0;

            Item.width = 26;
            Item.height = 28;
            Item.shoot = ModContent.ProjectileType<CompanionDungeonSoulPetProj>();
            Item.buffType = ModContent.BuffType<CompanionDungeonSoulPetBuff>();
            Item.rare = -11;

            Item.value = Item.sellPrice(silver: 50);
        }

        //hardmode recipe
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<CaughtDungeonSoulFreed>(), 1).AddIngredient(ModContent.ItemType<DesiccatedLeather>(), 1).AddIngredient(ItemID.Bone, 2).AddTile(TileID.CrystalBall).Register();
            CreateRecipe(1).AddIngredient(ModContent.ItemType<CompanionDungeonSoulPetItem2>(), 1).AddTile(TileID.CrystalBall).Register();
        }
    }
}
