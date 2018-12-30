using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs
{
    public class GobletBuff : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Cute Green Slime");
            Description.SetDefault("A cute green slime girl is following you.");
            Main.buffNoTimeDisplay[Type] = true;
            Main.vanityPet[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.buffTime[buffIndex] = 18000;
            player.GetModPlayer<PetPlayer>(mod).GobletPet = true;
            bool petProjectileNotSpawned = player.ownedProjectileCounts[mod.ProjectileType<GobletPet>()] <= 0;
            if (petProjectileNotSpawned && player.whoAmI == Main.myPlayer)
            {
                int i = Projectile.NewProjectile(player.position.X + (float)(player.width / 2), player.position.Y, 0f, 0f, mod.ProjectileType<GobletPet>(), 0, 0f, player.whoAmI, 0f, 0f);
            }
        }
    }
}
