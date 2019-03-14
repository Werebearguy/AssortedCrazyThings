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
            Tooltip.SetDefault("Summons a friendly Slime of a random color from your Knapsack to fight for you");
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
            item.buffTime = 3600;
        }

        public override void GetWeaponDamage(Player player, ref int damage)
        {
            AssPlayer mPlayer = player.GetModPlayer<AssPlayer>();
            if(mPlayer.selectedSlimePackMinionType == 1)
            {
                damage = (int)(damage * SlimePackMinion.SpikedIncrease); //from 26 to 36
            }
            else
            {
                //default
            }
        }

        public override void GetWeaponKnockback(Player player, ref float knockback)
        {
            AssPlayer mPlayer = player.GetModPlayer<AssPlayer>();
            if (mPlayer.selectedSlimePackMinionType == 1)
            {
                knockback *= SlimePackMinion.SpikedIncrease;
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
                type = mod.ProjectileType<SlimePackSpikedMinion>();
            }
            else
            {
                //default
            }
            Projectile.NewProjectile(player.position.X + (player.width / 2) - player.direction * 12f, player.position.Y - 8f, - player.velocity.X, player.velocity.Y - 6f, type, damage, knockBack, Main.myPlayer, 0f, 0f);
            return false;
        }

        //public override void HoldItem(Player player)
        //{
        //    player.itemLocation.X = 0; //kind of a hack, 
        //    player.itemLocation.Y = 0;
        //}

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.SlimeCrown, 1);
            recipe.AddIngredient(ItemID.Gel, 999);
            recipe.AddIngredient(ItemID.SoulofLight, 10);
            recipe.AddIngredient(ItemID.SoulofNight, 10);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
