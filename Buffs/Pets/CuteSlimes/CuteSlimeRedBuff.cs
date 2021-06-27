using AssortedCrazyThings.Projectiles.Pets.CuteSlimes;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets.CuteSlimes
{
    public class CuteSlimeRedBuff : CuteSlimeBuffBase
    {
        public override int PetType => ModContent.ProjectileType<CuteSlimeRedProj>();

        public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().CuteSlimeRed;

        public override void SafeSetDefaults()
        {
            DisplayName.SetDefault("Cute Red Slime");
            Description.SetDefault("A cute red slime is following you");
        }
    }
}
