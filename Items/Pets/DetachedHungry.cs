using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace AssortedCrazyThings.Items.Pets
{
    public class DetachedHungry : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Unconscious Hungry");
            Tooltip.SetDefault("Summons a detached Hungry to follow you");
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.ZephyrFish);
            Item.shoot = Mod.Find<ModProjectile>("DetachedHungry").Type;
            Item.buffType = Mod.Find<ModBuff>("DetachedHungry").Type;
            Item.rare = -11;
            Item.value = Item.sellPrice(gold: 1);
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.HellstoneBar, 4).AddIngredient(ItemID.RottenChunk, 10).AddTile(TileID.DemonAltar).Register();
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
