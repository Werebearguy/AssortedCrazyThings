using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Gitgud
{
	/// <summary>
	/// Serves as a base for all gitgud items
	/// </summary>
	[Content(ContentType.BossConsolation)]
	public abstract class GitgudItem : AssItem
	{
		public static LocalizedText OrText { get; private set; }
		public static string BossNameSeparator => $" {OrText} ";
		public static LocalizedText ConsolationPrizeText { get; private set; }
		public static LocalizedText ReducedDamageTakenBossText { get; private set; }
		//public static LocalizedText ReducedDamageTakenInvasionText { get; private set; }
		public static LocalizedText BuffImmunityText { get; private set; }
		public static LocalizedText TimesDiedText { get; private set; }

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			if (GitgudData.GetDataFromItemType(Item.type, out GitgudData data) &&
				tooltips.FindIndex(l => l.Name == "Social") == -1)
			{
				int insertIndex = tooltips.FindLastIndex(l => l.Name.StartsWith("Tooltip"));
				if (insertIndex == -1) insertIndex = tooltips.Count;
				tooltips.Insert(insertIndex++, new TooltipLine(Mod, nameof(ConsolationPrizeText), ConsolationPrizeText.ToString()));
				//data.Invasion != ""
				string reduced = ReducedDamageTakenBossText.Format(Math.Round(data.Reduction * 100), data.BossName);
				tooltips.Insert(insertIndex++, new TooltipLine(Mod, nameof(ReducedDamageTakenBossText), reduced));
				if (data.BuffTypeList.Length > 0)
				{
					int realLength = 1;
					if (data.BossName.Contains(BossNameSeparator))
					{
						realLength = 2;
					}
					tooltips.Insert(insertIndex++, new TooltipLine(Mod, nameof(BuffImmunityText), BuffImmunityText.Format(data.BuffNameFunc(), data.BossName, realLength)));
				}

				if (!(data.Accessory[Main.myPlayer] || Main.LocalPlayer.HasItem(Item.type) || Main.LocalPlayer.trashItem.type == Item.type))
				{
					tooltips.Insert(insertIndex++, new TooltipLine(Mod, nameof(TimesDiedText), TimesDiedText.Format(data.Counter[Main.myPlayer], data.CounterMax)));
				}
				tooltips.Insert(insertIndex++, new TooltipLine(Mod, "Gitgud", "[c/E180CE:'git gud']")); //No need to translate this one
			}
		}

		public sealed override void SetStaticDefaults()
		{
			string category = "Items.Gitgud.";
			OrText ??= Language.GetOrRegister(Mod.GetLocalizationKey($"{category}Or"));
			ConsolationPrizeText ??= Language.GetOrRegister(Mod.GetLocalizationKey($"{category}ConsolationPrize"));
			ReducedDamageTakenBossText ??= Language.GetOrRegister(Mod.GetLocalizationKey($"{category}ReducedDamageTakenBoss"));
			//ReducedDamageTakenInvasionText ??= Language.GetOrRegister(Mod.GetLocalizationKey($"{category}ReducedDamageTakenInvasion"));
			BuffImmunityText ??= Language.GetOrRegister(Mod.GetLocalizationKey($"{category}BuffImmunity"));
			TimesDiedText ??= Language.GetOrRegister(Mod.GetLocalizationKey($"{category}TimesDied"));

			SafeSetStaticDefaults();
		}

		public virtual void SafeSetStaticDefaults()
		{

		}

		public sealed override void SetDefaults()
		{
			Item.value = Item.sellPrice(copper: 1);
			Item.rare = -1;
			Item.accessory = true;

			//item.width = 32;
			//item.height = 32;

			SafeSetDefaults();
		}

		public virtual void SafeSetDefaults()
		{

		}
	}
}
