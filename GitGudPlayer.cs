using AssortedCrazyThings.Items.Gitgud;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
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
        public static GitgudData[] DataList = new GitgudData[1];

        private string ItemName { set; get; } //name for ToString
        public int BuffType { private set; get; } //buff immunity while boss is alive
        public int ItemType { private set; get; } //droped gitgud item
        public int[] BossTypeList { private set; get; } //boss type (usually only one, can be multiple for worms)
        public int[] NPCTypeList { private set; get; } //NPCs that deal damage during the boss fight (boss minions) (includes the boss itself by default)
        public int[] ProjTypeList { private set; get; } //Projectiles that deal damage during the boss fight
        public bool Accessory { private set; get; } //the bool that is set by the wearing accessory
        public byte Counter { set; get; } //number of times player died to the boss
        public byte CounterMax { private set; get; } //threshold, after which the item drops

        public GitgudData(string itemName, int itemType, int buffType,
            int[] bossTypeList, int[] nPCTypeList, int[] projTypeList,
            bool accessory, byte counter, byte counterMax)
        {
            ItemName = itemName;
            ItemType = itemType;
            BuffType = buffType;
            BossTypeList = bossTypeList;

            if (nPCTypeList == null) nPCTypeList = new int[1];

            //int[] combined = new int[nPCTypeList.Length + bossTypeList.Length];
            //Array.Copy(bossTypeList, combined, bossTypeList.Length);
            //Array.Copy(nPCTypeList, 0, combined, bossTypeList.Length, nPCTypeList.Length);
            //NPCTypeList = combined;
            NPCTypeList = AssUtils.ConcatArray(nPCTypeList, bossTypeList);

            if (projTypeList == null) projTypeList = new int[1];

            ProjTypeList = projTypeList;
            Accessory = accessory;
            Counter = counter;
            CounterMax = counterMax;
        }

        public override string ToString()
        {
            return ItemName;
        }

        public static void Add(string itemName, int buffType,
            int[] bossTypeList, int[] nPCTypeList = null, int[] projTypeList = null,
            bool accessory = false, byte counter = 0, byte counterMax = 5)
        {
            int itemType = AssUtils.Instance.ItemType(itemName);
            if (itemType == 0) throw new Exception("no gitgud item called '" + itemName + "' found. Did you spell it correctly?");

            DataList[DataList.Length - 1] = new GitgudData(itemName, itemType, buffType, bossTypeList, nPCTypeList, projTypeList, accessory, counter, counterMax);
            Array.Resize(ref DataList, DataList.Length + 1);
        }

        public static void Add(string itemName, int buffType,
            int bossType, int[] nPCTypeList = null, int[] projTypeList = null,
            bool accessory = false, byte counter = 0, byte counterMax = 5)
        {
            Add(itemName, buffType, new int[] { bossType }, nPCTypeList, projTypeList, accessory, counter, counterMax);
        }

        public static void SendCounters(ModPacket packet)
        {
            if (DataList != null)
            {
                //Length is synced on both sides anyway
                for (int i = 0; i < DataList.Length; i++)
                {
                    packet.Send((byte)DataList[i].Counter);
                }
            }
        } //in OnEnterWorld

        public static void RecvCounters(BinaryReader reader)
        {
            if (DataList != null)
            {
                //Length is synced on both sides anyway
                for (int i = 0; i < DataList.Length; i++)
                {
                    DataList[i].Counter = reader.ReadByte();
                }
            }
        } //in HandlePacket

        public static void Reset(int bossType) //in NPCLoot, called when npc.boss == true
        {
            if (DataList != null)
            {
                //Single and Server only
                for (int i = 0; i < 255; i++)
                {
                    if (Main.player[i].active)
                    {
                        for (int j = 0; j < DataList.Length; j++)
                        {
                            //resets even when all but one player is dead and boss is defeated
                            if (Array.IndexOf(DataList[j].BossTypeList, bossType) != -1)
                            {
                                DataList[j].Counter = 0;
                                if (Main.netMode == NetmodeID.Server)
                                {
                                    ModPacket packet = AssUtils.Instance.GetPacket();
                                    packet.Write((byte)AssMessageType.ResetGitGud);
                                    packet.Write((byte)j);
                                    packet.Send(); //send to all clients
                                }
                            }
                        }
                    }
                }
            }
        }

        public static void RecvReset(byte index)
        {
            if (DataList != null)
            {
                DataList[index].Counter = 0;
            }
        } //in HandlePacket

        public static void SpawnItem(Player player) //called in OnRespawn
        {
            if (DataList != null)
            {
                for (int i = 0; i < DataList.Length; i++)
                {
                    if (DataList[i].Counter >= DataList[i].CounterMax)
                    {
                        DataList[i].Counter = 0;
                        if (!player.HasItem(DataList[i].ItemType) && !DataList[i].Accessory)
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

        public static void ApplyBuffImmune(Player player) //called in UpdateEquips
        {
            if (DataList != null)
            {
                for (int i = 0; i < DataList.Length; i++)
                {
                    if (DataList[i].Accessory && DataList[i].BuffType != -1 && AssUtils.AnyNPCs(DataList[i].BossTypeList)) player.buffImmune[DataList[i].BuffType] = true;
                }
            }
        }

        public static void IncreaseCounters() //called in PreKill
        {
            if (DataList != null)
            {
                for (int k = 0; k < 200; k++)
                {
                    if (Main.npc[k].active)
                    {
                        for (int i = 0; i < DataList.Length; i++)
                        {
                            if (Array.IndexOf(DataList[i].BossTypeList, Main.npc[k].type) != -1) DataList[i].Counter++;
                        }
                    }
                }
            }
        }

        public static bool CanReduceDamageNPC(int npcType)
        {
            if (DataList != null)
            {
                for (int i = 0; i < DataList.Length; i++)
                {
                    if (DataList[i].Accessory)
                    {
                        for (int j = 0; j < DataList[i].NPCTypeList.Length; j++)
                        {
                            if (Array.IndexOf(DataList[i].NPCTypeList, npcType) != -1) return true;
                        }
                    }
                }
            }
            return false;
        } //ModifyHitByNPC

        public static bool CanReduceDamageProj(int projType)
        {
            if (DataList != null)
            {
                for (int i = 0; i < DataList.Length; i++)
                {
                    if (DataList[i].Accessory)
                    {
                        for (int j = 0; j < DataList[i].ProjTypeList.Length; j++)
                        {
                            if (Array.IndexOf(DataList[i].ProjTypeList, projType) != -1) return true;
                        }
                    }
                }
            }
            return false;
        } //ModifyHitByProjectile

        public static void UpdateAccessories(bool[] accessories) //passed with mPlayer.accessory to set the datalist stuff
        {
            if (DataList != null)
            {
                if (accessories.Length != DataList.Length) throw new Exception("number of gitgud accessory bools don't match with the registered gitgud items");
                for (int i = 0; i < accessories.Length; i++) //has to have the same order as DataList
                {
                    DataList[i].Accessory = accessories[i];
                }
            }
        }

        public static void LoadCounters(byte[] counters) //Load
        {
            if (DataList != null)
            {
                if (counters.Length != DataList.Length) throw new Exception("number of gitgud counters don't match with the registered gitgud counters");
                for (int i = 0; i < counters.Length; i++) //has to have the same order as DataList
                {
                    DataList[i].Counter = counters[i];
                }
            }
        }

        public static byte[] SaveCounters() //Save
        {
            List<byte> counterList = new List<byte>();
            if (DataList != null)
            {
                for (int i = 0; i < DataList.Length; i++)
                {
                    counterList.Add(DataList[i].Counter);
                }
            }
            return counterList.ToArray();
        }

        public static void Load()
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
            //since Add always increases the array size by one, it will make it so the last element is null

            Array.Resize(ref DataList, DataList.Length - 1);
        }
    }

    public class GitGudPlayer : ModPlayer
    {
        //other places where adjustments are needed:
        //GitGudReset in AssGlobalNPC
        //HandlePacket in ACT, and the enum

        public const byte kingSlimeGitgudCounterMax = 5;
        public byte kingSlimeGitgudCounter = 0;
        public bool kingSlimeGitgud = false;

        public const byte eyeOfCthulhuGitgudCounterMax = 5;
        public byte eyeOfCthulhuGitgudCounter = 0;
        public bool eyeOfCthulhuGitgud = false;

        public const byte brainOfCthulhuGitgudCounterMax = 5;
        public byte brainOfCthulhuGitgudCounter = 0;
        public bool brainOfCthulhuGitgud = false;

        public const byte eaterOfWorldsGitgudCounterMax = 5;
        public byte eaterOfWorldsGitgudCounter = 0;
        public bool eaterOfWorldsGitgud = false;

        public const byte queenBeeGitgudCounterMax = 5;
        public byte queenBeeGitgudCounter = 0;
        public bool queenBeeGitgud = false;

        public const byte skeletronGitgudCounterMax = 5;
        public byte skeletronGitgudCounter = 0;
        public bool skeletronGitgud = false;

        public const byte wallOfFleshGitgudCounterMax = 5;
        public byte wallOfFleshGitgudCounter = 0;
        public bool wallOfFleshGitgud = false;

        public const byte planteraGitgudCounterMax = 2;
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
            if ((kingSlimeGitgud && (npc.type == NPCID.KingSlime || npc.type == NPCID.BlueSlime)) ||
                (eyeOfCthulhuGitgud && (npc.type == NPCID.EyeofCthulhu || npc.type == NPCID.ServantofCthulhu)) ||
                (brainOfCthulhuGitgud && (npc.type == NPCID.BrainofCthulhu || npc.type == NPCID.Creeper)) ||
                (eaterOfWorldsGitgud && (npc.type == NPCID.EaterofWorldsBody || npc.type == NPCID.EaterofWorldsTail || npc.type == NPCID.EaterofWorldsHead || npc.type == NPCID.VileSpit)) ||
                (queenBeeGitgud && (npc.type == NPCID.QueenBee || npc.type == NPCID.Bee || npc.type == NPCID.BeeSmall)) ||
                (skeletronGitgud && (npc.type == NPCID.SkeletronHead || npc.type == NPCID.SkeletronHand)) ||
                (wallOfFleshGitgud && (npc.type == NPCID.WallofFlesh || npc.type == NPCID.WallofFleshEye || npc.type == NPCID.TheHungry || npc.type == NPCID.TheHungryII)) ||
               (planteraGitgud && (npc.type == NPCID.Plantera || npc.type == NPCID.PlanterasHook || npc.type == NPCID.PlanterasTentacle)))
            {
                damage = (int)(damage * 0.85f);
            }
        }

        public override void ModifyHitByProjectile(Projectile proj, ref int damage, ref bool crit)
        {
            if ((kingSlimeGitgud && proj.type == ProjectileID.SpikedSlimeSpike) ||
                /*(eyeOfCthulhuGitgud) ||*/
                /*(brainOfCthulhuGitgud) ||*/
                /*(eaterOfWorldsGitgud) ||*/
                (queenBeeGitgud && proj.type == ProjectileID.Stinger) ||
                (skeletronGitgud && proj.type == ProjectileID.Skull) ||
                (wallOfFleshGitgud && proj.type == ProjectileID.EyeLaser) ||
                (planteraGitgud && (proj.type == ProjectileID.ThornBall || proj.type == ProjectileID.SeedPlantera || proj.type == ProjectileID.PoisonSeedPlantera)))
            {
                damage = (int)(damage * 0.85f);
            }
        }

        public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
        {
            //to not call NPC.AnyNPCs() for every boss, do it manually
            for (int k = 0; k < 200; k++)
            {
                if (Main.npc[k].active)
                {
                    if (Main.npc[k].type == NPCID.KingSlime) kingSlimeGitgudCounter++;
                    if (Main.npc[k].type == NPCID.EyeofCthulhu) eyeOfCthulhuGitgudCounter++;
                    if (Main.npc[k].type == NPCID.BrainofCthulhu) brainOfCthulhuGitgudCounter++;
                    if (Main.npc[k].type == NPCID.EaterofWorldsHead) eaterOfWorldsGitgudCounter++;
                    if (Main.npc[k].type == NPCID.QueenBee) queenBeeGitgudCounter++;
                    if (Main.npc[k].type == NPCID.SkeletronHead) skeletronGitgudCounter++;
                    if (Main.npc[k].type == NPCID.WallofFlesh) wallOfFleshGitgudCounter++;
                    if (Main.npc[k].type == NPCID.Plantera) planteraGitgudCounter++;
                }
            }

            return true;
        }

        private void UpdateGitGud(Player player)
        {
            if (kingSlimeGitgudCounter >= kingSlimeGitgudCounterMax)
            {
                kingSlimeGitgudCounter = 0;
                if (!player.HasItem(mod.ItemType<KingSlimeGitgud>()) && !kingSlimeGitgud)
                {
                    Item.NewItem(player.getRect(), mod.ItemType<KingSlimeGitgud>());
                }
            }

            if (eyeOfCthulhuGitgudCounter >= eyeOfCthulhuGitgudCounterMax)
            {
                eyeOfCthulhuGitgudCounter = 0;
                if (!player.HasItem(mod.ItemType<EyeOfCthulhuGitgud>()) && !eyeOfCthulhuGitgud)
                {
                    Item.NewItem(player.getRect(), mod.ItemType<EyeOfCthulhuGitgud>());
                }
            }

            if (brainOfCthulhuGitgud && NPC.AnyNPCs(NPCID.BrainofCthulhu)) player.buffImmune[BuffID.Slow] = true;

            if (brainOfCthulhuGitgudCounter >= brainOfCthulhuGitgudCounterMax)
            {
                brainOfCthulhuGitgudCounter = 0;
                if (!player.HasItem(mod.ItemType<BrainOfCthulhuGitgud>()) && !brainOfCthulhuGitgud)
                {
                    Item.NewItem(player.getRect(), mod.ItemType<BrainOfCthulhuGitgud>());
                }
            }

            if (eaterOfWorldsGitgud && NPC.AnyNPCs(NPCID.EaterofWorldsHead)) player.buffImmune[BuffID.Weak] = true;

            if (eaterOfWorldsGitgudCounter >= eaterOfWorldsGitgudCounterMax)
            {
                eaterOfWorldsGitgudCounter = 0;
                if (!player.HasItem(mod.ItemType<EaterOfWorldsGitgud>()) && !eaterOfWorldsGitgud)
                {
                    Item.NewItem(player.getRect(), mod.ItemType<EaterOfWorldsGitgud>());
                }
            }

            if (queenBeeGitgud && NPC.AnyNPCs(NPCID.QueenBee)) player.buffImmune[BuffID.Poisoned] = true;

            if (queenBeeGitgudCounter >= queenBeeGitgudCounterMax)
            {
                queenBeeGitgudCounter = 0;
                if (!player.HasItem(mod.ItemType<QueenBeeGitgud>()) && !queenBeeGitgud)
                {
                    Item.NewItem(player.getRect(), mod.ItemType<QueenBeeGitgud>());
                }
            }

            if (skeletronGitgud && NPC.AnyNPCs(NPCID.SkeletronHead)) player.buffImmune[BuffID.Bleeding] = true;

            if (skeletronGitgudCounter >= skeletronGitgudCounterMax)
            {
                skeletronGitgudCounter = 0;
                if (!player.HasItem(mod.ItemType<SkeletronGitgud>()) && !skeletronGitgud)
                {
                    Item.NewItem(player.getRect(), mod.ItemType<SkeletronGitgud>());
                }
            }

            if (wallOfFleshGitgudCounter >= wallOfFleshGitgudCounterMax)
            {
                wallOfFleshGitgudCounter = 0;
                if (!player.HasItem(mod.ItemType<WallOfFleshGitgud>()) && !wallOfFleshGitgud)
                {
                    Item.NewItem(player.getRect(), mod.ItemType<WallOfFleshGitgud>());
                }
            }

            if (planteraGitgud && NPC.AnyNPCs(NPCID.Plantera)) player.buffImmune[BuffID.Poisoned] = true;

            if (planteraGitgudCounter >= planteraGitgudCounterMax)
            {
                planteraGitgudCounter = 0;
                if (!player.HasItem(mod.ItemType<GreenThumb>()) && !planteraGitgud)
                {
                    int spawnX = Main.spawnTileX - 1;
                    int spawnY = Main.spawnTileY - 1;
                    if (player.SpawnX != -1 && player.SpawnY != -1)
                    {
                        spawnX = player.SpawnX;
                        spawnY = player.SpawnY;
                    }
                    Item.NewItem(new Vector2(spawnX, spawnY) * 16, mod.ItemType<GreenThumb>());
                }
            }

            //others
        }

        public override void OnEnterWorld(Player player)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                //sent in OnEnterWorld to tell the server about the loaded values in tagcompound
                ModPacket packet = mod.GetPacket();
                packet.Write((byte)AssMessageType.SendClientChangesGitGud);
                packet.Write((byte)player.whoAmI);
                packet.Write((byte)kingSlimeGitgudCounter);
                packet.Write((byte)eyeOfCthulhuGitgudCounter);
                packet.Write((byte)brainOfCthulhuGitgudCounter);
                packet.Write((byte)eaterOfWorldsGitgudCounter);
                packet.Write((byte)queenBeeGitgudCounter);
                packet.Write((byte)skeletronGitgudCounter);
                packet.Write((byte)wallOfFleshGitgudCounter);
                packet.Write((byte)planteraGitgudCounter);
                packet.Send();
            }
        }

        public override void OnRespawn(Player player)
        {
            UpdateGitGud(player);
        }
    }
}
