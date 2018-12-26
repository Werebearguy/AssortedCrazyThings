using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using AssortedCrazyThings.NPCs.DungeonBird;
using Terraria.ModLoader.IO;
using System.IO;

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

        public override void OnHitAnything(float x, float y, Entity victim)
        {
            NPC npc = new NPC();
            if (victim is NPC)
            {
                npc = (NPC)victim;
            }
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

        public override void PreUpdate()
        {
            if(player.ZoneOverworldHeight || player.ZoneDungeon) //change to dungeon
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
    }
}
