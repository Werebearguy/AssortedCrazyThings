using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using AssortedCrazyThings.NPCs.DungeonBird;
using Terraria.ModLoader.IO;
using System.IO;
using Microsoft.Xna.Framework;

namespace AssortedCrazyThings
{
    class AssPlayer : ModPlayer
    {
        public bool everburningCandleBuff;
        public bool everburningCursedCandleBuff;
        public bool everfrozenCandleBuff;
        //public bool variable_debuff_04;
        //public bool variable_debuff_05;
        public bool everburningShadowflameCandleBuff;
        //public bool variable_debuff_07;

        public uint slotsPlayer = 0;
        public uint slotsPlayerLast = 0;
        private bool resetSlots = false;
        private double lastTime = 0.0;

        public override void ResetEffects()
        {
            everburningCandleBuff = false;
            everburningCursedCandleBuff = false;
            everfrozenCandleBuff = false;
            //variable_debuff_04 = false;
            //variable_debuff_05 = false;
            everburningShadowflameCandleBuff = false;
            //variable_debuff_07 = false;
        }

        public void SendSlotData()
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                ModPacket packet = mod.GetPacket();
                packet.Write((byte)AssMessageType.PetAccessorySlots);
                packet.Write((byte)player.whoAmI);
                packet.Write(slotsPlayer);
                packet.Write(slotsPlayerLast);
                packet.Send();
            }
        }

        public override TagCompound Save()
        {
            return new TagCompound {
                {"slotsPlayer", (int)slotsPlayer},
            };
        }

        //idk why but I just left it in
        public override void LoadLegacy(BinaryReader reader)
        {
            int loadVersion = reader.ReadInt32();
            slotsPlayer = (uint)reader.ReadInt32();
        }

        public override void Load(TagCompound tag)
        {
            slotsPlayer = (uint)tag.GetInt("slotsPlayer");
        }

        public bool CanResetSlots(double currentTime)
        {
            if (Math.Abs(lastTime - currentTime) > 35.0) //(usetime + 1) x 3 + 1
            {
                resetSlots = false;
                lastTime = currentTime;
                return false; //step one
            }

            //step two and three have to be done in 35 ticks
            if (Math.Abs(lastTime - currentTime) <= 35.0)
            {
                if (!resetSlots)
                {
                    resetSlots = true;
                    return false; //step two
                }

                //if program gets to here, it is about to return true

                if (resetSlots)
                {
                    resetSlots = false;
                    return true; //step three
                }
            }
            //should never get here anyway
            return false;
        }

        public override void OnHitAnything(float x, float y, Entity victim)
        {
            NPC npc = victim as NPC;
            if(npc != null)
            {
                if (everburningCandleBuff)
                {
                    npc.AddBuff(BuffID.OnFire, 120, true);
                }
                if (everburningCursedCandleBuff)
                {
                    npc.AddBuff(BuffID.CursedInferno, 120, true);
                }
                if (everfrozenCandleBuff)
                {
                    npc.AddBuff(BuffID.Frostburn, 120, true);
                }
                //if (variable_debuff_04)
                //{
                //    npc.AddBuff(BuffID.Ichor, 120, true);
                //}
                //if (variable_debuff_05)
                //{
                //    npc.AddBuff(BuffID.Venom, 120, true);
                //}
                if (everburningShadowflameCandleBuff)
                {
                    npc.AddBuff(BuffID.ShadowFlame, 60, true);
                }
                //if (variable_debuff_07)
                //{
                //    npc.AddBuff(BuffID.Bleeding, 120, true);
                //}
            }
        }

        private void SpawnSoulsWhenHarvesterIsAlive()
        {
            if (player.ZoneOverworldHeight || player.ZoneDungeon) //change to dungeon
            {
                bool shouldDropSouls = false; //change to false
                for (short j = 0; j < 200; j++)
                {
                    if (Main.npc[j].active && Array.IndexOf(AssWorld.harvesterTypes, Main.npc[j].type) != -1)
                    {
                        shouldDropSouls = true;
                        break;
                    }
                }

                if (shouldDropSouls)
                {
                    for (short j = 0; j < 200; j++)
                    {
                        if (Main.npc[j].active && Main.npc[j].type != mod.NPCType(aaaSoul.name) && Array.IndexOf(AssWorld.harvesterTypes, Main.npc[j].type) == -1 && Main.npc[j].lifeMax > 5 && !Main.npc[j].friendly)
                        {
                            Main.npc[j].AddBuff(mod.BuffType("SoulBuff"), 60, true);
                        }
                    }
                }
            }
        }

        public override void PreUpdate()
        {
            SpawnSoulsWhenHarvesterIsAlive();
        }
    }
}
