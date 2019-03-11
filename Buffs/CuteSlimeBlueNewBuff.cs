using AssortedCrazyThings.Projectiles.Pets;

namespace AssortedCrazyThings.Buffs
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
            //this is for the altTexture thing where you can specify which slime has which texture
            color = (byte)CuteSlimeBaseProj.PetColor.Blue;
        }
    }
}
