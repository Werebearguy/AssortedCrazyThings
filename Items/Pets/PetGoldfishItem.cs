using AssortedCrazyThings.Buffs;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
    public class PetGoldfishItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Possessed Fish Idol");
            Tooltip.SetDefault("Summons a goldfish that follows you"
                + "\n'You feel like you lost something important in obtaining this idol...'"
                + "\nAppearance can be changed with Costume Suitcase");
        }

        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.ZephyrFish);
            item.shoot = ModContent.ProjectileType<PetGoldfishProj>();
            item.buffType = ModContent.BuffType<PetGoldfishBuff>();
            item.rare = -11;
            item.value = Item.sellPrice(copper: 10);
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
            recipe.AddIngredient(ItemID.FishStatue, 1);
            recipe.AddTile(TileID.DemonAltar);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
