When replacing/updating AmuletOfManyMinionsApi
* add the [Content(ConfigurationSystem.AllFlags)] attribute
* add to the AommMod getter: if (!ContentConfig.Instance.AommSupport) { return null; }
* ModSystem -> AssSystem
* Add below AommMod and add using Terraria.Localization;
```cs
public static LocalizedText AoMMVersionText { get; private set; }

public static LocalizedText ConcatenateTwoText { get; private set; }

public static LocalizedText AppendAoMMVersion(LocalizedText text)
{
	return ConcatenateTwoText.WithFormatArgs(text, AoMMVersionText);
}

public override void Load()
{
	versionString = apiVersion.ToString();
	AoMMVersionText = Language.GetOrRegister(Mod.GetLocalizationKey($"Common.AoMMVersion"));
	ConcatenateTwoText = Language.GetOrRegister(Mod.GetLocalizationKey($"Common.ConcatenateTwo"));
}
```