using Terraria;

namespace AssortedCrazyThings.Buffs
{
    [Content(ContentType.Bosses)]
    public class EmpoweringBuff : AssBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Empowering");
            Description.SetDefault("You get stronger the longer you don't receive damage");
        }

        public override void ModifyBuffTip(ref string tip, ref int rare)
        {
            AssPlayer mPlayer = Main.LocalPlayer.GetModPlayer<AssPlayer>();
            string add = "\nIncreased Damage: " + (int)(mPlayer.step * 100) + "%";
            tip += add;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<AssPlayer>().empoweringBuff = true;
        }
    }
}
