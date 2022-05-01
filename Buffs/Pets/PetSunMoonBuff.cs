using AssortedCrazyThings.Base;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets
{
	public class PetSunMoonBuff : SimplePetBuffBase
	{
		public override int PetType => ModContent.ProjectileType<PetSunProj>();

		public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().PetSun;

		public override void SafeSetDefaults()
		{
			DisplayName.SetDefault("Personal Sun and Moon");
			Description.SetDefault("A small sun and moon are providing you with constant light"
				+ "\n'No adverse gravitational effects will happen'");
			Main.vanityPet[Type] = false;
			Main.lightPet[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.buffTime[buffIndex] = 18000;
			PetPlayer petPlayer = player.GetModPlayer<PetPlayer>();
			petPlayer.PetSun = true;
			petPlayer.PetMoon = true;
			if (player.whoAmI == Main.myPlayer)
			{
				var source = player.GetSource_Buff(buffIndex);

				int sun = ModContent.ProjectileType<PetSunProj>();
				bool moreThanOneSun = player.ownedProjectileCounts[sun] > 0;
				int moon = ModContent.ProjectileType<PetMoonProj>();
				bool moreThanOneMoon = player.ownedProjectileCounts[moon] > 0;
				if (!moreThanOneSun) Projectile.NewProjectile(source, player.position.X + (player.width / 2), player.position.Y + (player.height / 3), 0f, 0f, sun, 0, 0f, player.whoAmI);
				if (!moreThanOneMoon) Projectile.NewProjectile(source, player.position.X + (player.width / 2), player.position.Y + (player.height / 3), 0f, 0f, moon, 0, 0f, player.whoAmI);
			}
		}

		public override void ModifyBuffTip(ref string tip, ref int rare)
		{
			tip += "\n" + AssUtils.GetMoonPhaseAsString();
			tip += "\n" + AssUtils.GetTimeAsString();
		}
	}
}
