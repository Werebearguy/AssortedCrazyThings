using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

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
            Item.CloneDefaults(ItemID.ZephyrFish);
            Item.shoot = Mod.Find<ModProjectile>("BabyIchorSticker").Type;
            Item.buffType = Mod.Find<ModBuff>("BabyIchorSticker").Type;
            Item.rare = -11;
            Item.value = Item.sellPrice(gold: 2, silver: 70);
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.Ichor, 30).AddTile(TileID.DemonAltar).Register();
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
