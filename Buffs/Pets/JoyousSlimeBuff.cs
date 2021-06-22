using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets
{
    [Autoload]
    public class JoyousSlimeBuff : SimplePetBuffBase
    {
        public override int PetType => ModContent.ProjectileType<JoyousSlimeProj>();

        public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().JoyousSlime;

        public override void SafeSetDefaults()
        {
            DisplayName.SetDefault("Joyous Slime");
            Description.SetDefault("A Joyous Slime is following you");
        }
    }
}
