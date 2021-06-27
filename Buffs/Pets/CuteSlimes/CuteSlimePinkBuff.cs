using AssortedCrazyThings.Projectiles.Pets.CuteSlimes;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets.CuteSlimes
{
    public class CuteSlimePinkBuff : CuteSlimeBuffBase
    {
        public override int PetType => ModContent.ProjectileType<CuteSlimePinkProj>();

        public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().CuteSlimePink;

        public override void SafeSetDefaults()
        {
            DisplayName.SetDefault("Cute Pink Slime");
            Description.SetDefault("A cute pink slime is following you");
        }
    }
}
