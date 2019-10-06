using AssortedCrazyThings.Base;
using AssortedCrazyThings.Buffs;
using AssortedCrazyThings.Projectiles.Minions.CompanionDungeonSouls;
using AssortedCrazyThings.UI;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Weapons
{

    /// <summary>
    /// Holds the type of Dungeon Soul (All just for Everhallowed Lantern)
    /// </summary>
    [Flags]
    public enum SoulType : byte
    {
        None = 0,
        Dungeon = 1,
        Fright = 2,
        Sight = 4,
        Might = 8,
    }

    /// <summary>
    /// Holds data about a SoulType
    /// </summary>
    public struct SoulData
    {
        public readonly int ProjType;
        public readonly string Name;

        public readonly float DmgModifier;
        public readonly float KBModifier;
        public readonly string Tooltip;
        public readonly string ToUnlock;
        public readonly Func<bool> Unlocked;

        public SoulData(int projType, string name, string desc = "", string toUnlock = "", Func<bool> unlocked = null, float dmgModifier = 0f, float kBModifier = 0f)
        {
            ProjType = projType;
            Name = name;
            DmgModifier = dmgModifier;
            KBModifier = kBModifier;
            ToUnlock = toUnlock;
            Unlocked = unlocked ?? (() => true);
            string stats = "\nBase Damage: " + (int)(EverhallowedLantern.BaseDmg * (DmgModifier + 1f))
             + "\nBase Knockback: " + Math.Round(EverhallowedLantern.BaseKB * (KBModifier + 1f), 1);
            Tooltip = Name + stats + "\n" + desc;
        }
    }

    public class EverhallowedLantern : MinionItemBase
    {
        public const int BaseDmg = 26;
        public const float BaseKB = 0.5f;

        #region Static Methods
        /// <summary>
        /// Sets the data up for a SoulType
        /// </summary>
        public static SoulData SetSoulData(SoulType selected)
        {
            switch (selected)
            {
                case SoulType.Dungeon:
                    return new SoulData
                        (
                        projType: ModContent.ProjectileType<CompanionDungeonSoulPostWOFMinion>(),
                        name: "Dungeon Soul",
                        dmgModifier: 0.1f
                        );
                case SoulType.Fright:
                    return new SoulData
                        (
                        projType: ModContent.ProjectileType<CompanionDungeonSoulFrightMinion>(),
                        name: "Soul of Fright",
                        desc: "Inflicts Ichor and Posioned",
                        toUnlock: "Defeat Skeletron Prime",
                        unlocked: () => NPC.downedMechBoss3,
                        dmgModifier: 0.25f,
                        kBModifier: 3f
                        );
                case SoulType.Sight:
                    return new SoulData
                        (
                        projType: ModContent.ProjectileType<CompanionDungeonSoulSightMinion>(),
                        name: "Soul of Sight",
                        desc: "Inflicts Cursed Inferno",
                        toUnlock: "Defeat The Twins",
                        unlocked: () => NPC.downedMechBoss2,
                        dmgModifier: -0.15f
                        );
                case SoulType.Might:
                    return new SoulData
                        (
                        projType: ModContent.ProjectileType<CompanionDungeonSoulMightMinion>(),
                        name: "Soul of Might",
                        toUnlock: "Defeat The Destroyer",
                        unlocked: () => NPC.downedMechBoss1,
                        dmgModifier: 0.55f,
                        kBModifier: 7f
                        );
                default:
                    throw new Exception("No SoulData specified");
            }
        }

        /// <summary>
        /// Holds data about each SoulType
        /// </summary>
        public static SoulData[] DataList;

        /// <summary>
        /// Used to access a particular SoulTypes data
        /// </summary>
        public static SoulData GetSoulData(SoulType selected)
        {
            return DataList[(int)Math.Log((int)selected, 2)];
        }

        public static CircleUIConf GetUIConf()
        {
            List<string> tooltips = new List<string>();
            List<string> toUnlock = new List<string>();
            List<string> textureNames = new List<string>();
            List<bool> unlocked = new List<bool>();

            foreach (SoulType type in Enum.GetValues(typeof(SoulType)))
            {
                if (type != SoulType.None)
                {
                    SoulData data = GetSoulData(type);
                    textureNames.Add(Main.projectileTexture[data.ProjType].Name);
                    unlocked.Add(data.Unlocked());
                    tooltips.Add(data.Tooltip);
                    toUnlock.Add(data.ToUnlock);
                }
            }

            return new CircleUIConf(8, -1, textureNames, unlocked, tooltips, toUnlock);
        }

        /// <summary>
        /// Called in Mod.Load
        /// </summary>
        public static void Load()
        {
            Array a = Enum.GetValues(typeof(SoulType));
            DataList = new SoulData[a.Length - 1]; //without None
            int i = 0;
            foreach (SoulType type in a)
            {
                if (type != SoulType.None)
                {
                    DataList[i++] = SetSoulData(type);
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
            DisplayName.SetDefault("Everhallowed Lantern");
            //"Summons a Soul to fight for you" is changed for the appropriate type in ModifyTooltips
            Tooltip.SetDefault("Summons a Soul to fight for you"
                + "\nRight click to pick from available forms");
        }

        public override void SetDefaults()
        {
            //Defaults for damage, shoot and knockback dont matter too much here, only for the first summon
            //default to PostWol
            item.damage = BaseDmg;
            item.knockBack = BaseKB;
            item.summon = true;
            item.mana = 10;
            item.width = 26;
            item.height = 40;
            item.useTime = 30;
            item.useAnimation = 30;
            item.useStyle = 4; //4 for life crystal
            item.noMelee = true;
            item.value = Item.sellPrice(0, 0, 95, 0);
            item.rare = -11;
            item.UseSound = SoundID.Item44;
            item.shoot = ModContent.ProjectileType<CompanionDungeonSoulPostWOFMinion>();
            item.shootSpeed = 10f;
            item.buffType = ModContent.BuffType<CompanionDungeonSoulMinionBuff>();
        }

        public override void ModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat)
        {
            AssPlayer mPlayer = player.GetModPlayer<AssPlayer>();

            SoulType selected = mPlayer.selectedSoulMinionType;
            add += GetSoulData(selected).DmgModifier;
        }

        public override void GetWeaponKnockback(Player player, ref float knockback)
        {
            AssPlayer mPlayer = player.GetModPlayer<AssPlayer>();

            SoulType selected = mPlayer.selectedSoulMinionType;
            knockback *= GetSoulData(selected).KBModifier;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            AssPlayer mPlayer = player.GetModPlayer<AssPlayer>();
            SoulType selected = mPlayer.selectedSoulMinionType;
            type = GetSoulData(selected).ProjType;

            Vector2 spawnPos = new Vector2(player.Center.X + player.direction * 8f, player.Bottom.Y - 12f);
            Vector2 spawnVelo = new Vector2(player.velocity.X + player.direction * 1.5f, player.velocity.Y - 1f);

            Projectile.NewProjectile(spawnPos, spawnVelo, type, damage, knockBack, Main.myPlayer, 0f, 0f);
            return false;
        }

        public override void HoldItem(Player player)
        {
            player.itemLocation.X = player.Center.X;
            player.itemLocation.Y = player.Bottom.Y + 2f;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            AssPlayer mPlayer = Main.LocalPlayer.GetModPlayer<AssPlayer>();
            SoulType selected = mPlayer.selectedSoulMinionType;

            SoulData data = GetSoulData(selected);

            TooltipLine line = new TooltipLine(mod, "dummy", "dummy");

            for (int i = 0; i < tooltips.Count; i++)
            {
                if (Main.LocalPlayer.HasItem(ModContent.ItemType<EverhallowedLantern>()))
                {
                    if (tooltips[i].mod == "Terraria" && tooltips[i].Name == "ItemName")
                    {
                        tooltips[i].text += " (" + data.Name + ")";
                    }
                }

                if (tooltips[i].mod == "Terraria" && tooltips[i].Name == "Tooltip0")
                {
                    line = tooltips[i];
                }
            }

            int tooltipIndex = tooltips.FindLastIndex(l => l.Name.StartsWith("Tooltip"));

            if (line.Name != "dummy") tooltips.Remove(line);

            bool allUnlocked = true;
            foreach (SoulType type in Enum.GetValues(typeof(SoulType)))
            {
                if (type != SoulType.None)
                {
                    data = GetSoulData(type);
                    if (!data.Unlocked())
                    {
                        allUnlocked = false;
                    }
                }
            }

            if (!(allUnlocked && Main.LocalPlayer.HasItem(item.type)))
            {
                tooltips.Insert(tooltipIndex++, new TooltipLine(mod, "Mech", "Defeat mechanical bosses to unlock new minions"));
            }

            tooltips.Insert(tooltipIndex++, new TooltipLine(mod, "Boost", "30% damage increase from wearing the 'Soul Savior' Set"));
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.SoulofNight, 5);
            recipe.AddIngredient(ItemID.SoulofLight, 5);
            recipe.AddIngredient(ModContent.ItemType<EverglowLantern>(), 1);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
