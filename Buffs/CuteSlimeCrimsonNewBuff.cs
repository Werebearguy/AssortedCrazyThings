using AssortedCrazyThings.Projectiles.Pets;

namespace AssortedCrazyThings.Buffs
{
    public class CuteSlimeDungeonNewBuff : CuteSlimeBaseBuff
    {
        protected override void MoreSetDefaults()
        {
            DisplayName.SetDefault("Cute Dungeon Slime");
            Description.SetDefault("A cute Dungeon slime girl is following you");
        }

        protected override void MoreUpdate(PetPlayer mPlayer)
        {
            mPlayer.CuteSlimeDungeonNew = true;
            projType = mod.ProjectileType<CuteSlimeDungeonNewPet>();
            //this is for the altTexture thing where you can specify which slime has which texture
            color = (byte)CuteSlimeBasePet.PetColor.Dungeon;
        }
    }
}
