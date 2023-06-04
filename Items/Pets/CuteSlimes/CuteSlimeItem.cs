using AssortedCrazyThings.Base;
using AssortedCrazyThings.Items.PetAccessories;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets.CuteSlimes
{
	[Content(ContentType.CuteSlimes)]
	public abstract class CuteSlimeItem : SimplePetItemBase
	{
		public static LocalizedText AccessoryBlacklistedText { get; private set; }
		public static LocalizedText AccessoryNoneText { get; private set; }
		public static LocalizedText HatSlotText { get; private set; }
		public static LocalizedText BodySlotText { get; private set; }
		public static LocalizedText CarriedSlotText { get; private set; }
		public static LocalizedText AccessorySlotText { get; private set; }

		public virtual bool CanShimmerItem => true;

		public override void SafeSetStaticDefaults()
		{
			string category = "Items.CuteSlime.";
			AccessoryBlacklistedText ??= Language.GetOrRegister(Mod.GetLocalizationKey($"{category}AccessoryBlacklisted"));
			AccessoryNoneText ??= Language.GetOrRegister(Mod.GetLocalizationKey($"{category}AccessoryNone"));
			HatSlotText ??= Language.GetOrRegister(Mod.GetLocalizationKey($"{category}HatSlot"));
			BodySlotText ??= Language.GetOrRegister(Mod.GetLocalizationKey($"{category}BodySlot"));
			CarriedSlotText ??= Language.GetOrRegister(Mod.GetLocalizationKey($"{category}CarriedSlot"));
			AccessorySlotText ??= Language.GetOrRegister(Mod.GetLocalizationKey($"{category}AccessorySlot"));

			if (CanShimmerItem)
			{
				ItemID.Sets.ShimmerTransformToItem[Item.type] = ModContent.ItemType<CuteSlimeShimmerItem>();
			}
		}

		public override void SafeSetDefaults()
		{
			base.SafeSetDefaults();

			Item.value = Item.sellPrice(copper: 10);
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			try
			{
				PetPlayer pPlayer = Main.LocalPlayer.GetModPlayer<PetPlayer>();
				if (pPlayer.HasValidSlimePet(out SlimePet slimePet))
				{
					Projectile projectile = Main.projectile[pPlayer.slimePetIndex];
					//checks if this item is infact a pet slime summoning item
					if (Item.shoot == projectile.type)
					{
						for (byte slotNumber = 1; slotNumber < 5; slotNumber++) //0 is None, reserved
						{
							string tooltip = string.Empty;

							if (slimePet.IsSlotTypeBlacklisted[slotNumber])
							{
								tooltip = AccessoryBlacklistedText.ToString();
							}
							else
							{
								if (pPlayer.TryGetAccessoryInSlot(slotNumber, out PetAccessory petAccessory))
								{
									if (ContentSamples.ItemsByType[petAccessory.Type].ModItem is PetAccessoryItem petAccessoryItem)
									{
										tooltip = petAccessoryItem.ShortNameText.ToString();
										tooltip += petAccessory.HasAlts ? $" ({petAccessory.AltTextureDisplayNames[petAccessory.AltTextureIndex]})" : "";
									}
								}
							}

							if (string.IsNullOrEmpty(tooltip))
							{
								tooltip = AccessoryNoneText.ToString();
							}

							tooltips.Add(new TooltipLine(Mod, ((SlotType)slotNumber).ToString(), SlotToString(slotNumber).Format(tooltip)));
						}
					}
				}
			}
			catch (Exception e)
			{
				Main.NewText(e, Color.Red);
			}
		}

		private static LocalizedText SlotToString(byte b)
		{
			if (b == (byte)SlotType.Hat)
			{
				return HatSlotText;
			}
			if (b == (byte)SlotType.Body)
			{
				return BodySlotText;
			}
			if (b == (byte)SlotType.Carried)
			{
				return CarriedSlotText;
			}
			if (b == (byte)SlotType.Accessory)
			{
				return AccessorySlotText;
			}
			throw new Exception("Unknown Slot number: " + b);
		}
	}
}
