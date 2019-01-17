using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using AssortedCrazyThings.NPCs.DungeonBird;
using Terraria.ModLoader.IO;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using AssortedCrazyThings.Projectiles;
using AssortedCrazyThings.Projectiles.Minions;

namespace AssortedCrazyThings
{
    class AssPlayer : ModPlayer
    {
        public bool everburningCandleBuff = false;
        public bool everburningCursedCandleBuff = false;
        public bool everfrozenCandleBuff = false;
        //public bool variable_debuff_04;
        //public bool variable_debuff_05;
        public bool everburningShadowflameCandleBuff = false;
        //public bool variable_debuff_07;

        public bool teleportHome = false;
        public bool canTeleportHome = false;
        public const short TeleportHomeTimerMax = 600; //in seconds //10 ingame minutes
        public short teleportHomeTimer = 0; //gets saved when you relog so you cant cheese it

        //TECHNICALLY NOT DEFENCE; YOU JUST GET 1 DAMAGE FROM EVERYTHING FOR A CERTAIN DURATION
        public bool getDefense = false;
        public bool canGetDefense = false;
        public const short GetDefenseTimerMax = 600; //in seconds //10 ingame minutes
        private const short GetDefenseDurationMax = 600; //in ticks //10 ingame seconds
        public short getDefenseDuration = 0;
        public short getDefenseTimer = 0; //gets saved when you relog so you cant cheese it

        //slime accessory stuff
        public int slimePetIndex = -1;
        public int slimePetType = 0;
        public bool petTypeChanged = false;
        public uint slotsPlayer = 0;
        public uint slotsPlayerLast = 0;
        private bool resetSlots = false;
        private double lastTime = 0.0;
        //private byte joinDelaySend = 60;
        //public int counter = 30;
        //public int clientcounter = 30;

        //docile demon eye stuff
        public int eyePetIndex = -1;
        public byte eyePetType = 0; //texture type, not ID

        public bool mechFrogCrown = false;

        public bool soulMinion = false;
        public bool tempSoulMinion = false;

        //empowering buff stuff
        public bool empoweringBuff = false;
        private const short empoweringTimerMax = 60; //in seconds //one minute until it caps out (independent of buff duration)
        private short empoweringTimer = 0;
        public static float empoweringTotal = 1.5f; //this gets modified in AssWorld load and is updated there aswell
        public float step;

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
            soulMinion = false;
            tempSoulMinion = false;
            empoweringBuff = false;
        }

        public override void clientClone(ModPlayer clientClone)
        {
            //AssPlayer clone = clientClone as AssPlayer;
            //// Here we would make a backup clone of values that are only correct on the local players Player instance.
            //// Some examples would be RPG stats from a GUI, Hotkey states, and Extra Item Slots
            //clone.petIndex = petIndex;
            //clone.slotsPlayer = slotsPlayer;
        }

        public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
        {
            ////like OnEnterWorld but serverside
            ////HarvesterBase.Print("send SyncPlayer " + toWho + " " + fromWho + " " + newPlayer);
            //ModPacket packet = mod.GetPacket();
            //packet.Write((byte)AssMessageType.SyncPlayer);
            //packet.Write((byte)player.whoAmI);
            //packet.Write(slotsPlayer);
            //packet.Send(toWho, fromWho);
        }

