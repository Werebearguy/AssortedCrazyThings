using AssortedCrazyThings.Projectiles.Pets.CuteSlimes;

namespace AssortedCrazyThings.Buffs.CuteSlimes
{
    public class CuteSlimeBlueNewBuff : CuteSlimeBaseBuff
    {
        protected override void MoreSetDefaults()
        {
            DisplayName.SetDefault("Cute Blue Slime");
            Description.SetDefault("A cute blue slime girl is following you");
        }

        protected override void MoreUpdate(PetPlayer mPlayer)
        {
            mPlayer.CuteSlimeBlueNew = true;
            projType = mod.ProjectileType<CuteSlimeBlueNewProj>();
        }
    }
}
