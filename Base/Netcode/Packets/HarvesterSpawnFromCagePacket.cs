using AssortedCrazyThings.Tiles;
using Microsoft.Xna.Framework;
using System.IO;
using Terraria;
using Terraria.ID;

namespace AssortedCrazyThings.Base.Netcode.Packets
{
	public class HarvesterSpawnFromCagePacket : PlayerPacket
	{
		private readonly Vector2 spawnPos;

		//For reflection
		public HarvesterSpawnFromCagePacket() { }

		public HarvesterSpawnFromCagePacket(Player player, Vector2 spawnPos) : base(player)
		{
			this.spawnPos = spawnPos;
		}

		protected override void PostSend(BinaryWriter writer, Player player)
		{
			writer.WriteVector2(spawnPos);
		}

		protected override void PostReceive(BinaryReader reader, int sender, Player player)
		{
			Vector2 spawnPos = reader.ReadVector2();

			bool resend = Main.netMode == NetmodeID.Server;
			AntiqueCageUnlockedTile.SpawnFromCage(player, spawnPos, resend);
		}
	}
}
