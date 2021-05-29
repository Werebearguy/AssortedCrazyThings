using AssortedCrazyThings.Buffs;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace AssortedCrazyThings.Items.Pets
{
    public class DrumstickElementalItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Magical Drumstick");
            Tooltip.SetDefault("Summons a delicious Drumstick Elemental to follow you");
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.ZephyrFish);
            Item.shoot = ModContent.ProjectileType<DrumstickElementalProj>();
            Item.buffType = ModContent.BuffType<DrumstickElementalBuff>();
            Item.rare = -11;
            Item.value = Item.sellPrice(silver: 7, copper: 50);
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
            CreateRecipe(1).AddIngredient(ItemID.Duck).AddTile(TileID.CookingPots).Register();
            CreateRecipe(1).AddIngredient(ItemID.MallardDuck).AddTile(TileID.CookingPots).Register();
        }
    }
}
