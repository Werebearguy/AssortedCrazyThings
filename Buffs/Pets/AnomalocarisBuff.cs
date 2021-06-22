using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets
{
    [Autoload]
    public class AnomalocarisBuff : SimplePetBuffBase
    {
        public override int PetType => ModContent.ProjectileType<AnomalocarisProj>();

        public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().PetAnomalocaris;

        public override void SafeSetDefaults()
        {
            DisplayName.SetDefault("Anomalocaris");
            Description.SetDefault("A little Anomalocaris is following you");
        }
    }
}
