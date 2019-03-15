using AssortedCrazyThings.Projectiles.Pets;

namespace AssortedCrazyThings.Buffs
{
    public class CuteSlimeCorruptNewBuff : CuteSlimeBaseBuff
    {
        protected override void MoreSetDefaults()
        {
            DisplayName.SetDefault("Cute Corrupt Slime");
            Description.SetDefault("A cute corrupt slime girl is following you");
        }

        protected override void MoreUpdate(PetPlayer mPlayer)
        {
            mPlayer.CuteSlimeCorruptNew = true;
            projType = mod.ProjectileType<CuteSlimeCorruptNewProj>();
        }
    }
}
