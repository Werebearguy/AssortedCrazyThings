using Terraria;

namespace AssortedCrazyThings.Buffs
{
	[Content(ContentType.Bosses)]
	public class EmpoweringBuff : AssBuff
	{
		public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare)
		{
			AssPlayer mPlayer = Main.LocalPlayer.GetModPlayer<AssPlayer>();
			string add = "\nIncreased Damage: " + (int)(mPlayer.empoweringStep * 100) + "%";
			tip += add;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.GetModPlayer<AssPlayer>().empoweringBuff = true;
		}
	}
}
