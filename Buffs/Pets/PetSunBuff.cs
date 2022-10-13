using AssortedCrazyThings.Base;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets
{
	public class PetSunBuff : SimplePetBuffBase
	{
		public override int PetType => ModContent.ProjectileType<PetSunProj>();

		public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().PetSun;

		public override void SafeSetStaticDefaults()
		{
			Main.vanityPet[Type] = false;
			Main.lightPet[Type] = true;
		}

		public override void ModifyBuffTip(ref string tip, ref int rare)
		{
			tip += "\n" + AssUtils.GetTimeAsString();
		}
	}
}
