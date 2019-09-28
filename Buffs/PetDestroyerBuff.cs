using AssortedCrazyThings.Items.Pets;
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
            Description.SetDefault("A tiny Destroyer and two tiny Probes are following you");
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
                PetDestroyerItem.Spawn(player);
            }
        }
    }
}
