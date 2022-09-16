using AssortedCrazyThings.Projectiles.Pets.CuteSlimes;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets.CuteSlimes
{
	public class CuteSlimeDungeonBuff : CuteSlimeBuffBase
	{
		public override int PetType => ModContent.ProjectileType<CuteSlimeDungeonProj>();

		public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().CuteSlimeDungeon;

		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Cute Dungeon Slime");
			Description.SetDefault("A cute dungeon slime is following you");
		}
	}
}
