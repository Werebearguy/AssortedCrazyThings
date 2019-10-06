using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs
{
    public class PigronataBuff : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Pigronata");
            Description.SetDefault("A Pigronata is thankful that you did not bust it");
            Main.buffNoTimeDisplay[Type] = true;
            Main.vanityPet[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.buffTime[buffIndex] = 18000;
            player.GetModPlayer<PetPlayer>().Pigronata = true;
            bool petProjectileNotSpawned = player.ownedProjectileCounts[mod.ProjectileType("Pigronata")] <= 0;
            if (petProjectileNotSpawned && player.whoAmI == Main.myPlayer)
            {
                Projectile.NewProjectile(player.position.X + (float)(player.width / 2), player.position.Y + (float)(player.height / 2), 0f, 0f, mod.ProjectileType("Pigronata"), 0, 0f, player.whoAmI, 0f, 0f);
            }
        }
    }
}
