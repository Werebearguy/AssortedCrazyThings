using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets
{
    public class StingSlimeOrangeBuff : SimplePetBuffBase
    {
        public override int PetType => ModContent.ProjectileType<StingSlimeOrangeProj>();

        public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().StingSlimeOrange;

        public override void SafeSetDefaults()
        {
            DisplayName.SetDefault("Sting Slime");
            Description.SetDefault("A Sting Slime is following you");
        }
    }
}
