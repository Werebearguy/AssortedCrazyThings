using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace AssortedCrazyThings.Items.Pets
{
    public class AlienHornet : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Vortex Nectar");
            Tooltip.SetDefault("Summons a friendly Alien Hornet to follow you");
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.ZephyrFish);
            Item.shoot = Mod.Find<ModProjectile>("AlienHornet").Type;
            Item.buffType = Mod.Find<ModBuff>("AlienHornet").Type;
            Item.rare = -11;
            Item.value = Item.sellPrice(gold: 5);
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.Nectar, 1).AddIngredient(ItemID.FragmentVortex, 10).AddTile(TileID.DemonAltar).Register();
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
