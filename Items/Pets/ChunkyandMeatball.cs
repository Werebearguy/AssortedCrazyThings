using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace AssortedCrazyThings.Items.Pets
{
    public class ChunkyandMeatball : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Chunky and Meatball");
            Tooltip.SetDefault("Summons a pair of inseperable brothers to follow you");
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.ZephyrFish);
            Item.shoot = Mod.Find<ModProjectile>("ChunkyProj").Type;
            Item.buffType = Mod.Find<ModBuff>("ChunkyandMeatball").Type;
            Item.rare = -11;
            Item.value = Item.sellPrice(silver: 4);
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient<ChunkysEye>().AddIngredient<MeatballsEye>().AddTile(TileID.DemonAltar).Register();
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
