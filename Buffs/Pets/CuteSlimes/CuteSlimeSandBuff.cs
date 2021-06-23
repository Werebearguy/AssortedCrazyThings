using AssortedCrazyThings.Projectiles.Pets.CuteSlimes;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets.CuteSlimes
{
    public class CuteSlimeSandBuff : CuteSlimeBuffBase
    {
        public override int PetType => ModContent.ProjectileType<CuteSlimeSandProj>();

        public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().CuteSlimeSand;

        public override void SafeSetDefaults()
        {
            DisplayName.SetDefault("Cute Sand Slime");
            Description.SetDefault("A cute sand slime girl is following you");
        }
    }
}
