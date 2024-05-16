using System.IO;
using Terraria;

namespace AssortedCrazyThings.Base.Netcode.Packets
{
	public class ConvertInertSoulsInventoryPacket : MPPacket
	{
		public ConvertInertSoulsInventoryPacket() { }

		public override void Send(BinaryWriter writer)
		{
			//No-op
		}

		public override void Receive(BinaryReader reader, int sender)
		{
			var assPlayer = Main.LocalPlayer.GetModPlayer<AssPlayer>();
			assPlayer.ConvertInertSoulsInventory();
		}
	}
}
