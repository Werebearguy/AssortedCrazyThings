using AssortedCrazyThings.Buffs.Pets.CuteSlimes;
using AssortedCrazyThings.Projectiles.Pets.CuteSlimes;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets.CuteSlimes
{
    public class CuteSlimeQueenItem : CuteSlimeItem
    {
        public override int PetType => ModContent.ProjectileType<CuteSlimeQueenProj>();

        public override int BuffType => ModContent.BuffType<CuteSlimeQueenBuff>();

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bottled Cute Queen Slime");
            Tooltip.SetDefault("Summons a friendly Cute Queen Slime to follow you");
        }

        public override void SafeSetDefaults()
        {
            Item.value = Item.sellPrice(copper: 20);
        }

        public override void AddRecipes()
        {
            //TODO obtainment
            //CreateRecipe(1).AddIngredient(ModContent.ItemType<PrinceSlimeItem>()).AddIngredient(ModContent.ItemType<CuteSlimeBlueItem>()).AddTile(TileID.Solidifier).Register();
        }
    }
}
