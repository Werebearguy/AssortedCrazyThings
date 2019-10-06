using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs
{
    public class PetGoldfishBuff : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Possessed Goldfish");
            Description.SetDefault("A possessed goldfish is following you");
            Main.buffNoTimeDisplay[Type] = true;
            Main.vanityPet[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.buffTime[buffIndex] = 18000;
            player.GetModPlayer<PetPlayer>().PetGoldfish = true;
            bool petProjectileNotSpawned = player.ownedProjectileCounts[ModContent.ProjectileType<PetGoldfishProj>()] <= 0;
            if (petProjectileNotSpawned && player.whoAmI == Main.myPlayer)
            {
                Projectile.NewProjectile(player.position.X + player.width / 2, player.position.Y + player.height / 2, 0f, 0f, ModContent.ProjectileType<PetGoldfishProj>(), 0, 0f, player.whoAmI, 0f, 0f);
            }
        }
    }
}
