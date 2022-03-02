using AssortedCrazyThings.Base;
using AssortedCrazyThings.Items;
using AssortedCrazyThings.NPCs.DungeonBird;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs
{
    [Content(ContentType.Bosses)]
    public class HarvesterGlobalNPC : AssGlobalNPC
    {
        public bool shouldSoulDrop = false;

        public override bool InstancePerEntity => true;

        public override void ResetEffects(NPC npc)
        {
            shouldSoulDrop = false;
        }

        public override void OnKill(NPC npc)
        {
            //Soul spawn from dead enemies while harvester alive
            if (shouldSoulDrop)
            {
                int soulType = ModContent.NPCType<DungeonSoul>();
                if (npc.type != soulType)
                {
                    //NewNPC starts looking for the first !active from 0 to 200
                    int soulID = NPC.NewNPC(npc.GetSpawnSource_NPCHurt(), (int)npc.position.X + DungeonSoulBase.wid / 2, (int)npc.position.Y + DungeonSoulBase.hei / 2, soulType); //Spawn coords are actually the tile where its supposed to spawn on
                    Main.npc[soulID].timeLeft = DungeonSoulBase.SoulActiveTime;
                    if (Main.netMode == NetmodeID.Server)
                    {
                        NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, soulID);
                    }
                }
            }

            //Other
            if (!AssWorld.downedHarvester && !AssWorld.droppedHarvesterSpawnItemThisSession && !AssUtils.AnyNPCs(AssortedCrazyThings.harvesterTypes))
            {
                int index = npc.FindClosestPlayer();
                if (index != -1)
                {
                    Player player = Main.player[index];
                    int idolType = ModContent.ItemType<IdolOfDecay>();
                    if (player.ZoneDungeon && !player.HasItem(idolType))
                    {
                        if (Main.rand.NextBool(200))
                        {
                            Item.NewItem(npc.GetSpawnSource_NPCHurt(), npc.getRect(), idolType);
                            //To prevent the item dropping more than once in a single game instance if boss is not defeated
                            AssWorld.droppedHarvesterSpawnItemThisSession = true;
                        }
                    }
                }
            }
        }
    }
}
