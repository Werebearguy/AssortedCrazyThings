using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs
{
    public class ChunkyandMeatball : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Chunky and Meatball");
            Description.SetDefault("Two reunited brothers are following you");
            Main.buffNoTimeDisplay[Type] = true;
            Main.vanityPet[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.buffTime[buffIndex] = 18000;
            player.GetModPlayer<PetPlayer>().ChunkyandMeatball = true;
            bool petProjectileNotSpawned = player.ownedProjectileCounts[mod.ProjectileType("ChunkyProj")] <= 0;
            if (petProjectileNotSpawned && player.whoAmI == Main.myPlayer)
            {
                Projectile.NewProjectile(player.position.X + (player.width / 2), player.position.Y + (player.height / 2), 0f, 0f, mod.ProjectileType("ChunkyProj"), 0, 0f, player.whoAmI, 0f, 0f);
                Projectile.NewProjectile(player.position.X + (player.width / 2), player.position.Y, 0f, 0f, mod.ProjectileType("MeatballProj"), 0, 0f, player.whoAmI, 0f, 0f);
            }
        }
    }
}
