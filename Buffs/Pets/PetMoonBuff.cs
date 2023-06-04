using AssortedCrazyThings.Base;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets
{
	public class PetMoonBuff : SimplePetBuffBase
	{
		public override int PetType => ModContent.ProjectileType<PetMoonProj>();

		public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().PetMoon;

		public override void SafeSetStaticDefaults()
		{
			Main.vanityPet[Type] = false;
			Main.lightPet[Type] = true;
		}

		public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare)
		{
			tip += "\n" + AssUtils.GetMoonPhaseAsString();
		}
	}
}
