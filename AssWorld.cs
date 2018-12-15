using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using AssortedCrazyThings.NPCs;
using System;

namespace AssortedCrazyThings
{
	public class AssWorld : ModWorld
	{
        //basically "if they were alive last update"
        public bool lilmegalodonAlive = false;
        public bool megalodonAlive = false;
        public bool miniocramAlive = false;
        //"are they alive this update"
        bool lilmegalodonSpawned;
        bool isMegalodonSpawned;
        bool isMiniocramSpawned;
        //static names, in case you want to change them later
        public static string lilmegalodonName = LittleMegalodon.name;
        public static string megalodonName = Megalodon.name;
        public static string miniocramName = SpawnOfOcram.name;
        public static string lilmegalodonMessage = Megalodon.message;
        public static string megalodonMessage = Megalodon.message;
        public static string miniocramMessage = SpawnOfOcram.message;
        //the megalodon messages are modified down below in the Disappear message

        //Soul stuff, tracks all the npc indexes of each soul
        public static string soulName = aaaSoul.name;
        public static string harvesterName = aaaHarvester.name;
        private static readonly short maxCounter = 4;
        private short counter = 0;
        private static readonly short maxSouls = 50;
        public int[] soulList = new int[maxSouls];
        private short soulIndex = -1;

        private short counter2 = 0;

        public override void Initialize()
        {
            lilmegalodonAlive = false;
            megalodonAlive = false;
            miniocramAlive = false;

            Array.Clear(soulList, 0, soulList.Length);
        }

        //small methods I made for myself to not make the code cluttered since I have to use these six times
        public static void AwakeningMessage(string message)
        {
            if (Main.netMode == NetmodeID.SinglePlayer)
            {
                Main.NewText(message, 175, 75, 255);
            }
            else if (Main.netMode == NetmodeID.Server)
            {
                NetMessage.BroadcastChatMessage(NetworkText.FromLiteral(message), new Color(175, 75, 255));
            }
        }

        public static void DisappearMessage(string name)
        {
            if (Main.netMode == NetmodeID.SinglePlayer)
            {
                Main.NewText(name + " disappeared... for now.", 175, 255, 175);
            }
            else if (Main.netMode == NetmodeID.Server)
            {
                NetMessage.BroadcastChatMessage(NetworkText.FromLiteral(name + " disappeared... for now."), new Color(175, 255, 175));
            }
        }

        //not used anywhere, but might be helpful
        private void KillInstantly(NPC npc)
        {
            // These 3 lines instantly kill the npc without showing damage numbers, dropping loot, or playing DeathSound. Use this for instant deaths
            npc.life = 0;
            npc.HitEffect();
            npc.active = false;
            Main.PlaySound(SoundID.NPCDeath16, npc.position); // plays a fizzle sound
        }

        public override void PostUpdate()
		{
            //those flags are checked for trueness each update
            lilmegalodonSpawned = false;
            isMiniocramSpawned = false;
            isMegalodonSpawned = false;
            for (short j = 0; j < 200; j++)
            {
                if (Main.npc[j].active)
                {
                    if(Main.npc[j].TypeName == lilmegalodonName && !lilmegalodonSpawned)
                    {
                        lilmegalodonSpawned = true;
                        //check if it wasnt alive in previous update
                        if(!lilmegalodonAlive)
                        {
                            AwakeningMessage(lilmegalodonMessage);
                            lilmegalodonAlive = true;
                        }
                    }
                    if (Main.npc[j].TypeName == megalodonName && !isMegalodonSpawned)
                    {
                        isMegalodonSpawned = true;
                        if (!megalodonAlive)
                        {
                            AwakeningMessage(megalodonMessage);
                            megalodonAlive = true;
                        }
                    }
                    if (Main.npc[j].TypeName == miniocramName && !isMiniocramSpawned)
                    {
                        isMiniocramSpawned = true;
                        if (!miniocramAlive)
                        {
                            AwakeningMessage(miniocramMessage);
                            miniocramAlive = true;
                        }
                    }
                }
            }
            //after this we know that either atleast one miniboss is active or not
            //if alive, but not active, print disappear message
            if (!lilmegalodonSpawned && lilmegalodonAlive)
            {
                lilmegalodonAlive = false;
                DisappearMessage("The " + megalodonName);
            }
            if (!isMegalodonSpawned && megalodonAlive)
            {
                megalodonAlive = false;
                DisappearMessage("The " + megalodonName);
            }
            if (!isMiniocramSpawned && miniocramAlive)
            {
                miniocramAlive = false;
                DisappearMessage("The " + miniocramName);
            }

            //
            
            counter2++;
            if (counter2 > 60)
            {
                counter2 = 0;
                short number = 0;
                for (short j = 0; j < 200; j++)
                {
                    if (Main.npc[j].active && Main.npc[j].type == mod.NPCType(soulName))
                    {
                        //Main.NewText(Main.npc[j].TypeName + " " + j);
                        number++;
                    }
                }
                //Main.NewText("number : " + number);
            }
        }