        public override void SendClientChanges(ModPlayer clientPlayer)
        {
            //// Here we would sync something like an RPG stat whenever the player changes it.
            //AssPlayer clone = clientPlayer as AssPlayer;
            //if (clone.slotsPlayer != slotsPlayer || clone.petIndex != petIndex || (petIndex != -1 && clone.petIndex != -1 && Main.projectile[clone.petIndex].type != Main.projectile[petIndex].type))
            //{
            //    //if (clone.slotsPlayer != slotsPlayer) HarvesterBase.Print("clone.slotsPlayer != slotsPlayer ");
            //    //if (clone.petIndex != petIndex) HarvesterBase.Print("clone.petIndex != petIndex");
            //    //if ((petIndex != -1 && clone.petIndex != -1 && Main.projectile[clone.petIndex].type != Main.projectile[petIndex].type)) HarvesterBase.Print("other thing");
            //    //HarvesterBase.Print("send SendClientChanges " + Main.netMode);
            //    // Send a Mod Packet with the changes.
            //    var packet = mod.GetPacket();
            //    packet.Write((byte)AssMessageType.SendClientChanges);
            //    packet.Write((byte)player.whoAmI);
            //    //packet.Write(petType);
            //    packet.Write(petIndex);
            //    packet.Write(slotsPlayer);
            //    packet.Send();
            //}
        }

        public void SendRedrawPetAccessories(int toClient = -1, int ignoreClient = -1)
        {
            ////HarvesterBase.Print("send SendRedrawPetAccessories " + Main.netMode);
            //var packet = mod.GetPacket();
            //packet.Write((byte)AssMessageType.RedrawPetAccessories);
            //packet.Write((byte)player.whoAmI);
            //packet.Write(petIndex);
            //packet.Write(slotsPlayer);
            //packet.Send(toClient, ignoreClient);
        }

        public void SendSlotData()
        {
            //if (Main.netMode == NetmodeID.MultiplayerClient)
            //{
            //    //Main.NewText("send from " + player.whoAmI);
            //    ModPacket packet = mod.GetPacket();
            //    //packet.Write((byte)AssMessageType.PetAccessorySlots);
            //    packet.Write((byte)player.whoAmI);
            //    packet.Write(petIndex);
            //    packet.Write(slotsPlayer);
            //    packet.Write(slotsPlayerLast);
            //    packet.Send();
            //}
        }

        public uint GetAccessoryPlayer(byte slotNumber)
        {
            slotNumber -= 1;
            return (slotsPlayer >> (slotNumber * 8)) & 255; //shift the selected 8 bits of the slot into the rightmost position
        }

        public void ApplyPetAccessories(int i, uint j)
        {
            if (i != -1 && Main.projectile[i].active)
            {
                PetAccessoryProj gProjectile = Main.projectile[i].GetGlobalProjectile<PetAccessoryProj>(mod);
                gProjectile.SetAccessoryAll(j);
            }
        }

        /*
         * 
         * 
         * 
         * public override void NetSend(BinaryWriter writer)
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
         */

        public override TagCompound Save()
        {
            BitsByte flags = new BitsByte();
            flags[0] = mechFrogCrown;
            return new TagCompound {
                {"slotsPlayer", (int)slotsPlayer},
                {"teleportHomeWhenLowTimer", (int)teleportHomeTimer},
                {"getDefenseTimer", (int)getDefenseTimer},
                {"eyePetType", (byte)eyePetType},
                {"byte1", (byte)flags},
            };
        }

        public override void Load(TagCompound tag)
        {
            slotsPlayer = (uint)tag.GetInt("slotsPlayer");
            teleportHomeTimer = (short)tag.GetInt("teleportHomeWhenLowTimer");
            getDefenseTimer = (short)tag.GetInt("getDefenseTimer");
            eyePetType = tag.GetByte("eyePetType");

            BitsByte flags = new BitsByte();
            flags = (BitsByte)tag.GetByte("byte1");
            mechFrogCrown = flags[0];
        }

        //public override void OnEnterWorld(Player player)
        //{
        //    joinDelaySend = 60;
        //}

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

        private void ResetEmpoweringTimer()
        {
            if (empoweringBuff)
            {
                for (int i = 0; i < empoweringTimer; i++)
                {
                    Dust dust = Dust.NewDustPerfect(player.Center, 135, new Vector2(Main.rand.NextFloat(-3f, 3f), Main.rand.NextFloat(-3f, 3f)) + (new Vector2(Main.rand.Next(-1, 1), Main.rand.Next(-1, 1)) * ((6 * empoweringTimer) / empoweringTimerMax)), 26, new Color(255, 255, 255), Main.rand.NextFloat(1.5f, 2.4f));
                    dust.noLight = true;
                    dust.noGravity = true;
                    dust.fadeIn = Main.rand.NextFloat(1f, 2.3f);
                }
                empoweringTimer = 0;
            }
        }

