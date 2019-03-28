using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs
{
    public class BipolarBuff : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Bipolar");
            Description.SetDefault("Increased and decreased spawn rate");
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.ZoneWaterCandle = true;
            player.ZonePeaceCandle = true;
        }
    }
}