        //public override void NetSend(BinaryWriter writer)
        //{
        //    BitsByte flags = new BitsByte();
        //    flags[0] = megasnailAlive;
        //    flags[1] = miniocramAlive;
        //    flags[2] = megalodonAlive;
        //    writer.Write(flags);
        //}

        //public override void NetReceive(BinaryReader reader)
        //{
        //    BitsByte flags = reader.ReadByte();
        //    megasnailAlive = flags[0];
        //    miniocramAlive = flags[1];
        //    megalodonAlive = flags[2];
        //}

        public override void PreUpdate()
        {
            //track fallen NPCs every 6 ticks and replace them with souls

            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                ////do things every maxCounter ticks
                ////check if atleast one soul harvester is active, then do that stuff 
                //bool shouldDropSouls = true;
                //for (short j = 0; j < 200; j++)
                //{
                //    if (Main.npc[j].type == mod.NPCType(harvesterName) && Main.npc[j].active)
                //    {
                //        shouldDropSouls = true;
                //        break;
                //    }
                //}

                //if (shouldDropSouls)
                //{
                //    for (short j = 0; j < 200; j++)
                //    {
                //        if (Main.npc[j].active && Main.npc[j].type != mod.NPCType(soulName) && Main.npc[j].lifeMax > 5)
                //        {
                //            //Main.NewText("set shouldSoulDrop " + Main.npc[j].TypeName);
                //            //Main.npc[j].GetGlobalNPC<AssGlobalNPC>(mod).shouldSoulDrop = true;
                //        }
                //    }
                //}

                //do things every maxCounter ticks
                //check if atleast one soul harvester is active, then do that stuff 
                //if (shouldDropSouls)
                //{
                //    //
                //    for (short j = 0; j < 200; j++)
                //    {
                //        if (!Main.npc[j].active && Main.npc[j].type != 0)
                //        {
                //            if (Main.npc[j].TypeName != soulName)
                //            {
                //                DisappearMessage(Main.npc[j].TypeName);
                //                int soulType = mod.NPCType(soulName);

                //                //NewNPC starts looking for the first !active from 0 to 200
                //                int soulID = NPC.NewNPC((int)Main.npc[j].Center.X, (int)Main.npc[j].Center.Y, soulType);
                //                Main.npc[soulID].timeLeft = 2500;
                //                //++soulIndex;
                //                Main.NewText("spawned soul");
                //                //if (soulIndex >= maxSouls) soulIndex = 0;
                //                if (Main.netMode == NetmodeID.Server && soulID < 200)
                //                {
                //                    NetMessage.SendData(23, -1, -1, null, soulID);
                //                }

                //                //soulList[soulIndex] = soulID;




                //                //Main.NewText(" " + j + " " + soulID);
                //            }
                //        }

                //    } //end for loop
                //} //end shouldDropSouls
            } //end Main.NetMode
        } //end PreUpdate 
    }
}
