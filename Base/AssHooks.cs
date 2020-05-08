using AssortedCrazyThings.NPCs.CuteSlimes;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace AssortedCrazyThings.Base
{
    /// <summary>
    /// contains Hooks that will be loaded in Mod.Load. Currently unused
    /// </summary>
    public static class AssHooks
    {
        public static void Load()
        {
            //On.Terraria.NPC.CatchNPC += NPC_CatchNPC;
        }

        /// <summary>
        /// Method swap when a cute slime is captured
        /// </summary>
        private static void NPC_CatchNPC(On.Terraria.NPC.orig_CatchNPC orig, int i, int who)
        {
            NPC npc = Main.npc[i];
            if (npc.active && npc.modNPC != null && npc.modNPC is CuteSlimeBaseNPC)
            {
                CuteSlimeBaseNPC slime = (CuteSlimeBaseNPC)npc.modNPC;
                if (Main.netMode == NetmodeID.MultiplayerClient)
                {
                    npc.active = false;
                    NetMessage.SendData(MessageID.BugCatching, -1, -1, null, i, who, 0f, 0f, 0, 0, 0);
                    return;
                }
                if (npc.catchItem > 0)
                {
                    if (npc.SpawnedFromStatue)
                    {
                        Vector2 position = npc.Center - new Vector2(20f);
                        Utils.PoofOfSmoke(position);
                        npc.active = false;
                        NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, i, 0f, 0f, 0f, 0, 0, 0);
                        NetMessage.SendData(MessageID.PoofOfSmoke, -1, -1, null, (int)position.X, position.Y, 0f, 0f, 0, 0, 0);
                        return;
                    }
                    Item item = new Item();
                    item.SetDefaults(npc.catchItem);
                    Player player = Main.player[who];
                    Item.NewItem((int)player.Center.X, (int)player.Center.Y, 0, 0, npc.catchItem, 1, false, 0, true, false);
                    // NEW
                    slime.DropRandomItem(player.getRect());
                    slime.MoreNPCLoot(player.getRect());
                    // NEW
                    npc.active = false;
                    NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, i, 0f, 0f, 0f, 0, 0, 0);
                }
            }
            else
            {
                orig(i, who);
            }
        }
    }
}
