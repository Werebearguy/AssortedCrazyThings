using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
    public class DrumstickElementalItem : SimplePetItemBase
    {
        public override int PetType => ModContent.ProjectileType<DrumstickElementalProj>();

        public override int BuffType => ModContent.BuffType<DrumstickElementalBuff>();

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Magical Drumstick");
            Tooltip.SetDefault("Summons a delicious Drumstick Elemental to follow you");
        }

        public override void SafeSetDefaults()
        {
            Item.CloneDefaults(ItemID.ZephyrFish);
            Item.rare = -11;
            Item.value = Item.sellPrice(silver: 7, copper: 50);
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.Duck).AddTile(TileID.CookingPots).Register();
            CreateRecipe(1).AddIngredient(ItemID.MallardDuck).AddTile(TileID.CookingPots).Register();
        }
    }
}
