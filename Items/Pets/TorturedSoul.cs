using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace AssortedCrazyThings.Items.Pets
{
    public class TorturedSoul : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tortured Soul");
            Tooltip.SetDefault("Summons an unfriendly Tortured Soul to follow you");
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.ZephyrFish);
            Item.shoot = Mod.Find<ModProjectile>("TorturedSoul").Type;
            Item.buffType = Mod.Find<ModBuff>("TorturedSoul").Type;
            Item.rare = -11;
            Item.value = Item.sellPrice(silver: 50);
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.TaxCollectorsStickOfDoom, 1).AddTile(TileID.DemonAltar).Register();
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
