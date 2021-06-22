using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets
{
    [Autoload]
    public class DynamiteBunnyBuff : SimplePetBuffBase
    {
        public override int PetType => ModContent.ProjectileType<DynamiteBunnyProj>();

        public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().DynamiteBunny;

        public override void SafeSetDefaults()
        {
            DisplayName.SetDefault("Dynamite Bunny");
            Description.SetDefault("A little Dynamite Bunny is following you");
        }
    }
}
