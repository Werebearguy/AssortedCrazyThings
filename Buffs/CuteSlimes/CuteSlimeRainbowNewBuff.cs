using AssortedCrazyThings.Projectiles.Pets.CuteSlimes;

namespace AssortedCrazyThings.Buffs.CuteSlimes
{
    public class CuteSlimeRainbowNewBuff : CuteSlimeBaseBuff
    {
        protected override void MoreSetDefaults()
        {
            DisplayName.SetDefault("Cute Rainbow Slime");
            Description.SetDefault("A cute rainbow slime girl is following you");
        }

        protected override void MoreUpdate(PetPlayer mPlayer)
        {
            mPlayer.CuteSlimeRainbowNew = true;
            projType = mod.ProjectileType<CuteSlimeRainbowNewProj>();
        }
    }
}
