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
    public class EverhallowedLantern : MinionItemBase
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Everhallowed Lantern");
            //"Summons a Soul to fight for you" is changed for the appropriate type in ModifyTooltips
            Tooltip.SetDefault("Summons a Soul to fight for you"
                + "\nRight click to cycle through available forms");
        }

        public override void SetDefaults()
        {
            //Defaults for damage, shoot and knockback dont matter too much here, only for the first summon
            //default to PostWol
            item.damage = (int)(CompanionDungeonSoulMinionBase.DefDamage * 1.1f);
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
            item.shoot = mod.ProjectileType<CompanionDungeonSoulPostWOLMinion>();
            item.shootSpeed = 10f;
            item.knockBack = CompanionDungeonSoulMinionBase.DefKnockback;
            item.buffType = mod.BuffType<CompanionDungeonSoulMinionBuff>();
            item.buffTime = 3600;
        }

        public override bool AltFunctionUse(Player player)
        {
            return false;
        }

        public override bool CanUseItem(Player player)
        {
            //if (!Main.hardMode && player.itemTime == 0)
            //{
            //    if (player.altFunctionUse != 2) CombatText.NewText(new Rectangle((int)player.position.X, (int)player.position.Y, player.width, player.height),
            //     CombatText.DamagedFriendly, "Only usable in a hardmode world");
            //    return false;
            //}

            if (player.altFunctionUse == 2 && player.itemTime == 0 && player.whoAmI == Main.myPlayer)
            {
                AssPlayer mPlayer = player.GetModPlayer<AssPlayer>(mod);
                CompanionDungeonSoulMinionBase.SoulStats stats = CompanionDungeonSoulMinionBase.GetAssociatedStats(mod, mPlayer.CycleSoulType()); //<- switch here
                item.damage = stats.Damage;
                item.shoot = stats.Type;
                item.knockBack = stats.Knockback;

                CompanionDungeonSoulMinionBase.SoulType soulType = (CompanionDungeonSoulMinionBase.SoulType)stats.SoulType;
                if(soulType == CompanionDungeonSoulMinionBase.SoulType.Dungeon)
                {
                    CombatText.NewText(new Rectangle((int)player.position.X, (int)player.position.Y, player.width, player.height),
                        CombatText.HealLife, "Selected: " + soulType.ToString() + " Soul");
                }
                else
                {
                    CombatText.NewText(new Rectangle((int)player.position.X, (int)player.position.Y, player.width, player.height),
                        CombatText.HealLife, "Selected: Soul of " + soulType.ToString());
                }
                return true;
            }
            return true;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            if (player.altFunctionUse != 2)
            {
                AssPlayer mPlayer = player.GetModPlayer<AssPlayer>(mod);
                mPlayer.SpawnSoul(item.shoot, item.damage, item.knockBack);
            }
            return false;
        }

        public override void HoldItem(Player player)
        {
            player.itemLocation.X = player.Center.X;
            player.itemLocation.Y = player.Bottom.Y + 2f;
        }

        public override void MoreModifyTooltips(List<TooltipLine> tooltips)
        {
            AssPlayer mPlayer = Main.LocalPlayer.GetModPlayer<AssPlayer>(mod);
            CompanionDungeonSoulMinionBase.SoulType soulType = (CompanionDungeonSoulMinionBase.SoulType)mPlayer.selectedSoulMinionType;

            string soulDesc = "Soul of " + soulType.ToString();
            if (soulType == CompanionDungeonSoulMinionBase.SoulType.Dungeon)
            {
                soulDesc = soulType.ToString() + " Soul";
            }

            TooltipLine line = new TooltipLine(mod, "dummy", "dummy");

            for (int i = 0; i < tooltips.Count; i++)
            {
                if (Main.LocalPlayer.HasItem(mod.ItemType<EverhallowedLantern>()))
                {
                    if (tooltips[i].mod == "Terraria" && tooltips[i].Name == "ItemName")
                    {
                        tooltips[i].text += " (" + soulDesc + ")";
                    }
                }

                if (tooltips[i].mod == "Terraria" && tooltips[i].Name == "Tooltip0")
                {
                    line = tooltips[i];
                }
            }

            if (line.Name != "dummy") tooltips.Remove(line);

            if (!(NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3 && Main.LocalPlayer.HasItem(mod.ItemType<EverhallowedLantern>())))
            {
                tooltips.Add(new TooltipLine(mod, "Mech", "Defeat mechanical bosses to unlock new minions"));
            }

            tooltips.Add(new TooltipLine(mod, "Boost", "30% damage increase from wearing the 'Soul Savior' Set"));
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.SoulofNight, 5);
            recipe.AddIngredient(ItemID.SoulofLight, 5);
            recipe.AddIngredient(mod.ItemType<EverglowLantern>(), 1);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
