using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets
{
	[Content(ContentType.DroppedPets | ContentType.OtherPets, needsAllToFilter: true)]
	public class TorchGodLightPetBuff : SimplePetBuffBase
	{
		public override int PetType => ModContent.ProjectileType<TorchGodLightPetProj>();

		public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().TorchGodLightPet;

		public override void SafeSetStaticDefaults()
		{
			Main.vanityPet[Type] = false;
			Main.lightPet[Type] = true;
		}

		public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare)
		{
			if (!Main.SmartCursorIsUsed)
			{
				tip += "\nEnable 'Smart Cursor' to automatically place torches";
			}

			if (!Main.LocalPlayer.HasItem(ItemID.Torch))
			{
				tip += "\nNo normal torches found to place";
			}
		}
	}
}
