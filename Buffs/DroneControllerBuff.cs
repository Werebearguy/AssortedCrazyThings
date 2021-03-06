﻿using AssortedCrazyThings.Items.Weapons;
using System;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs
{
    public class DroneControllerBuff : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Drone Controller");
            Description.SetDefault("Command a swarm of drones");
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            AssPlayer modPlayer = player.GetModPlayer<AssPlayer>();
            if (DroneController.SumOfSummonedDrones(player) > 0)
            {
                modPlayer.droneControllerMinion = true;
            }
            if (!modPlayer.droneControllerMinion)
            {
                player.DelBuff(buffIndex);
                buffIndex--;
            }
            else
            {
                player.buffTime[buffIndex] = 18000;
            }
        }

        public override void ModifyBuffTip(ref string tip, ref int rare)
        {
            foreach (DroneType type in Enum.GetValues(typeof(DroneType)))
            {
                if (type != DroneType.None)
                {
                    DroneData data = DroneController.GetDroneData(type);
                    int ownedCount = Main.LocalPlayer.ownedProjectileCounts[data.ProjType];
                    if (ownedCount > 0)
                    {
                        tip += "\n" + data.Name + "s : " + ownedCount;
                    }
                }
            }
        }
    }
}
