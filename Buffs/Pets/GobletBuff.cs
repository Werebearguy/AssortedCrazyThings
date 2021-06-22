using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets
{
    [Content(ContentType.DroppedPets)]
    public class GobletBuff : SimplePetBuffBase
    {
        public override int PetType => ModContent.ProjectileType<GobletProj>();

        public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().Goblet;

        public override void SafeSetDefaults()
        {
            DisplayName.SetDefault("Goblet");
            Description.SetDefault("A tiny Goblin is following you");
        }
    }
}
