using System.IO;
using Terraria;
using Terraria.ID;

namespace AssortedCrazyThings.Base.Netcode.Packets
{
	public class ClientChangesAssPlayerPacket : PlayerPacket
	{
		//For reflection
		public ClientChangesAssPlayerPacket() { }

		public ClientChangesAssPlayerPacket(Player player) : base(player)
		{
			
		}

		protected override void PostSend(BinaryWriter writer, Player player)
		{
			var assPlayer = player.GetModPlayer<AssPlayer>();
			assPlayer.SendClientChangesPacket(writer);
		}

		protected override void PostReceive(BinaryReader reader, int sender, Player player)
		{
			var assPlayer = player.GetModPlayer<AssPlayer>();

			assPlayer.ReceiveClientChangesPacket(reader);

			//server transmits to others
			if (Main.netMode == NetmodeID.Server)
			{
				new ClientChangesAssPlayerPacket(player).Send(from: player.whoAmI);
			}
		}
	}
}
