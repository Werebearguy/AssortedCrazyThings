using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets
{
    public class FairySwarmBuff : SimplePetBuffBase
    {
        public override int PetType => ModContent.ProjectileType<FairySwarmProj>();

        public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().FairySwarm;

        public override void SafeSetDefaults()
        {
            DisplayName.SetDefault("Fairy Swarm");
            Description.SetDefault("A Fairy Swarm is following you");
        }
    }
}
