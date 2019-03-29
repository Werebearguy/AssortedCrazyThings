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
 * In GitgudData:
 *      Add the byte to SetCounter() in the proper order, adjust the case <number> things so it's in order properly
 *      Add new item into Items/Gitgud/ (with the proper bool in UpdateAccessory())
 *      Register the item and its properties in RegisterItems()
 * 
 */

namespace AssortedCrazyThings
{
    public class GitgudData
    {
        public static GitgudData[] DataList;

        private string ItemName { set; get; } //name for ToString
        public int BuffType { private set; get; } //buff immunity while boss is alive
        public int ItemType { private set; get; } //droped gitgud item
        public int[] BossTypeList { private set; get; } //boss type (usually only one, can be multiple for worms)
        public int[] NPCTypeList { private set; get; } //NPCs that deal damage during the boss fight (boss minions) (includes the boss itself by default)
        public int[] ProjTypeList { private set; get; } //Projectiles that deal damage during the boss fight
        public byte CounterMax { private set; get; } //threshold, after which the item drops

        //used from outside, 255 long
        public BitArray Accessory { private set; get; } //the bool that is set by the wearing accessory
        public byte[] Counter { private set; get; } //number of times player died to the boss

        public GitgudData(string itemName, int itemType, int buffType,
            int[] bossTypeList, int[] nPCTypeList, int[] projTypeList, byte counterMax)
        {
            ItemName = itemName;
            ItemType = itemType;
            BuffType = buffType;
            BossTypeList = bossTypeList;

            if (nPCTypeList == null) nPCTypeList = new int[1];

            NPCTypeList = AssUtils.ConcatArray(nPCTypeList, bossTypeList);

            if (projTypeList == null) projTypeList = new int[1];

            ProjTypeList = projTypeList;
            CounterMax = counterMax;

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

        public static void Add(string itemName, int buffType,
            int[] bossTypeList, int[] nPCTypeList = null, int[] projTypeList = null, byte counterMax = 5)
        {
            int itemType = AssUtils.Instance.ItemType(itemName);
            if (itemType == 0) throw new Exception("no gitgud item called '" + itemName + "' found. Did you spell it correctly?");

            DataList[DataList.Length - 1] = new GitgudData(itemName, itemType, buffType, bossTypeList, nPCTypeList, projTypeList, counterMax);
            Array.Resize(ref DataList, DataList.Length + 1);
        }

        public static void Add(string itemName, int buffType,
            int bossType, int[] nPCTypeList = null, int[] projTypeList = null, byte counterMax = 5)
        {
            Add(itemName, buffType, new int[] { bossType }, nPCTypeList, projTypeList, counterMax);
        }

        private static void SetCounter(int whoAmI, int index, byte value, bool packet = false)
        {
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
                default: //shouldn't get there hopefully
                    if (packet) ErrorLogger.Log("Recieved unspecified GitgudReset Packet " + index);
                    else throw new Exception("Unspecified index in the gitgud array");
                    break;
            }
        } //sets the player values

        public static void SendCounters(int whoAmI)
        {
            if (DataList != null)
            {
                //Length is synced on both sides anyway
                ModPacket packet = AssUtils.Instance.GetPacket();
                packet.Write((byte)AssMessageType.GitgudCounters);
                packet.Write((byte)whoAmI);
                for (int i = 0; i < DataList.Length; i++)
                {
                    packet.Write((byte)DataList[i].Counter[whoAmI]);
                }
                packet.Send();
            }
        } //OnEnterWorld

        public static void RecvCounters(BinaryReader reader)
        {
            if (DataList != null)
            {
                //Length is synced on both sides anyway
                int whoAmI = reader.ReadByte();
                byte[] tempArray = new byte[DataList.Length];
                for (int i = 0; i < DataList.Length; i++)
                {
                    tempArray[i] = reader.ReadByte();
                }
                for (int i = 0; i < DataList.Length; i++)
                {
                    DataList[i].Counter[whoAmI] = tempArray[i];
                    SetCounter(whoAmI, i, tempArray[i], true);
                }
            }
        } //Mod.HandlePacket

