using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Pets;

namespace AssortedCrazyThings.Items.Pets
{
    [Autoload]
    [LegacyName("LifelikeMechanicalFrog")]
    public class LifelikeMechanicalFrogItem : SimplePetItemBase
    {
        public override int PetType => ModContent.ProjectileType<LifelikeMechanicalFrogProj>();

        public override int BuffType => ModContent.BuffType<LifelikeMechanicalFrogBuff>();

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lifelike Mechanical Frog");
            Tooltip.SetDefault("Summons a friendly Frog to follow you"
                + "\nAppearance can be changed with Costume Suitcase");
        }

        public override void SafeSetDefaults()
        {
            Item.rare = -11;
            Item.value = Item.sellPrice(copper: 10);
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.Frog, 1).AddTile(TileID.Anvils).Register();
        }
    }
}
