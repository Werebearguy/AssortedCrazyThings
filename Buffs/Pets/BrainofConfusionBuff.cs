using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets
{
    public class BrainofConfusionBuff : SimplePetBuffBase
    {
        public override int PetType => ModContent.ProjectileType<BrainofConfusionProj>();

        public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().BrainofConfusion;

        public override void SafeSetDefaults()
        {
            DisplayName.SetDefault("Brain of Confusion");
            Description.SetDefault("A Brain of Confusion is following you");
        }
    }
}