        private void SpawnSoul()
        {
            if (tempSoulMinion && player.whoAmI == Main.myPlayer)
            {
                bool checkIfAlive = false;
                for (int i = 0; i < 1000; i++)
                {
                    if (Main.projectile[i].active && Main.projectile[i].owner == player.whoAmI && Main.projectile[i].type == mod.ProjectileType<CompanionDungeonSoulMinion>())
                    {
                        if (Main.projectile[i].minionSlots == 0f)
                        {
                            checkIfAlive = true;
                            break;
                        }
                    }
                }
                
                if (!checkIfAlive)
                {
                    //twice the damage
                    int i = Projectile.NewProjectile(player.position.X + (player.width / 2), player.position.Y + (player.height / 2), player.direction * 0.5f, -0.5f,
                        mod.ProjectileType<CompanionDungeonSoulMinion>(),
                        CompanionDungeonSoulMinion.Damage * 2,
                        CompanionDungeonSoulMinion.Knockback,
                        player.whoAmI, 0f, 0f);
                    Main.projectile[i].minionSlots = 0f;
                    Main.projectile[i].timeLeft = 600; //10 seconds
                }
            }
        }

        private bool SoulBuffBlacklist(int type)
        {
            //returns true if type exists in the blacklist
            return Array.IndexOf(AssortedCrazyThings.soulBuffBlacklist, type) != -1;
        }

