using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
	public class GoblinMinerItem : SimplePetItemBase
	{
		public override int PetType => ModContent.ProjectileType<GoblinMinerProj>();

		public override int BuffType => ModContent.BuffType<GoblinMinerBuff>();

		public override void SafeSetDefaults()
		{
			Item.width = 18;
			Item.height = 22;
			Item.value = Item.sellPrice(gold: 9, silver: 20); //TODO miner, sold by demo NPC
		}
	}

	//Light pet, no Aomm form
}
