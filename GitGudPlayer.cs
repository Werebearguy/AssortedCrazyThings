using AssortedCrazyThings.Base;
using Microsoft.Xna.Framework;
using System;
using System.Collections;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

//GITGUD BEHAVIOR GOES HERE
/*
 * In GitGudPlayer:
 *      Add these two in order:
 *      public byte bossNameGitgudCounter = 0;
 *      public bool bossNameGitgud = false;
 *
 *      **IMPORTANT**: IN ORDER OF BOSS PROGRESSION, HAS TO BE THE SAME ORDER IN EVERY CONTEXT
 *      Add the byte to Save(), Load() and OnEnterWorld()
 *      Add the bool to ResetEffects() and Initialize()
 *      
 * Add new item into Items/Gitgud/ (with the proper bool in UpdateAccessory())
 * 
 * In GitgudData:
 *      Add the byte to SetCounter() in the proper order, adjust the case <number> things so it's in order properly
 *      Register the item and its properties in RegisterItems()
 * 
 */

namespace AssortedCrazyThings
{
    /// <summary>
    /// Holds static functions and data related to the Gitgud accessories
    /// </summary>
    public class GitgudData
    {
        /// <summary>
        /// Holds the data of all Gitgud Accessories, and a list of counters per player
        /// </summary>
        public static GitgudData[] DataList;
        /// <summary>
        /// Name for the delete message
        /// </summary>
        public string ItemName { set; get; }
        /// <summary>
        /// Boss name for tooltip
        /// </summary>
        public string BossName { set; get; }
        /// <summary>
        /// Buff immunity while boss is alive
        /// </summary>
        public int BuffType { private set; get; }
        /// <summary>
        /// Buff name for tooltip
        /// </summary>
        public string BuffName { private set; get; }
        /// <summary>
        /// Dropped gitgud item
        /// </summary>
        public int ItemType { private set; get; }
        /// <summary>
        /// Boss type (usually only one, can be multiple for worms)
        /// </summary>
        public int[] BossTypeList { private set; get; }
        /// <summary>
        /// NPCs that deal damage during the boss fight (boss minions) (includes the boss itself by default)
        /// </summary>
        public int[] NPCTypeList { private set; get; }
        /// <summary>
        /// Projectiles that deal damage during the boss fight
        /// </summary>
        public int[] ProjTypeList { private set; get; }
        /// <summary>
        /// Threshold, after which the item drops
        /// </summary>
        public byte CounterMax { private set; get; }
        /// <summary>
        /// Percentage by which the damage gets reduced
        /// </summary>
        public float Reduction { private set; get; }
        /// <summary>
        /// Invasion name, unused
        /// </summary>
        public string Invasion { private set; get; }
        /// <summary>
        /// Invasion in progress, ignore the lists for damage reduction
        /// </summary>
        public Func<bool> InvasionBool { private set; get; }

        //255 long
        /// <summary>
        ///  The bool that is set by the wearing accessory
        /// </summary>
        public BitArray Accessory { private set; get; }
        /// <summary>
        /// number of times player died to the boss
        /// </summary>
        public byte[] Counter { private set; get; }

        public GitgudData(string displayName, string buffName, int itemType, int buffType,
            int[] bossTypeList, int[] nPCTypeList, int[] projTypeList, byte counterMax, float reduction, string invasion, Func<bool> invasionBool)
        {
            ItemName = displayName;
            ItemType = itemType;
            BuffName = buffName;
            BuffType = buffType;
            BossTypeList = bossTypeList;

            NPC npc = new NPC();
            npc.SetDefaults(bossTypeList[0]);
            BossName = npc.GetFullNetName().ToString();
            if (bossTypeList.Length > 1)
            {
                for (int i = 1; i < bossTypeList.Length; i++)
                {
                    npc = new NPC();
                    npc.SetDefaults(bossTypeList[i]);
                    if (!BossName.Contains(npc.GetFullNetName().ToString()))
                    {
                        BossName += " or " + npc.GetFullNetName().ToString();
                    }
                }
            }
            npc.active = false;

            if (nPCTypeList == null) nPCTypeList = new int[1];
            if (projTypeList == null) projTypeList = new int[1];

            NPCTypeList = AssUtils.ConcatArray(nPCTypeList, bossTypeList);
            ProjTypeList = projTypeList;
            CounterMax = counterMax;
            Reduction = reduction;
            Invasion = invasion;
            InvasionBool = invasionBool ?? (() => false);

            Array.Sort(BossTypeList);
            Array.Sort(NPCTypeList);
            Array.Sort(ProjTypeList);

            Counter = new byte[255];
            Accessory = new BitArray(255);
        }

