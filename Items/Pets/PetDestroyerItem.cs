using AssortedCrazyThings.Buffs;
using AssortedCrazyThings.Projectiles.Pets;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
    public class PetDestroyerItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Destroyer's Core");
            Tooltip.SetDefault("Summons a tiny Destroyer and two tiny Probes to follow you");
        }

        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.ZephyrFish);
            item.shoot = ModContent.ProjectileType<PetDestroyerHead>();
            item.buffType = ModContent.BuffType<PetDestroyerBuff>();
            item.rare = -11;
        }

        public override void UseStyle(Player player)
        {
            if (player.itemTime == 0)
            {
                for (int i = 0; i < Main.maxProjectiles; i++)
                {
                    Projectile proj = Main.projectile[i];
                    if (proj.active && proj.owner == player.whoAmI && (Array.IndexOf(PetDestroyerBase.wormTypes, proj.type) > -1 || proj.type == ModContent.ProjectileType<PetDestroyerProbe>()))
                    {
                        proj.Kill();
                    }
                }
                if (player.whoAmI == Main.myPlayer)
                {
                    Spawn(player);

                    player.AddBuff(item.buffType, 3600, true);
                }
            }
        }

        public static void Spawn(Player player)
        {
            //prevIndex stuff only needed for when replacing/summoning the minion segments individually

            int index = Projectile.NewProjectile(player.Center.X, player.Center.Y, player.direction, -player.gravDir, ModContent.ProjectileType<PetDestroyerHead>(), 0, 0f, player.whoAmI, 0f, 0f);
            int prevIndex = index;
            int off = PetDestroyerBase.DISTANCE_BETWEEN_SEGMENTS;

            for (int i = 1; i <= PetDestroyerBase.NUMBER_OF_BODY_SEGMENTS; i++)
            {
                index = Projectile.NewProjectile(player.Center.X - off * player.direction, player.Center.Y, 0f, 0f, ModContent.ProjectileType<PetDestroyerBody1>(), 0, 0f, player.whoAmI, index, 0f);
                Main.projectile[prevIndex].localAI[1] = index;
                prevIndex = index;
                off += PetDestroyerBase.DISTANCE_BETWEEN_SEGMENTS;
                index = Projectile.NewProjectile(player.Center.X - off * player.direction, player.Center.Y, 0f, 0f, ModContent.ProjectileType<PetDestroyerBody2>(), 0, 0f, player.whoAmI, index, 0f);
                Main.projectile[prevIndex].localAI[1] = index;
                prevIndex = index;
                off += PetDestroyerBase.DISTANCE_BETWEEN_SEGMENTS;
            }
            index = Projectile.NewProjectile(player.Center.X - off * player.direction, player.Center.Y, 0f, 0f, ModContent.ProjectileType<PetDestroyerTail>(), 0, 0f, player.whoAmI, index, 0f);
            Main.projectile[prevIndex].localAI[1] = index;

            //spawn probes
            Projectile.NewProjectile(player.Center.X, player.Center.Y, 0f, 0f, ModContent.ProjectileType<PetDestroyerProbe>(), 0, 0f, player.whoAmI, 0f, 0f);
            Projectile.NewProjectile(player.Center.X, player.Center.Y, 0f, 0f, ModContent.ProjectileType<PetDestroyerProbe>(), 0, 0f, player.whoAmI, 0f, ((player.whoAmI + 1) * 13) % PetDestroyerProbe.AttackDelay);
        }
    }
}
