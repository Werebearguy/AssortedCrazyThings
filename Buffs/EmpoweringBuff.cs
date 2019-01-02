using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs
{
    public class EmpoweringBuff : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Empowering");
            Description.SetDefault("You get stronger the longer you don't receive damage.");
        }

        public override void ModifyBuffTip(ref string tip, ref int rare)
        {
            AssPlayer mPlayer = Main.LocalPlayer.GetModPlayer<AssPlayer>(mod);
            string add = "\nIncreased Damage: " + (int)(mPlayer.step * 100) + "%";
            tip = tip + add;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<AssPlayer>().empoweringBuff = true;
        }
    }
}