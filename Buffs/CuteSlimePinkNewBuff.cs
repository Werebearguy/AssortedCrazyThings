using AssortedCrazyThings.Projectiles.Pets;

namespace AssortedCrazyThings.Buffs
{
    public class CuteSlimePinkNewBuff : CuteSlimeBaseBuff
    {
        protected override void MoreSetDefaults()
        {
            DisplayName.SetDefault("Cute Pink Slime");
            Description.SetDefault("A cute pink slime girl is following you");
        }

        protected override void MoreUpdate(PetPlayer mPlayer)
        {
            mPlayer.CuteSlimePinkNew = true;
            projType = mod.ProjectileType<CuteSlimePinkNewPet>();
            //this is for the altTexture thing where you can specify which slime has which texture
            color = (byte)CuteSlimeBasePet.PetColor.Pink;
        }
    }
}
