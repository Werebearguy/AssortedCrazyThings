using AssortedCrazyThings.Base;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs
{
	[Content(ContentType.CuteSlimes)]
	public class CuteSlimeSpawnEnableBuff : AssBuff
	{
		public LocalizedText CuteSlimesCanSpawnText { get; private set; } //Cute Slimes that can spawn:
		public LocalizedText NoneText { get; private set; } //None

		public override void SetStaticDefaults()
		{
			CuteSlimesCanSpawnText = this.GetLocalization("CuteSlimesCanSpawn");
			NoneText = this.GetLocalization("None");
		}

		public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare)
		{
			string none = NoneText.ToString();
			string suffix = none;

			foreach (SpawnConditionType type in Enum.GetValues(typeof(SpawnConditionType)))
			{
				if (SlimePets.CanSpawn(Main.LocalPlayer, type))
				{
					var nameList = SlimePets.slimePetNPCsEnumToNames[(int)type];
					if (nameList != null)
					{
						if (suffix == none) suffix = "\n";
						for (int i = 0; i < nameList.Count; i++)
						{
							suffix += nameList[i].ToString();
							if (i < nameList.Count - 1) suffix += ", "; //if not last element, add a comma
							else suffix += "\n"; //if last element, add line break
						}
					}
				}
			}
			tip += "\n" + CuteSlimesCanSpawnText.ToString() + AssUISystem.GetColon() + suffix;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.GetModPlayer<AssPlayer>().cuteSlimeSpawnEnable = true;
		}
	}
}
