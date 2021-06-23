using AssortedCrazyThings.Base;
using AssortedCrazyThings.Items;
using AssortedCrazyThings.Items.DroneUnlockables;
using AssortedCrazyThings.Items.Pets;
using AssortedCrazyThings.Items.Placeable;
using AssortedCrazyThings.Items.Weapons;
using AssortedCrazyThings.NPCs.DungeonBird;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs
{
    public class AssGlobalNPC : GlobalNPC
    {
        public bool shouldSoulDrop = false;

        public override bool InstancePerEntity => true;

        public override void ResetEffects(NPC npc)
        {
            shouldSoulDrop = false;
        }

        public override void OnKill(NPC npc)
        {
            //TODO convert this to a drop rule
            if (npc.type == NPCID.TheDestroyer)
            {
                AssUtils.DropItemInstanced(npc, npc.Center, npc.Size, ModContent.ItemType<DroneParts>(),
                    condition: delegate (NPC n, Player player)
                    {
                        return !DroneController.AllUnlocked(player);
                    });
            }

            //Soul spawn from dead enemies while harvester alive

            if (shouldSoulDrop)
            {
                int soulType = ModContent.NPCType<DungeonSoul>();
                if (npc.type != soulType)
                {
                    //NewNPC starts looking for the first !active from 0 to 200
                    int soulID = NPC.NewNPC((int)npc.position.X + DungeonSoulBase.wid / 2, (int)npc.position.Y + DungeonSoulBase.hei / 2, soulType); //Spawn coords are actually the tile where its supposed to spawn on
                    Main.npc[soulID].timeLeft = DungeonSoulBase.SoulActiveTime;
                    if (Main.netMode == NetmodeID.Server)
                    {
                        NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, soulID);
                    }
                }
            }

            //Other
            if (AConfigurationConfig.Instance.Bosses && !AssWorld.downedHarvester && !AssWorld.droppedHarvesterSpawnItemThisSession && !AssUtils.AnyNPCs(AssortedCrazyThings.harvesterTypes))
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
                            Item.NewItem(npc.getRect(), idolType);
                            //To prevent the item dropping more than once in a single game instance if boss is not defeated
                            AssWorld.droppedHarvesterSpawnItemThisSession = true;
                        }
                    }
                }
            }

            GitgudData.Reset(npc);
        }

        public override void SetupShop(int type, Chest shop, ref int nextSlot)
        {
            if (type == NPCID.PartyGirl && NPC.downedSlimeKing)
            {
                shop.item[nextSlot].SetDefaults(ModContent.ItemType<SlimeBeaconItem>());
                nextSlot++;
            }
        }

        public override void SetupTravelShop(int[] shop, ref int nextSlot)
        {
            if (Main.rand.NextBool(4))
            {
                shop[nextSlot] = ModContent.ItemType<SuspiciousNuggetItem>();
                nextSlot++;
            }
        }
    }
}
