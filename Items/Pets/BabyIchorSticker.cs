using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
    public class BabyIchorSticker : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sleeping Ichor Sticker");
            Tooltip.SetDefault("Summons a Baby Ichor Sticker to follow you");
        }

        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.ZephyrFish);
            item.shoot = mod.ProjectileType("BabyIchorSticker");
            item.buffType = mod.BuffType("BabyIchorSticker");
            item.rare = -11;
            item.value = Item.sellPrice(gold: 2, silver: 70);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Ichor, 30);
            recipe.AddTile(TileID.DemonAltar);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override void UseStyle(Player player)
        {
            if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
            {
                player.AddBuff(item.buffType, 3600, true);
            }
        }
    }
}