        public override string ToString()
        {
            return ItemName;
        }

        public static void Add(string itemName, string displayName, string buffName, int buffType,
            int[] bossTypeList, int[] nPCTypeList = null, int[] projTypeList = null, byte counterMax = 5, float reduction = 0.15f, string invasion = "", Func<bool> invasionBool = null)
        {
            int itemType = AssUtils.Instance.ItemType(itemName);
            if (itemType == 0) throw new Exception("No gitgud item called '" + itemName + "' found. Did you spell it correctly?");

            DataList[DataList.Length - 1] = new GitgudData(displayName, buffName, itemType, buffType, bossTypeList, nPCTypeList, projTypeList, counterMax, reduction, invasion, invasionBool);
            Array.Resize(ref DataList, DataList.Length + 1);
        }

        public static void Add(string itemName, string displayName, string buffName, int buffType,
            int bossType, int[] nPCTypeList = null, int[] projTypeList = null, byte counterMax = 5, float reduction = 0.15f, string invasion = "", Func<bool> invasionBool = null)
        {
            Add(itemName, displayName, buffName, buffType, new int[] { bossType }, nPCTypeList, projTypeList, counterMax, reduction, invasion, invasionBool);
        }

        /// <summary>
        /// Called in Reset and RecvReset. Deletes the item from the inventory, trash slot, mouse item, and accessories
        /// </summary>
        private static void DeleteItemFromInventory(Player player, int index)
        {
            int itemType = DataList[index].ItemType;
            string itemName = DataList[index].ItemName;

            bool deleted = false;

            Item[][] inventoryArray = { player.inventory, player.bank.item, player.bank2.item, player.bank3.item, player.armor }; //go though player inv
            for (int y = 0; y < inventoryArray.Length; y++)
            {
                for (int e = 0; e < inventoryArray[y].Length; e++)
                {
                    if (inventoryArray[y][e].type == itemType) //find gitgud item
                    {
                        inventoryArray[y][e].TurnToAir();
                        deleted = true;
                    }
                }
            }

            //trash slot
            if (player.trashItem.type == itemType)
            {
                player.trashItem.TurnToAir();
                deleted = true;
            }

            //mouse item
            if (Main.netMode != NetmodeID.Server && Main.myPlayer == player.whoAmI && Main.mouseItem.type == itemType)
            {
                Main.mouseItem.TurnToAir();
                deleted = true;
            }

            if (deleted) Main.NewText("You won't be needing the " + itemName + " anymore...", new Color(255, 175, 0));
        }

        /// <summary>
        /// Sets the counter on both the DataList and the players respective field
        /// </summary>
        private static void SetCounter(int whoAmI, int index, byte value, bool packet = false)
        {
            DataList[index].Counter[whoAmI] = value;
            GitGudPlayer gPlayer = Main.player[whoAmI].GetModPlayer<GitGudPlayer>();
            switch (index)
            {
                case 0:
                    gPlayer.kingSlimeGitgudCounter = value;
                    break;
                case 1:
                    gPlayer.eyeOfCthulhuGitgudCounter = value;
                    break;
                case 2:
                    gPlayer.brainOfCthulhuGitgudCounter = value;
                    break;
                case 3:
                    gPlayer.eaterOfWorldsGitgudCounter = value;
                    break;
                case 4:
                    gPlayer.queenBeeGitgudCounter = value;
                    break;
                case 5:
                    gPlayer.skeletronGitgudCounter = value;
                    break;
                case 6:
                    gPlayer.wallOfFleshGitgudCounter = value;
                    break;
                //HARDMODE
                case 7:
                    gPlayer.destroyerGitgudCounter = value;
                    break;
                case 8:
                    gPlayer.twinsGitgudCounter = value;
                    break;
                case 9:
                    gPlayer.skeletronPrimeGitgudCounter = value;
                    break;
                case 10:
                    gPlayer.planteraGitgudCounter = value;
                    break;
                case 11:
                    gPlayer.golemGitgudCounter = value;
                    break;
                case 12:
                    gPlayer.dukeFishronGitgudCounter = value;
                    break;
                case 13:
                    gPlayer.lunaticCultistGitgudCounter = value;
                    break;
                case 14:
                    gPlayer.moonLordGitgudCounter = value;
                    break;
                //INVASIONS
                //case 15:
                //    gPlayer.pirateInvasionGitgudCounter = value;
                //    break;
                default: //shouldn't get there hopefully
                    if (packet) AssUtils.Instance.Logger.Warn("Recieved unspecified GitgudReset Packet " + index);
                    else throw new Exception("Unspecified index in the gitgud array " + index);
                    break;
            }
        }

