using AssortedCrazyThings.Buffs;
using AssortedCrazyThings.Projectiles.Pets;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace AssortedCrazyThings.Items.Pets
{
    public class PetEaterofWorldsItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cracked Worm Egg");
            Tooltip.SetDefault("Summons a tiny Eater of Worlds to follow you");
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.ZephyrFish);
            Item.shoot = ModContent.ProjectileType<PetEaterofWorldsHead>();
            Item.buffType = ModContent.BuffType<PetEaterofWorldsBuff>();
            Item.rare = -11;
        }

        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {
            if (player.itemTime == 0)
            {
                for (int i = 0; i < Main.maxProjectiles; i++)
                {
                    Projectile proj = Main.projectile[i];
                    if (proj.active && proj.owner == player.whoAmI && Array.IndexOf(PetEaterofWorldsBase.wormTypes, proj.type) > -1)
                    {
                        proj.Kill();
                    }
                }
                if (player.whoAmI == Main.myPlayer)
                {
                    Spawn(player, item: Item);

                    player.AddBuff(Item.buffType, 3600, true);
                }
            }
        }

        public static void Spawn(Player player, int buffIndex = -1, Item item = null)
        {
            IProjectileSource source;
            if (buffIndex > -1)
            {
                source = player.GetProjectileSource_Buff(buffIndex);
            }
            else if (item != null)
            {
                source = player.GetProjectileSource_Item(item);
            }
            else
            {
                return;
            }

            //prevIndex stuff only needed for when replacing/summoning the minion segments individually

            int index = Projectile.NewProjectile(source, player.Center.X, player.Center.Y, player.direction, -player.gravDir, ModContent.ProjectileType<PetEaterofWorldsHead>(), 0, 0f, player.whoAmI, 0f, 0f);
            int prevIndex = index;
            int off = PetEaterofWorldsBase.DISTANCE_BETWEEN_SEGMENTS;

            for (int i = 1; i <= PetEaterofWorldsBase.NUMBER_OF_BODY_SEGMENTS; i++)
            {
                index = Projectile.NewProjectile(source, player.Center.X - off * player.direction, player.Center.Y, 0f, 0f, ModContent.ProjectileType<PetEaterofWorldsBody1>(), 0, 0f, player.whoAmI, index, 0f);
                Main.projectile[prevIndex].localAI[1] = index;
                prevIndex = index;
                off += PetEaterofWorldsBase.DISTANCE_BETWEEN_SEGMENTS;
                index = Projectile.NewProjectile(source, player.Center.X - off * player.direction, player.Center.Y, 0f, 0f, ModContent.ProjectileType<PetEaterofWorldsBody2>(), 0, 0f, player.whoAmI, index, 0f);
                Main.projectile[prevIndex].localAI[1] = index;
                prevIndex = index;
                off += PetEaterofWorldsBase.DISTANCE_BETWEEN_SEGMENTS;
            }
            index = Projectile.NewProjectile(source, player.Center.X - off * player.direction, player.Center.Y, 0f, 0f, ModContent.ProjectileType<PetEaterofWorldsTail>(), 0, 0f, player.whoAmI, index, 0f);
            Main.projectile[prevIndex].localAI[1] = index;
        }
    }
}
