using AssortedCrazyThings.Projectiles.Pets.CuteSlimes;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets.CuteSlimes
{
	public class CuteSlimePrincessBuff : CuteSlimeBuffBase
	{
		public override int PetType => ModContent.ProjectileType<CuteSlimePrincessProj>();

		public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().CuteSlimePrincess;

		public override void SafeSetDefaults()
		{
			DisplayName.SetDefault("Cute Princess Slime");
			Description.SetDefault("A cute princess slime is following you");
		}
	}
}
