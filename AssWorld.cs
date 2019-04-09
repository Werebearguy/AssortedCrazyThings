using AssortedCrazyThings.NPCs;
using AssortedCrazyThings.NPCs.DungeonBird;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.Graphics.Effects;

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
        public static int[] harvesterTypes = new int[5];
        public static int harvesterTalonLeft;
        public static int harvesterTalonRight;
        public static int harvesterIndex = -1;
        public static bool downedHarvester;
        public static bool spawnHarvester;

        public static bool slimeRainSky = false;

        private void InitHarvesterSouls()
        {
            harvesterTypes[0] = mod.NPCType<Harvester1>();
            harvesterTypes[1] = mod.NPCType<Harvester2>();
            harvesterTypes[2] = mod.NPCType<Harvester>();
            harvesterTypes[3] = harvesterTalonLeft = mod.NPCType<HarvesterTalonLeft>();
            harvesterTypes[4] = harvesterTalonRight = mod.NPCType<HarvesterTalonRight>();
            downedHarvester = false;
            spawnHarvester = false;
        }

        public override void Initialize()
        {
            InitHarvesterSouls();
        }

        public override TagCompound Save()
        {
            var downed = new List<string>();
            if (downedHarvester)
            {
                downed.Add("harvester");
            }

            return new TagCompound {
                {"downed", downed}
            };
        }

        public override void Load(TagCompound tag)
        {
            var downed = tag.GetList<string>("downed");
            downedHarvester = downed.Contains("harvester");
        }

        public override void NetSend(BinaryWriter writer)
        {
            BitsByte flags = new BitsByte();
            flags[0] = downedHarvester;
            writer.Write(flags);

        }

        public override void NetReceive(BinaryReader reader)
        {
            BitsByte flags = reader.ReadByte();
            downedHarvester = flags[0];
        }

        //small methods I made for myself to not make the code cluttered since I have to use these six times
        public static void AwakeningMessage(string message, Vector2 pos = default(Vector2), int soundStyle = -1)
        {
            if (soundStyle != -1) Main.PlaySound(SoundID.Roar, pos, soundStyle); //soundStyle 2 for screech, 0 for regular roar
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

        public static void ToggleSlimeRainSky()
        {
            if (!Main.slimeRain && Main.netMode != 1)
            {
                if (!slimeRainSky)
                {
                    SkyManager.Instance.Activate("Slime", default(Vector2));
                    CombatText.NewText(Main.LocalPlayer.getRect(), CombatText.HealLife, "Background Activated");
                    slimeRainSky = true;
                }
                else
                {
                    SkyManager.Instance.Deactivate("Slime");
                    CombatText.NewText(Main.LocalPlayer.getRect(), CombatText.DamagedFriendly, "Background Deactivated");
                    slimeRainSky = false;
                }
            }
        }

        public static void DisableSlimeRainSky()
        {
            if (!Main.slimeRain && slimeRainSky && Main.netMode != 1)
            {
                SkyManager.Instance.Deactivate("Slime");
                CombatText.NewText(Main.LocalPlayer.getRect(), CombatText.DamagedFriendly, "Background Deactivated");
                slimeRainSky = false;
            }
        }

        //not used anywhere, but might be helpful
        //private void KillInstantly(NPC npc)
        //{
        //    // These 3 lines instantly kill the npc without showing damage numbers, dropping loot, or playing DeathSound. Use this for instant deaths
        //    npc.life = 0;
        //    npc.HitEffect();
        //    npc.active = false;
        //    Main.PlaySound(SoundID.NPCDeath16, npc.position); // plays a fizzle sound
        //}

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
                            if (Main.player[k].active && !Main.player[k].dead && Main.player[k].ZoneDungeon)
                            {
                                NPC.SpawnOnPlayer(k, harvesterTypes[0]);

                                if (false)
                                {
                                    /*integrate that somehow either into AI or here
                                    if (this.ai[0] >= 650f && Main.netMode != 1)
                                    {
                                        this.ai[0] = 1f;
                                        int num91 = (int)Main.player[target].position.X / 16;
                                        int num92 = (int)Main.player[target].position.Y / 16;
                                        int num93 = (int)base.position.X / 16;
                                        int num94 = (int)base.position.Y / 16;
                                        int num95 = 20;
                                        int num96 = 0;
                                        bool flag4 = false;
                                        if (Math.Abs(base.position.X - Main.player[target].position.X) + Math.Abs(base.position.Y - Main.player[target].position.Y) > 2000f)
                                        {
                                            num96 = 100;
                                            flag4 = true;
                                        }
                                        while (!flag4 && num96 < 100)
                                        {
                                            int num2 = num96;
                                            num96 = num2 + 1;
                                            int num97 = Main.rand.Next(num91 - num95, num91 + num95);
                                            int num98 = Main.rand.Next(num92 - num95, num92 + num95);
                                            for (int num99 = num98; num99 < num92 + num95; num99 = num2 + 1)
                                            {
                                                if ((num99 < num92 - 4 || num99 > num92 + 4 || num97 < num91 - 4 || num97 > num91 + 4) && (num99 < num94 - 1 || num99 > num94 + 1 || num97 < num93 - 1 || num97 > num93 + 1) && Main.tile[num97, num99].nactive())
                                                {
                                                    bool flag5 = true;
                                                    if ((type == 32 || (type >= 281 && type <= 286)) && !Main.wallDungeon[Main.tile[num97, num99 - 1].wall])
                                                    {
                                                        flag5 = false;
                                                    }
                                                    else if (Main.tile[num97, num99 - 1].lava())
                                                    {
                                                        flag5 = false;
                                                    }
                                                    if (flag5 && Main.tileSolid[Main.tile[num97, num99].type] && !Collision.SolidTiles(num97 - 1, num97 + 1, num99 - 4, num99 - 1))
                                                    {
                                                        this.ai[1] = 20f;
                                                        this.ai[2] = (float)num97;
                                                        this.ai[3] = (float)num99;
                                                        flag4 = true;
                                                        break;
                                                    }
                                                }
                                                num2 = num99;
                                            }
                                        }
                                        netUpdate = true;
                                    }
                                    */
                                }

                                AwakeningMessage(HarvesterBase.message);
                                spawnHarvester = false; //disallow it to spawn this night anymore
                                break;
                            }
                        }
                    }
                    //if (Main.time >= 32400.0) //32400 is the last tick of the night
                    //{
                    //    spawnHarvester = true; //allow it to spawn the next night (after world reload)
                    //}
                }
            }
            else //if day
            {
                //Main.NewText("day: " + Main.time);
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
                                if (Main.player[n].active && Main.player[n].statLifeMax2 >= 300)
                                {
                                    flag3 = true;
                                    break;
                                }
                            }
                            if (flag3 && Main.rand.NextBool(5))
                            {
                                spawnHarvester = true;
                                AwakeningMessage("Bring a bug net to the dungeon, don't ask why...");
                            }
                        }
                    }
                }
            }
        }

        private void LimitSoulCount()
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                if (Main.time % 30 == 15 && NPC.CountNPCS(mod.NPCType<DungeonSoul>()) > 10) //limit soul count in the world to 15
                {
                    short oldest = 200;
                    int timeleftmin = int.MaxValue;
                    for (short j = 0; j < 200; j++)
                    {
                        if (Main.npc[j].active && Main.npc[j].type == mod.NPCType<DungeonSoul>())
                        {
                            if (Main.npc[j].timeLeft < timeleftmin)
                            {
                                timeleftmin = Main.npc[j].timeLeft;
                                oldest = j;
                            }
                        }
                    }
                    if (oldest != 200)
                    {
                        Main.npc[oldest].active = false;
                        Main.npc[oldest].netUpdate = true;
                        if (Main.netMode == NetmodeID.Server && oldest < 200)
                        {
                            NetMessage.SendData(23, -1, -1, null, oldest);
                        }
                        //poof visual
                        for (int i = 0; i < 15; i++)
                        {
                            Dust dust = Dust.NewDustPerfect(Main.npc[oldest].Center, 59, new Vector2(Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-2f, 1.5f)), 26, Color.White, Main.rand.NextFloat(1.5f, 2.4f));
                            dust.noLight = true;
                            dust.noGravity = true;
                            dust.fadeIn = Main.rand.NextFloat(0.1f, 0.6f);
                        }
                    }
                }
            } //end Main.NetMode
        }

        private void UpdateEmpoweringFactor()
        {
            if (NPC.downedPlantBoss && AssPlayer.empoweringTotal < 1f) AssPlayer.empoweringTotal = 1f;
            else if (Main.hardMode && AssPlayer.empoweringTotal < 0.75f) AssPlayer.empoweringTotal = 0.75f;
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
                    if (Main.npc[j].TypeName == lilmegalodonName && !lilmegalodonSpawned)
                    {
                        lilmegalodonSpawned = true;
                        //check if it wasnt alive in previous update
                        if (!lilmegalodonAlive)
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
                DisappearMessage("The " + megalodonName + " disappeared... for now");
            }
            if (!isMegalodonSpawned && megalodonAlive)
            {
                megalodonAlive = false;
                DisappearMessage("The " + megalodonName + " disappeared... for now");
            }
            if (!isMiniocramSpawned && miniocramAlive)
            {
                miniocramAlive = false;
                DisappearMessage("The " + miniocramName + " disappeared... for now");
            }
        }
        
        public override void PreUpdate()
        {
            LimitSoulCount();

            UpdateEmpoweringFactor();

            UpdateHarvesterSpawn();

            if (harvesterIndex >= 0 && !Main.npc[harvesterIndex].active)
            {
                harvesterIndex = -1;
            }
        } 
    }
}
