using AssortedCrazyThings.Buffs;
using AssortedCrazyThings.Projectiles.Minions;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Weapons
{
    public class SlimeHandlerKnapsack : MinionItemBase
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Slime Handler Knapsack");
            Tooltip.SetDefault("Summons a friendly Slime of a random color from your Knapsack to fight for you"
                + "\nRight click to pick from available forms");
        }

        public override void SetDefaults()
        {
            //change damage in SlimePackMinion.cs
            item.damage = SlimePackMinion.DefDamage;
            item.summon = true;
            item.mana = 10;
            item.width = 24;
            item.height = 30;
            item.useTime = 36;
            item.useAnimation = 36;
            item.useStyle = 4; //4 for life crystal
            item.noMelee = true;
            item.noUseGraphic = true;
            item.value = Item.sellPrice(0, 0, 75, 0);
            item.rare = -11;
            item.UseSound = SoundID.Item44;
            item.shoot = mod.ProjectileType<SlimePackMinion>();
            item.shootSpeed = 10f;
            item.knockBack = SlimePackMinion.DefKnockback;
            item.buffType = mod.BuffType<SlimePackMinionBuff>();
        }

        public override void ModifyWeaponDamage(Player player, ref float add, ref float mult)
        {
            AssPlayer mPlayer = player.GetModPlayer<AssPlayer>();
            if (mPlayer.selectedSlimePackMinionType == 2)
            {
                add += SlimePackMinion.SpikedIncrease;
            }
        }

        public override void GetWeaponKnockback(Player player, ref float knockback)
        {
            AssPlayer mPlayer = player.GetModPlayer<AssPlayer>();
            if (mPlayer.selectedSlimePackMinionType == 2)
            {
                knockback *= 1f + SlimePackMinion.SpikedIncrease;
            }
            else
            {
                //default
            }
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            AssPlayer mPlayer = player.GetModPlayer<AssPlayer>();
            if (mPlayer.selectedSlimePackMinionType == 1)
            {
                type = mod.ProjectileType<SlimePackAssortedMinion>();
            }
            else if (mPlayer.selectedSlimePackMinionType == 2)
            {
                type = mod.ProjectileType<SlimePackSpikedMinion>();
            }
            else
            {
                //default
            }
            Vector2 spawnPos = new Vector2(player.Center.X - player.direction * 12f, player.position.Y - 8f);
            if (Collision.SolidCollision(spawnPos + new Vector2(-player.direction * 18f, 0f), 12, 1))
            {
                spawnPos.X = player.Center.X + player.direction * 8f;
                spawnPos.Y = player.Center.Y;
            }
            Projectile.NewProjectile(spawnPos.X, spawnPos.Y, - player.velocity.X, player.velocity.Y - 6f, type, damage, knockBack, Main.myPlayer, 0f, 0f);
            return false;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.SlimeCrown, 1);
            recipe.AddIngredient(ItemID.Gel, 200);
            recipe.AddIngredient(ItemID.SoulofLight, 5);
            recipe.AddIngredient(ItemID.SoulofNight, 5);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
