using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using AssortedCrazyThings.NPCs.DungeonBird;

namespace AssortedCrazyThings.NPCs
{
	public class AssGlobalNPC : GlobalNPC
	{
        public bool shouldSoulDrop = false;

		public override bool InstancePerEntity
		{
			get
			{
				return true;
			}
		}

        public override void ResetEffects(NPC npc)
        {
            shouldSoulDrop = false;
        }

        public override void NPCLoot(NPC npc)
        {
            if(npc.type == NPCID.GoblinArcher || npc.type == NPCID.GoblinPeon || npc.type == NPCID.GoblinSorcerer || npc.type == NPCID.GoblinSummoner || npc.type == NPCID.GoblinThief || npc.type == NPCID.GoblinWarrior)
            {
                if (Main.rand.NextBool(99))
                {
                    Item.NewItem(npc.getRect(), mod.ItemType("GobletItem"));
                }
            }
        }

        public override void HitEffect(NPC npc, int hitDirection, double damage)
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                if (npc.life <= 0 && shouldSoulDrop)
                {
                    if (npc.type != mod.NPCType(aaaSoul.name))
                    {
                        int soulType = mod.NPCType(aaaSoul.name);

                        //NewNPC starts looking for the first !active from 0 to 200
                        int soulID = NPC.NewNPC((int)npc.Center.X - aaaSoul.wid, (int)npc.Center.Y - aaaSoul.hei, soulType);
                        Main.npc[soulID].timeLeft = 5000; //change later
                        if (Main.netMode == NetmodeID.Server && soulID < 200)
                        {
                            NetMessage.SendData(23, -1, -1, null, soulID);
                        }
                    }
                }
            }
        }
    }
}
