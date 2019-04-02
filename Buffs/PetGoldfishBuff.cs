using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs
{
    public class PetGoldfishBuff : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Pet Goldfish");
            Description.SetDefault("A goldfish is following you"
                + "\n'It only wants to swim with you!'");
            Main.buffNoTimeDisplay[Type] = true;
            Main.vanityPet[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.buffTime[buffIndex] = 18000;
            player.GetModPlayer<PetPlayer>(mod).PetGoldfish = true;
            bool petProjectileNotSpawned = player.ownedProjectileCounts[mod.ProjectileType<PetGoldfishProj>()] <= 0;
            if (petProjectileNotSpawned && player.whoAmI == Main.myPlayer)
            {
                Projectile.NewProjectile(player.position.X + player.width / 2, player.position.Y + player.height / 2, 0f, 0f, mod.ProjectileType<PetGoldfishProj>(), 0, 0f, player.whoAmI, 0f, 0f);
            }
        }
    }
}
