using Terraria;

namespace AssortedCrazyThings.Buffs
{
	[Content(ContentType.Bosses)]
	public class EnhancedHunterBuff : AssBuff
	{
		public override void Update(Player player, ref int buffIndex)
		{
			player.GetModPlayer<AssPlayer>().enhancedHunterBuff = true;
			player.detectCreature = true;
		}
	}
}
