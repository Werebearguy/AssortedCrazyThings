using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs
{
    public class CuteSlimeSpawnEnableBuff : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Cute Slime Spawn Enable Buff");
            Description.SetDefault("CuteSlimeSpawnEnableBuff Tooltip");
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<AssPlayer>().cuteSlimeSpawnEnable = true;
        }
    }
}
