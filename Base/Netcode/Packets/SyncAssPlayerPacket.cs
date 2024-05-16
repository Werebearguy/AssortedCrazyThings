using System.IO;
using Terraria;

namespace AssortedCrazyThings.Base.Netcode.Packets
{
	public class SyncAssPlayerPacket : PlayerPacket
	{
		//For reflection
		public SyncAssPlayerPacket() { }

		public SyncAssPlayerPacket(Player player) : base(player)
		{
		}

		protected override void PostSend(BinaryWriter writer, Player player)
		{
			var assPlayer = player.GetModPlayer<AssPlayer>();
			assPlayer.SendSyncPlayer(writer);
		}

		protected override void PostReceive(BinaryReader reader, int sender, Player player)
		{
			var assPlayer = player.GetModPlayer<AssPlayer>();
			assPlayer.ReceiveSyncPlayer(reader);
		}
	}
}
