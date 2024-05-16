using Microsoft.Xna.Framework;
using System.IO;
using Terraria;
using Terraria.Chat;
using Terraria.ID;
using Terraria.Localization;

namespace AssortedCrazyThings.Base.Netcode.Packets
{
	public class RequestChatMessagePacket : MPPacket
	{
		private readonly NetworkText text;
		private readonly Color color;

		public RequestChatMessagePacket() { }

		public RequestChatMessagePacket(NetworkText text, Color color)
		{
			this.text = text;
			this.color = color;
		}

		public override void Send(BinaryWriter writer)
		{
			text.Serialize(writer);
			writer.WriteRGB(color);
		}

		public override void Receive(BinaryReader reader, int sender)
		{
			NetworkText text = NetworkText.Deserialize(reader);
			Color color = reader.ReadRGB();
			if (Main.netMode == NetmodeID.Server)
			{
				ChatHelper.BroadcastChatMessage(text, color);
			}
		}
	}
}
