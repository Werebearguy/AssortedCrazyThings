using AssortedCrazyThings.Base;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets
{
    [Autoload]
    public class PetMoonBuff : SimplePetBuffBase
    {
        public override int PetType => ModContent.ProjectileType<PetMoonProj>();

        public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().PetMoon;

        public override void SafeSetDefaults()
        {
            DisplayName.SetDefault("Personal Moon");
            Description.SetDefault("A small moon is providing you with constant moonlight");
            Main.vanityPet[Type] = false;
            Main.lightPet[Type] = true;
        }

        public override void ModifyBuffTip(ref string tip, ref int rare)
        {
            tip += "\n" + AssUtils.GetMoonPhaseAsString();
        }
    }
}
