using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets
{
    public class ChunkyandMeatballBuff : SimplePetBuffBase
    {
        public override int PetType => ModContent.ProjectileType<ChunkyProj>();

        public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().ChunkyandMeatball;

        public override void SafeSetDefaults()
        {
            DisplayName.SetDefault("Chunky and Meatball");
            Description.SetDefault("Two reunited brothers are following you");
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.buffTime[buffIndex] = 18000;
            PetBool(player) = true;
            if (player.whoAmI == Main.myPlayer)
            {
                int chunky = ModContent.ProjectileType<ChunkyProj>();
                bool chunkyNotSpawned = player.ownedProjectileCounts[chunky] <= 0;
                int meatball = ModContent.ProjectileType<MeatballProj>();
                bool meatballNotSpawned = player.ownedProjectileCounts[meatball] <= 0;
                if (chunkyNotSpawned)
                {
                    Projectile.NewProjectile(player.GetProjectileSource_Buff(buffIndex), player.position.X + (player.width / 2), player.position.Y + (player.height / 2), 0f, 0f, chunky, 0, 0f, player.whoAmI);
                }
                if (meatballNotSpawned)
                {
                    Projectile.NewProjectile(player.GetProjectileSource_Buff(buffIndex), player.position.X + (player.width / 2), player.position.Y, 0f, 0f, meatball, 0, 0f, player.whoAmI);
                }
            }
        }
    }
}
