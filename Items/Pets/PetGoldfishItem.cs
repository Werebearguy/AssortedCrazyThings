using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
    public class PetGoldfishItem : SimplePetItemBase
    {
        public override int PetType => ModContent.ProjectileType<PetGoldfishProj>();

        public override int BuffType => ModContent.BuffType<PetGoldfishBuff>();

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Possessed Fish Idol");
            Tooltip.SetDefault("Summons a goldfish that follows you"
                + "\n'You feel like you lost something important in obtaining this idol...'"
                + "\nAppearance can be changed with Costume Suitcase");
        }

        public override void SafeSetDefaults()
        {
            Item.rare = -11;
            Item.value = Item.sellPrice(copper: 10);
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.FishStatue, 1).AddTile(TileID.DemonAltar).Register();
        }
    }
}
