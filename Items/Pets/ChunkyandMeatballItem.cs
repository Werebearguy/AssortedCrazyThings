using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Pets;

namespace AssortedCrazyThings.Items.Pets
{
    [LegacyName("ChunkyandMeatball")]
    public class ChunkyandMeatballItem : SimplePetItemBase
    {
        public override int PetType => ModContent.ProjectileType<ChunkyProj>();

        public override int BuffType => ModContent.BuffType<ChunkyandMeatballBuff>();

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Chunky and Meatball");
            Tooltip.SetDefault("Summons a pair of inseperable brothers to follow you");
        }

        public override void SafeSetDefaults()
        {
            Item.rare = -11;
            Item.value = Item.sellPrice(silver: 4);
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient<ChunkysEye>().AddIngredient<MeatballsEye>().AddTile(TileID.DemonAltar).Register();
        }
    }
}
