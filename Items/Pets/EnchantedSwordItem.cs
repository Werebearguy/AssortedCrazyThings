using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Pets;

namespace AssortedCrazyThings.Items.Pets
{
    [LegacyName("EnchantedSword")]
    public class EnchantedSwordItem : SimplePetItemBase
    {
        public override int PetType => ModContent.ProjectileType<EnchantedSwordProj>();

        public override int BuffType => ModContent.BuffType<EnchantedSwordBuff>();

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Enchanted Sword");
            Tooltip.SetDefault("Summons an Enchanted Sword to watch over you");
        }

        public override void SafeSetDefaults()
        {
            Item.CloneDefaults(ItemID.ZephyrFish);
            Item.rare = -11;

            Item.value = Item.sellPrice(silver: 40);
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.EnchantedSword, 1).AddTile(TileID.DemonAltar).Register();
        }
    }
}