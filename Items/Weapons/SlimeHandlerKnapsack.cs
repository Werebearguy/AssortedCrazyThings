using AssortedCrazyThings.Base;
using AssortedCrazyThings.Buffs;
using AssortedCrazyThings.Projectiles.Minions;
using AssortedCrazyThings.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Weapons
{
    public class SlimeHandlerKnapsack : MinionItemBase
    {
        public static CircleUIConf GetUIConf()
        {
            List<Asset<Texture2D>> assets = new List<Asset<Texture2D>>() {
                        AssUtils.Instance.GetTexture("Projectiles/Minions/SlimePackMinions/SlimeMinionPreview"),
                        AssUtils.Instance.GetTexture("Projectiles/Minions/SlimePackMinions/SlimeMinionAssortedPreview"),
                        AssUtils.Instance.GetTexture("Projectiles/Minions/SlimePackMinions/SlimeMinionSpikedPreview") };
            List<string> tooltips = new List<string>
                    {
                        "Default"
                        + "\nBase Damage: " + SlimePackMinion.DefDamage
                        + "\nBase Knockback: " + SlimePackMinion.DefKnockback,
                        "Assorted"
                        + "\nBase Damage: " + SlimePackMinion.DefDamage
                        + "\nBase Knockback: " + SlimePackMinion.DefKnockback,
                        "Spiked"
                        + "\nBase Damage: " + Math.Round(SlimePackMinion.DefDamage * (SlimePackMinion.SpikedIncrease + 1))
                        + "\nBase Knockback: " + Math.Round(SlimePackMinion.DefKnockback * (SlimePackMinion.SpikedIncrease + 1), 1)
                        + "\nShoots spikes while fighting"
                    };
            List<string> toUnlock = new List<string>() { "Default", "Default", "Defeat Plantera" };

            List<bool> unlocked = new List<bool>()
                    {
                        true,                // 0
                        true,                // 1
                        NPC.downedPlantBoss, // 2
                    };

            return new CircleUIConf(0, -1, assets, unlocked, tooltips, toUnlock, drawOffset: new Vector2(0f, -2f));
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Slime Handler Knapsack");
            Tooltip.SetDefault("Summons a friendly Slime of a random color from your Knapsack to fight for you"
                + "\nRight click to pick from available forms");
        }

        public override void SetDefaults()
        {
            //change damage in SlimePackMinion.cs
            Item.damage = SlimePackMinion.DefDamage;
            Item.DamageType = DamageClass.Summon;
            Item.mana = 10;
            Item.width = 24;
            Item.height = 30;
            Item.useTime = 36;
            Item.useAnimation = 36;
            Item.useStyle = ItemUseStyleID.HoldUp; //4 for life crystal
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.value = Item.sellPrice(0, 0, 75, 0);
            Item.rare = -11;
            Item.UseSound = SoundID.Item44;
            Item.shoot = ModContent.ProjectileType<SlimePackMinion>();
            Item.shootSpeed = 10f;
            Item.knockBack = SlimePackMinion.DefKnockback;
            Item.buffType = ModContent.BuffType<SlimePackMinionBuff>();
        }

        public override void ModifyWeaponKnockback(Player player, ref StatModifier knockback, ref float flat)
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

        public override void ModifyWeaponDamage(Player player, ref StatModifier damage, ref float flat)
        {
            AssPlayer mPlayer = player.GetModPlayer<AssPlayer>();
            if (mPlayer.selectedSlimePackMinionType == 2)
            {
                damage += SlimePackMinion.SpikedIncrease;
            }
        }

        public override bool Shoot(Player player, ProjectileSource_Item_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            player.AddBuff(Item.buffType, 2);

            AssPlayer mPlayer = player.GetModPlayer<AssPlayer>();
            if (mPlayer.selectedSlimePackMinionType == 1)
            {
                type = ModContent.ProjectileType<SlimePackAssortedMinion>();
            }
            else if (mPlayer.selectedSlimePackMinionType == 2)
            {
                type = ModContent.ProjectileType<SlimePackSpikedMinion>();
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
            int index = Projectile.NewProjectile(source, spawnPos.X, spawnPos.Y, -player.velocity.X, player.velocity.Y - 6f, type, damage, knockback, Main.myPlayer, 0f, 0f);
            Main.projectile[index].originalDamage = damage;
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.SlimeCrown, 1).AddIngredient(ItemID.Gel, 200).AddIngredient(ItemID.SoulofLight, 5).AddIngredient(ItemID.SoulofNight, 5).AddTile(TileID.MythrilAnvil).Register();
        }
    }
}
