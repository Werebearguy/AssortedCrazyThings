using AssortedCrazyThings.Buffs;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace AssortedCrazyThings.Items.Pets
{
    public class PetSunItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bottled Sun");
            Tooltip.SetDefault("Summons a small sun that provides you with constant sunlight"
                + "\nShows the current time in the buff tip");
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.ZephyrFish);
            Item.shoot = ModContent.ProjectileType<PetSunProj>();
            Item.buffType = ModContent.BuffType<PetSunBuff>();
            Item.width = 20;
            Item.height = 26;
            Item.rare = -11;
            Item.value = Item.sellPrice(gold: 9, silver: 20);
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
            CreateRecipe(1).AddIngredient(ItemID.Bottle).AddIngredient(ItemID.SunStone).AddIngredient(ItemID.Hellstone, 25).AddTile(TileID.CrystalBall).Register();
        }
    }
}
