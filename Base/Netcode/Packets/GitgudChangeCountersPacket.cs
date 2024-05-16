using System.IO;
using Terraria;

namespace AssortedCrazyThings.Base.Netcode.Packets
{
	public class GitgudChangeCountersPacket : MPPacket
	{
		private readonly int index;
		private readonly byte value;

		//For reflection
		public GitgudChangeCountersPacket() { }

		public GitgudChangeCountersPacket(int index, byte value)
		{
			this.index = index;
			this.value = value;
		}

		public override void Send(BinaryWriter writer)
		{
			writer.Write((byte)index);
			writer.Write((byte)value);
		}

		public override void Receive(BinaryReader reader, int sender)
		{
			int whoAmI = Main.myPlayer;
			int index = reader.ReadByte();
			byte value = reader.ReadByte();
			GitgudData.SetCounter(whoAmI, index, value, true);
			if (value == 0)
			{
				GitgudData.DeleteItemFromInventory(Main.player[whoAmI], index);
			}
		}
	}
}
