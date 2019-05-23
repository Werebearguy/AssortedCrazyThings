using AssortedCrazyThings.Items.Weapons;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs
{
    public class DroneControllerBuff : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Drone Controller");
            //Description.SetDefault("A friendly Soul is fighting for you");
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            AssPlayer modPlayer = player.GetModPlayer<AssPlayer>(mod);
            if (DroneController.SumOfCombatDrones(player) + DroneController.SumOfSupportDrones(player) > 0)
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
    }
}
