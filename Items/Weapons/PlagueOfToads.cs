using AssortedCrazyThings.Projectiles.Weapons;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
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
            //item.knockback = 0f;
            //item.value = Item.sellPrice(gold: 3, silver: 50);
            //item.magic = true;

            Item.mana = 20;
            Item.damage = 8;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.shootSpeed = 16f;
            Item.shoot = ModContent.ProjectileType<PlagueOfToadsFired>();
            Item.width = 26;
            Item.height = 28;
            Item.UseSound = SoundID.Item66;
            Item.useAnimation = 22;
            Item.useTime = 22;
            Item.rare = -11;
            Item.noMelee = true;
            Item.knockBack = 0f;
            Item.value = Item.sellPrice(silver: 25);
            Item.DamageType = DamageClass.Magic;
        }

        public override bool Shoot(Player player, ProjectileSource_Item_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, Main.myPlayer, Main.mouseX + Main.screenPosition.X, Main.mouseY + Main.screenPosition.Y);
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.Frog, 3).AddIngredient(ItemID.WandofSparking, 1).AddTile(TileID.Anvils).Register();
        }
    }
}
