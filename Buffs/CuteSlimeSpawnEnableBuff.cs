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

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<AssPlayer>().cuteSlimeSpawnEnable = true;
        }
    }
}
