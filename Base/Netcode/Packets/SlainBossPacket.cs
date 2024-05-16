using System.IO;
using Terraria;

namespace AssortedCrazyThings.Base.Netcode.Packets
{
	public class SlainBossPacket : MPPacket
	{
		private readonly int type;

		public SlainBossPacket() { }

		public SlainBossPacket(int type)
		{
			this.type = type;
		}

		public override void Send(BinaryWriter writer)
		{
			writer.Write7BitEncodedInt(type);
		}

		public override void Receive(BinaryReader reader, int sender)
		{
			int type = reader.Read7BitEncodedInt();

			var assPlayer = Main.LocalPlayer.GetModPlayer<AssPlayer>();
			assPlayer.SlainBoss(type);
		}
	}
}
