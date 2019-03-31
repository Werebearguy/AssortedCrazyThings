using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs
{
    public class ObservingEyeBuff : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Observing Eye");
            Description.SetDefault("A Tiny Eye Of Cthulhu is following you");
            Main.buffNoTimeDisplay[Type] = true;
            Main.vanityPet[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.buffTime[buffIndex] = 18000;
            PetPlayer mPlayer = player.GetModPlayer<PetPlayer>(mod);
            player.GetModPlayer<PetPlayer>(mod).ObservingEye = true;
            bool petProjectileNotSpawned = player.ownedProjectileCounts[mod.ProjectileType<ObservingEyeProj>()] <= 0;
            if (petProjectileNotSpawned && player.whoAmI == Main.myPlayer)
            {
                int i = Projectile.NewProjectile(player.position.X + (player.width / 2), player.position.Y, -player.direction, -0.5f, mod.ProjectileType<ObservingEyeProj>(), 0, 0f, player.whoAmI, 0f, 0f);
            }
        }
    }
}
