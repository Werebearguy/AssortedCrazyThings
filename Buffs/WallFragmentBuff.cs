using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs
{
    public class WallFragmentBuff : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Wall Fragment");
            Description.SetDefault("Several fragments of the Wall are following you");
            Main.buffNoTimeDisplay[Type] = true;
            Main.vanityPet[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.buffTime[buffIndex] = 18000;
            player.GetModPlayer<PetPlayer>(mod).WallFragment = true;
            bool petProjectileNotSpawned = player.ownedProjectileCounts[mod.ProjectileType<WallFragmentMouth>()] <= 0;
            if (petProjectileNotSpawned && player.whoAmI == Main.myPlayer)
            {
                Projectile.NewProjectile(player.Center.X, player.position.Y - 6f               , player.direction * 0.75f, -0.5f, mod.ProjectileType<WallFragmentEye1>(), 0, 0f, player.whoAmI, 0f, 0f);
                Projectile.NewProjectile(player.Center.X, player.position.Y + player.height / 2, player.direction, 0f, mod.ProjectileType<WallFragmentMouth>(), 0, 0f, player.whoAmI, 0f, 0f);
                Projectile.NewProjectile(player.Center.X, player.position.Y + player.height +6f, player.direction * 0.75f, 0.5f, mod.ProjectileType<WallFragmentEye2>(), 0, 0f, player.whoAmI, 0f, 0f);
            }
        }
    }
}
