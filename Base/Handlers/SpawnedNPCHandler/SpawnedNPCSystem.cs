using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;

namespace AssortedCrazyThings.Base.Handlers.SpawnedNPCHandler
{
	[Content(ConfigurationSystem.AllFlags)]
	internal class SpawnedNPCSystem : AssSystem
	{
		private int prevCount;
		private HashSet<Point> prevIdentities;

		public delegate void SpawnedNPCDelegate(NPC npc);
		public static event SpawnedNPCDelegate OnSpawnedNPC;

		public override void OnWorldLoad()
		{
			prevCount = Main.maxNPCs; //Set to max so that on first tick, if NPCs already exist, it doesn't trigger
			prevIdentities = new HashSet<Point>();
		}

		public override void Unload()
		{
			prevIdentities = null;
		}

		public override void PostUpdateNPCs()
		{
			//Runs on all sides
			int curCount = 0;
			HashSet<Point> currIdentities = new HashSet<Point>();
			for (int i = 0; i < Main.maxNPCs; i++)
			{
				NPC npc = Main.npc[i];
				if (npc.active)
				{
					curCount++;
					currIdentities.Add(new Point(i, npc.type));
				}
			}

			if (prevCount < curCount)
			{
				currIdentities.ExceptWith(prevIdentities); //Multiple can spawn in the same tick
				foreach (var identity in currIdentities)
				{
					OnSpawnedNPC?.Invoke(Main.npc[identity.X]);
				}
			}

			prevCount = curCount;
			prevIdentities = currIdentities;
		}

		//Special check for Eater, as he is not a boss. So just check for "first spawned head"
		public static bool IsABoss(NPC npc)
		{
			bool onlyOneEater = false;
			if (npc.type == NPCID.EaterofWorldsHead)
			{
				onlyOneEater = true;
				for (int i = 0; i < Main.maxNPCs; i++)
				{
					NPC other = Main.npc[i];

					if (other.active && i != npc.whoAmI && other.type == NPCID.EaterofWorldsHead)
					{
						onlyOneEater = false;
						break;
					}
				}
			}

			//Some "bosses" are not actual bosses but have health bar (i.e. OOA mage)
			return npc.boss || onlyOneEater || npc.GetBossHeadTextureIndex() > -1;
		}
	}
}
