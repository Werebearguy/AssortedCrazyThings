using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.Core;

namespace AssortedCrazyThings
{
	[Content(ConfigurationSystem.AllFlags)]
	public class AssLocalization : AssSystem
	{
		private static Dictionary<Type, Dictionary<string, LocalizedText>> EnumTypeToLocalizationMapping { get; set; }

		public static LocalizedText ConcatenateTwoText { get; private set; }

		public override void OnModLoad()
		{
			LoadEnumText();

			ConcatenateTwoText = Language.GetOrRegister(Mod.GetLocalizationKey($"Common.ConcatenateTwo"));
		}

		public override void OnModUnload()
		{
			EnumTypeToLocalizationMapping = null;
		}

		private void LoadEnumText()
		{
			EnumTypeToLocalizationMapping = new Dictionary<Type, Dictionary<string, LocalizedText>>();

			foreach (var type in AssemblyManager.GetLoadableTypes(Mod.Code)
				.Where(t => t.IsEnum && t.IsDefined(typeof(LocalizeEnumAttribute), false)))
			{
				var attr = (LocalizeEnumAttribute)Attribute.GetCustomAttribute(type, typeof(LocalizeEnumAttribute));
				var category = attr.Category ?? type.Name;
				var dict = EnumTypeToLocalizationMapping[type] = new Dictionary<string, LocalizedText>();
				foreach (var name in Enum.GetNames(type))
				{
					dict[name] = RegisterEnumText(category, name);
				}
			}
		}

		private LocalizedText RegisterEnumText(string category, string suffix)
		{
			string commonKey = $"{category}.";
			return Mod.GetLocalization($"{commonKey}{suffix}", () => Regex.Replace(suffix, "([A-Z])", " $1").Trim());
		}

		public static LocalizedText GetEnumText<T>(T enumValue) where T : Enum
		{
			return EnumTypeToLocalizationMapping[typeof(T)][enumValue.ToString()];
		}
	}

	/// <summary>
	/// Marker for localizing enums, automatically registered (<see cref="AssLocalization,RegisterEnumText"/>) and accessible (<see cref="AssLocalization.GetEnumText{T}(T)"/>)
	/// </summary>
	[AttributeUsage(AttributeTargets.Enum)]
	public class LocalizeEnumAttribute : Attribute
	{
		/// <summary>
		/// Can be null
		/// </summary>
		public string Category { get; init; } = null;
	}
}
