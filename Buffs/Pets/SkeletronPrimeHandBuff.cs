using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets
{
    [Content(ContentType.DroppedPets)]
    public class SkeletronPrimeHandBuff : SimplePetBuffBase
    {
        public override int PetType => ModContent.ProjectileType<SkeletronPrimeHandProj>();

        public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().SkeletronPrimeHand;

        public override void SafeSetDefaults()
        {
            DisplayName.SetDefault("Skeletron Prime's Spare Hand");
            Description.SetDefault("Skeletron Prime's Hand is attached to you");
        }
    }
}
