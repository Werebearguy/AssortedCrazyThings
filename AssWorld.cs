using AssortedCrazyThings.Items.PetAccessories;
using AssortedCrazyThings.NPCs;
using AssortedCrazyThings.NPCs.DungeonBird;
using AssortedCrazyThings.Projectiles.Pets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

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

        //Soul stuff
        public static int[] harvesterTypes = new int[3];
        public static bool downedHarvester;
        public static bool spawnHarvester;

        //Slime stuff
        public static int[] slimeTypes = new int[9];
        public static int[] slimeAccessoryItems = new int[100];
        public static Texture2D[] slimeAccessoryTextures = new Texture2D[100];
        public static int[] slimeAccessoryItemsIndexed;

        //Mods loaded
        public static bool isPlayerHealthManaBarLoaded = false;

        private void InitMinibosses()
        {
            lilmegalodonAlive = false;
            megalodonAlive = false;
            miniocramAlive = false;
        }

        private void InitHarvesterSouls()
        {
            harvesterTypes[0] = mod.NPCType<aaaHarvester1>();
            harvesterTypes[1] = mod.NPCType<aaaHarvester2>();
            harvesterTypes[2] = mod.NPCType<aaaHarvester3>();

            downedHarvester = false; //not really used anywhere properly
            spawnHarvester = false;

            isPlayerHealthManaBarLoaded = ModLoader.GetMod("PlayerHealthManaBar") != null;
        }

        private void InitSlimes()
        {
            int i = 0;
            slimeTypes[i++] = mod.ProjectileType<CuteSlimeBlackPet>();
            slimeTypes[i++] = mod.ProjectileType<CuteSlimeBluePet>();
            slimeTypes[i++] = mod.ProjectileType<CuteSlimeGreenPet>();
            slimeTypes[i++] = mod.ProjectileType<CuteSlimePinkPet>();
            slimeTypes[i++] = mod.ProjectileType<CuteSlimePurplePet>();
            slimeTypes[i++] = mod.ProjectileType<CuteSlimeRainbowPet>();
            slimeTypes[i++] = mod.ProjectileType<CuteSlimeRedPet>();
            slimeTypes[i++] = mod.ProjectileType<CuteSlimeXmasPet>();
            slimeTypes[i++] = mod.ProjectileType<CuteSlimeYellowPet>();
        }

        private void InitPetAccessories()
        {
            /* Here you add the items from PetAccessories in two arrays,
             * one is the slimeAccessoryItems one (mainly for searching when applying the accessories)
             * the other one is the texture array, follow the same pattern (this is for taking the texture in each draw call)
             * 
             */


            //------------------------------------------------------------------------------------------------------
            //------------------------------------------------------------------------------------------------------
            //------------------------------------------------------------------------------------------------------
            //ive set the limit to 100 different accessories for now, we can expand that later
            //(check definition of slimeAccessoryItems)
            int itemIndex = 0;
            slimeAccessoryItems[itemIndex++] = mod.ItemType<PetAccessoryBow>();
            slimeAccessoryItems[itemIndex++] = mod.ItemType<PetAccessoryXmasHat>();
            //add more here, for example like this:
            //slimeAccessoryItems[itemIndex++] = mod.ItemType<PetAccessoryStrapOn>();

            Array.Resize(ref slimeAccessoryItems, itemIndex);

            int[] parameters = new int[slimeAccessoryItems.Length * 2];
            for (int i = 0; i < slimeAccessoryItems.Length; i++)
            {
                parameters[2 * i] = slimeAccessoryItems[i];
                parameters[2 * i + 1] = i + 1;
            }
            slimeAccessoryItemsIndexed = IntSet(parameters);
            //-> slimeAccessoryItemsIndexed[mod.ItemType<PetAccessoryXmasHat>()] returns 2

            //------------------------------------------------------------------------------------------------------
            //------------------------------------------------------------------------------------------------------
            //------------------------------------------------------------------------------------------------------
            slimeAccessoryTextures[slimeAccessoryItemsIndexed[mod.ItemType<PetAccessoryBow>()]] = mod.GetTexture("Items/PetAccessories/PetAccessoryBow_Draw");
            slimeAccessoryTextures[slimeAccessoryItemsIndexed[mod.ItemType<PetAccessoryXmasHat>()]] = mod.GetTexture("Items/PetAccessories/PetAccessoryXmasHat_Draw");
            //for every new line, just add the new items class name in the <> and then the texture with _Draw in the ""

            //finishing up, ignore
            Array.Resize(ref slimeAccessoryTextures, itemIndex + 1); //since index starts at 1
        }

        public int[] IntSet(int[] inputs)
        {
            //inputs.Length % 2 == 0
            int[] temp = new int[inputs.Length];
            Array.Copy(inputs, temp, inputs.Length);
            Array.Sort(temp); //highest index should hold the max value of inputs
            int[] ret = new int[temp[temp.Length - 1] + 1];//length == max value of inputs
            for (int i = 0; i < ret.Length; i++)
            {
                ret[i] = 0; //fill array with 0
            }
            for (int j = 0; j < inputs.Length; j += 2)
            {
                ret[inputs[j]] = inputs[j + 1]; //fill array with pair of key:value
            }
            return ret;
        }

        public override void Initialize()
        {
            InitMinibosses();
            InitHarvesterSouls();
            InitSlimes();
            InitPetAccessories();
        }

        private void UpdateHarvesterSpawn()
        {
            if (!Main.dayTime) //if night
            {
                if (!Main.fastForwardTime)
                {
                    if (spawnHarvester && Main.netMode != 1 && Main.time > 4860.0) //after 4860.0 ticks, 81 seconds, spawn
                    {
                        for (int k = 0; k < 255; k++)
                        {
                            if (Main.player[k].active && !Main.player[k].dead/* && (double)Main.player[k].position.Y < Main.worldSurface * 16.0*/)
                            {
                                NPC.SpawnOnPlayer(k, harvesterTypes[0]);
                                AwakeningMessage(BaseHarvester.message);
                                spawnHarvester = false; //disallow it to spawn this night anymore
                                break;
                            }
                        }
                    }
                    if (Main.time >= 32400.0) //32400 is the last tick of the night
                    {
                        spawnHarvester = true; //allow it to spawn the next night (after world reload)
                    }
                }
            }
            else //if day
            {
                //32400
                if (Main.time >= 54000.0) //54000 is the last tick of the day
                {
                    if (!Main.fastForwardTime)
                    {
                        //skeletron defeated
                        if (!downedHarvester && NPC.downedBoss3 && Main.netMode != 1)
                        {
                            bool flag3 = false;
                            for (int n = 0; n < 255; n++)
                            {
                                if (Main.player[n].active && Main.player[n].statLifeMax >= 300)
                                {
                                    flag3 = true;
                                    break;
                                }
                            }
                            if (flag3/* && Main.rand.Next(3) == 0*/)
                            {
                                spawnHarvester = true;
                                AwakeningMessage("Bring a bug net to the dungeon, don't ask why...");
                            }
                        }
                    }
                }
            }
        }

        //small methods I made for myself to not make the code cluttered since I have to use these six times
        public static void AwakeningMessage(string message, Vector2 pos = default(Vector2), int soundStyle = -1)
        {
            if(soundStyle != -1) Main.PlaySound(SoundID.Roar, pos, soundStyle); //soundStyle 2 for screech, 0 for regular roar
            if (Main.netMode == NetmodeID.SinglePlayer)
            {
                Main.NewText(message, 175, 75, 255);
            }
            else if (Main.netMode == NetmodeID.Server)
            {
                NetMessage.BroadcastChatMessage(NetworkText.FromLiteral(message), new Color(175, 75, 255));
            }
        }

        public static void DisappearMessage(string message)
        {
            if (Main.netMode == NetmodeID.SinglePlayer)
            {
                Main.NewText(message, 175, 255, 175);
            }
            else if (Main.netMode == NetmodeID.Server)
            {
                NetMessage.BroadcastChatMessage(NetworkText.FromLiteral(message), new Color(175, 255, 175));
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
            UpdateHarvesterSpawn();
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
                            AwakeningMessage(lilmegalodonMessage, Main.npc[j].position, 0);
                            lilmegalodonAlive = true;
                        }
                    }
                    if (Main.npc[j].TypeName == megalodonName && !isMegalodonSpawned)
                    {
                        isMegalodonSpawned = true;
                        if (!megalodonAlive)
                        {
                            AwakeningMessage(megalodonMessage, Main.npc[j].position, 0);
                            megalodonAlive = true;
                        }
                    }
                    if (Main.npc[j].TypeName == miniocramName && !isMiniocramSpawned)
                    {
                        isMiniocramSpawned = true;
                        if (!miniocramAlive)
                        {
                            AwakeningMessage(miniocramMessage, Main.npc[j].position, 0);
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
                DisappearMessage("The " + megalodonName + " disappeared... for now.");
            }
            if (!isMegalodonSpawned && megalodonAlive)
            {
                megalodonAlive = false;
                DisappearMessage("The " + megalodonName + " disappeared... for now.");
            }
            if (!isMiniocramSpawned && miniocramAlive)
            {
                miniocramAlive = false;
                DisappearMessage("The " + miniocramName + " disappeared... for now.");
            }
        }

        public override void PreUpdate()
        {
            //track fallen NPCs every 6 ticks and replace them with souls

            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                if(Main.time % 60 == 15 && NPC.CountNPCS(mod.NPCType(aaaSoul.name)) > 10) //limit soul count in the world to 10
                {
                    short oldest = 200;
                    int timeleftmin = int.MaxValue;
                    for (short j = 0; j < 200; j++)
                    {
                        if (Main.npc[j].active && Main.npc[j].type == mod.NPCType(aaaSoul.name))
                        {
                            if(Main.npc[j].timeLeft < timeleftmin)
                            {
                                timeleftmin = Main.npc[j].timeLeft;
                                oldest = j;
                            }
                        }
                    }
                    if (oldest != 200)
                    {
                        Main.npc[oldest].life = 0;
                        Main.npc[oldest].active = false;
                    }
                }
            } //end Main.NetMode
        } //end PreUpdate 
    }
}
