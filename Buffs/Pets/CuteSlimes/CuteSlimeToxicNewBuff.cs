using AssortedCrazyThings.Projectiles.Pets.CuteSlimes;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets.CuteSlimes
{
    public class CuteSlimeToxicNewBuff : SimplePetBuffBase
    {
        public override int PetType => ModContent.ProjectileType<CuteSlimeToxicNewProj>();

        public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().CuteSlimeToxicNew;

        public override void SafeSetDefaults()
        {
            DisplayName.SetDefault("Cute Toxic Slime");
            Description.SetDefault("A cute toxic slime girl is following you");
        }
    }
}
