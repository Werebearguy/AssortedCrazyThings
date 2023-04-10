using Terraria;

namespace AssortedCrazyThings.Buffs
{
	[Content(ContentType.Bosses)]
	public class EnhancedHunterBuff : AssBuff
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Enhanced Hunter");
			// Description.SetDefault("Shows the location of enemies outside your vision range");
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.GetModPlayer<AssPlayer>().enhancedHunterBuff = true;
			player.detectCreature = true;
		}
	}
}
