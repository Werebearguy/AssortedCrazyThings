using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
    [LegacyName("DocileDemonEye")]
    public class DocileDemonEyeItem : SimplePetItemBase
    {
        public override int PetType => ModContent.ProjectileType<DocileDemonEyeProj>();

        public override int BuffType => ModContent.BuffType<DocileDemonEyeBuff>();

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Docile Demon Eye");
            Tooltip.SetDefault("Summons a docile Demon Eye to follow you"
                + "\nAppearance can be changed with Costume Suitcase");
        }

        public override void SafeSetDefaults()
        {
            Item.width = 34;
            Item.height = 22;
            Item.rare = -11;
            Item.value = Item.sellPrice(silver: 10);
        }

        public override void AddRecipes()
        {
            //regular recipe, dont delete
            CreateRecipe(1).AddIngredient(ItemID.BlackLens, 1).AddIngredient(ItemID.Lens, 1).AddTile(TileID.DemonAltar).Register();
        }
    }
}
