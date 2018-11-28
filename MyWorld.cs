using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Harblesnargits_Mod_01.NPCs;

namespace Harblesnargits_Mod_01
{
	public class MyWorld : ModWorld
	{
        //basically "if they were alive last update"
        public bool megasnailAlive = false;
        public bool miniocramAlive = false;
        public bool megalodonAlive = false;
        //"are they alive this update"
        bool isMegasnailSpawned;
        bool isMiniocramSpawned;
        bool isMegalodonSpawned;
        //static names, in case you want to change them later
        public static string megasnailName = enemy_megasnail_01.name;
        public static string miniocramName = enemy_miniocram_01.name;
        public static string megalodonName = enemy_shark_07.name;

        public override void Initialize()
        {
            megasnailAlive = false;
            miniocramAlive = false;
            megalodonAlive = false;
        }

        //small methods I made for myself to not make the code cluttered since I have to use these six times
        private void AwakeningMessage(NPC npc)
        {
            if (Main.netMode == NetmodeID.SinglePlayer)
            {
                Main.NewText(Language.GetTextValue("Announcement.HasAwoken", npc.TypeName), 175, 75, 255);
            }
            else if (Main.netMode == NetmodeID.Server)
            {
                NetMessage.BroadcastChatMessage(NetworkText.FromKey("Announcement.HasAwoken", npc.GetTypeNetName()), new Color(175, 75, 255));
            }
        }

        private void DisappearMessage(string name)
        {
            if (Main.netMode == NetmodeID.SinglePlayer)
            {
                Main.NewText(name + " disappeared... for now", 175, 255, 175);
            }
            else if (Main.netMode == NetmodeID.Server)
            {
                NetMessage.BroadcastChatMessage(NetworkText.FromLiteral(name + " disappeared... for now"), new Color(175, 255, 175));
            }
        }

        public override void PostUpdate()
		{
            //those flags are checked for trueness each update
            isMegasnailSpawned = false;
            isMiniocramSpawned = false;
            isMegalodonSpawned = false;
            for (short j = 0; j < 200; j++)
            {
                if (Main.npc[j].active)
                {
                    if(Main.npc[j].TypeName == megasnailName && !isMegasnailSpawned)
                    {
                        isMegasnailSpawned = true;
                        //check if it wasnt alive in previous update
                        if(!megasnailAlive)
                        {
                            AwakeningMessage(Main.npc[j]);
                            megasnailAlive = true;
                        }
                    }
                    if (Main.npc[j].TypeName == miniocramName && !isMiniocramSpawned)
                    {
                        isMiniocramSpawned = true;
                        if (!miniocramAlive)
                        {
                            AwakeningMessage(Main.npc[j]);
                            miniocramAlive = true;
                        }
                    }
                    if (Main.npc[j].TypeName == megalodonName && !isMegalodonSpawned)
                    {
                        isMegalodonSpawned = true;
                        if (!megalodonAlive)
                        {
                            AwakeningMessage(Main.npc[j]);
                            megalodonAlive = true;
                        }
                    }
                }
            }
            //after this we know that either atleast one miniboss is active or not
            //if alive, but not active, print disappear message
            if (!isMegasnailSpawned && megasnailAlive)
            {
                megasnailAlive = false;
                DisappearMessage(megasnailName);
            }
            if (!isMiniocramSpawned && miniocramAlive)
            {
                miniocramAlive = false;
                DisappearMessage(miniocramName);
            }
            if (!isMegalodonSpawned && megalodonAlive)
            {
                megalodonAlive = false;
                DisappearMessage(megalodonName);
            }
        }

        public override void NetSend(BinaryWriter writer)
        {
            BitsByte flags = new BitsByte();
            flags[0] = megasnailAlive;
            flags[1] = miniocramAlive;
            flags[2] = megalodonAlive;
            writer.Write(flags);
        }

        public override void NetReceive(BinaryReader reader)
        {
            BitsByte flags = reader.ReadByte();
            megasnailAlive = flags[0];
            miniocramAlive = flags[1];
            megalodonAlive = flags[2];
        }
    }
}
