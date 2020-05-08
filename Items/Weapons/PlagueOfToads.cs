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
            Tooltip.SetDefault("Summons a cloud to rain toads on your foes");
        }

        public override void SetDefaults()
        {
            //item.mana = 10;
            //item.damage = 36;
            //item.useStyle = 1;
            //item.shootSpeed = 16f;
            //item.shoot = ModContent.ProjectileType<PlagueOfToadsFired>();
            //item.width = 26;
            //item.height = 28;
            //item.UseSound = SoundID.Item66;
            //item.useAnimation = 22;
            //item.useTime = 22;
            //item.rare = -11;
            //item.noMelee = true;
            //item.knockBack = 0f;
            //item.value = Item.sellPrice(gold: 3, silver: 50);
            //item.magic = true;

            item.mana = 20;
            item.damage = 8;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.shootSpeed = 16f;
            item.shoot = ModContent.ProjectileType<PlagueOfToadsFired>();
            item.width = 26;
            item.height = 28;
            item.UseSound = SoundID.Item66;
            item.useAnimation = 22;
            item.useTime = 22;
            item.rare = -11;
            item.noMelee = true;
            item.knockBack = 0f;
            item.value = Item.sellPrice(silver: 25);
            item.magic = true;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, Main.myPlayer, Main.mouseX + Main.screenPosition.X, Main.mouseY + Main.screenPosition.Y);
            return false;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Frog, 3);
            recipe.AddIngredient(ItemID.WandofSparking, 1);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
