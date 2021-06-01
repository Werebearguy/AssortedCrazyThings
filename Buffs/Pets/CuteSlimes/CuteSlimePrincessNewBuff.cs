using AssortedCrazyThings.Projectiles.Pets.CuteSlimes;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets.CuteSlimes
{
    public class CuteSlimePrincessNewBuff : SimplePetBuffBase
    {
        public override int PetType => ModContent.ProjectileType<CuteSlimePrincessNewProj>();

        public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().CuteSlimePrincessNew;

        public override void SafeSetDefaults()
        {
            DisplayName.SetDefault("Cute Princess Slime");
            Description.SetDefault("A cute princess slime girl is following you");
        }
    }
}
