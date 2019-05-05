using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs
{
    public class PetDestroyerBuff : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Tiny Destroyer");
            Description.SetDefault("A tiny Destroyer is following you");
            Main.buffNoTimeDisplay[Type] = true;
            Main.vanityPet[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.buffTime[buffIndex] = 18000;
            player.GetModPlayer<PetPlayer>(mod).PetDestroyer = true;
            bool petProjectileNotSpawned = player.ownedProjectileCounts[mod.ProjectileType<PetDestroyerHead>()] <= 0;
            if (petProjectileNotSpawned && player.whoAmI == Main.myPlayer)
            {
                //prevIndex stuff only needed for when replacing/summoning the minion segments individually
                int index = Projectile.NewProjectile(player.Center.X, player.Center.Y, player.direction, -player.gravDir, mod.ProjectileType<PetDestroyerHead>(), 0, 0f, player.whoAmI, 0f, 0f);
                index = Projectile.NewProjectile(player.Center.X - 16 * player.direction, player.Center.Y, 0f, 0f, mod.ProjectileType<PetDestroyerBody1>(), 0, 0f, player.whoAmI, index, 0f);
                int prevIndex = index;
                index = Projectile.NewProjectile(player.Center.X - 32 * player.direction, player.Center.Y, 0f, 0f, mod.ProjectileType<PetDestroyerBody2>(), 0, 0f, player.whoAmI, index, 0f);
                Main.projectile[prevIndex].localAI[1] = index;
                prevIndex = index;
                index = Projectile.NewProjectile(player.Center.X - 48 * player.direction, player.Center.Y, 0f, 0f, mod.ProjectileType<PetDestroyerBody1>(), 0, 0f, player.whoAmI, index, 0f);
                Main.projectile[prevIndex].localAI[1] = index;
                prevIndex = index;
                index = Projectile.NewProjectile(player.Center.X - 64 * player.direction, player.Center.Y, 0f, 0f, mod.ProjectileType<PetDestroyerBody2>(), 0, 0f, player.whoAmI, index, 0f);
                Main.projectile[prevIndex].localAI[1] = index;
                prevIndex = index;
                index = Projectile.NewProjectile(player.Center.X - 72 * player.direction, player.Center.Y, 0f, 0f, mod.ProjectileType<PetDestroyerBody1>(), 0, 0f, player.whoAmI, index, 0f);
                Main.projectile[prevIndex].localAI[1] = index;
                prevIndex = index;
                index = Projectile.NewProjectile(player.Center.X - 88 * player.direction, player.Center.Y, 0f, 0f, mod.ProjectileType<PetDestroyerBody2>(), 0, 0f, player.whoAmI, index, 0f);
                Main.projectile[prevIndex].localAI[1] = index;
                prevIndex = index;
                index = Projectile.NewProjectile(player.Center.X - 104 * player.direction, player.Center.Y, 0f, 0f, mod.ProjectileType<PetDestroyerTail>(), 0, 0f, player.whoAmI, index, 0f);
                Main.projectile[prevIndex].localAI[1] = index;

                //spawn probes
                Projectile.NewProjectile(player.Center.X, player.Center.Y, 0f, 0f, mod.ProjectileType<PetDestroyerProbe>(), 0, 0f, player.whoAmI, 0f, 0f);
                Projectile.NewProjectile(player.Center.X, player.Center.Y, 0f, 0f, mod.ProjectileType<PetDestroyerProbe>(), 0, 0f, player.whoAmI, 0f, (player.whoAmI * 13) % PetDestroyerProbe.AttackDelay);

            }
        }
    }
}
