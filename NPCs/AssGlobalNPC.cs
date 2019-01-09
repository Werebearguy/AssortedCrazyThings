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
                    if (npc.type != mod.NPCType<aaaDungeonSoul>())
                    {
                        int soulType = mod.NPCType<aaaDungeonSoul>();

                        //NewNPC starts looking for the first !active from 0 to 200
                        int soulID = NPC.NewNPC((int)npc.position.X + aaaDungeonSoulBase.wid / 2, (int)npc.position.Y + aaaDungeonSoulBase.hei / 2, soulType); //Spawn coords are actually the tile where its supposed to spawn on
                        Main.npc[soulID].timeLeft = NPC.activeTime * 5; //change later
                        if (Main.netMode == NetmodeID.Server)
                        {
                            NetMessage.SendData(23, -1, -1, null, soulID);
                        }
                    }
                }
            }
        }
    }
}
