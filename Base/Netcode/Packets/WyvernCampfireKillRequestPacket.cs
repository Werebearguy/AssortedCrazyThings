using AssortedCrazyThings.NPCs.Harvester;
using System.IO;
using Terraria;
using Terraria.ID;

namespace AssortedCrazyThings.Base.Netcode.Packets
{
	public class WyvernCampfireKillRequestPacket : NPCPacket
	{
		public WyvernCampfireKillRequestPacket() { }

		public WyvernCampfireKillRequestPacket(NPC npc) : base(npc)
		{
		}

		protected override void PostSend(BinaryWriter writer, NPC npc)
		{
			//No-op
		}

		protected override void PostReceive(BinaryReader reader, int sender, NPC npc)
		{
			if (npc.type == NPCID.WyvernHead)
			{
				DungeonSoulBase.KillInstantly(npc);
				if (npc.whoAmI < Main.maxNPCs)
				{
					NetMessage.SendData(MessageID.SyncNPC, number: npc.whoAmI);
				}
			}
			else
			{
				//In case it fails, kill all wyverns in the world
				for (int k = 0; k < Main.maxNPCs; k++)
				{
					NPC other = Main.npc[k];
					if (other.active && other.type == NPCID.WyvernHead)
					{
						DungeonSoulBase.KillInstantly(other);
						NetMessage.SendData(MessageID.SyncNPC, number: k);
						break;
					}
				}
			}
		}
	}
}
