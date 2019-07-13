using AssortedCrazyThings.NPCs;
using AssortedCrazyThings.NPCs.DungeonBird;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace AssortedCrazyThings
{
    public class AssWorld : ModWorld
    {
        //basically "if they were alive last update"
        public bool lilmegalodonAlive = false;
        public bool megalodonAlive = false;
        public bool miniocramAlive = false;
        //"are they alive this update"
        bool isLilmegalodonSpawned;
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
        //To prevent the item dropping more than once in a single game instance if boss is not defeated
        public static bool droppedHarvesterSpawnItemThisSession;

        public static bool slimeRainSky = false;

        private void InitHarvesterSouls()
        {
            harvesterTypes[0] = mod.NPCType<Harvester1>();
            harvesterTypes[1] = mod.NPCType<Harvester2>();
            harvesterTypes[2] = mod.NPCType<Harvester>();
            harvesterTypes[3] = harvesterTalonLeft = mod.NPCType<HarvesterTalonLeft>();
            harvesterTypes[4] = harvesterTalonRight = mod.NPCType<HarvesterTalonRight>();
            downedHarvester = false;
            droppedHarvesterSpawnItemThisSession = false;
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
            }
        }

        private void UpdateEmpoweringFactor()
        {
            if (NPC.downedPlantBoss && AssPlayer.empoweringTotal < 1f)
                AssPlayer.empoweringTotal = 1f;
            else if (Main.hardMode && AssPlayer.empoweringTotal < 0.75f)
                AssPlayer.empoweringTotal = 0.75f;
        }

        public override void ResetNearbyTileEffects()
        {
            Main.LocalPlayer.GetModPlayer<AssPlayer>().wyvernCampfire = false;
        }

        public override void PostUpdate()
        {
            //this code is when I first started modding, terrible stuff
            //those flags are checked for trueness each update
            isLilmegalodonSpawned = false;
            isMegalodonSpawned = false;
            isMiniocramSpawned = false;
            for (short j = 0; j < 200; j++)
            {
                if (Main.npc[j].active)
                {
                    if (Main.npc[j].TypeName == lilmegalodonName && !isLilmegalodonSpawned)
                    {
                        isLilmegalodonSpawned = true;
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
            if (!isLilmegalodonSpawned && lilmegalodonAlive)
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

            if (harvesterIndex >= 0 && !Main.npc[harvesterIndex].active)
            {
                harvesterIndex = -1;
            }
        }
    }
}
