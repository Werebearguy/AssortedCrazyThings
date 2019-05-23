using AssortedCrazyThings.Base;
using AssortedCrazyThings.Buffs;
using AssortedCrazyThings.Projectiles.Minions;
using AssortedCrazyThings.Projectiles.Pets;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Weapons
{
    public class DroneController : MinionItemBase
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Drone Controller");
            Tooltip.SetDefault("Summons a friendly Drone to support or fight for you"
                + "\nRight click to pick from available forms");
        }

        public override void SetDefaults()
        {
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
            item.shoot = mod.ProjectileType<HeavyLaserDrone>();
            item.shootSpeed = 10f;
            item.knockBack = SlimePackMinion.DefKnockback;
            item.buffType = mod.BuffType<DroneControllerBuff>();
            item.buffTime = 3600;
        }

        public override void GetWeaponDamage(Player player, ref int damage)
        {
            //AssPlayer mPlayer = player.GetModPlayer<AssPlayer>();
            //if (mPlayer.selectedSlimePackMinionType == 2)
            //{
            //    damage = (int)(damage * SlimePackMinion.SpikedIncrease); //from 26 to 36
            //}
            //else
            //{
            //    //default
            //}
        }

        public override void GetWeaponKnockback(Player player, ref float knockback)
        {
            //AssPlayer mPlayer = player.GetModPlayer<AssPlayer>();
            //if (mPlayer.selectedSlimePackMinionType == 2)
            //{
            //    knockback *= SlimePackMinion.SpikedIncrease;
            //}
            //else
            //{
            //    //default
            //}
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            AssPlayer mPlayer = player.GetModPlayer<AssPlayer>();
            //if (mPlayer.selectedSlimePackMinionType == 1)
            //{
            //    type = mod.ProjectileType<SlimePackAssortedMinion>();
            //}
            //else if (mPlayer.selectedSlimePackMinionType == 2)
            //{
            //    type = mod.ProjectileType<SlimePackSpikedMinion>();
            //}
            //else
            //{
            //    //default
            //}
            Vector2 spawnPos = new Vector2(player.Center.X, player.Center.Y);
            int currentCount = GetSlotOfNextCombatDrone(player);
            Projectile.NewProjectile(spawnPos.X, spawnPos.Y, - player.velocity.X, player.velocity.Y - 6f, type, damage, knockBack, Main.myPlayer, 0f, currentCount);
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

        public static int GetSlotOfNextCombatDrone(Player player)
        {
            int[] combatDrones = new int[] { AssUtils.Instance.ProjectileType<MissileDrone>() };
            int slot = 0;
            int min = 1000;
            for (int i = 0; i < 1000; i++)
            {
                Projectile proj = Main.projectile[i];
                if (proj.active && proj.owner == player.whoAmI && Array.IndexOf(combatDrones, proj.type) != -1)
                {
                    min = Math.Min(min, (int)proj.ai[1]);
                    if (proj.ai[1] > slot) slot = (int)proj.ai[1];
                }
            }
            if (min > 0) return 0;

            return slot + 1;
        }

        /// <summary>
        /// Determines position for the current projectile based on minionPos
        /// </summary>
        public static Vector2 GetPosition(Projectile projectile, int minionPos)
        {
            int initialVecticalOffset = -70;
            int initialHorizontalOffset = 0;
            int verticalOffset = -(int)(projectile.height * 1.2f);
            int horizontalOffset = (int)(projectile.width * 1.75f);
            int tempPos = minionPos % 6;
            int side = minionPos % 2; //1 means left, 0 means right
            int columnOffset = minionPos / 6 + 1; //1 for <6, 2 for <12, etc

            int rowOffset = tempPos/ 2; //0, 1 or 2

            int middleOffset = 0;
            if (rowOffset == 1) middleOffset = horizontalOffset / 2; //the middle one

            Vector2 offset = new Vector2
                (
                initialHorizontalOffset + columnOffset * horizontalOffset + middleOffset,
                initialVecticalOffset + rowOffset * verticalOffset
                );
            offset.X *= side == 1 ? -1 : 1;
            return offset;
        }

        public static int SumOfCombatDrones(Player player)
        {
            int sum = player.ownedProjectileCounts[AssUtils.Instance.ProjectileType<MissileDrone>()];
            sum += player.ownedProjectileCounts[AssUtils.Instance.ProjectileType<HeavyLaserDrone>()];
            return sum;
        }

        public static int SumOfSupportDrones(Player player)
        {
            int sum = player.ownedProjectileCounts[AssUtils.Instance.ProjectileType<HealingDroneProj>()];
            return sum;
        }
    }
}
