using System.Collections.Generic;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.Default;

namespace AssortedCrazyThings.Items
{
	//Responsible for showing why an item was unloaded/disabled
	[Content(ConfigurationSystem.AllFlags)]
	public class DisabledGlobalItem : AssGlobalItem
	{
		public static LocalizedText AommDisabledText { get; private set; }
		public static LocalizedText DisabledByConfigText { get; private set; }

		public override void SetStaticDefaults()
		{
			string category = $"Items.UnloadedItem.";
			AommDisabledText ??= Mod.GetLocalization($"{category}AommDisabled");
			DisabledByConfigText ??= Mod.GetLocalization($"{category}DisabledByConfig");
		}

		public override bool AppliesToEntity(Item entity, bool lateInstantiation)
		{
			//Needs the item to be instantiated (ModItem assigned) before applying global
			return lateInstantiation && entity.ModItem is UnloadedItem;
		}

		public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
		{
			if (item.ModItem is not UnloadedItem unloadedItem)
			{
				return;
			}

			if (unloadedItem.ModName != Mod.Name)
			{
				return;
			}

			if (ConfigurationSystem.NonLoadedNames.TryGetValue(unloadedItem.ItemName, out ContentType type))
			{
				string text;
				if (type == ContentType.AommSupport && !ModLoader.HasMod("AmuletOfManyMinions"))
				{
					text = AommDisabledText.ToString();
				}
				else
				{
					text = DisabledByConfigText.Format(ConfigurationSystem.ContentTypeToString(type));
				}
				tooltips.Add(new TooltipLine(Mod, "UnloadedSource", text));
			}
		}
	}
}
