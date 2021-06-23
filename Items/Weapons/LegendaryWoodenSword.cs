using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace AssortedCrazyThings.Items.Weapons
{
    [Content(ContentType.Weapons)]
    public class LegendaryWoodenSword : AssItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Legendary Wooden Sword");
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.IronShortsword);
            //item.useStyle = ItemUseStyleID.SwingThrow;
            Item.width = 32;
            Item.height = 32;
            Item.rare = -11;
            Item.value = Item.sellPrice(0, 0, 25, 0);
        }

        public override void AddRecipes()
        {
            //ModRecipe recipe = new ModRecipe(mod);
            //recipe.AddIngredient(ItemID.AncientBattleArmorMaterial, 777);
            //recipe.AddTile(TileID.DemonAltar);
            //recipe.SetResult(this);
            //recipe.AddRecipe();
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(2))
            {
                //162 for "sparks"
                //169 for just light
                int dustType = 169;
                Dust dust = Dust.NewDustDirect(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, dustType, player.velocity.X * 0.2f + (player.direction * 3), player.velocity.Y * 0.2f, 100, Color.White, 1.25f);
                dust.noGravity = true;
                dust.velocity *= 2f;
            }
        }
    }
}
