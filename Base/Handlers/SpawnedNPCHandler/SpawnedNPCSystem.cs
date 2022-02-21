using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Base.Handlers.SpawnedNPCHandler
{
    [Content(ConfigurationSystem.AllFlags, needsAllToFilter: true)]
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
    }
}
