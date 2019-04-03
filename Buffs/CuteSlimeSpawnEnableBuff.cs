using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs
{
    public class CuteSlimeSpawnEnableBuff : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Jellied Ale");
            Description.SetDefault("Your perception of slimes is a bit off...");
        }

        public override void ModifyBuffTip(ref string tip, ref int rare)
        {
            string canSpawn = "\nSlimes that can spawn: ";
            string suffix = "";
            
            foreach (SlimePets.SpawnConditionType type in Enum.GetValues(typeof(SlimePets.SpawnConditionType)))
            {
                if (SlimePets.CanSpawn(Main.LocalPlayer, type)) Main.NewText(type.ToString() +" " + SlimePets.GetSpawnChance(Main.LocalPlayer, type));
                if (SlimePets.CanSpawn(Main.LocalPlayer, type))
                {
                    List<string> nameList = SlimePets.slimePetNPCsEnumToNames[(int)type];
                    if (nameList != null)
                    {
                        if (suffix == "") suffix += "\n";
                        for (int i = 0; i < nameList.Count; i++)
                        {
                            suffix += nameList[i];
                            if (i < nameList.Count - 1) suffix += ", ";
                            else suffix += "\n";
                        }
                    }
                }
            }
            if (suffix != "")
            {
                tip += canSpawn + suffix;
            }
            else
            {
                tip += canSpawn += "None";
            }
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<AssPlayer>().cuteSlimeSpawnEnable = true;
        }
    }
}
