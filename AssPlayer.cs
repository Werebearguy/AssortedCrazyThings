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

        public bool teleportHome = false;
        public bool canTeleportHome = false;
        private const short TeleportHomeTimerMax = 30; //in seconds //10 ingame minutes
        public short teleportHomeTimer = 0; //gets saved when you relog so you cant cheese it

        //TECHNICALLY NOT DEFENCE; YOU JUST GET 1 DAMAGE FROM EVERYTHING FOR A CERTAIN DURATION
        public bool getDefense = false;
        public bool canGetDefense = false;
        private const short GetDefenseTimerMax = 30; //in seconds //10ingame minutes
        private const short GetDefenseDurationMax = 600; //in ticks //10 ingame seconds
        public short getDefenseDuration = 0;
        public short getDefenseTimer = 0; //gets saved when you relog so you cant cheese it

        public uint slotsPlayer = 0;
        public uint slotsPlayerLast = 0;
        private bool resetSlots = false;
        private double lastTime = 0.0;

        public bool soulArmorMinions = false;

        public override void ResetEffects()
        {
            everburningCandleBuff = false;
            everburningCursedCandleBuff = false;
            everfrozenCandleBuff = false;
            //variable_debuff_04 = false;
            //variable_debuff_05 = false;
            everburningShadowflameCandleBuff = false;
            //variable_debuff_07 = false;
            teleportHome = false;
            getDefense = false;
            soulArmorMinions = false;
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
                {"teleportHomeWhenLowTimer", (int)teleportHomeTimer},
                {"getDefenseTimer", (int)getDefenseTimer},
            };
        }

        //idk why but I just left it in
        public override void LoadLegacy(BinaryReader reader)
        {
            int loadVersion = reader.ReadInt32();
            slotsPlayer = (uint)reader.ReadInt32();
            teleportHomeTimer = (short)reader.ReadInt32();
            getDefenseTimer = (short)reader.ReadInt32();
        }

        public override void Load(TagCompound tag)
        {
            slotsPlayer = (uint)tag.GetInt("slotsPlayer");
            teleportHomeTimer = (short)tag.GetInt("teleportHomeWhenLowTimer");
            getDefenseTimer = (short)tag.GetInt("getDefenseTimer");
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
                        if (Main.npc[j].active && Main.npc[j].type != mod.NPCType<aaaDungeonSoul>() && Array.IndexOf(AssWorld.harvesterTypes, Main.npc[j].type) == -1 && Main.npc[j].lifeMax > 5 && !Main.npc[j].friendly)
                        {
                            Main.npc[j].AddBuff(mod.BuffType("SoulBuff"), 60, true);
                        }
                    }
                }
            }
        }

        public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
        {
            //getDefense before teleportHome (so you dont teleport BEFORE you gain the defense)
            if (getDefense)
            {
                if (canGetDefense)
                {
                    player.statLife += (int)damage;
                    player.AddBuff(BuffID.RapidHealing, 300);
                    CombatText.NewText(new Rectangle((int)player.position.X, (int)player.position.Y, player.width, player.height), CombatText.HealLife, "Defense increased");

                    getDefenseTimer = GetDefenseTimerMax;
                    getDefenseDuration = GetDefenseDurationMax;
                    return false;
                }
            }

            if (teleportHome)
            {
                if (canTeleportHome)
                {
                    //this part here is from vanilla magic mirror code
                    int num3;
                    for (int num326 = 0; num326 < 70; num326 = num3 + 1)
                    {
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
                    
                    //inserted before player.Spawn()
                    player.statLife += (int)damage;

                    player.Spawn();
                    for (int num328 = 0; num328 < 70; num328 = num3 + 1)
                    {
                        Dust.NewDust(player.position, player.width, player.height, 15, 0f, 0f, 150, default(Color), 1.5f);
                        num3 = (int)(num328 * 1.5f);
                    }
                    //end
                    
                    player.AddBuff(BuffID.RapidHealing, 300);

                    teleportHomeTimer = TeleportHomeTimerMax;
                    return false;
                }
            }

            return base.PreKill(damage, hitDirection, pvp, ref playSound, ref genGore, ref damageSource);
        }

        public override bool PreHurt(bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
        {
            if (getDefenseDuration != 0)
            {
                damage = 1;
            }

            return base.PreHurt(pvp, quiet, ref damage, ref hitDirection, ref crit, ref customDamage, ref playSound, ref genGore, ref damageSource);
        }

        private void UpdateTeleportHomeWhenLow()
        {
            //this code runs even when the accessory is not equipped
            canTeleportHome = teleportHomeTimer <= 0;

            if (!canTeleportHome && Main.time % 60 == 59)
            {
                teleportHomeTimer--;
            }
        }

        private void UpdateGetDefenseWhenLow()
        {
            //this code runs even when the accessory is not equipped
            canGetDefense = getDefenseTimer <= 0;

            if (!canGetDefense && Main.time % 60 == 59)
            {
                getDefenseTimer--;
            }

            if (getDefenseDuration != 0)
            {
                getDefenseDuration--;
            }
        }

        public override void PreUpdate()
        {
            SpawnSoulsWhenHarvesterIsAlive();

            UpdateTeleportHomeWhenLow();

            UpdateGetDefenseWhenLow();
        }
    }
}
