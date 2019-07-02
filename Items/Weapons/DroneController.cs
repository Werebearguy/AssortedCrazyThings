using AssortedCrazyThings.Base;
using AssortedCrazyThings.Buffs;
using AssortedCrazyThings.Projectiles.Minions.Drones;
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
        /// Checks if drones are unlocked 
        /// </summary>
        public static bool AllUnlocked(DroneType unlocked)
        {
            foreach (DroneType type in Enum.GetValues(typeof(DroneType)))
            {
                if (type != DroneType.None)
                {
                    if (!unlocked.HasFlag(type))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Checks if all drones are unlocked for the player
        /// </summary>
        public static bool AllUnlocked(Player player)
        {
            return AllUnlocked(player.GetModPlayer<AssPlayer>(AssUtils.Instance).droneControllerUnlocked);
        }

        /// <summary>
        /// Returns the custom MinionPos
        /// </summary>
        public static int GetSlotOfNextDrone(Projectile self)
        {
            int slot = 0;
            int min = 1000;
            for (int i = 0; i < 1000; i++)
            {
                Projectile proj = Main.projectile[i];
                if (proj.active)
                {
                    if (proj.owner == self.owner && proj.identity != self.identity)
                    {
                        if (proj.modProjectile != null && proj.modProjectile is DroneBase)
                        {
                            DroneBase drone = (DroneBase)proj.modProjectile;
                            if (drone.IsCombatDrone)
                            {
                                int minionPos = drone.MinionPos;
                                min = Math.Min(min, minionPos);
                                if (minionPos > slot) slot = minionPos;
                            }
                        }
                    }
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

        public static int SumOfSummonedDrones(Player player)
        {
            int sum = 0;
            for (int i = 0; i < DataList.Length; i++)
            {
                sum += player.ownedProjectileCounts[DataList[i].ProjType];
            }
            return sum;
        }

        public static bool CanSpawn(Player player, DroneType selected)
        {
            bool canSpawn = true;
            if (selected == DroneType.Healing || selected == DroneType.Shield)
            {
                canSpawn = player.ownedProjectileCounts[GetDroneData(selected).ProjType] == 0;
            }
            canSpawn &= player.GetModPlayer<AssPlayer>().droneControllerUnlocked.HasFlag(selected);
            return canSpawn;
        }

        private static void PreSync(Projectile proj)
        {
            if (proj.modProjectile != null && proj.modProjectile is DroneBase)
            {
                DroneBase drone = (DroneBase)proj.modProjectile;
                if (drone.IsCombatDrone) drone.MinionPos = GetSlotOfNextDrone(proj);
            }
        }

        /// <summary>
        /// Sets the data up for a DroneType
        /// </summary>
        public static DroneData SetDroneData(DroneType selected)
        {
            switch (selected)
            {
                case DroneType.BasicLaser:
                    return new DroneData
                        (
                        projType: AssUtils.Instance.ProjectileType<BasicLaserDrone>(),
                        name: "Basic Laser Drone",
                        desc: "Rapidly fires lasers",
                        firerate: "High"
                        );
                case DroneType.HeavyLaser:
                    return new DroneData
                        (
                        projType: AssUtils.Instance.ProjectileType<HeavyLaserDrone>(),
                        name: "Heavy Laser Drone",
                        desc: "Fires a penetrating laser after a long delay",
                        misc: "Occupies two minion slots",
                        firerate: "Extremely slow",
                        dmgModifier: 8.091f,
                        kBModifier: 3f
                        );
                case DroneType.Missile:
                    return new DroneData
                        (
                        projType: AssUtils.Instance.ProjectileType<MissileDrone>(),
                        name: "Missile Drone",
                        desc: "Fires a salvo of missiles after a long delay",
                        misc: "Occupies two minion slots",
                        firerate: "Very slow",
                        dmgModifier: 0.2f,
                        kBModifier: 0.333334f
                        );
                case DroneType.Healing:
                    return new DroneData
                        (
                        projType: AssUtils.Instance.ProjectileType<HealingDrone>(),
                        name: "Healing Drone",
                        desc: "Heals you when hurt",
                        misc: "Only one can be summoned",
                        combat: false
                        );
                case DroneType.Shield:
                    return new DroneData
                        (
                        projType: AssUtils.Instance.ProjectileType<ShieldDrone>(),
                        name: "Shield Drone",
                        desc: "Creates a damage reducing shield",
                        misc: "Only one can be summoned\nShield resets if drone despawns",
                        combat: false
                        );
                default:
                    throw new Exception("No DroneType specified");
            }
        }

        /// <summary>
        /// Holds data about each DroneType
        /// </summary>
        public static DroneData[] DataList;

        /// <summary>
        /// Used to access a particular DroneTypes data
        /// </summary>
        public static DroneData GetDroneData(DroneType selected)
        {
            return DataList[(int)Math.Log((int)selected, 2)];
        }

        /// <summary>
        /// Called in Mod.Load
        /// </summary>
        public static void Load()
        {
            Array a = Enum.GetValues(typeof(DroneType));
            DataList = new DroneData[a.Length - 1]; //without None
            int i = 0;
            foreach (DroneType type in a)
            {
                if (type != DroneType.None)
                {
                    DataList[i++] = SetDroneData(type);
                }
            }
        }

        /// <summary>
        /// Called in Mod.Unload
        /// </summary>
        public static void Unload()
        {
            DataList = null;
        }
        #endregion

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Drone Controller");
            Tooltip.SetDefault("Summons a friendly Drone to support or fight for you"
                + "\nRight click to pick from available drones"
                + "\nHolding the item improves the Drones supportive and offensive abilities");
            ItemID.Sets.StaffMinionSlotsRequired[item.type] = 2;
        }

        public const int BaseDmg = 22;
        public const float BaseKB = 2.5f;

        public override void SetDefaults()
        {
            item.damage = BaseDmg;
            item.summon = true;
            item.mana = 10;
            item.width = 24;
            item.height = 30;
            item.useTime = 36;
            item.useAnimation = 36;
            item.useStyle = 4;
            item.noMelee = true;
            item.noUseGraphic = true;
            item.value = Item.sellPrice(0, 0, 75, 0);
            item.rare = -11;
            item.UseSound = SoundID.Item44;
            item.shoot = mod.ProjectileType<BasicLaserDrone>();
            item.shootSpeed = 10f;
            item.knockBack = BaseKB;
            item.buffType = mod.BuffType<DroneControllerBuff>();
        }

        public override void ModifyWeaponDamage(Player player, ref float add, ref float mult)
        {
            AssPlayer mPlayer = player.GetModPlayer<AssPlayer>();

            DroneType selected = mPlayer.selectedDroneControllerMinionType;
            add += GetDroneData(selected).DmgModifier;
        }

        public override void GetWeaponKnockback(Player player, ref float knockback)
        {
            AssPlayer mPlayer = player.GetModPlayer<AssPlayer>();

            DroneType selected = mPlayer.selectedDroneControllerMinionType;
            knockback *= GetDroneData(selected).KBModifier;
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
            type = GetDroneData(selected).ProjType;

            AssUtils.NewProjectile(player.Center.X, player.Center.Y, 0f, player.velocity.Y - 6f, type, damage, knockBack, preSync: PreSync);
            return false;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.HallowedBar, 1);
            recipe.AddIngredient(ItemID.Switch, 2);
            recipe.AddIngredient(ItemID.Wire, 10);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            AssPlayer mPlayer = Main.LocalPlayer.GetModPlayer<AssPlayer>();
            DroneType selected = mPlayer.selectedDroneControllerMinionType;

            DroneData data = GetDroneData(selected);

            int damageIndex = -1;
            int knockbackIndex = -1;

            for (int i = 0; i < tooltips.Count; i++)
            {
                if (Main.LocalPlayer.HasItem(mod.ItemType<DroneController>()))
                {
                    if (tooltips[i].mod == "Terraria" && tooltips[i].Name == "ItemName")
                    {
                        tooltips[i].text += " (" + data.Name + ")";
                    }
                }

                if (tooltips[i].mod == "Terraria" && tooltips[i].Name == "Damage")
                {
                    damageIndex = i;
                }

                if (tooltips[i].mod == "Terraria" && tooltips[i].Name == "Knockback")
                {
                    knockbackIndex = i;
                }
            }

            if (damageIndex > -1)
            {
                if (!data.Combat)
                {
                    tooltips.RemoveAt(damageIndex);
                }
            }

            if (knockbackIndex != -1)
            {
                if (data.Combat)
                {
                    if (data.Firerate != "") tooltips.Insert(knockbackIndex, new TooltipLine(mod, "Firerate", data.Firerate + " firerate"));
                }
                else
                {
                    //here damageIndex one is removed, so find knockbackindex again
                    knockbackIndex = tooltips.FindIndex(line => line.Name == "Knockback");
                    if (knockbackIndex > -1) tooltips.RemoveAt(knockbackIndex);
                }
            }

            bool allUnlocked = AllUnlocked(mPlayer.droneControllerUnlocked);

            if (!(allUnlocked && Main.LocalPlayer.HasItem(item.type)))
            {
                tooltips.Add(new TooltipLine(mod, "Destroyer", "Defeat the destroyer to unlock more drones"));
            }

            if (!CanSpawn(Main.LocalPlayer, selected))
            {
                tooltips.Add(new TooltipLine(mod, "CanSpawn", "Only one " + data.Name + " can be out at once"));
            }
        }
    }

    /// <summary>
    /// Holds data about a DroneType
    /// </summary>
    public struct DroneData
    {
        public readonly int ProjType;
        public readonly string Name;
        public string PreviewTextureName
        {
            get
            {
                return "Projectiles/Minions/Drones/" + Name.Replace(" ", "") + "Preview";
            }
        }

        public readonly float DmgModifier;
        public readonly float KBModifier;
        public readonly string UITooltip;
        public readonly string Firerate;
        public readonly bool Combat;

        public DroneData(int projType, string name, string desc, string misc = "", string firerate = "", float dmgModifier = 0f, float kBModifier = 0f, bool combat = true)
        {
            ProjType = projType;
            Name = name;
            DmgModifier = dmgModifier;
            KBModifier = kBModifier;
            Firerate = firerate;
            string stats = combat ? ("\nBase Damage: " + (int)(DroneController.BaseDmg * (DmgModifier + 1f))
             + "\nBase Knockback: " + Math.Round(DroneController.BaseKB * (KBModifier + 1f), 1)) : "";
            UITooltip = Name + stats + "\n" + desc + "\n" + misc;
            Combat = combat;
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
        Shield = 16
        //Unused1 = 8,
        //Unused2 = 16,
        //Unused3 = 32,
    }
}
