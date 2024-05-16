using System.IO;
using Terraria;
using Terraria.ID;

namespace AssortedCrazyThings.Base.Netcode.Packets
{
	public class ClientChangesPlayerVanityPacket : PlayerPacket
	{
		private readonly byte changes;
		private readonly byte index;

		//For reflection
		public ClientChangesPlayerVanityPacket() { }

		public ClientChangesPlayerVanityPacket(Player player, byte changes, byte index) : base(player)
		{
			this.changes = changes;
			this.index = index;
		}

		protected override void PostSend(BinaryWriter writer, Player player)
		{
			var petPlayer = player.GetModPlayer<PetPlayer>();
			petPlayer.SendClientChangesPacketSub(writer, changes, index);
		}

		protected override void PostReceive(BinaryReader reader, int sender, Player player)
		{
			var petPlayer = player.GetModPlayer<PetPlayer>();

			byte changes = reader.ReadByte();
			byte index = reader.ReadByte();
			petPlayer.RecvClientChangesPacketSub(reader, changes, index);

			//server transmits to others
			if (Main.netMode == NetmodeID.Server)
			{
				new ClientChangesPlayerVanityPacket(player, changes, index).Send(from: player.whoAmI);
			}
		}
	}
}
