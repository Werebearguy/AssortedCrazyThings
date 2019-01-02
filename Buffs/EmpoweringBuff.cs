using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs
{
    public class EmpoweringBuff : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Empowering");
            Description.SetDefault("You get stronger the longer you don't recieve damage.");
            Main.buffNoTimeDisplay[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<AssPlayer>().empoweringBuff = true;
        }
    }
}