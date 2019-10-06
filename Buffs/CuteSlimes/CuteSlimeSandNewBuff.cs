using AssortedCrazyThings.Projectiles.Pets.CuteSlimes;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.CuteSlimes
{
    public class CuteSlimeSandNewBuff : CuteSlimeBaseBuff
    {
        protected override void MoreSetDefaults()
        {
            DisplayName.SetDefault("Cute Sand Slime");
            Description.SetDefault("A cute sand slime girl is following you");
        }

        protected override void MoreUpdate(PetPlayer mPlayer)
        {
            mPlayer.CuteSlimeSandNew = true;
            projType = ModContent.ProjectileType<CuteSlimeSandNewProj>();
        }
    }
}
