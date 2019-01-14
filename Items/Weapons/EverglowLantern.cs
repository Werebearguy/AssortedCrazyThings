using AssortedCrazyThings.Buffs;
using AssortedCrazyThings.Projectiles.Minions;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Weapons
{
    //imported from my tAPI mod because I'm lazy
    public class EverglowLantern : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Everglow Lantern");
            Tooltip.SetDefault("Summons two freed Dungeon Souls at a time to fight for you.\nEach Dungeon Soul occupies only half a minion slot.");
        }

        public override void SetDefaults()
        {
            item.damage = CompanionDungeonSoulMinion.Damage;
            item.summon = true;
            item.mana = 10;
            item.width = 18;
            item.height = 46;
            item.useTime = 36;
            item.useAnimation = 36;
            item.useStyle = 4; //4 for life crystal
            item.noMelee = true;
            item.value = Item.sellPrice(0, 0, 75, 0);
            item.rare = -11;
            item.UseSound = SoundID.Item44;
            item.shoot = mod.ProjectileType<CompanionDungeonSoulMinion>();
            item.shootSpeed = 10f;
            item.knockBack = 0.5f;
            item.buffType = mod.BuffType<CompanionDungeonSoulMinionBuff>();
            item.buffTime = 3600;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Projectile.NewProjectile(player.position.X + (player.width / 2) + player.direction * 8f, player.position.Y - 2f, player.velocity.X + player.direction * 1.5f, -1f, item.shoot, item.damage, item.knockBack, player.whoAmI, 0f, 0f);
            Projectile.NewProjectile(player.position.X + (player.width / 2) + player.direction * 8f, player.position.Y, player.velocity.X + player.direction * 1, -1/2f, item.shoot, item.damage, item.knockBack, player.whoAmI, 0f, 0f);
            //return player.altFunctionUse != 2;
            return false;
        }

        public override Vector2? HoldoutOffset()
        {
            return null;// new Vector2(0, 20);
        }

        public override bool UseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                player.MinionNPCTargetAim();
            }
            return base.UseItem(player);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.MeteoriteBar, 5);
            recipe.AddIngredient(mod.ItemType<CaughtDungeonSoulAwakened>(), 2);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
