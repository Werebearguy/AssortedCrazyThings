using AssortedCrazyThings.NPCs.Harvester;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs
{
	[Content(ContentType.Bosses)]
	public class HarvesterGlobalNPC : AssGlobalNPC
	{
		public bool shouldSoulDrop = false;

		public override bool InstancePerEntity => true;

		public override void ResetEffects(NPC npc)
		{
			shouldSoulDrop = false;
		}

		public override void OnKill(NPC npc)
		{
			//Soul spawn from dead enemies while harvester alive
			if (shouldSoulDrop)
			{
				int soulType = ModContent.NPCType<DungeonSoul>();
				if (npc.type != soulType)
				{
					//NewNPC starts looking for the first !active from 0 to 200
					int soulID = NPC.NewNPC(npc.GetSource_Death(), (int)npc.Center.X, (int)npc.Top.Y + DungeonSoulBase.hei / 2, soulType);
					if (Main.netMode == NetmodeID.Server)
					{
						NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, soulID);
					}
				}
			}
		}
	}
}
