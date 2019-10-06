using AssortedCrazyThings.Projectiles.Pets.CuteSlimes;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.CuteSlimes
{
    public class CuteSlimeIlluminantNewBuff : CuteSlimeBaseBuff
    {
        protected override void MoreSetDefaults()
        {
            DisplayName.SetDefault("Cute Illumimant Slime");
            Description.SetDefault("A cute illumimant slime girl is following you");
        }

        protected override void MoreUpdate(PetPlayer mPlayer)
        {
            mPlayer.CuteSlimeIlluminantNew = true;
            projType = ModContent.ProjectileType<CuteSlimeIlluminantNewProj>();
        }
    }
}
