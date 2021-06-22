using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
    [Autoload]
    public class PetSunMoonItem : SimplePetItemBase
    {
        public override int PetType => ModContent.ProjectileType<PetMoonProj>();

        public override int BuffType => ModContent.BuffType<PetSunMoonBuff>();

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bottled Sun and Moon");
            Tooltip.SetDefault("Summons a small sun and moon that provide you with constant light"
                + "\nShows the current time in the buff tip"
                + "\nShows the current moon cycle in the buff tip"
                + "\nAppearance can be changed with Costume Suitcase");
        }

        public override void SafeSetDefaults()
        {
            Item.width = 38;
            Item.height = 26;
            Item.rare = -11;
            Item.value = Item.sellPrice(gold: 16, silver: 20);
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<PetSunItem>()).AddIngredient(ModContent.ItemType<PetMoonItem>()).AddTile(TileID.CrystalBall).Register();
        }
    }
}
