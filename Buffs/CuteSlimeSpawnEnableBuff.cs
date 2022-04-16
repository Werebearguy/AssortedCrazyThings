using AssortedCrazyThings.Base;
using System;
using System.Collections.Generic;
using Terraria;

namespace AssortedCrazyThings.Buffs
{
	[Content(ContentType.CuteSlimes)]
	public class CuteSlimeSpawnEnableBuff : AssBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Jellied Ale");
			Description.SetDefault("Your perception of slimes is a bit off...");
		}

		public override void ModifyBuffTip(ref string tip, ref int rare)
		{
			string canSpawn = "\nCute Slimes that can spawn: ";
			string suffix = "None";

			foreach (SpawnConditionType type in Enum.GetValues(typeof(SpawnConditionType)))
			{
				if (SlimePets.CanSpawn(Main.LocalPlayer, type))
				{
					List<string> nameList = SlimePets.slimePetNPCsEnumToNames[(int)type];
					if (nameList != null)
					{
						if (suffix == "None") suffix = "\n";
						for (int i = 0; i < nameList.Count; i++)
						{
							suffix += nameList[i];
							if (i < nameList.Count - 1) suffix += ", "; //if not last element, add a comma
							else suffix += "\n"; //if last element, add line break
						}
					}
				}
			}
			tip += canSpawn + suffix;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.GetModPlayer<AssPlayer>().cuteSlimeSpawnEnable = true;
		}
	}
}
