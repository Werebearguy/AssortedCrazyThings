using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using AssortedCrazyThings.NPCs.DungeonBird;
using Terraria.ModLoader.IO;
using System.IO;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

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

        public bool teleportHomeWhenLow = false;
        public bool canTeleportHomeWhenLow = false;
        private const short TeleportHomeWhenLowTimerMax = 140; //1200 //20 ingame minutes
        public short teleportHomeWhenLowTimer = 0; //gets saved when you relog so you cant cheese it

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
            teleportHomeWhenLow = false;
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
                {"teleportHomeWhenLowTimer", (int)teleportHomeWhenLowTimer},
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
            teleportHomeWhenLowTimer = (short)tag.GetInt("teleportHomeWhenLowTimer");
        }

        public bool ThreeTimesUseTime(double currentTime)
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

        public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
        {
            if (teleportHomeWhenLow)
            {
                if (canTeleportHomeWhenLow)
                {
                    //player.immuneTime = 120; //2 seconds immune time

                    //actually do something
                    //player.Teleport(new Vector2(player.SpawnX, player.SpawnY), Style: 0);

                    //this part here is from vanilla magic mirror code
                    int num3;
                    for (int num326 = 0; num326 < 70; num326 = num3 + 1)
                    {
                        //Dust.NewDust(base.position, width, height, 15, base.velocity.X * 0.5f, base.velocity.Y * 0.5f, 150, default(Color), 1.5f);
                        num3 = num326;
                    }
                    player.grappling[0] = -1;
                    player.grapCount = 0;
                    for (int num327 = 0; num327 < 1000; num327 = num3 + 1)
                    {
                        if (Main.projectile[num327].active && Main.projectile[num327].owner == player.whoAmI && Main.projectile[num327].aiStyle == 7)
                        {
                            Main.projectile[num327].Kill();
                        }
                        num3 = num327;
                    }
                    player.Spawn();
                    for (int num328 = 0; num328 < 70; num328 = num3 + 1)
                    {
                        Dust.NewDust(player.position, player.width, player.height, 15, 0f, 0f, 150, default(Color), 1.5f);
                        num3 = (int)(num328 * 1.5f);
                    }
                    //end

                    player.statLife += 50;
                    player.HealEffect(50, broadcast: true);

                    teleportHomeWhenLowTimer = TeleportHomeWhenLowTimerMax;
                    return false;
                }
            }
            return base.PreKill(damage, hitDirection, pvp, ref playSound, ref genGore, ref damageSource);
        }

        private void UpdateTeleportHomeWhenLow()
        {
            //this code runs even when the accessory is not equipped
            canTeleportHomeWhenLow = teleportHomeWhenLowTimer <= 0;

            if (!canTeleportHomeWhenLow && Main.time % 60 == 59)
            {
                teleportHomeWhenLowTimer--;
            }
        }

        public override void PreUpdate()
        {
            SpawnSoulsWhenHarvesterIsAlive();

            UpdateTeleportHomeWhenLow();
        }
    }
}
