using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
    [LegacyName("BabyCrimera")]
    public class BabyCrimeraItem : SimplePetItemBase
    {
        public override int PetType => ModContent.ProjectileType<BabyCrimeraProj>();

        public override int BuffType => ModContent.BuffType<BabyCrimeraBuff>();

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Juicy Vertebrae");
            Tooltip.SetDefault("Summons a baby Crimera to follow you");
        }

        public override void SafeSetDefaults()
        {
            Item.rare = -11;
            Item.value = Item.sellPrice(copper: 10);
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.Vertebrae, 30).AddTile(TileID.DemonAltar).Register();
        }
    }
}
