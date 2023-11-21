using Terraria;

namespace AssortedCrazyThings.Base.Chatter
{
	public class BossSpawnChatterParams : IChatterParams
	{
		public NPC NPC { get; init; }

		public BossSpawnChatterParams(NPC npc)
		{
			NPC = npc;
		}
	}
}
