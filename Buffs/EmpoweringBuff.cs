using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs
{
	[Content(ContentType.Bosses)]
	public class EmpoweringBuff : AssBuff
	{
		public static LocalizedText IncreasedDamageText { get; private set; }

		public override void SetStaticDefaults()
		{
			IncreasedDamageText = this.GetLocalization("IncreasedDamage");
		}

		public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare)
		{
			AssPlayer mPlayer = Main.LocalPlayer.GetModPlayer<AssPlayer>();
			string add = "\n" + IncreasedDamageText.Format((int)(mPlayer.empoweringStep * 100));
			tip += add;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.GetModPlayer<AssPlayer>().empoweringBuff = true;
		}
	}
}
