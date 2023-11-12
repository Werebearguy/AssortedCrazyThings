When replacing/updating AmuletOfManyMinionsApi
* namespace AssortedCrazyThings.Base.ModSupport.AoMM
* ModSystem -> AssSystem
* add the [Content(ConfigurationSystem.AllFlags)] attribute to class
* add to the AommMod getter: if (!ContentConfig.Instance.AommSupport) { return null; }
* Add below AommMod and add `using Terraria.Localization;`:
```cs
public static LocalizedText AoMMVersionText { get; private set; }

public static LocalizedText AppendAoMMVersion(LocalizedText text)
{
	return AssLocalizationConcatenateTwoText.WithFormatArgs(text, AoMMVersionText);
}

public override void Load()
{
	versionString = apiVersion.ToString();
	AoMMVersionText = Language.GetOrRegister(Mod.GetLocalizationKey($"Common.AoMMVersion"));
}
```