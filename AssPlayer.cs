using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using AssortedCrazyThings.NPCs.DungeonBird;
using AssortedCrazyThings.Projectiles;

namespace AssortedCrazyThings
{
    class AssPlayer : ModPlayer
    {
        public bool everburningCandleBuff;
        public bool everburningCursedCandleBuff;
        public bool everfrozenCandleBuff;
        public bool variable_debuff_04;
        public bool variable_debuff_05;
        public bool everburningShadowflameCandleBuff;
        public bool variable_debuff_07;

        private bool apply = false;
        public uint slotsPlayer = 0;
        private int applyDelay = 5;

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

            //if (applyDelay > 0)
            //{
            //    applyDelay--;
            //}
            //else if (apply)
            //{
            //    apply = false;
            //    for (int i = 0; i < 1000; i++)
            //    {
            //        if (Main.projectile[i].active)
            //        {
            //            if (Main.projectile[i].owner == player.whoAmI && Array.IndexOf(AssWorld.slimeTypes, Main.projectile[i].type) != -1)
            //            {
            //                Main.projectile[i].GetGlobalProjectile<AssGlobalProjectile>(mod).slots = slotsPlayer; //gets automatically applied since PostDraw is per tick
            //                break;
            //            }
            //            //find first occurence of a player owned cute slime
            //        }
            //    }
            //}
        }

        public override void OnRespawn(Player player)
        {
            apply = true;
            applyDelay = 5;
        }
    }
}
