using Microsoft.Xna.Framework;
using System;
using System.Collections;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

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
                    gPlayer.planteraGitgudCounter = value;
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
        } //in OnEnterWorld

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
        } //in HandlePacket

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
                            if (npc.boss && Array.IndexOf(DataList[i].BossTypeList, npc.type) != -1)
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
        } //in NPCLoot, called when npc.boss == true, resets for all players

        public static void RecvReset(int whoAmI, BinaryReader reader)
        {
            if (DataList != null)
            {
                int index = reader.ReadByte();
                DataList[index].Counter[whoAmI] = 0;
                SetCounter(whoAmI, index, 0, true);
            }
        } //in HandlePacket

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
        } //called in OnRespawn

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
        } //called in UpdateEquips

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
                            if (!increasedFor[i] && Array.IndexOf(DataList[i].BossTypeList, Main.npc[k].type) != -1)
                            {
                                DataList[i].Counter[whoAmI]++;
                                SetCounter(whoAmI, i, DataList[i].Counter[whoAmI]);
                                 increasedFor[i] = true;
                            }
                        }
                    }
                }
            }
        } //called in PreKill

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
                            if (Array.IndexOf(DataList[i].NPCTypeList, npcType) != -1 && AssUtils.AnyNPCs(DataList[i].BossTypeList))
                            {
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
                            if (Array.IndexOf(DataList[i].ProjTypeList, projType) != -1 && AssUtils.AnyNPCs(DataList[i].BossTypeList))
                            {
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
        } //passed with mPlayer.accessory to set the datalist stuff

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
        } //OnEnterWorld clientside, and in HandlePacket serverside
        
        public static void RegisterItems()
        {
            Add("KingSlimeGitgud", -1,
                NPCID.KingSlime,
                new int[] { NPCID.BlueSlime },
                new int[] { ProjectileID.SpikedSlimeSpike });
            Add("EyeOfCthulhuGitgud", -1,
                NPCID.EyeofCthulhu,
                new int[] { NPCID.ServantofCthulhu });
            Add("BrainOfCthulhuGitgud", BuffID.Slow,
                NPCID.BrainofCthulhu,
                new int[] { NPCID.Creeper });
            Add("EaterOfWorldsGitgud", BuffID.Weak,
                new int[] { NPCID.EaterofWorldsBody, NPCID.EaterofWorldsTail, NPCID.EaterofWorldsHead },
                new int[] { NPCID.VileSpit });
            Add("QueenBeeGitgud", BuffID.Poisoned,
                NPCID.QueenBee,
                new int[] { NPCID.Bee, NPCID.BeeSmall },
                new int[] { ProjectileID.Stinger });
            Add("SkeletronGitgud", BuffID.Bleeding,
                NPCID.SkeletronHead,
                new int[] { NPCID.SkeletronHand },
                new int[] { ProjectileID.Skull });
            Add("WallOfFleshGitgud", -1,
                NPCID.WallofFlesh,
                new int[] { NPCID.WallofFleshEye },
                new int[] { ProjectileID.EyeLaser });
            Add("GreenThumb", BuffID.Poisoned,
                NPCID.Plantera,
                new int[] { NPCID.PlanterasHook, NPCID.PlanterasTentacle },
                new int[] { ProjectileID.ThornBall, ProjectileID.SeedPlantera, ProjectileID.PoisonSeedPlantera });
        } //

        public static void Load()
        {
            DataList = new GitgudData[1];

            RegisterItems();

            //since Add always increases the array size by one, it will make it so the last element is null
            Array.Resize(ref DataList, DataList.Length - 1);
            if (DataList.Length == 0) DataList = null;
        }
    }

    public class GitGudPlayer : ModPlayer
    {
        //other places where adjustments are needed:
        //GitGudReset in AssGlobalNPC
        //HandlePacket in ACT, and the enum

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
        
        public byte planteraGitgudCounter = 0;
        public bool planteraGitgud = false;

        public override void ResetEffects()
        {
            kingSlimeGitgud = false;
            eyeOfCthulhuGitgud = false;
            brainOfCthulhuGitgud = false;
            eaterOfWorldsGitgud = false;
            queenBeeGitgud = false;
            skeletronGitgud = false;
            wallOfFleshGitgud = false;
            planteraGitgud = false;
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
                planteraGitgud,
            }
            ));

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
                {"planteraGitgudCounter", (byte)planteraGitgudCounter},
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
            if (tag.ContainsKey("planteraGitGudCounter"))
            {
                planteraGitgudCounter = tag.GetByte("planteraGitGudCounter"); //the one with the typo
            }
            else
            {
                planteraGitgudCounter = tag.GetByte("planteraGitgudCounter");
            }
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
                planteraGitgudCounter,
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
        }

        public override void OnRespawn(Player player)
        {
            GitgudData.SpawnItem(player);
        }
    }
}
