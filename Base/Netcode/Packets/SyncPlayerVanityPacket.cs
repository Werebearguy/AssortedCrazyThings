using System.IO;
using Terraria;

namespace AssortedCrazyThings.Base.Netcode.Packets
{
	public class SyncPlayerVanityPacket : PlayerPacket
	{
		//For reflection
		public SyncPlayerVanityPacket() { }

		public SyncPlayerVanityPacket(Player player) : base(player)
		{
		}

		protected override void PostSend(BinaryWriter writer, Player player)
		{
			var petPlayer = player.GetModPlayer<PetPlayer>();
			//no "changes" packet
			petPlayer.SendFieldValues(writer);
		}

		protected override void PostReceive(BinaryReader reader, int sender, Player player)
		{
			var petPlayer = player.GetModPlayer<PetPlayer>();
			//no "changes" packet
			petPlayer.RecvSyncPlayerVanitySub(reader);
		}
	}
}
