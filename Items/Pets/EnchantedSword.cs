using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace AssortedCrazyThings.Items.Pets
{
    public class EnchantedSword : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Enchanted Sword");
            Tooltip.SetDefault("Summons an Enchanted Sword to watch over you");
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.ZephyrFish);
            Item.shoot = Mod.Find<ModProjectile>("EnchantedSword").Type;
            Item.buffType = Mod.Find<ModBuff>("EnchantedSword").Type;
            Item.rare = -11;

            Item.value = Item.sellPrice(silver: 40);
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.EnchantedSword, 1).AddTile(TileID.DemonAltar).Register();
        }

        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {
            if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
            {
                player.AddBuff(Item.buffType, 3600, true);
            }
        }
    }
}
