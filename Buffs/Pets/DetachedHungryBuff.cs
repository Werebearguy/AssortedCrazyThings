using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets
{
    [Autoload]
    public class DetachedHungryBuff : SimplePetBuffBase
    {
        public override int PetType => ModContent.ProjectileType<DetachedHungryProj>();

        public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().DetachedHungry;

        public override void SafeSetDefaults()
        {
            DisplayName.SetDefault("Detached Hungry");
            Description.SetDefault("You might be the next meal...");
        }
    }
}
