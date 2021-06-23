using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets
{
    [Content(ContentType.DroppedPets)]
    public class RainbowSlimeBuff : SimplePetBuffBase
    {
        public override int PetType => ModContent.ProjectileType<RainbowSlimeProj>();

        public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().RainbowSlime;

        public override void SafeSetDefaults()
        {
            DisplayName.SetDefault("Rainbow Slime");
            Description.SetDefault("A Rainbow Slime is following you");
        }
    }
}
