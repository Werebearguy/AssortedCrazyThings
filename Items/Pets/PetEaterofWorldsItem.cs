using AssortedCrazyThings.Projectiles.Pets;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
    public class PetEaterofWorldsItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tiny Eater of worlds");
            Tooltip.SetDefault("Summons a tiny Eater of Worlds to follow you");
        }

        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.ZephyrFish);
            item.shoot = mod.ProjectileType("PetEaterofWorldsHead");
            item.buffType = mod.BuffType("PetEaterofWorldsBuff");
            item.rare = -11;
        }

        public override void UseStyle(Player player)
        {
            if (player.itemTime == 0)
            {
                for (int i = 0; i < Main.maxProjectiles; i++)
                {
                    Projectile proj = Main.projectile[i];
                    if (proj.active && proj.owner == player.whoAmI && Array.IndexOf(PetEaterofWorldsBase.wormTypes, proj.type) >= 0)
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
            int index = Projectile.NewProjectile(player.Center.X, player.Center.Y, player.direction, -player.gravDir, PetEaterofWorldsBase.wormTypes[0], 0, 0f, player.whoAmI, 0f, 0f);
            index = Projectile.NewProjectile(player.Center.X - 16 * player.direction, player.Center.Y, 0f, 0f, PetEaterofWorldsBase.wormTypes[1], 0, 0f, player.whoAmI, index, 0f);
            int prevIndex = index;
            index = Projectile.NewProjectile(player.Center.X - 32 * player.direction, player.Center.Y, 0f, 0f, PetEaterofWorldsBase.wormTypes[2], 0, 0f, player.whoAmI, index, 0f);
            Main.projectile[prevIndex].localAI[1] = index;
            prevIndex = index;
            index = Projectile.NewProjectile(player.Center.X - 48 * player.direction, player.Center.Y, 0f, 0f, PetEaterofWorldsBase.wormTypes[3], 0, 0f, player.whoAmI, index, 0f);
            Main.projectile[prevIndex].localAI[1] = index;
        }
    }
}
