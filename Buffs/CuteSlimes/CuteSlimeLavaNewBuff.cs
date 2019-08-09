using AssortedCrazyThings.Projectiles.Pets.CuteSlimes;

namespace AssortedCrazyThings.Buffs.CuteSlimes
{
    public class CuteSlimeLavaNewBuff : CuteSlimeBaseBuff
    {
        protected override void MoreSetDefaults()
        {
            DisplayName.SetDefault("Cute Lava Slime");
            Description.SetDefault("A cute lava slime girl is following you");
        }

        protected override void MoreUpdate(PetPlayer mPlayer)
        {
            mPlayer.CuteSlimeLavaNew = true;
            projType = mod.ProjectileType<CuteSlimeLavaNewProj>();
        }
    }
}
