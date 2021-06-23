using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
    public class AbeeminationItem : SimplePetItemBase
    {
        public override int PetType => ModContent.ProjectileType<AbeeminationProj>();

        public override int BuffType => ModContent.BuffType<AbeeminationBuff>();

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Abeemination");
            Tooltip.SetDefault("Summons a friendly Abeemination to follow you"
                + "\nAppearance can be changed with Costume Suitcase");
        }

        public override void SafeSetDefaults()
        {
            Item.rare = -11;
            Item.value = Item.sellPrice(gold: 2);
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.Abeemination, 1).AddIngredient(ItemID.LifeFruit, 1).AddTile(TileID.DemonAltar).Register();
        }
    }
}
