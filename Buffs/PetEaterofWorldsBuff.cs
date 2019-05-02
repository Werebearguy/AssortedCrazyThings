using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs
{
    public class PetEaterofWorldsBuff : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Tiny Eater of Worlds");
            Description.SetDefault("A tiny Eater of Worlds is following you");
            Main.buffNoTimeDisplay[Type] = true;
            Main.vanityPet[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.buffTime[buffIndex] = 18000;
            player.GetModPlayer<PetPlayer>(mod).PetEaterofWorlds = true;
            bool petProjectileNotSpawned = player.ownedProjectileCounts[mod.ProjectileType<PetEaterofWorldsHead>()] <= 0;
            if (petProjectileNotSpawned && player.whoAmI == Main.myPlayer)
            {
                //prevIndex stuff only needed for when replacing/summoning the minion segments individually
                int index = Projectile.NewProjectile(player.Center.X, player.Center.Y, player.direction, -player.gravDir, mod.ProjectileType<PetEaterofWorldsHead>(), 0, 0f, player.whoAmI, 0f, 0f);
                index = Projectile.NewProjectile(player.Center.X - 16 * player.direction, player.Center.Y, 0f, 0f, mod.ProjectileType<PetEaterofWorldsBody1>(), 0, 0f, player.whoAmI, index, 0f);
                int prevIndex = index;
                index = Projectile.NewProjectile(player.Center.X - 32 * player.direction, player.Center.Y, 0f, 0f, mod.ProjectileType<PetEaterofWorldsBody2>(), 0, 0f, player.whoAmI, index, 0f);
                Main.projectile[prevIndex].localAI[1] = index;
                prevIndex = index;
                index = Projectile.NewProjectile(player.Center.X - 48 * player.direction, player.Center.Y, 0f, 0f, mod.ProjectileType<PetEaterofWorldsTail>(), 0, 0f, player.whoAmI, index, 0f);
                Main.projectile[prevIndex].localAI[1] = index;
            }
        }
    }
}
