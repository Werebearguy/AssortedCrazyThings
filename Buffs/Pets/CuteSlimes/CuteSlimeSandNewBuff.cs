using AssortedCrazyThings.Projectiles.Pets.CuteSlimes;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets.CuteSlimes
{
    public class CuteSlimeSandNewBuff : SimplePetBuffBase
    {
        public override int PetType => ModContent.ProjectileType<CuteSlimeSandNewProj>();

        public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().CuteSlimeSandNew;

        public override void SafeSetDefaults()
        {
            DisplayName.SetDefault("Cute Sand Slime");
            Description.SetDefault("A cute sand slime girl is following you");
        }
    }
}
