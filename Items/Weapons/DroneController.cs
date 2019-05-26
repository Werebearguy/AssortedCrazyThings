using AssortedCrazyThings.Base;
using AssortedCrazyThings.Buffs;
using AssortedCrazyThings.Projectiles.Minions;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Weapons
{
    public class DroneController : MinionItemBase
    {

        #region Static Methods
        /// <summary>
        /// Returns the custom MinionPos
        /// </summary>
        public static int GetSlotOfNextCombatDrone(Player player, int[] combatDrones)
        {
            int slot = 0;
            int min = 1000;
            for (int i = 0; i < 1000; i++)
            {
                Projectile proj = Main.projectile[i];
                if (proj.active && proj.owner == player.whoAmI && Array.IndexOf(combatDrones, proj.type) != -1)
                {
                    //proj.ai[1] is MinionPos
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

            int rowOffset = tempPos / 2; //0, 1 or 2

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
            int sum = player.ownedProjectileCounts[AssUtils.Instance.ProjectileType<BasicLaserDrone>()];
            sum += player.ownedProjectileCounts[AssUtils.Instance.ProjectileType<HeavyLaserDrone>()];
            sum += player.ownedProjectileCounts[AssUtils.Instance.ProjectileType<MissileDrone>()];
            return sum;
        }

        public static int SumOfSupportDrones(Player player)
        {
            int sum = player.ownedProjectileCounts[AssUtils.Instance.ProjectileType<HealingDrone>()];
            return sum;
        }


        public static int GetDroneType(DroneType selected)
        {
            int type = 0;
            switch (selected)
            {
                case DroneType.BasicLaser:
                    return AssUtils.Instance.ProjectileType<BasicLaserDrone>();
                case DroneType.HeavyLaser:
                    return AssUtils.Instance.ProjectileType<HeavyLaserDrone>();
                case DroneType.Missile:
                    return AssUtils.Instance.ProjectileType<MissileDrone>();
                //case DroneType.Unused1:
                //    break;
                //case DroneType.Unused2:
                //    break;
                //case DroneType.Unused3:
                //    break;
                case DroneType.Healing:
                    return AssUtils.Instance.ProjectileType<HealingDrone>();
                //case DroneType.Shield:
                //    break;
                case DroneType.None:
                default:
                    break;
            }
            return type;
        }
        public static float GetKBModifier(DroneType selected)
        {
            float modifier = 1f;
            switch (selected)
            {
                case DroneType.BasicLaser:
                    modifier = 1f;
                    break;
                case DroneType.HeavyLaser:
                    modifier = 4f;
                    break;
                case DroneType.Missile:
                    modifier = 1.333334f;
                    break;
                //case DroneType.Unused1:
                //    //modifier = 1f;
                //    break;
                //case DroneType.Unused2:
                //    //modifier = 1f;
                //    break;
                //case DroneType.Unused3:
                //    //modifier = 1f;
                //    break;
                case DroneType.Healing:
                    modifier = 0f;
                    break;
                //case DroneType.Shield:
                //    break;
                case DroneType.None:
                default:
                    break;
            }
            return modifier;
        }

        public static float GetDamageModifier(DroneType selected)
        {
            float modifier = 1f;
            switch (selected)
            {
                case DroneType.BasicLaser:
                    modifier = 1f;
                    break;
                case DroneType.HeavyLaser:
                    modifier = 9.091f;
                    break;
                case DroneType.Missile:
                    modifier = 1.2f;
                    break;
                //case DroneType.Unused1:
                //    modifier = 1f;
                //    break;
                //case DroneType.Unused2:
                //    modifier = 1f;
                //    break;
                //case DroneType.Unused3:
                //    modifier = 1f;
                //    break;
                case DroneType.Healing:
                    modifier = 0f;
                    break;
                //case DroneType.Shield:
                //    break;
                case DroneType.None:
                default:
                    break;
            }
            return modifier;
        }

        public static string GetTooltip(DroneType selected, bool onlyName = false)
        {
            string name;
            string stats = "\nBase Damage: " + (int)(BaseDmg * GetDamageModifier(selected))
                         + "\nBase Knockback: " + Math.Round(SlimePackMinion.DefKnockback * GetKBModifier(selected), 1);
            string desc = "";
            string misc = "";
            switch (selected)
            {
                case DroneType.BasicLaser:
                    name = "Basic Laser Drone";
                    desc = "Rapidly fires lasers";
                    break;
                case DroneType.HeavyLaser:
                    name = "Heavy Laser Drone";
                    desc = "Fires a penetrating laser after a long delay";
                    misc = "Occupies two minion slots";
                    break;
                case DroneType.Missile:
                    name = "Missile Launcher Drone";
                    desc = "Fires a salvo of missiles after a long delay";
                    misc = "Occupies two minion slots";
                    break;
                //case DroneType.Unused1:
                //    name = "Basic Laser Drone";
                //    desc = "Rapidly fires lasers";
                //    break;
                //case DroneType.Unused2:
                //    name = "Basic Laser Drone";
                //    desc = "Rapidly fires lasers";
                //    break;
                //case DroneType.Unused3:
                //    name = "Basic Laser Drone";
                //    desc = "Rapidly fires lasers";
                //    break;
                case DroneType.Healing:
                    name = "Healing Drone";
                    stats = "";
                    desc = "Heals you when hurt";
                    misc = "Only one can be summoned";
                    break;
                //case DroneType.Shield:
                //    name = "Basic Laser Drone";
                //    desc = "Rapidly fires lasers";
                //    break;
                case DroneType.None:
                default:
                    return "";
            }
            return name + (!onlyName ? (stats + "\n" + desc + "\n" + misc) : "");
        }

        public static bool CanSpawn(Player player, DroneType selected)
        {
            bool canSpawn = true;
            switch (selected)
            {
                case DroneType.Healing:
                    canSpawn = player.ownedProjectileCounts[GetDroneType(selected)] == 0;
                    break;
                //case DroneType.Shield:
                //    break;
                default:
                    break;
            }
            return canSpawn;
        }
        #endregion

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Drone Controller");
            Tooltip.SetDefault("Summons a friendly Drone to support or fight for you"
                + "\nRight click to pick from available drones");
        }

        public const int BaseDmg = 22;
        public const float BaseKB = 3f;

        public override void SetDefaults()
        {
            item.damage = BaseDmg;
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
            item.shoot = mod.ProjectileType<BasicLaserDrone>();
            item.shootSpeed = 10f;
            item.knockBack = BaseKB;
            item.buffType = mod.BuffType<DroneControllerBuff>();
            item.buffTime = 3600;
        }

        public override void GetWeaponDamage(Player player, ref int damage)
        {
            AssPlayer mPlayer = player.GetModPlayer<AssPlayer>();

            DroneType selected = mPlayer.selectedDroneControllerMinionType;
            damage = (int)(damage * GetDamageModifier(selected));
        }

        public override void GetWeaponKnockback(Player player, ref float knockback)
        {
            AssPlayer mPlayer = player.GetModPlayer<AssPlayer>();

            DroneType selected = mPlayer.selectedDroneControllerMinionType;
            knockback *= GetKBModifier(selected);
        }

        public override bool CanUseItem(Player player)
        {
            if (Main.netMode != NetmodeID.Server && player.whoAmI == Main.myPlayer)
            {
                AssPlayer mPlayer = player.GetModPlayer<AssPlayer>();
                DroneType selected = mPlayer.selectedDroneControllerMinionType;

                if (!CanSpawn(player, selected))
                {
                    return false;
                }
            }
            return true;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            AssPlayer mPlayer = player.GetModPlayer<AssPlayer>();
            DroneType selected = mPlayer.selectedDroneControllerMinionType;
            type = GetDroneType(selected);

            int currentCount = 0;
            int[] combatDrones = new int[]
            {
            AssUtils.Instance.ProjectileType<BasicLaserDrone>(),
            AssUtils.Instance.ProjectileType<HeavyLaserDrone>(),
            AssUtils.Instance.ProjectileType<MissileDrone>()
            };
            if (Array.IndexOf(combatDrones, type) != -1)
            {
                currentCount = GetSlotOfNextCombatDrone(player, combatDrones);
            }
            Vector2 spawnPos = new Vector2(player.Center.X, player.Center.Y);
            Projectile.NewProjectile(spawnPos.X, spawnPos.Y, -player.velocity.X, player.velocity.Y - 6f, type, damage, knockBack, Main.myPlayer, 0f, currentCount);
            return false;
        }

        public override void AddRecipes()
        {
            //TODO Recipe
            //ModRecipe recipe = new ModRecipe(mod);
            //recipe.AddIngredient(ItemID.SlimeCrown, 1);
            //recipe.AddIngredient(ItemID.Gel, 200);
            //recipe.AddIngredient(ItemID.SoulofLight, 5);
            //recipe.AddIngredient(ItemID.SoulofNight, 5);
            //recipe.AddTile(TileID.MythrilAnvil);
            //recipe.SetResult(this);
            //recipe.AddRecipe();
        }
        public override void MoreModifyTooltips(List<TooltipLine> tooltips)
        {
            AssPlayer mPlayer = Main.LocalPlayer.GetModPlayer<AssPlayer>();
            DroneType selected = mPlayer.selectedDroneControllerMinionType;

            string name = GetTooltip(selected, onlyName: true);

            for (int i = 0; i < tooltips.Count; i++)
            {
                if (Main.LocalPlayer.HasItem(mod.ItemType<DroneController>()))
                {
                    if (tooltips[i].mod == "Terraria" && tooltips[i].Name == "ItemName")
                    {
                        tooltips[i].text += " (" + name + ")";
                    }
                }
            }

            bool allUnlocked = true;
            foreach (DroneType type in Enum.GetValues(typeof(DroneType)))
            {
                if (type != DroneType.None)
                {
                    if (!selected.HasFlag(type))
                    {
                        allUnlocked = false;
                    }
                }
            }

            if (!(allUnlocked && Main.LocalPlayer.HasItem(mod.ItemType<DroneController>())))
            {
                tooltips.Add(new TooltipLine(mod, "Destroyer", "Defeat the destroyer to unlock more drones"));
            }

            if (!CanSpawn(Main.LocalPlayer, selected))
            {
                tooltips.Add(new TooltipLine(mod, "CanSpawn", "Only one " + name + " can be out at once"));
            }
        }
    }

    /// <summary>
    /// The type of drone enumerated, so you can check against it via .HasFlag(DroneType.SomeType)
    /// </summary>
    [Flags]
    public enum DroneType: byte
    {
        None = 0,
        BasicLaser = 1,
        HeavyLaser = 2,
        Missile = 4,
        Healing = 8,
        //Unused1 = 8,
        //Unused2 = 16,
        //Unused3 = 32,
        //Shield = 128
    }
}
