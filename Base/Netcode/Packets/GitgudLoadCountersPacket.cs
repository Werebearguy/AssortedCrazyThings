using System.IO;
using Terraria;

namespace AssortedCrazyThings.Base.Netcode.Packets
{
	public class GitgudLoadCountersPacket : PlayerPacket
	{
		//For reflection
		public GitgudLoadCountersPacket() { }

		public GitgudLoadCountersPacket(Player player) : base(player)
		{
		}

		protected override void PostSend(BinaryWriter writer, Player player)
		{
			GitgudData.SendCounters(writer, player);
		}

		protected override void PostReceive(BinaryReader reader, int sender, Player player)
		{
			GitgudData.RecvCounters(reader, player);
		}
	}
}