        public static void Reset(NPC npc)
        {
            //Single and Server only
            if (DataList != null)
            {
                for (int j = 0; j < 255; j++)
                {
                    if (Main.player[j].active && npc.playerInteraction[j])
                    {
                        for (int i = 0; i < DataList.Length; i++)
                        {
                            //resets even when all but one player is dead and boss is defeated
                            if (npc.boss && Array.BinarySearch(DataList[i].BossTypeList, npc.type) > -1)
                            {
                                DataList[i].Counter[j] = 0;
                                SetCounter(j, i, 0);
                                if (Main.netMode == NetmodeID.Server)
                                {
                                    ModPacket packet = AssUtils.Instance.GetPacket();
                                    packet.Write((byte)AssMessageType.GitgudReset);
                                    packet.Write((byte)i);
                                    packet.Send(); //send to all clients
                                }
                            }
                        }
                    }
                }
            }
        } //NPCLoot, resets for all players

        public static void RecvReset(int whoAmI, BinaryReader reader)
        {
            if (DataList != null)
            {
                int index = reader.ReadByte();
                DataList[index].Counter[whoAmI] = 0;
                SetCounter(whoAmI, index, 0, true);
            }
        } //Mod.HandlePacket

        public static void SpawnItem(Player player)
        {
            if (DataList != null)
            {
                for (int i = 0; i < DataList.Length; i++)
                {
                    if (DataList[i].Counter[player.whoAmI] >= DataList[i].CounterMax)
                    {
                        DataList[i].Counter[player.whoAmI] = 0;
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
        } //OnRespawn

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
        } //PostUpdateEquips

        public static void IncreaseCounters(int whoAmI)
        {
            if (DataList != null)
            {
                bool[] increasedFor = new bool[DataList.Length];
                for (int k = 0; k < 200; k++)
                {
                    if (Main.npc[k].active && Main.npc[k].playerInteraction[whoAmI])
                    {
                        for (int i = 0; i < DataList.Length; i++)
                        {
                            if (!increasedFor[i] && Array.BinarySearch(DataList[i].BossTypeList, Main.npc[k].type) > -1)
                            {
                                AssUtils.Print("increased counter");
                                DataList[i].Counter[whoAmI]++;
                                SetCounter(whoAmI, i, DataList[i].Counter[whoAmI]);
                                increasedFor[i] = true;
                            }
                        }
                    }
                }
            }
        } //PreKill

        public static bool CanReduceDamageNPC(int whoAmI, int npcType)
        {
            if (DataList != null)
            {
                for (int i = 0; i < DataList.Length; i++)
                {
                    if (DataList[i].Accessory[whoAmI])
                    {
                        for (int j = 0; j < DataList[i].NPCTypeList.Length; j++)
                        {
                            //only reduce damage if accessory worn and a boss alive
                            if (Array.BinarySearch(DataList[i].NPCTypeList, npcType) > -1 && AssUtils.AnyNPCs(DataList[i].BossTypeList))
                            {
                                AssUtils.Print("reduced damage " + npcType);
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        } //ModifyHitByNPC

        public static bool CanReduceDamageProj(int whoAmI, int projType)
        {
            if (DataList != null)
            {
                for (int i = 0; i < DataList.Length; i++)
                {
                    if (DataList[i].Accessory[whoAmI])
                    {
                        for (int j = 0; j < DataList[i].ProjTypeList.Length; j++)
                        {
                            //only reduce damage if accessory worn and a boss alive
                            if (Array.BinarySearch(DataList[i].ProjTypeList, projType) > -1 && AssUtils.AnyNPCs(DataList[i].BossTypeList))
                            {
                                AssUtils.Print("reduced damage " + projType);
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        } //ModifyHitByProjectile

        public static void UpdateAccessories(int whoAmI, BitArray accessories)
        {
            if (DataList != null)
            {
                if (accessories.Length != DataList.Length) throw new Exception("number of gitgud accessory bools don't match with the registered gitgud items");
                for (int i = 0; i < accessories.Length; i++) //has to have the same order as DataList
                {
                    DataList[i].Accessory[whoAmI] = accessories[i];
                }
            }
        } //PostUpdateEquips

        public static void LoadCounters(int whoAmI, byte[] counters)
        {
            if (DataList != null)
            {
                if (counters.Length != DataList.Length) throw new Exception("number of gitgud counters don't match with the registered gitgud counters");
                for (int i = 0; i < counters.Length; i++) //has to have the same order as DataList
                {
                    DataList[i].Counter[whoAmI] = counters[i];
                }
            }
        } //OnEnterWorld clientside, Mod.HandlePacket serverside

        public static void Load()
        {
            DataList = new GitgudData[1];

            RegisterItems();

            //since Add always increases the array size by one, it will make it so the last element is null
            Array.Resize(ref DataList, DataList.Length - 1);
            if (DataList.Length == 0) DataList = null;
        } //Mod.Load

        private static void RegisterItems()
        {
            Add("KingSlimeGitgud", -1,
                NPCID.KingSlime,
                nPCTypeList: new int[] { NPCID.BlueSlime },
                projTypeList: new int[] { ProjectileID.SpikedSlimeSpike });
            Add("EyeOfCthulhuGitgud", -1,
                NPCID.EyeofCthulhu,
                nPCTypeList: new int[] { NPCID.ServantofCthulhu });
            Add("BrainOfCthulhuGitgud", BuffID.Slow,
                NPCID.BrainofCthulhu,
                nPCTypeList: new int[] { NPCID.Creeper });
            Add("EaterOfWorldsGitgud", BuffID.Weak,
                new int[] { NPCID.EaterofWorldsBody, NPCID.EaterofWorldsTail, NPCID.EaterofWorldsHead },
                nPCTypeList: new int[] { NPCID.VileSpit });
            Add("QueenBeeGitgud", BuffID.Poisoned,
                NPCID.QueenBee,
                nPCTypeList: new int[] { NPCID.Bee, NPCID.BeeSmall },
                projTypeList: new int[] { ProjectileID.Stinger });
            Add("SkeletronGitgud", BuffID.Bleeding,
                NPCID.SkeletronHead,
                nPCTypeList: new int[] { NPCID.SkeletronHand },
                projTypeList: new int[] { ProjectileID.Skull });
            Add("WallOfFleshGitgud", -1,
                NPCID.WallofFlesh,
                nPCTypeList: new int[] { NPCID.WallofFleshEye },
                projTypeList: new int[] { ProjectileID.EyeLaser });
            //HARDMODE
            Add("DestroyerGitgud", -1,
                NPCID.TheDestroyer,
                nPCTypeList: new int[] { NPCID.TheDestroyerBody, NPCID.TheDestroyerTail, NPCID.Probe },
                projTypeList: new int[] { ProjectileID.PinkLaser });
            Add("TwinsGitgud", BuffID.CursedInferno,
                new int[] { NPCID.Retinazer, NPCID.Spazmatism },
                projTypeList: new int[] { ProjectileID.EyeLaser, ProjectileID.CursedFlameHostile, ProjectileID.EyeFire, });
            Add("SkeletronPrimeGitgud", -1,
                NPCID.SkeletronPrime,
                nPCTypeList: new int[] { NPCID.PrimeCannon, NPCID.PrimeLaser, NPCID.PrimeSaw, NPCID.PrimeVice, },
                projTypeList: new int[] { ProjectileID.DeathLaser, ProjectileID.BombSkeletronPrime, });
            Add("GreenThumb", BuffID.Poisoned,
                NPCID.Plantera,
                nPCTypeList: new int[] { NPCID.PlanterasHook, NPCID.PlanterasTentacle },
                projTypeList: new int[] { ProjectileID.ThornBall, ProjectileID.SeedPlantera, ProjectileID.PoisonSeedPlantera });
            Add("GolemGitgud", BuffID.OnFire,
                NPCID.Golem,
                nPCTypeList: new int[] { NPCID.GolemFistLeft, NPCID.GolemFistRight, NPCID.GolemHead, NPCID.GolemHeadFree },
                projTypeList: new int[] { ProjectileID.Fireball, ProjectileID.EyeBeam });
            Add("DukeFishronGitgud", -1,
                NPCID.DukeFishron,
                nPCTypeList: new int[] { NPCID.DetonatingBubble, NPCID.Sharkron, NPCID.Sharkron2 },
                projTypeList: new int[] { ProjectileID.Sharknado, ProjectileID.SharknadoBolt, ProjectileID.Cthulunado });
            Add("LunaticCultistGitgud", BuffID.OnFire,
                NPCID.CultistBoss,
                nPCTypeList: new int[] { NPCID.AncientCultistSquidhead,/* NPCID.CultistBossClone,*/ },
                projTypeList: new int[] { ProjectileID.CultistBossIceMist, ProjectileID.CultistBossLightningOrb, ProjectileID.CultistBossLightningOrbArc, ProjectileID.CultistBossFireBall, ProjectileID.CultistBossFireBallClone });
            Add("MoonLordGitgud", -1,
                NPCID.MoonLordCore,
                nPCTypeList: new int[] { NPCID.MoonLordFreeEye,/* NPCID.MoonLordHand, NPCID.MoonLordHead, NPCID.MoonLordLeechBlob */}, //don't deal any damage
                projTypeList: new int[] { ProjectileID.PhantasmalEye, ProjectileID.PhantasmalSphere, ProjectileID.PhantasmalDeathray, ProjectileID.PhantasmalBolt });
            //Add("NameOfClassOfItem", <BuffID, or -1>,
            //    <NPCID of boss, or new int[] {NPCID1, NPCID2 etc } if multiple segments of a boss>,
            //    <nPCTypeList: new int[] { NPCID1, NPCID2 etc } for the minions of the boss>,
            //    <projTypeList: new int[] { ProjectileID1, ProjectileID2 etc } for the projectiles of the boss>);
            // Last two optional (if boss is super basic, but I can't think of one)
        }
    }

    public class GitGudPlayer : ModPlayer
    {
        public Func<BitArray> gitgudAccessories;
        //public Func<byte[]> gitgudCounters;
        
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
        }

        public override void Initialize()
        {
            gitgudAccessories = new Func<BitArray>(()=> new BitArray(new bool[]
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
            }
            ));

            //unused atm
            //gitgudCounters = new Func<byte[]>(() => new byte[]
            //{
            //    kingSlimeGitgudCounter,
            //    eyeOfCthulhuGitgudCounter,
            //    brainOfCthulhuGitgudCounter,
            //    eaterOfWorldsGitgudCounter,
            //    queenBeeGitgudCounter,
            //    skeletronGitgudCounter,
            //    wallOfFleshGitgudCounter,
            //    planteraGitgudCounter,
            //}
            //);
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
        }

        public override void ModifyHitByNPC(NPC npc, ref int damage, ref bool crit)
        {
            if (GitgudData.CanReduceDamageNPC(player.whoAmI, npc.type)) damage = (int)(damage * 0.85f);
        }

        public override void ModifyHitByProjectile(Projectile proj, ref int damage, ref bool crit)
        {
            if (GitgudData.CanReduceDamageProj(player.whoAmI, proj.type)) damage = (int)(damage * 0.85f);
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
            });

            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                GitgudData.SendCounters(player.whoAmI);
            }
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
