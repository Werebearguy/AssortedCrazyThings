using AssortedCrazyThings.Items.PetAccessories;
using AssortedCrazyThings.Projectiles.Minions;
using AssortedCrazyThings.Projectiles.Pets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace AssortedCrazyThings
{
    class AssortedCrazyThings : Mod
    {
        public AssortedCrazyThings()
        {
            Properties = new ModProperties()
            {
                Autoload = true,
                AutoloadGores = true,
                AutoloadSounds = true
            };
        }

        //Slime pet legacy
        public static int[] slimePetLegacy = new int[9];
        public static int[] slimePetNoHair = new int[6];

        //Soul item animated textures
        public static Texture2D[] animatedSoulTextures;

        //Soul NPC spawn blacklist
        public static int[] soulBuffBlacklist;

        private void InitPets()
        {
            int index = 0;
            slimePetLegacy[index++] = ProjectileType<CuteSlimeBlackPet>();
            slimePetLegacy[index++] = ProjectileType<CuteSlimeBluePet>();
            slimePetLegacy[index++] = ProjectileType<CuteSlimeGreenPet>();
            slimePetLegacy[index++] = ProjectileType<CuteSlimePinkPet>();
            slimePetLegacy[index++] = ProjectileType<CuteSlimePurplePet>();
            slimePetLegacy[index++] = ProjectileType<CuteSlimeRainbowPet>();
            slimePetLegacy[index++] = ProjectileType<CuteSlimeRedPet>();
            slimePetLegacy[index++] = ProjectileType<CuteSlimeXmasPet>();
            slimePetLegacy[index++] = ProjectileType<CuteSlimeYellowPet>();

            index = 0;

            slimePetNoHair[index++] = ProjectileType<CuteSlimeBlackNewPet>();
            slimePetNoHair[index++] = ProjectileType<CuteSlimeBlueNewPet>();
            slimePetNoHair[index++] = ProjectileType<CuteSlimePurpleNewPet>();
            slimePetNoHair[index++] = ProjectileType<CuteSlimePinkNewPet>();
            slimePetNoHair[index++] = ProjectileType<CuteSlimeRedNewPet>();
            slimePetNoHair[index++] = ProjectileType<CuteSlimeYellowNewPet>();

            if (!Main.dedServ && Main.netMode != 2)
            {
                PetAccessory.Load(this);
            }
        }

        private void InitSoulBuffBlacklist()
        {
            soulBuffBlacklist = new int[40];
            int index = 0;
            soulBuffBlacklist[index++] = NPCID.GiantWormBody;
            soulBuffBlacklist[index++] = NPCID.GiantWormTail;
            soulBuffBlacklist[index++] = NPCID.DiggerBody;
            soulBuffBlacklist[index++] = NPCID.DiggerTail;
            soulBuffBlacklist[index++] = NPCID.DevourerBody;
            soulBuffBlacklist[index++] = NPCID.DevourerTail;
            soulBuffBlacklist[index++] = NPCID.EaterofWorldsBody;
            soulBuffBlacklist[index++] = NPCID.EaterofWorldsTail;
            soulBuffBlacklist[index++] = NPCID.SeekerBody;
            soulBuffBlacklist[index++] = NPCID.SeekerTail;
            soulBuffBlacklist[index++] = NPCID.TombCrawlerBody;
            soulBuffBlacklist[index++] = NPCID.TombCrawlerTail;
            soulBuffBlacklist[index++] = NPCID.LeechBody;
            soulBuffBlacklist[index++] = NPCID.LeechTail;
            soulBuffBlacklist[index++] = NPCID.BoneSerpentBody;
            soulBuffBlacklist[index++] = NPCID.BoneSerpentTail;
            soulBuffBlacklist[index++] = NPCID.DuneSplicerBody;
            soulBuffBlacklist[index++] = NPCID.DuneSplicerTail;
            soulBuffBlacklist[index++] = NPCID.SpikeBall;
            soulBuffBlacklist[index++] = NPCID.BlazingWheel;

            soulBuffBlacklist[index++] = NPCID.BlueSlime;
            soulBuffBlacklist[index++] = NPCID.SlimeSpiked;
            //immune to all debuffs anyway
            //soulBuffBlacklist[index++] = NPCID.TheDestroyerBody;
            //soulBuffBlacklist[index++] = NPCID.TheDestroyerTail;
            //soulBuffBlacklist[index++] = NPCID.CultistDragonBody1;
            //soulBuffBlacklist[index++] = NPCID.CultistDragonBody2;
            //soulBuffBlacklist[index++] = NPCID.CultistDragonBody3;
            //soulBuffBlacklist[index++] = NPCID.CultistDragonBody4;
            //soulBuffBlacklist[index++] = NPCID.CultistDragonTail;

            Array.Resize(ref soulBuffBlacklist, index + 1);
        }

        private void AddToSoulBuffBlacklist()
        {
            int index = soulBuffBlacklist.Length - 1; //last index

            Array.Resize(ref soulBuffBlacklist, index + 40); //buffer


            Mod pinkymod = ModLoader.GetMod("pinkymod");
            if (pinkymod != null)
            {
                soulBuffBlacklist[index++] = pinkymod.NPCType("BoneLeechBody");
                soulBuffBlacklist[index++] = pinkymod.NPCType("BoneLeechTail");
            }

            Array.Resize(ref soulBuffBlacklist, index + 1);
        }

        public override void Load()
        {
            InitPets();

            InitSoulBuffBlacklist();

            if (!Main.dedServ && Main.netMode != 2)
            {
                animatedSoulTextures = new Texture2D[2];

                animatedSoulTextures[0] = GetTexture("Items/CaughtDungeonSoulAnimated");
                animatedSoulTextures[1] = GetTexture("Items/CaughtDungeonSoulFreedAnimated");
            }
        }

        public override void Unload()
        {
            if (!Main.dedServ && Main.netMode != 2)
            {
                PetAccessory.Unload();
                animatedSoulTextures = null;
            }
        }

        public override void PostSetupContent()
        {
            AddToSoulBuffBlacklist();

            //https://forums.terraria.org/index.php?threads/boss-checklist-in-game-progression-checklist.50668/
            Mod bossChecklist = ModLoader.GetMod("BossChecklist");
            if (bossChecklist != null)
            {
                //5.1f means just after skeletron
                bossChecklist.Call("AddMiniBossWithInfo", NPCs.DungeonBird.Harvester.name, 5.1f, (Func<bool>)(() => AssWorld.downedHarvester), "Use a [i:" + ItemType<Items.IdolOfDecay>() + "] in the dungeon after Skeletron has been defeated");
            }
        }

        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            AssMessageType msgType = (AssMessageType)reader.ReadByte();
            short knapSackSlimeIndex;
            int arrayLength;
            byte knapSackSlimeTexture;
            byte playernumber;
            AssPlayer mPlayer;

            switch (msgType)
            {
                case AssMessageType.SyncKnapSackSlimeTexture:
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                    {
                        knapSackSlimeIndex = reader.ReadInt16();
                        knapSackSlimeTexture = reader.ReadByte();
                        if (Main.projectile[knapSackSlimeIndex].type == ProjectileType<SlimePackMinion>())
                        {
                            Main.projectile[knapSackSlimeIndex].localAI[1] = knapSackSlimeTexture;
                        }
                    }
                    break;

                case AssMessageType.OnEnterWorld:
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                    {
                        playernumber = reader.ReadByte();
                        mPlayer = Main.player[playernumber].GetModPlayer<AssPlayer>();
                        mPlayer.mechFrogCrown = reader.ReadBoolean();

                        arrayLength = reader.ReadInt16();
                        //Main.NewText(arrayLength);
                        short[] indexes = new short[arrayLength];
                        byte[] textures = new byte[arrayLength];

                        for (int i = 0; i < arrayLength; i++)
                        {
                            indexes[i] = reader.ReadInt16();
                            textures[i] = reader.ReadByte();
                        }
                        for (int i = 0; i < arrayLength; i++)
                        {
                            //Main.NewText("recv SyncKnapSackSlimeTextureOnEnterWorld with " + indexes[i] + " " + textures[i]);
                            if (Main.projectile[indexes[i]].type == ProjectileType<SlimePackMinion>())
                            {
                                Main.projectile[indexes[i]].localAI[1] = textures[i];
                            }
                        }
                    }
                    break;
                case AssMessageType.SendClientChanges:
                    playernumber = reader.ReadByte();

                    if (Main.netMode == NetmodeID.MultiplayerClient)
                    {
                        Main.NewText("recv sendclientchanges from " + playernumber);
                    }
                    mPlayer = Main.player[playernumber].GetModPlayer<AssPlayer>();
                    mPlayer.mechFrogCrown = reader.ReadBoolean();
                    // Unlike SyncPlayer, here we have to relay/forward these changes to all other connected clients
                    if (Main.netMode == NetmodeID.Server)
                    {
                        NetMessage.BroadcastChatMessage(NetworkText.FromLiteral("server send SendClientChanges from " + playernumber), new Color(255, 25, 25));
                        ModPacket packet = GetPacket();
                        packet.Write((byte)AssMessageType.SendClientChanges);
                        packet.Write(playernumber);
                        packet.Write(mPlayer.mechFrogCrown);
                        packet.Send(-1, playernumber);
                    }
                    break;
                default:
                    ErrorLogger.Log("AssortedCrazyThings: Unknown Message type: " + msgType);
                    break;
            }
            if (false)
            {
                /*
                AssMessageType msgType = (AssMessageType)reader.ReadByte();
                byte playernumber;
                //Player tempPlayer;
                AssPlayer mPlayer;
                int petType;
                int petIndex = -1;
                uint slotsPlayer = 0;
                //uint slotsPlayerLast;
                switch (msgType)
                {
                    //case AssMessageType.PetAccessorySlots:
                    //    playernumber = reader.ReadByte();
                    //    petIndex = reader.ReadInt32();
                    //    petTypeChanged = reader.ReadBoolean();
                    //    slotsPlayer = (uint)reader.ReadInt32();
                    //    slotsPlayerLast = (uint)reader.ReadInt32();

                    //    tempPlayer = Main.player[playernumber];
                    //    mPlayer = tempPlayer.GetModPlayer<AssPlayer>();
                    //    mPlayer.petIndex = petIndex;
                    //    mPlayer.slotsPlayer = slotsPlayer;
                    //    mPlayer.slotsPlayerLast = slotsPlayerLast;
                    //    if (Main.netMode == NetmodeID.Server)
                    //    {
                    //        Console.WriteLine("recieved a PetAccessorySlots packet from " + tempPlayer.name);
                    //    }
                    //    if (Main.netMode == NetmodeID.MultiplayerClient)
                    //    {
                    //        try
                    //        {
                    //            mPlayer.ApplyPetAccessories(petIndex, slotsPlayer);
                    //        }
                    //        catch (Exception e)
                    //        {
                    //            ErrorLogger.Log(e);
                    //        }
                    //        Main.NewText("recieved a PetAccessorySlots packet " + tempPlayer.name);

                    //    }
                    //    if (Main.netMode == NetmodeID.Server)
                    //    {
                    //        Console.WriteLine("send ignoring " + tempPlayer.name);
                    //        ModPacket packet = GetPacket();
                    //        packet.Write((byte)AssMessageType.PetAccessorySlots);
                    //        packet.Write(playernumber);
                    //        packet.Write(petIndex);
                    //        packet.Write(slotsPlayer);
                    //        packet.Write(slotsPlayerLast);
                    //        packet.Send(-1, playernumber);
                    //    }

                    //    break;

                    case AssMessageType.RedrawPetAccessories:
                        //HarvesterBase.Print("try recv RedrawPetAccessories");
                        try
                        {
                            playernumber = reader.ReadByte();
                            mPlayer = Main.player[playernumber].GetModPlayer<AssPlayer>();

                            petIndex = reader.ReadInt32();
                            slotsPlayer = (uint)reader.ReadInt32();

                            mPlayer.ApplyPetAccessories(petIndex, slotsPlayer);


                            if (Main.netMode == NetmodeID.Server)
                            {
                                mPlayer.SendRedrawPetAccessories(-1, playernumber);
                            }
                            //HarvesterBase.Print("recv RedrawPetAccessories from " + playernumber);
                        }
                        catch (Exception e)
                        {
                            ErrorLogger.Log(petIndex);
                            ErrorLogger.Log(slotsPlayer);
                            ErrorLogger.Log(e);
                        }
                        break;

                    case AssMessageType.SyncPlayer:
                        try
                        {
                            playernumber = reader.ReadByte();
                            mPlayer = Main.player[playernumber].GetModPlayer<AssPlayer>();

                            slotsPlayer = (uint)reader.ReadInt32();

                            mPlayer.slotsPlayer = slotsPlayer;
                            //HarvesterBase.Print("recv SyncPlayer from " + playernumber);
                            // SyncPlayer will be called automatically, so there is no need to forward this data to other clients.
                        }
                        catch (Exception e)
                        {
                            ErrorLogger.Log(e);
                        }
                        break;

                    case AssMessageType.SendClientChanges:
                        try
                        {
                            playernumber = reader.ReadByte();
                            mPlayer = Main.player[playernumber].GetModPlayer<AssPlayer>();

                            //petType = reader.ReadInt32();
                            petIndex = reader.ReadInt32();
                            slotsPlayer = (uint)reader.ReadInt32();

                            if (petIndex != -1 && mPlayer.petIndex != -1 && Main.projectile[mPlayer.petIndex].type != Main.projectile[petIndex].type)
                            {
                                Main.NewText("redraw");
                                mPlayer.ApplyPetAccessories(petIndex, slotsPlayer);
                            }

                            //mPlayer.petType = petType;
                            mPlayer.petIndex = petIndex;
                            //mPlayer.slotsPlayer = slotsPlayer;
                            // Unlike SyncPlayer, here we have to relay/forward these changes to all other connected clients
                            if (Main.netMode == NetmodeID.Server)
                            {
                                ModPacket packet = GetPacket();
                                packet.Write((byte)AssMessageType.SendClientChanges);
                                packet.Write(playernumber);
                                //packet.Write(mPlayer.petType);
                                packet.Write(petIndex);
                                packet.Write(slotsPlayer);
                                packet.Send(-1, playernumber);
                            }
                            if (Main.netMode == NetmodeID.MultiplayerClient)
                            {
                                try
                                {
                                    //mPlayer.ApplyPetAccessories(petIndex, slotsPlayer);
                                }
                                catch (Exception e)
                                {
                                    ErrorLogger.Log(e);
                                }
                            }
                            //HarvesterBase.Print("recv SendClientChanges from " + playernumber);
                        }
                        catch (Exception e)
                        {
                            ErrorLogger.Log(e);
                        }
                        break;
                    default:
                        ErrorLogger.Log("AssortedCrazyThings: Unknown Message type: " + msgType);
                        break;
                }
                */
            }
        }
    }

    enum AssMessageType : byte
    {
        RedrawPetAccessories,
        SendClientChanges,
        SyncPlayer,
        SyncKnapSackSlimeTexture,
        OnEnterWorld
    }
}
