using AssortedCrazyThings.Buffs;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

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
            Item.CloneDefaults(ItemID.ZephyrFish);
            Item.shoot = ModContent.ProjectileType<PetMoonProj>();
            Item.buffType = ModContent.BuffType<PetSunMoonBuff>();
            Item.width = 38;
            Item.height = 26;
            Item.rare = -11;
            Item.value = Item.sellPrice(gold: 16, silver: 20);
        }

        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {
            if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
            {
                player.AddBuff(Item.buffType, 3600, true);
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<PetSunItem>()).AddIngredient(ModContent.ItemType<PetMoonItem>()).AddTile(TileID.CrystalBall).Register();
        }
    }
}
