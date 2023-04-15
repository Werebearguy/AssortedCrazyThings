using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets
{
	[Content(ContentType.DroppedPets | ContentType.OtherPets, needsAllToFilterOut: true)]
	public class TorchGodLightPetBuff : SimplePetBuffBase
	{
		public static LocalizedText EnableSmartCursorText { get; private set; } //Enable 'Smart Cursor' to automatically place torches
		public static LocalizedText NoTorchesText { get; private set; } //No normal torches found to place

		public override int PetType => ModContent.ProjectileType<TorchGodLightPetProj>();

		public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().TorchGodLightPet;

		public override void SafeSetStaticDefaults()
		{
			EnableSmartCursorText = this.GetLocalization("EnableSmartCursor");
			NoTorchesText = this.GetLocalization("NoTorches");

			Main.vanityPet[Type] = false;
			Main.lightPet[Type] = true;
		}

		public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare)
		{
			if (!Main.SmartCursorIsUsed)
			{
				tip += "\n" + EnableSmartCursorText.ToString();
			}

			if (!Main.LocalPlayer.HasItem(ItemID.Torch))
			{
				tip += "\n" + NoTorchesText.ToString();
			}
		}
	}
}
