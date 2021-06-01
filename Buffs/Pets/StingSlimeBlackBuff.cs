using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets
{
    public class StingSlimeBlackBuff : SimplePetBuffBase
    {
        public override int PetType => ModContent.ProjectileType<StingSlimeBlackProj>();

        public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().StingSlimeBlack;

        public override void SafeSetDefaults()
        {
            DisplayName.SetDefault("Sting Slime");
            Description.SetDefault("A Sting Slime is following you");
        }
    }
}
