using AssortedCrazyThings.Projectiles.Pets;

namespace AssortedCrazyThings.Buffs
{
    public class CuteSlimeDungeonNewBuff : CuteSlimeBaseBuff
    {
        protected override void MoreSetDefaults()
        {
            DisplayName.SetDefault("Cute Dungeon Slime");
            Description.SetDefault("A cute dungeon slime girl is following you");
        }

        protected override void MoreUpdate(PetPlayer mPlayer)
        {
            mPlayer.CuteSlimeDungeonNew = true;
            projType = mod.ProjectileType<CuteSlimeDungeonNewProj>();
        }
    }
}
