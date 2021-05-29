using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs
{
    public class GobletBuff : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Goblet");
            Description.SetDefault("A tiny Goblin is following you");
            Main.buffNoTimeDisplay[Type] = true;
            Main.vanityPet[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.buffTime[buffIndex] = 18000;
            player.GetModPlayer<PetPlayer>().Goblet = true;
            bool petProjectileNotSpawned = player.ownedProjectileCounts[ModContent.ProjectileType<GobletProj>()] <= 0;
            if (petProjectileNotSpawned && player.whoAmI == Main.myPlayer)
            {
                int i = Projectile.NewProjectile(player.GetProjectileSource_Buff(buffIndex), player.position.X + player.width / 2, player.position.Y, 0f, 0f, ModContent.ProjectileType<GobletProj>(), 0, 0f, player.whoAmI, 0f, 0f);
            }
        }
    }
}
