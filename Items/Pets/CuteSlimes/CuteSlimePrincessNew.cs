using AssortedCrazyThings.Buffs.Pets.CuteSlimes;
using AssortedCrazyThings.Projectiles.Pets.CuteSlimes;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets.CuteSlimes
{
    public class CuteSlimePrincessNew : CuteSlimeItem
    {
        public override int PetType => ModContent.ProjectileType<CuteSlimePrincessNewProj>();

        public override int BuffType => ModContent.BuffType<CuteSlimePrincessNewBuff>();

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bottled Cute Princess Slime");
            Tooltip.SetDefault("Summons a friendly Cute Princess Slime to follow you");
        }

        public override void SafeSetDefaults()
        {
            Item.value = Item.sellPrice(copper: 20);
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<PrinceSlimeItem>()).AddIngredient(ModContent.ItemType<CuteSlimeBlueNew>()).AddTile(TileID.Solidifier).Register();
        }
    }
}