        private void SpawnSoulsWhenHarvesterIsAlive()
        {
            //ALWAYS GENERATE SOULS WHEN ONE IS ALIVE (otherwise he will never eat stuff when you aren't infront of dungeon walls
            if(Main.time % 30 == 4)
            {
                bool shouldDropSouls = false;
                int index = 200;
                for (short j = 0; j < 200; j++)
                {
                    if (Main.npc[j].active && Array.IndexOf(AssWorld.harvesterTypes, Main.npc[j].type) != -1)
                    {
                        shouldDropSouls = true;
                        index = j;
                        break;
                    }
                }

                if (shouldDropSouls)
                {
                    int distance = (int)(Main.npc[index].Center - player.Center).Length();
                    if (distance < 2880 || player.ZoneDungeon) //one and a half screens or in dungeon
                    {
                        for (short j = 0; j < 200; j++)
                        {
                            if (Main.npc[j].active && Main.npc[j].type != mod.NPCType<DungeonSoul>() && Array.IndexOf(AssWorld.harvesterTypes, Main.npc[j].type) == -1 && !SoulBuffBlacklist(Main.npc[j].type))
                            {
                                if (Main.npc[j].lifeMax > 5 && !Main.npc[j].friendly)
                                {
                                    Main.npc[j].AddBuff(mod.BuffType("SoulBuff"), 60, true);
                                }
                            }
                        }
                    }
                }
            }
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

        private void Empower()
        {
            if (empoweringBuff)
            {
                if (Main.time % 60 == 0)
                {
                    if (empoweringTimer < empoweringTimerMax)
                    {
                        empoweringTimer++;
                        step = (empoweringTimer * (empoweringTotal - 1f)) / empoweringTimerMax;
                    }
                }
                
                player.meleeDamage *= 1f + step;
                player.thrownDamage *= 1f + step;
                player.rangedDamage *= 1f + step;
                player.magicDamage *= 1f + step;
                player.minionDamage *= 1f + 0.25f * step;

                //adds crit from 0 to 5/7/10 (depends)
                player.meleeCrit += (int)(10 * step);
                player.thrownCrit += (int)(10 * step);
                player.rangedCrit += (int)(10 * step);
                player.magicCrit += (int)(10 * step);
            }
            else empoweringTimer = 0;
        }

        private void ApplyCandleDebuffs(NPC npc)
        {
            if (npc != null)
            {
                if (everburningCandleBuff)
                {
                    npc.AddBuff(BuffID.OnFire, 120);
                }
                if (everburningCursedCandleBuff)
                {
                    npc.AddBuff(BuffID.CursedInferno, 120);
                }
                if (everfrozenCandleBuff)
                {
                    npc.AddBuff(BuffID.Frostburn, 120);
                }
                //if (variable_debuff_04)
                //{
                //    npc.AddBuff(BuffID.Ichor, 120);
                //}
                //if (variable_debuff_05)
                //{
                //    npc.AddBuff(BuffID.Venom, 120);
                //}
                if (everburningShadowflameCandleBuff)
                {
                    npc.AddBuff(BuffID.ShadowFlame, 60);
                }
                //if (variable_debuff_07)
                //{
                //    npc.AddBuff(BuffID.Bleeding, 120);
                //}
            }
        }

        public override void OnHitByNPC(NPC npc, int damage, bool crit)
        {
            ResetEmpoweringTimer();

            SpawnSoul();
        }

        public override void OnHitByProjectile(Projectile proj, int damage, bool crit)
        {
            ResetEmpoweringTimer();
        }

        public override void OnHitAnything(float x, float y, Entity victim)
        {
            ApplyCandleDebuffs((NPC)victim);
        }

        public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
        {
            //getDefense before teleportHome (so you dont teleport BEFORE you gain the defense)
            if (getDefense)
            {
                if (canGetDefense)
                {
                    player.statLife += (int)damage;
                    player.AddBuff(BuffID.RapidHealing, 600);
                    CombatText.NewText(new Rectangle((int)player.position.X, (int)player.position.Y, player.width, player.height), CombatText.HealLife, "Defense increased");

                    getDefenseTimer = GetDefenseTimerMax;
                    getDefenseDuration = GetDefenseDurationMax;
                    return false;
                }
            }

            if (teleportHome)
            {
                if (canTeleportHome && player.whoAmI == Main.myPlayer)
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

                    player.AddBuff(BuffID.RapidHealing, 300, false);

                    if (Main.netMode == NetmodeID.Server)
                    {
                        NetMessage.SendData(MessageID.PlayerControls, -1, -1, null, player.whoAmI);
                    }

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

        public override void PostUpdateBuffs()
        {
            UpdateTeleportHomeWhenLow();

            UpdateGetDefenseWhenLow();

            Empower();
        }

        public override void PreUpdate()
        {
            SpawnSoulsWhenHarvesterIsAlive();

            //if (joinDelaySend > 0)
            //{
            //    joinDelaySend--;
            //    if (joinDelaySend == 0 && petIndex != -1 && Main.netMode == NetmodeID.MultiplayerClient) SendRedrawPetAccessories();
            //}

            //if (Main.netMode == NetmodeID.Server)
            //{
            //    if (counter == 0)
            //    {
            //        //Console.WriteLine(player.name + " slots " + slotsPlayer);
            //        counter = 240;
            //    }
            //    counter--;
            //}

            //if (Main.netMode == NetmodeID.MultiplayerClient)
            //{
            //    if (clientcounter == 0)
            //    {
            //        for (int players = 0; players < Main.player.Length; players++)
            //        {
            //            if (Main.player[players].active)
            //            {
            //                if (Main.LocalPlayer.whoAmI == player.whoAmI)
            //                {
            //                    //Main.NewText("SELF:" + " slots " + slotsPlayer);
            //                }
            //                else
            //                {
            //                    //Main.NewText("OTHE:" +" slots " + slotsPlayer);
            //                }
            //            }
            //        }

            //        clientcounter = 240;
            //    }
            //    clientcounter--;
            //}
        }
    }
}
