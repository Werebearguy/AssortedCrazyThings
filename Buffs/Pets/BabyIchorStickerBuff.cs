using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets
{
	public class BabyIchorStickerBuff : SimplePetBuffBase
	{
		public override int PetType => ModContent.ProjectileType<BabyIchorStickerProj>();

		public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().BabyIchorSticker;
	}

	public class BabyIchorStickerBuff_AoMM : SimplePetBuffBase_AoMM<BabyIchorStickerBuff>
	{

	}
}
