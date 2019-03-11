using AssortedCrazyThings.Projectiles.Pets;

namespace AssortedCrazyThings.Buffs
{
    public class CuteSlimeIceNewBuff : CuteSlimeBaseBuff
    {
        protected override void MoreSetDefaults()
        {
            DisplayName.SetDefault("Cute Ice Slime");
            Description.SetDefault("A cute ice slime girl is following you");
        }

        protected override void MoreUpdate(PetPlayer mPlayer)
        {
            mPlayer.CuteSlimeIceNew = true;
            projType = mod.ProjectileType<CuteSlimeIceNewProj>();
            //this is for the altTexture thing where you can specify which slime has which texture
            color = (byte)CuteSlimeBaseProj.PetColor.Ice;
        }
    }
}