        /// <summary>
        /// Used in GitgudItem.ModifyTooltips
        /// </summary>
        public static int GetIndexFromItemType(int type)
        {
            if (DataList != null)
            {
                for (int i = 0; i < DataList.Length; i++)
                {
                    if (DataList[i].ItemType == type) return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// Called in LoadCounters
        /// </summary>
        public static void SendCounters(int whoAmI)
        {
            if (DataList != null)
            {
                //Length is synced on both sides anyway
                ModPacket packet = AssUtils.Instance.GetPacket();
                packet.Write((byte)AssMessageType.GitgudLoadCounters);
                packet.Write((byte)whoAmI);
                for (int i = 0; i < DataList.Length; i++)
                {
                    packet.Write((byte)DataList[i].Counter[whoAmI]);
                }
                packet.Send();
            }
        }

        /// <summary>
        /// Called in Mod.HandlePacket. Reads the whole list when the player joins to synchronize
        /// </summary>
        public static void RecvCounters(BinaryReader reader)
        {
            if (DataList != null)
            {
                //Length is synced on both sides anyway
                int whoAmI = reader.ReadByte();
                byte[] tempArray = new byte[DataList.Length];
                for (int i = 0; i < DataList.Length; i++) //probably unnecessary but idk
                {
                    tempArray[i] = reader.ReadByte();
                }
                for (int i = 0; i < DataList.Length; i++)
                {
                    //DataList[i].Counter[whoAmI] = tempArray[i];
                    SetCounter(whoAmI, i, tempArray[i], true);
                }
            }
        }

        /// <summary>
        /// Called in Mod.HandlePacket
        /// </summary>
        public static void RecvChangeCounter(BinaryReader reader)
        {
            if (DataList != null && Main.netMode == NetmodeID.MultiplayerClient)
            {
                int whoAmI = Main.LocalPlayer.whoAmI;
                int index = reader.ReadByte();
                byte value = reader.ReadByte();
                //DataList[index].Counter[whoAmI] = value;
                SetCounter(whoAmI, index, value, true);
                if (value == 0) DeleteItemFromInventory(Main.player[whoAmI], index);
                //AssUtils.Print("recv changecounter from server with " + whoAmI + " " + index + " " + value);
            }
        }

        /// <summary>
        /// Called in IncreaseCounters and Reset. Serverside
        /// </summary>
        private static void SendChangeCounter(int whoAmI, int index, byte value)
        {
            if (DataList != null && Main.netMode == NetmodeID.Server)
            {
                //Length is synced on both sides anyway
                ModPacket packet = AssUtils.Instance.GetPacket();
                packet.Write((byte)AssMessageType.GitgudChangeCounters);
                packet.Write((byte)index);
                packet.Write((byte)value);
                packet.Send(toClient: whoAmI);
                //AssUtils.Print("send changecounter from server with " + whoAmI + " " + index + " " + value);
            }
        }

        /// <summary>
        /// Called in AssGlobalNPC.NPCLoot. Sends all participating players a reset packet
        /// </summary>
        public static void Reset(NPC npc)
        {
            //Single and Server only
            if (DataList != null)
            {
                for (int j = 0; j < 255; j++)
                {
                    if (Main.player[j].active && npc.playerInteraction[j]) //playerInteraction is only accurate in single and server
                    {
                        for (int i = 0; i < DataList.Length; i++)
                        {
                            //resets even when all but one player is dead and boss is defeated
                            bool canReset = Array.BinarySearch(DataList[i].BossTypeList, npc.type) > -1;
                            if (i == 3) //EaterOfWorlds
                            {
                                canReset &= npc.boss;
                            }
                            if (i == 8) //Twins
                            {
                                if (npc.type == NPCID.Retinazer)
                                {
                                    canReset &= !NPC.AnyNPCs(NPCID.Spazmatism);
                                }
                                else
                                {
                                    canReset &= !NPC.AnyNPCs(NPCID.Retinazer);
                                }
                            }

                            if (canReset)
                            {
                                DeleteItemFromInventory(Main.player[j], i);

                                //only send a packet if necessary
                                if (DataList[i].Counter[j] != 0)
                                {
                                    //DataList[i].Counter[j] = 0;
                                    SetCounter(j, i, 0);
                                    SendChangeCounter(j, i, 0);
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Called in OnRespawn, spawns the item if the counter was overflown. Not clientside (not even sure myself at this point)
        /// </summary>
        public static void SpawnItem(Player player)
        {
            if (DataList != null)
            {
                for (int i = 0; i < DataList.Length; i++)
                {
                    if (DataList[i].Counter[player.whoAmI] >= DataList[i].CounterMax)
                    {
                        //DataList[i].Counter[player.whoAmI] = 0;
                        SetCounter(player.whoAmI, i, 0);
                        if (!player.HasItem(DataList[i].ItemType) && !DataList[i].Accessory[player.whoAmI])
                        {
                            int spawnX = Main.spawnTileX - 1;
                            int spawnY = Main.spawnTileY - 1;
                            if (player.SpawnX != -1 && player.SpawnY != -1)
                            {
                                spawnX = player.SpawnX;
                                spawnY = player.SpawnY;
                            }
                            Item.NewItem(new Vector2(spawnX, spawnY) * 16, DataList[i].ItemType);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Called in PostUpdateEquips
        /// </summary>
        public static void ApplyBuffImmune(Player player)
        {
            if (DataList != null)
            {
                for (int i = 0; i < DataList.Length; i++)
                {
                    if (DataList[i].Accessory[player.whoAmI] && DataList[i].BuffType != -1 && AssUtils.AnyNPCs(DataList[i].BossTypeList))
                    {
                        player.buffImmune[DataList[i].BuffType] = true;
                    }
                }
            }
        }

        /// <summary>
        /// Called in PreKill, checks what NPCs are alive, then increases the counter by 1
        /// </summary>
        public static void IncreaseCounters(int whoAmI)
        {
            if (DataList != null)
            {
                bool[] increasedFor = new bool[DataList.Length];
                for (int k = 0; k < 200; k++)
                {
                    if (Main.npc[k].active && Main.npc[k].playerInteraction[whoAmI]) //playerInteraction is only accurate in single and server
                    {
                        for (int i = 0; i < DataList.Length; i++)
                        {
                            if (!increasedFor[i] && Array.BinarySearch(DataList[i].BossTypeList, Main.npc[k].type) > -1)
                            {
                                //AssUtils.Print("increased counter of " + whoAmI + " from " + DataList[i].Counter[whoAmI] + " to " + (DataList[i].Counter[whoAmI] + 1));
                                //DataList[i].Counter[whoAmI]++;
                                SetCounter(whoAmI, i, (byte)(DataList[i].Counter[whoAmI] + 1));
                                SendChangeCounter(whoAmI, i, (byte)(DataList[i].Counter[whoAmI] + 1));
                                increasedFor[i] = true;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Called in ModifyHitByNPC
        /// </summary>
        public static void ReduceDamageNPC(int whoAmI, int npcType, ref int damage)
        {
            if (DataList != null)
            {
                for (int i = 0; i < DataList.Length; i++)
                {
                    if (DataList[i].Accessory[whoAmI])
                    {
                        //only reduce damage if accessory worn and (either an invasion is going on or a boss alive)
                        if (DataList[i].InvasionBool() ||
                            (Array.BinarySearch(DataList[i].NPCTypeList, npcType) > -1 && AssUtils.AnyNPCs(DataList[i].BossTypeList)))
                        {
                            damage = (int)(damage * (1 - DataList[i].Reduction));
                            return;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Called in ModifyHitByProjectile
        /// </summary>
        public static void ReduceDamageProj(int whoAmI, int projType, ref int damage)
        {
            if (DataList != null)
            {
                for (int i = 0; i < DataList.Length; i++)
                {
                    if (DataList[i].Accessory[whoAmI])
                    {
                        //only reduce damage if accessory worn and (either an invasion is going on or a boss alive)
                        if (DataList[i].InvasionBool() ||
                            (Array.BinarySearch(DataList[i].ProjTypeList, projType) > -1 && AssUtils.AnyNPCs(DataList[i].BossTypeList)))
                        {
                            damage = (int)(damage * (1 - DataList[i].Reduction));
                            return;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Called in PostUpdateEquips. Updates the DataList with if the accessories are worn
        /// </summary>
        public static void UpdateAccessories(int whoAmI, BitArray accessories)
        {
            if (DataList != null)
            {
                if (accessories.Length != DataList.Length) throw new Exception("Number of gitgud accessory bools don't match with the registered gitgud items");
                for (int i = 0; i < accessories.Length; i++) //has to have the same order as DataList
                {
                    DataList[i].Accessory[whoAmI] = accessories[i];
                }
            }
        }

        /// <summary>
        /// Called in OnEnterWorld, Sets up the counters in the DataList for each accessory. Clientside
        /// </summary>
        public static void LoadCounters(int whoAmI, byte[] counters)
        {
            if (DataList != null)
            {
                if (counters.Length != DataList.Length) throw new Exception("Number of gitgud counters don't match with the registered gitgud counters");
                for (int i = 0; i < counters.Length; i++) //has to have the same order as DataList
                {
                    DataList[i].Counter[whoAmI] = counters[i];
                }

                if (Main.netMode == NetmodeID.MultiplayerClient)
                {
                    GitgudData.SendCounters(whoAmI);
                }
            }
        }

        /// <summary>
        /// Called in Mod.PostSetupContent. Basically a RegisterItems()
        /// <seealso cref="RegisterItems"/>
        /// </summary>
        public static void Load()
        {
            DataList = new GitgudData[1];

            RegisterItems();

            //since Add always increases the array size by one, it will make it so the last element is null
            Array.Resize(ref DataList, DataList.Length - 1);
            if (DataList.Length == 0) DataList = null;
        }

        /// <summary>
        /// Called in Mod.Unload
        /// </summary>
        public static void Unload()
        {
            DataList = null;
        }

        /// <summary>
        /// Fills the DataList with data for each accessory
        /// </summary>
        private static void RegisterItems()
        {
            Add("KingSlimeGitgud", "Slime Inquisition Notice",
                "", -1,
                NPCID.KingSlime,
                nPCTypeList: new int[] { NPCID.BlueSlime },
                projTypeList: new int[] { ProjectileID.SpikedSlimeSpike });
            Add("EyeOfCthulhuGitgud", "Broken Lens",
                "", -1,
                NPCID.EyeofCthulhu,
                nPCTypeList: new int[] { NPCID.ServantofCthulhu });
            Add("BrainOfCthulhuGitgud", "Insanity-B-Gone",
                "Slow", BuffID.Slow,
                NPCID.BrainofCthulhu,
                nPCTypeList: new int[] { NPCID.Creeper });
            Add("EaterOfWorldsGitgud", "Toy Seagull",
                "Weak", BuffID.Weak,
                new int[] { NPCID.EaterofWorldsBody, NPCID.EaterofWorldsTail, NPCID.EaterofWorldsHead },
                nPCTypeList: new int[] { NPCID.VileSpit });
            Add("QueenBeeGitgud", "Honey Thimble",
                "Poisoned", BuffID.Poisoned,
                NPCID.QueenBee,
                nPCTypeList: new int[] { NPCID.Bee, NPCID.BeeSmall },
                projTypeList: new int[] { ProjectileID.Stinger });
            Add("SkeletronGitgud", "Carton of Soy Milk",
                "Bleeding", BuffID.Bleeding,
                NPCID.SkeletronHead,
                nPCTypeList: new int[] { NPCID.SkeletronHand },
                projTypeList: new int[] { ProjectileID.Skull });
            Add("WallOfFleshGitgud", "Wall of Flesh Voodoo Doll",
                "", -1,
                NPCID.WallofFlesh,
                nPCTypeList: new int[] { NPCID.WallofFleshEye },
                projTypeList: new int[] { ProjectileID.EyeLaser });

            //HARDMODE
            Add("DestroyerGitgud", "Metal Dreamcatcher",
                "", -1,
                NPCID.TheDestroyer,
                nPCTypeList: new int[] { NPCID.TheDestroyerBody, NPCID.TheDestroyerTail, NPCID.Probe },
                projTypeList: new int[] { ProjectileID.PinkLaser });
            Add("TwinsGitgud", "Metal Contact Lens",
                "Cursed Inferno", BuffID.CursedInferno,
                new int[] { NPCID.Retinazer, NPCID.Spazmatism },
                projTypeList: new int[] { ProjectileID.EyeLaser, ProjectileID.CursedFlameHostile, ProjectileID.EyeFire, });
            Add("SkeletronPrimeGitgud", "Clock Set Ten Years Ahead",
                "", -1,
                NPCID.SkeletronPrime,
                nPCTypeList: new int[] { NPCID.PrimeCannon, NPCID.PrimeLaser, NPCID.PrimeSaw, NPCID.PrimeVice, },
                projTypeList: new int[] { ProjectileID.DeathLaser, ProjectileID.BombSkeletronPrime, });
            Add("GreenThumb", "Green Thumb",
                "Poisoned", BuffID.Poisoned,
                NPCID.Plantera,
                nPCTypeList: new int[] { NPCID.PlanterasHook, NPCID.PlanterasTentacle },
                projTypeList: new int[] { ProjectileID.ThornBall, ProjectileID.SeedPlantera, ProjectileID.PoisonSeedPlantera });
            Add("GolemGitgud", "Rechargeable Solar Battery",
                "On Fire!", BuffID.OnFire,
                NPCID.Golem,
                nPCTypeList: new int[] { NPCID.GolemFistLeft, NPCID.GolemFistRight, NPCID.GolemHead, NPCID.GolemHeadFree },
                projTypeList: new int[] { ProjectileID.Fireball, ProjectileID.EyeBeam });
            Add("DukeFishronGitgud", "White Hook",
                "", -1,
                NPCID.DukeFishron,
                nPCTypeList: new int[] { NPCID.DetonatingBubble, NPCID.Sharkron, NPCID.Sharkron2 },
                projTypeList: new int[] { ProjectileID.Sharknado, ProjectileID.SharknadoBolt, ProjectileID.Cthulunado });
            Add("LunaticCultistGitgud", "Notice of Occupational Termination",
                "On Fire!", BuffID.OnFire,
                NPCID.CultistBoss,
                nPCTypeList: new int[] { NPCID.AncientCultistSquidhead,/* NPCID.CultistBossClone,*/ },
                projTypeList: new int[] { ProjectileID.CultistBossIceMist, ProjectileID.CultistBossLightningOrb, ProjectileID.CultistBossLightningOrbArc, ProjectileID.CultistBossFireBall, ProjectileID.CultistBossFireBallClone });
            Add("MoonLordGitgud", "Bright Side of the Moon",
                "", -1,
                new int[] { NPCID.MoonLordHead, NPCID.MoonLordCore },
                nPCTypeList: new int[] { NPCID.MoonLordFreeEye,/* NPCID.MoonLordHand, NPCID.MoonLordHead, NPCID.MoonLordLeechBlob */}, //don't deal any damage
                projTypeList: new int[] { ProjectileID.PhantasmalEye, ProjectileID.PhantasmalSphere, ProjectileID.PhantasmalDeathray, ProjectileID.PhantasmalBolt });
            //INVASIONS
            //Add("PirateInvasionGitgud",
            //    "", -1,
            //    NPCID.PirateShip,
            //    reduction: 0.10f,
            //    invasion: "The Pirate Invasion",
            //    invasionBool: ()=> (Main.invasionType == InvasionID.PirateInvasion));

            //Add("ClassNameOfItem",
            //    <BuffName, should be "" when the other thing is -1>, <BuffID, or -1>,
            //    <NPCID of boss, or new int[] {NPCID1, NPCID2 etc } if multiple segments of a boss>,
            //    <nPCTypeList: new int[] { NPCID1, NPCID2 etc } for the minions of the boss>,
            //    <projTypeList: new int[] { ProjectileID1, ProjectileID2 etc } for the projectiles of the boss>);
            // Last two optional (if boss is super basic, but I can't think of one)
        }
    }

    public class GitGudPlayer : ModPlayer
    {
        public Func<BitArray> gitgudAccessories;

        public byte kingSlimeGitgudCounter = 0;
        public bool kingSlimeGitgud = false;

        public byte eyeOfCthulhuGitgudCounter = 0;
        public bool eyeOfCthulhuGitgud = false;

        public byte brainOfCthulhuGitgudCounter = 0;
        public bool brainOfCthulhuGitgud = false;

        public byte eaterOfWorldsGitgudCounter = 0;
        public bool eaterOfWorldsGitgud = false;

        public byte queenBeeGitgudCounter = 0;
        public bool queenBeeGitgud = false;

        public byte skeletronGitgudCounter = 0;
        public bool skeletronGitgud = false;

        public byte wallOfFleshGitgudCounter = 0;
        public bool wallOfFleshGitgud = false;

        //HARDMODE

        public byte destroyerGitgudCounter = 0;
        public bool destroyerGitgud = false;

        public byte twinsGitgudCounter = 0;
        public bool twinsGitgud = false;

        public byte skeletronPrimeGitgudCounter = 0;
        public bool skeletronPrimeGitgud = false;

        public byte planteraGitgudCounter = 0;
        public bool planteraGitgud = false;

        public byte golemGitgudCounter = 0;
        public bool golemGitgud = false;

        public byte dukeFishronGitgudCounter = 0;
        public bool dukeFishronGitgud = false;

        public byte lunaticCultistGitgudCounter = 0;
        public bool lunaticCultistGitgud = false;

        public byte moonLordGitgudCounter = 0;
        public bool moonLordGitgud = false;

        //INVASIONS

        //public byte pirateInvasionGitgudCounter = 0;
        //public bool pirateInvasionGitgud = false;

        public override void ResetEffects()
        {
            kingSlimeGitgud = false;
            eyeOfCthulhuGitgud = false;
            brainOfCthulhuGitgud = false;
            eaterOfWorldsGitgud = false;
            queenBeeGitgud = false;
            skeletronGitgud = false;
            wallOfFleshGitgud = false;
            destroyerGitgud = false;
            twinsGitgud = false;
            skeletronPrimeGitgud = false;
            planteraGitgud = false;
            golemGitgud = false;
            dukeFishronGitgud = false;
            lunaticCultistGitgud = false;
            moonLordGitgud = false;
            //pirateInvasionGitgud = false;
        }

        public override void Initialize()
        {
            gitgudAccessories = new Func<BitArray>(() => new BitArray(new bool[]
            {
                kingSlimeGitgud,
                eyeOfCthulhuGitgud,
                brainOfCthulhuGitgud,
                eaterOfWorldsGitgud,
                queenBeeGitgud,
                skeletronGitgud,
                wallOfFleshGitgud,
                destroyerGitgud,
                twinsGitgud,
                skeletronPrimeGitgud,
                planteraGitgud,
                golemGitgud,
                dukeFishronGitgud,
                lunaticCultistGitgud,
                moonLordGitgud,
                //pirateInvasionGitgud,
            }
            ));
        }

        //no need for syncplayer because the server handles the item drop stuff

        public override TagCompound Save()
        {
            return new TagCompound
            {
                {"kingSlimeGitgudCounter", (byte)kingSlimeGitgudCounter},
                {"eyeOfCthulhuGitgudCounter", (byte)eyeOfCthulhuGitgudCounter},
                {"brainOfCthulhuGitgudCounter", (byte)brainOfCthulhuGitgudCounter},
                {"eaterOfWorldsGitgudCounter", (byte)eaterOfWorldsGitgudCounter},
                {"queenBeeGitgudCounter", (byte)queenBeeGitgudCounter},
                {"skeletronGitgudCounter", (byte)skeletronGitgudCounter},
                {"wallOfFleshGitgudCounter", (byte)wallOfFleshGitgudCounter},
                {"destroyerGitgudCounter", (byte)destroyerGitgudCounter},
                {"twinsGitgudCounter", (byte)twinsGitgudCounter},
                {"skeletronPrimeGitgudCounter", (byte)skeletronPrimeGitgudCounter},
                {"planteraGitgudCounter", (byte)planteraGitgudCounter},
                {"golemGitgudCounter", (byte)golemGitgudCounter},
                {"dukeFishronGitgudCounter", (byte)dukeFishronGitgudCounter},
                {"lunaticCultistGitgudCounter", (byte)lunaticCultistGitgudCounter},
                {"moonLordGitgudCounter", (byte)moonLordGitgudCounter},
                //{"pirateInvasionGitgudCounter", (byte)pirateInvasionGitgudCounter},
            };
        }

        public override void Load(TagCompound tag)
        {
            kingSlimeGitgudCounter = tag.GetByte("kingSlimeGitgudCounter");
            eyeOfCthulhuGitgudCounter = tag.GetByte("eyeOfCthulhuGitgudCounter");
            brainOfCthulhuGitgudCounter = tag.GetByte("brainOfCthulhuGitgudCounter");
            eaterOfWorldsGitgudCounter = tag.GetByte("eaterOfWorldsGitgudCounter");
            queenBeeGitgudCounter = tag.GetByte("queenBeeGitgudCounter");
            skeletronGitgudCounter = tag.GetByte("skeletronGitgudCounter");
            wallOfFleshGitgudCounter = tag.GetByte("wallOfFleshGitgudCounter");
            destroyerGitgudCounter = tag.GetByte("destroyerGitgudCounter");
            twinsGitgudCounter = tag.GetByte("twinsGitgudCounter");
            skeletronPrimeGitgudCounter = tag.GetByte("skeletronPrimeGitgudCounter");
            planteraGitgudCounter = tag.GetByte("planteraGitgudCounter");
            golemGitgudCounter = tag.GetByte("golemGitgudCounter");
            dukeFishronGitgudCounter = tag.GetByte("dukeFishronGitgudCounter");
            lunaticCultistGitgudCounter = tag.GetByte("lunaticCultistGitgudCounter");
            moonLordGitgudCounter = tag.GetByte("moonLordGitgudCounter");
            //pirateInvasionGitgudCounter = tag.GetByte("pirateInvasionGitgudCounter");
        }

        public override void ModifyHitByNPC(NPC npc, ref int damage, ref bool crit)
        {
            GitgudData.ReduceDamageNPC(player.whoAmI, npc.type, ref damage);
        }

        public override void ModifyHitByProjectile(Projectile proj, ref int damage, ref bool crit)
        {
            GitgudData.ReduceDamageProj(player.whoAmI, proj.type, ref damage);
        }

        public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
        {
            GitgudData.IncreaseCounters(player.whoAmI);

            return true;
        }

        public override void OnEnterWorld(Player player)
        {
            GitgudData.LoadCounters(player.whoAmI, new byte[]
            {
                kingSlimeGitgudCounter,
                eyeOfCthulhuGitgudCounter,
                brainOfCthulhuGitgudCounter,
                eaterOfWorldsGitgudCounter,
                queenBeeGitgudCounter,
                skeletronGitgudCounter,
                wallOfFleshGitgudCounter,
                destroyerGitgudCounter,
                twinsGitgudCounter,
                skeletronPrimeGitgudCounter,
                planteraGitgudCounter,
                golemGitgudCounter,
                dukeFishronGitgudCounter,
                lunaticCultistGitgudCounter,
                moonLordGitgudCounter,
                //pirateInvasionGitgudCounter,
            });
        }

        public override void PostUpdateEquips()
        {
            GitgudData.UpdateAccessories(player.whoAmI, gitgudAccessories());
            GitgudData.ApplyBuffImmune(player);
            //AssUtils.Print(GitgudData.DataList.Length);
        }

        public override void OnRespawn(Player player)
        {
            GitgudData.SpawnItem(player);
        }
    }
}
