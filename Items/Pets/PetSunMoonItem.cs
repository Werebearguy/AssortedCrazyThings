using AssortedCrazyThings.Buffs;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
    public class PetSunMoonItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bottled Sun and Moon");
            Tooltip.SetDefault("Summons a small sun and moon that provide you with constant light"
                + "\nShows the current time in the buff tip"
                + "\nShows the current moon cycle in the buff tip"
                + "\nAppearance can be changed with Costume Suitcase");
        }

        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.ZephyrFish);
            item.shoot = mod.ProjectileType<PetMoonProj>();
            item.buffType = mod.BuffType<PetSunMoonBuff>();
            item.width = 38;
            item.height = 26;
            item.rare = -11;
            item.value = Item.sellPrice(gold: 16, silver: 20);
        }

        public override void UseStyle(Player player)
        {
            if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
            {
                player.AddBuff(item.buffType, 3600, true);
            }
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType<PetSunItem>());
            recipe.AddIngredient(mod.ItemType<PetMoonItem>());
            recipe.AddTile(TileID.CrystalBall);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
