using AssortedCrazyThings.NPCs;
using Terraria;

namespace AssortedCrazyThings.Buffs.NPCs
{
	[Content(ContentType.Bosses)]
	public class SoulBurnBuff : AssBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Soul Burn");
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.GetGlobalNPC<BuffGlobalNPC>().soulBurn = true;
		}
	}
}
