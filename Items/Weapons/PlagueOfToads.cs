using AssortedCrazyThings.Projectiles.Weapons;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Weapons
{
    public class PlagueOfToads : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Plague of Toads");
            Tooltip.SetDefault("Tooltip here");
        }

        public override void SetDefaults()
        {
            item.mana = 10;
            item.damage = 36;
            item.useStyle = 1;
            item.shootSpeed = 16f;
            item.shoot = mod.ProjectileType<PlagueOfToadsFired>();
            item.width = 26;
            item.height = 28;
            item.UseSound = SoundID.Item66;
            item.useAnimation = 22;
            item.useTime = 22;
            item.rare = -11;
            item.noMelee = true;
            item.knockBack = 0f;
            item.value = Item.sellPrice(gold: 3, silver: 50);
            item.magic = true;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, Main.myPlayer, Main.mouseX + Main.screenPosition.X, Main.mouseY + Main.screenPosition.Y);
            return false;
        }

        public override void AddRecipes()
        {
            //TODO
            //ModRecipe recipe = new ModRecipe(mod);
            //recipe.AddIngredient(ItemID.MeteoriteBar, 5);
            //recipe.AddIngredient(mod.ItemType<CaughtDungeonSoulFreed>(), 2);
            //recipe.AddTile(TileID.Anvils);
            //recipe.SetResult(this);
            //recipe.AddRecipe();
        }
    }
}
