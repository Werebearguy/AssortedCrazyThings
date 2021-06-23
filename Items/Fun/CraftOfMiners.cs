using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace AssortedCrazyThings.Items.Fun
{
    [Content(ContentType.Weapons)]
    public class CraftOfMiners : AssItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Craft of Miners");
            Tooltip.SetDefault("'Use those fists of yours to tear through any blocks in your way'"
                + "\n[c/44942e:'Dedicated to Anonymous']");
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.ShroomiteDiggingClaw);
            Item.damage = 13;
            Item.useAnimation = 3;
            Item.useTime = 3;
            Item.value = Item.sellPrice(gold: 5);
            Item.rare = -11;
            Item.noUseGraphic = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.ShroomiteDiggingClaw, 5).AddTile(TileID.CrystalBall).Register();
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(10))
            {
                Dust.NewDust(player.position, player.width, player.height, 15, 0f, 0f, 150, default(Color), 1.5f);
                Dust.NewDust(player.position, player.width, player.height, 15, 0f, 0f, 150, default(Color), 1.5f);
                Dust.NewDust(player.position, player.width, player.height, 15, 0f, 0f, 150, default(Color), 1.5f);
                Dust.NewDust(player.position, player.width, player.height, 15, 0f, 0f, 150, default(Color), 1.5f);
            }
        }
    }
}
