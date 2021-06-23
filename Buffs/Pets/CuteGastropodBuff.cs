using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets
{
    [Content(ContentType.CuteSlimes)]
    public class CuteGastropodBuff : SimplePetBuffBase
    {
        public override int PetType => ModContent.ProjectileType<CuteGastropodProj>();

        public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().CuteGastropod;

        public override void SafeSetDefaults()
        {
            DisplayName.SetDefault("Cute Gastropod");
            Description.SetDefault("A cute Gastropod is following you");
        }
    }
}
