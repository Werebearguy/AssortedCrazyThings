using AssortedCrazyThings.Items.Pets;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs
{
    public class PetEaterofWorldsBuff : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Tiny Eater of Worlds");
            Description.SetDefault("A tiny Eater of Worlds is following you");
            Main.buffNoTimeDisplay[Type] = true;
            Main.vanityPet[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.buffTime[buffIndex] = 18000;
            player.GetModPlayer<PetPlayer>(mod).PetEaterofWorlds = true;
            bool petProjectileNotSpawned = player.ownedProjectileCounts[mod.ProjectileType<PetEaterofWorldsHead>()] <= 0;
            if (petProjectileNotSpawned && player.whoAmI == Main.myPlayer)
            {
                PetEaterofWorldsItem.Spawn(player);
            }
        }
    }
}
