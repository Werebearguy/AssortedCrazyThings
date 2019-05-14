using AssortedCrazyThings.Base;
using AssortedCrazyThings.Items;
using AssortedCrazyThings.Items.PetAccessories;
using AssortedCrazyThings.Projectiles.Pets;
using AssortedCrazyThings.UI;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace AssortedCrazyThings
{
    public class PetPlayer : ModPlayer
    {
        private bool petAccessoryRework = false;

        //docile demon eye texture
        public byte petEyeType = 0; //texture type, not ID

        //mech frog texture
        public byte mechFrogCrown = 0;

        //cursed skull texture
        public byte cursedSkullType = 0;

        //young wyvern texture
        public byte youngWyvernType = 0;

        //young wyvern texture
        public byte petFishronType = 0;

        //moon pet texture
        public byte petMoonType = 0;

        //young harpy texture
        public byte youngHarpyType = 0;

        //abeeminiation texture
        public byte abeeminationType = 0;

        //lil wraps texture
        public byte lilWrapsType = 0;

        //vampire bat texture
        public byte vampireBatType = 0;

        //vampire bat texture
        public byte pigronataType = 0;

        //queen larva texture
        public byte queenLarvaType = 0;

        //ocean slime texture
        public byte oceanSlimeType = 0;

        //queen larva texture
        public byte miniAntlionType = 0;

        //pet goldfish texture
        public byte petGoldfishType = 0;

        //skeletron hand texture
        public byte skeletronHandType = 0;

        //skeletron prime hand texture
        public byte skeletronPrimeHandType = 0;

        //pet cultist texture
        public byte petCultistType = 0;

        //ALTERNATE
        ////name pet texture
        //public byte classNameType = 0;

        public bool DrumstickElemental = false;
        public bool MiniAntlion = false;
        public bool LilWraps = false;
        public bool PetFishron = false;
        public bool RainbowSlime = false;
        public bool PrinceSlime = false;
        public bool IlluminantSlime = false;
        public bool ChunkySlime = false;
        public bool FairySlime = false;
        public bool HornedSlime = false;
        public bool JoyousSlime = false;
        public bool MeatballSlime = false;
        public bool OceanSlime = false;
        public bool StingSlimeBlack = false;
        public bool StingSlimeOrange = false;
        public bool TurtleSlime = false;
        public bool Pigronata = false;
        public bool Abeemination = false;
        public bool CuteSlimeYellowNew = false;
        public bool CuteSlimeXmasNew = false;
        public bool CuteSlimeToxicNew = false;
        public bool CuteSlimeSandNew = false;
        public bool CuteSlimeRedNew = false;
        public bool CuteSlimeRainbowNew = false;
        public bool CuteSlimePurpleNew = false;
        public bool CuteSlimePrincessNew = false;
        public bool CuteSlimePinkNew = false;
        public bool CuteSlimeLavaNew = false;
        public bool CuteSlimeJungleNew = false;
        public bool CuteSlimeIlluminantNew = false;
        public bool CuteSlimeIceNew = false;
        public bool CuteSlimeGreenNew = false;
        public bool CuteSlimeDungeonNew = false;
        public bool CuteSlimeCrimsonNew = false;
        public bool CuteSlimeCorruptNew = false;
        public bool CuteSlimeBlueNew = false;
        public bool CuteSlimeBlackNew = false;
        public bool OrigamiCrane = false;
        public bool MiniMegalodon = false;
        public bool SmallMegalodon = false;
        public bool CuteSlimeXmas = false;
        public bool YoungHarpy = false;
        public bool CuteGastropod = false;
        public bool YoungWyvern = false;
        public bool BabyIchorSticker = false;
        public bool LifelikeMechanicalFrog = false;
        public bool CuteSlimeBlue = false;
        public bool CuteSlimeGreen = false;
        public bool CuteSlimePink = false;
        public bool CuteSlimeBlack = false;
        public bool CuteSlimePurple = false;
        public bool CuteSlimeRed = false;
        public bool CuteSlimeYellow = false;
        public bool CuteSlimeRainbow = false;
        public bool ChunkyandMeatball = false;
        public bool DemonHeart = false;
        public bool BrainofConfusion = false;
        public bool AlienHornet = false;
        public bool DetachedHungry = false;
        public bool BabyOcram = false;
        public bool CursedSkull = false;
        public bool BabyCrimera = false;
        public bool VampireBat = false;
        public bool TorturedSoul = false;
        public bool EnchantedSword = false;
        public bool Goblet = false;
        public bool SoulLightPet = false;
        public bool SoulLightPet2 = false;
        public bool DocileDemonEye = false;
        public bool QueenLarva = false;
        public bool HealingDrone = false;
        public bool PetSun = false;
        public bool PetMoon = false;
        public bool WallFragment = false;
        public bool TinyTwins = false;
        public bool ObservingEye = false;
        public bool PetGoldfish = false;
        public bool SkeletronHand = false;
        public bool SkeletronPrimeHand = false;
        public bool PetGolemHead = false;
        public bool TrueObservingEye = false;
        public bool PetCultist = false;
        public bool PetPlantera = false;
        public bool PetEaterofWorlds = false;
        public bool PetDestroyer = false;
        //ALTERNATE
        //public bool ClassName = false;

        public override void ResetEffects()
        {
            DrumstickElemental = false;
            MiniAntlion = false;
            LilWraps = false;
            PetFishron = false;
            RainbowSlime = false;
            PrinceSlime = false;
            IlluminantSlime = false;
            ChunkySlime = false;
            FairySlime = false;
            HornedSlime = false;
            JoyousSlime = false;
            MeatballSlime = false;
            OceanSlime = false;
            StingSlimeBlack = false;
            StingSlimeOrange = false;
            TurtleSlime = false;
            Pigronata = false;
            Abeemination = false;
            CuteSlimeYellowNew = false;
            CuteSlimeXmasNew = false;
            CuteSlimeToxicNew = false;
            CuteSlimeSandNew = false;
            CuteSlimeRedNew = false;
            CuteSlimeRainbowNew = false;
            CuteSlimePrincessNew = false;
            CuteSlimePurpleNew = false;
            CuteSlimePinkNew = false;
            CuteSlimeLavaNew = false;
            CuteSlimeJungleNew = false;
            CuteSlimeIceNew = false;
            CuteSlimeIlluminantNew = false;
            CuteSlimeGreenNew = false;
            CuteSlimeBlueNew = false;
            CuteSlimeBlackNew = false;
            CuteSlimeCrimsonNew = false;
            CuteSlimeCorruptNew = false;
            CuteSlimeDungeonNew = false;
            OrigamiCrane = false;
            MiniMegalodon = false;
            SmallMegalodon = false;
            CuteSlimeXmas = false;
            YoungHarpy = false;
            CuteGastropod = false;
            YoungWyvern = false;
            BabyIchorSticker = false;
            LifelikeMechanicalFrog = false;
            CuteSlimeBlue = false;
            CuteSlimeGreen = false;
            CuteSlimePink = false;
            CuteSlimeBlack = false;
            CuteSlimePurple = false;
            CuteSlimeRed = false;
            CuteSlimeYellow = false;
            CuteSlimeRainbow = false;
            ChunkyandMeatball = false;
            DemonHeart = false;
            BrainofConfusion = false;
            AlienHornet = false;
            DetachedHungry = false;
            BabyOcram = false;
            CursedSkull = false;
            BabyCrimera = false;
            VampireBat = false;
            TorturedSoul = false;
            EnchantedSword = false;
            Goblet = false;
            SoulLightPet = false;
            SoulLightPet2 = false;
            DocileDemonEye = false;
            QueenLarva = false;
            HealingDrone = false;
            PetSun = false;
            PetMoon = false;
            WallFragment = false;
            TinyTwins = false;
            ObservingEye = false;
            PetGoldfish = false;
            SkeletronHand = false;
            SkeletronPrimeHand = false;
            PetGolemHead = false;
            TrueObservingEye = false;
            PetCultist = false;
            PetPlantera = false;
            PetEaterofWorlds = false;
            PetDestroyer = false;
            //ALTERNATE
            //ClassName = false;
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

        public override TagCompound Save()
        {
            return new TagCompound {
                {"slots", (int)slots},
                {"color", (int)color},
                {"petAccessoryRework", (bool)petAccessoryRework},
                {"mechFrogCrown", (byte)mechFrogCrown},
                {"petEyeType", (byte)petEyeType},
                {"cursedSkullType", (byte)cursedSkullType},
                {"youngWyvernType", (byte)youngWyvernType},
                {"petFishronType", (byte)petFishronType},
                {"petMoonType", (byte)petMoonType},
                {"youngHarpyType", (byte)youngHarpyType},
                {"abeeminationType", (byte)abeeminationType},
                {"lilWrapsType", (byte)lilWrapsType},
                {"vampireBatType", (byte)vampireBatType},
                {"pigronataType", (byte)pigronataType},
                {"queenLarvaType", (byte)queenLarvaType},
                {"oceanSlimeType", (byte)oceanSlimeType},
                {"miniAntlionType", (byte)miniAntlionType},
                {"petGoldfishType", (byte)petGoldfishType},
                {"skeletronHandType", (byte)skeletronHandType},
                {"skeletronPrimeHandType", (byte)skeletronPrimeHandType},
                {"petCultistType", (byte)petCultistType},
                //ALTERNATE
                //{"classNameType", (byte)classNameType},
            };
        }

        public override void Load(TagCompound tag)
        {
            slots = (uint)tag.GetInt("slots");
            color = (uint)tag.GetInt("color");
            petAccessoryRework = tag.GetBool("petAccessoryRework");
            mechFrogCrown = tag.GetByte("mechFrogCrown");
            petEyeType = tag.GetByte("petEyeType");
            cursedSkullType = tag.GetByte("cursedSkullType");
            youngWyvernType = tag.GetByte("youngWyvernType");
            petFishronType = tag.GetByte("petFishronType");
            petMoonType = tag.GetByte("petMoonType");
            youngHarpyType = tag.GetByte("youngHarpyType");
            abeeminationType = tag.GetByte("abeeminationType");
            lilWrapsType = tag.GetByte("lilWrapsType");
            vampireBatType = tag.GetByte("vampireBatType");
            pigronataType = tag.GetByte("pigronataType");
            queenLarvaType = tag.GetByte("queenLarvaType");
            oceanSlimeType = tag.GetByte("oceanSlimeType");
            miniAntlionType = tag.GetByte("miniAntlionType");
            petGoldfishType = tag.GetByte("petGoldfishType");
            skeletronHandType = tag.GetByte("skeletronHandType");
            skeletronPrimeHandType = tag.GetByte("skeletronPrimeHandType");
            petCultistType = tag.GetByte("petCultistType");
            //ALTERNATE
            //classNameType = tag.GetByte("classNameType");
        }

        public override void clientClone(ModPlayer clientClone)
        {
            PetPlayer clone = clientClone as PetPlayer;
            clone.slots = slots;
            clone.color = color;
            clone.mechFrogCrown = mechFrogCrown;
            clone.petEyeType = petEyeType;
            clone.cursedSkullType = cursedSkullType;
            clone.youngWyvernType = youngWyvernType;
            clone.petFishronType = petFishronType;
            clone.petMoonType = petMoonType;
            clone.youngHarpyType = youngHarpyType;
            clone.abeeminationType = abeeminationType;
            clone.lilWrapsType = lilWrapsType;
            clone.vampireBatType = vampireBatType;
            clone.pigronataType = pigronataType;
            clone.queenLarvaType = queenLarvaType;
            clone.oceanSlimeType = oceanSlimeType;
            clone.miniAntlionType = miniAntlionType;
            clone.petGoldfishType = petGoldfishType;
            clone.skeletronHandType = skeletronHandType;
            clone.skeletronPrimeHandType = skeletronPrimeHandType;
            clone.petCultistType = petCultistType;
            //ALTERNATE
            //clone.classNameType = classNameType;
        }

        public override void SendClientChanges(ModPlayer clientPlayer)
        {
            PetPlayer clone = clientPlayer as PetPlayer;
            PetPlayerChanges changes = PetPlayerChanges.none;
            if (clone.slots != slots || clone.color != color) changes = PetPlayerChanges.slots;
            else if (clone.mechFrogCrown != mechFrogCrown) changes = PetPlayerChanges.mechFrogCrown;
            else if (clone.petEyeType != petEyeType) changes = PetPlayerChanges.petEyeType;
            else if (clone.cursedSkullType != cursedSkullType) changes = PetPlayerChanges.cursedSkullType;
            else if (clone.youngWyvernType != youngWyvernType) changes = PetPlayerChanges.youngWyvernType;
            else if (clone.petFishronType != petFishronType) changes = PetPlayerChanges.petFishronType;
            else if (clone.petMoonType != petMoonType) changes = PetPlayerChanges.petMoonType;
            else if (clone.youngHarpyType != youngHarpyType) changes = PetPlayerChanges.youngHarpyType;
            else if (clone.abeeminationType != abeeminationType) changes = PetPlayerChanges.abeeminationType;
            else if (clone.lilWrapsType != lilWrapsType) changes = PetPlayerChanges.lilWrapsType;
            else if (clone.vampireBatType != vampireBatType) changes = PetPlayerChanges.vampireBatType;
            else if (clone.pigronataType != pigronataType) changes = PetPlayerChanges.pigronataType;
            else if (clone.queenLarvaType != queenLarvaType) changes = PetPlayerChanges.queenLarvaType;
            else if (clone.oceanSlimeType != oceanSlimeType) changes = PetPlayerChanges.oceanSlimeType;
            else if (clone.miniAntlionType != miniAntlionType) changes = PetPlayerChanges.miniAntlionType;
            else if (clone.petGoldfishType != petGoldfishType) changes = PetPlayerChanges.petGoldfishType;
            else if (clone.skeletronHandType != skeletronHandType) changes = PetPlayerChanges.skeletronHandType;
            else if (clone.skeletronPrimeHandType != skeletronPrimeHandType) changes = PetPlayerChanges.skeletronPrimeHandType;
            else if (clone.petCultistType != petCultistType) changes = PetPlayerChanges.petCultistType;
            //ALTERNATE
            //else if (clone.classNameType != classNameType) changes = PetPlayerChanges.classNameType;

            //if (changes != PetPlayerChanges.none) Main.NewText("clientchanges with " + changes.ToString());

            if (changes != PetPlayerChanges.none) SendClientChangesPacket(changes);
        }

        public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
        {
            //from server to clients
            ModPacket packet = mod.GetPacket();
            packet.Write((byte)AssMessageType.SyncPlayerVanity);
            packet.Write((byte)player.whoAmI);
            //no "changes" packet
            SendFieldValues(packet);
            packet.Send(toWho, fromWho);
        }

        private void SendFieldValues(ModPacket packet)
        {
            packet.Write((uint)slots);
            packet.Write((uint)color);
            packet.Write((byte)mechFrogCrown);
            packet.Write((byte)petEyeType);
            packet.Write((byte)cursedSkullType);
            packet.Write((byte)youngWyvernType);
            packet.Write((byte)petFishronType);
            packet.Write((byte)petMoonType);
            packet.Write((byte)youngHarpyType);
            packet.Write((byte)abeeminationType);
            packet.Write((byte)lilWrapsType);
            packet.Write((byte)vampireBatType);
            packet.Write((byte)pigronataType);
            packet.Write((byte)queenLarvaType);
            packet.Write((byte)oceanSlimeType);
            packet.Write((byte)miniAntlionType);
            packet.Write((byte)petGoldfishType);
            packet.Write((byte)skeletronHandType);
            packet.Write((byte)skeletronPrimeHandType);
            packet.Write((byte)petCultistType);
            //ALTERNATE
            //packet.Write((byte)classNameType);
        }

        public void RecvSyncPlayerVanitySub(BinaryReader reader)
        {
            slots = reader.ReadUInt32();
            color = reader.ReadUInt32();
            mechFrogCrown = reader.ReadByte();
            petEyeType = reader.ReadByte();
            cursedSkullType = reader.ReadByte();
            youngWyvernType = reader.ReadByte();
            petFishronType = reader.ReadByte();
            petMoonType = reader.ReadByte();
            youngHarpyType = reader.ReadByte();
            abeeminationType = reader.ReadByte();
            lilWrapsType = reader.ReadByte();
            vampireBatType = reader.ReadByte();
            pigronataType = reader.ReadByte();
            queenLarvaType = reader.ReadByte();
            oceanSlimeType = reader.ReadByte();
            miniAntlionType = reader.ReadByte();
            petGoldfishType = reader.ReadByte();
            skeletronHandType = reader.ReadByte();
            skeletronPrimeHandType = reader.ReadByte();
            petCultistType = reader.ReadByte();
            //ALTERNATE
            //classNameType = reader.ReadByte();
        }

        public void RecvClientChangesPacketSub(BinaryReader reader, byte changes)
        {
            //AssUtils.Print("RecvClientChangesPacketSub " + changes + " from p " + player.whoAmI);
            switch (changes)
            {
                case (byte)PetPlayerChanges.all:
                    RecvSyncPlayerVanitySub(reader);
                    break;
                case (byte)PetPlayerChanges.slots:
                    slots = reader.ReadUInt32();
                    color = reader.ReadUInt32();
                    break;
                case (byte)PetPlayerChanges.mechFrogCrown:
                    mechFrogCrown = reader.ReadByte();
                    break;
                case (byte)PetPlayerChanges.petEyeType:
                    petEyeType = reader.ReadByte();
                    break;
                case (byte)PetPlayerChanges.cursedSkullType:
                    cursedSkullType = reader.ReadByte();
                    break;
                case (byte)PetPlayerChanges.youngWyvernType:
                    youngWyvernType = reader.ReadByte();
                    break;
                case (byte)PetPlayerChanges.petFishronType:
                    petFishronType = reader.ReadByte();
                    break;
                case (byte)PetPlayerChanges.petMoonType:
                    petMoonType = reader.ReadByte();
                    break;
                case (byte)PetPlayerChanges.youngHarpyType:
                    youngHarpyType = reader.ReadByte();
                    break;
                case (byte)PetPlayerChanges.abeeminationType:
                    abeeminationType = reader.ReadByte();
                    break;
                case (byte)PetPlayerChanges.lilWrapsType:
                    lilWrapsType = reader.ReadByte();
                    break;
                case (byte)PetPlayerChanges.vampireBatType:
                    vampireBatType = reader.ReadByte();
                    break;
                case (byte)PetPlayerChanges.pigronataType:
                    pigronataType = reader.ReadByte();
                    break;
                case (byte)PetPlayerChanges.queenLarvaType:
                    queenLarvaType = reader.ReadByte();
                    break;
                case (byte)PetPlayerChanges.oceanSlimeType:
                    oceanSlimeType = reader.ReadByte();
                    break;
                case (byte)PetPlayerChanges.miniAntlionType:
                    miniAntlionType = reader.ReadByte();
                    break;
                case (byte)PetPlayerChanges.petGoldfishType:
                    petGoldfishType = reader.ReadByte();
                    break;
                case (byte)PetPlayerChanges.skeletronHandType:
                    skeletronHandType = reader.ReadByte();
                    break;
                case (byte)PetPlayerChanges.skeletronPrimeHandType:
                    skeletronPrimeHandType = reader.ReadByte();
                    break;
                case (byte)PetPlayerChanges.petCultistType:
                    petCultistType = reader.ReadByte();
                    break;
                //ALTERNATE
                //case (byte)PetPlayerChanges.classNameType:
                //    classNameType = reader.ReadByte();
                //    break;
                default: //shouldn't get there hopefully
                    ErrorLogger.Log("Recieved unspecified PetPlayerChanges Packet " + changes);
                    break;
            }
        }

        public void SendClientChangesPacketSub(byte changes, int toClient = -1, int ignoreClient = -1)
        {
            //AssUtils.Print("SendClientChangesPacketSub " + changes + " from p " + player.whoAmI + ((Main.netMode == NetmodeID.MultiplayerClient)? " client":" server"));
            ModPacket packet = mod.GetPacket();
            packet.Write((byte)AssMessageType.SendClientChangesVanity);
            packet.Write((byte)player.whoAmI);
            packet.Write((byte)changes);

            switch (changes)
            {
                case (byte)PetPlayerChanges.all:
                    SendFieldValues(packet);
                    break;
                case (byte)PetPlayerChanges.slots:
                    packet.Write((uint)slots);
                    packet.Write((uint)color);
                    break;
                case (byte)PetPlayerChanges.mechFrogCrown:
                    packet.Write((byte)mechFrogCrown);
                    break;
                case (byte)PetPlayerChanges.petEyeType:
                    packet.Write((byte)petEyeType);
                    break;
                case (byte)PetPlayerChanges.cursedSkullType:
                    packet.Write((byte)cursedSkullType);
                    break;
                case (byte)PetPlayerChanges.youngWyvernType:
                    packet.Write((byte)youngWyvernType);
                    break;
                case (byte)PetPlayerChanges.petFishronType:
                    packet.Write((byte)petFishronType);
                    break;
                case (byte)PetPlayerChanges.petMoonType:
                    packet.Write((byte)petMoonType);
                    break;
                case (byte)PetPlayerChanges.youngHarpyType:
                    packet.Write((byte)youngHarpyType);
                    break;
                case (byte)PetPlayerChanges.abeeminationType:
                    packet.Write((byte)abeeminationType);
                    break;
                case (byte)PetPlayerChanges.lilWrapsType:
                    packet.Write((byte)lilWrapsType);
                    break;
                case (byte)PetPlayerChanges.vampireBatType:
                    packet.Write((byte)vampireBatType);
                    break;
                case (byte)PetPlayerChanges.pigronataType:
                    packet.Write((byte)pigronataType);
                    break;
                case (byte)PetPlayerChanges.queenLarvaType:
                    packet.Write((byte)queenLarvaType);
                    break;
                case (byte)PetPlayerChanges.oceanSlimeType:
                    packet.Write((byte)oceanSlimeType);
                    break;
                case (byte)PetPlayerChanges.miniAntlionType:
                    packet.Write((byte)miniAntlionType);
                    break;
                case (byte)PetPlayerChanges.petGoldfishType:
                    packet.Write((byte)petGoldfishType);
                    break;
                case (byte)PetPlayerChanges.skeletronHandType:
                    packet.Write((byte)skeletronHandType);
                    break;
                case (byte)PetPlayerChanges.skeletronPrimeHandType:
                    packet.Write((byte)skeletronPrimeHandType);
                    break;
                case (byte)PetPlayerChanges.petCultistType:
                    packet.Write((byte)petCultistType);
                    break;
                //ALTERNATE
                //case (byte)PetPlayerChanges.classNameType:
                //    packet.Write((byte)classNameType);
                //    break;
                default: //shouldn't get there hopefully
                    ErrorLogger.Log("Sending unspecified PetPlayerChanges " + changes);
                    break;
            }

            packet.Send(toClient, ignoreClient);
        }

        private void SendClientChangesPacket(PetPlayerChanges changes)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                SendClientChangesPacketSub((byte)changes);
            }
        }

        public override void OnEnterWorld(Player player)
        {
            if (!petAccessoryRework)
            {
                petAccessoryRework = true;
                ErrorLogger.Log("" + mod.Name + ": Reset pet vanity slots during update from 1.2.3 to " + mod.Version);
                slots = 0;
            }
            SendClientChangesPacket(PetPlayerChanges.all);
        }

        #region Slime Pet Vanity

        public int slimePetIndex = -1;
        public uint slotsLast = 0;
        private bool resetSlots = false;
        private double lastTime = 0.0;

        private const uint mask = 255;//0000 0000|0000 0000|0000 0000|1111 1111
        public uint slots = 0;        //0000 0000|0000 0000|0000 0000|0000 0000
        public uint color = 0;        //0000 0000|0000 0000|0000 0000|0000 0000
                                      //slot4    |slot3    |slot2    |slot1     


        public bool AddAccessory(PetAccessory petAccessory)
        {
            //id is between 0 and 255
            byte slotNumber = (byte)(petAccessory.Slot - 1);

            //returns false if accessory was already equipped   //for slotNumber = 1:
            uint setmask = mask << (slotNumber * 8);            //0000 0000|0000 0000|1111 1111|0000 0000
            uint clearmask = ~setmask; //setmask but inverted   //1111 1111|1111 1111|0000 0000|1111 1111

            uint id = (uint)petAccessory.ID << (slotNumber * 8);
            uint col = (uint)petAccessory.Color << (slotNumber * 8);
            uint tempslots = slots & setmask;
            uint tempcolor = color & setmask;
            if (id == tempslots && col == tempcolor) return false;

            //if accessory not the same as the applied one: override/set
            slots &= clearmask; //delete only current slot
            slots |= id; //set current slot id

            color &= clearmask; //delete only current slot
            color |= col; //set current slot color

            return true;
        }

        public void DelAccessory(PetAccessory petAccessory)
        {
            byte slotNumber = (byte)(petAccessory.Slot - 1);
            uint setmask = mask << (slotNumber * 8);
            uint clearmask = ~setmask; //setmask but inverted
            slots &= clearmask; //delete only current slot
            color &= clearmask; //delete only current slot color
        }

        public void ToggleAccessory(PetAccessory petAccessory)
        {
            if (petAccessory.Slot == SlotType.None) throw new Exception("can't toggle accessory on reserved slot");
            if (!AddAccessory(petAccessory)) DelAccessory(petAccessory);
        }

        public PetAccessory GetAccessoryInSlot(byte slotNumber)
        {
            byte slot = slotNumber;
            slotNumber -= 1;
            byte id = (byte)((slots >> (slotNumber * 8)) & mask); //(byte) only takes the rightmost byte
            byte col = (byte)((color >> (slotNumber * 8)) & mask);
            if (id == 0) return null;
            PetAccessory petAccessory = PetAccessory.GetAccessoryFromID((SlotType)slot, id);
            petAccessory.Color = col;
            return petAccessory;
        }
        #endregion

        #region CircleUI

        public List<Temp> CircleUIList;

        public override void Initialize()
        {
            CircleUIList = new List<Temp>();
            CircleUIList.Add(new Temp(
                triggerItem: AssUtils.Instance.ItemType<VanitySelector>(),
                condition: delegate
                {
                    return LifelikeMechanicalFrog;
                },
                uiConf: delegate
                {
                    List<Texture2D> textures = new List<Texture2D>() { AssUtils.Instance.GetTexture("Projectiles/Pets/LifelikeMechanicalFrog"),
                                                         AssUtils.Instance.GetTexture("Projectiles/Pets/LifelikeMechanicalFrogCrown") };

                    List<string> tooltips = new List<string>() { "Default", "Crowned" };

                    //no need for unlocked + toUnlock
                    return new CircleUIConf(
                        Main.projFrames[AssUtils.Instance.ProjectileType<LifelikeMechanicalFrog>()],
                        AssUtils.Instance.ProjectileType<LifelikeMechanicalFrog>(),
                        textures, null, tooltips, null);
                },
                onUIStart: delegate
                {
                    return mechFrogCrown;
                },
                onUIEnd: delegate
                {
                    mechFrogCrown = (byte)CircleUI.returned;
                },
                savedName: "mechFrogCrown"
            ));
            CircleUIList.Add(new Temp(
                triggerItem: AssUtils.Instance.ItemType<VanitySelector>(),
                condition: delegate
                {
                    return DocileDemonEye;
                },
                uiConf: delegate
                {
                    List<string> tooltips = new List<string>() { "Red", "Green", "Purple",
                "Red Fractured", "Green Fractured", "Purple Fractured",
                "Red Mechanical", "Green Mechanical", "Purple Mechanical",
                "Red Laser", "Green Laser", "Purple Laser" };

                    return Temp.PetConf("DocileDemonEyeProj", tooltips);
                },
                onUIStart: delegate
                {
                    return petEyeType;
                },
                onUIEnd: delegate
                {
                    petEyeType = (byte)CircleUI.returned;
                },
                savedName: "petEyeType"
            ));
            CircleUIList.Add(new Temp(
                triggerItem: AssUtils.Instance.ItemType<VanitySelector>(),
                condition: delegate
                {
                    return CursedSkull;
                },
                uiConf: delegate
                {
                    List<string> tooltips = new List<string>() { "Default", "Dragon" };

                    return Temp.PetConf("CursedSkull", tooltips);
                },
                onUIStart: delegate
                {
                    return cursedSkullType;
                },
                onUIEnd: delegate
                {
                    cursedSkullType = (byte)CircleUI.returned;
                },
                savedName: "cursedSkullType"
            ));
            CircleUIList.Add(new Temp(
                triggerItem: AssUtils.Instance.ItemType<VanitySelector>(),
                condition: delegate
                {
                    return YoungWyvern;
                },
                uiConf: delegate
                {
                    List<string> tooltips = new List<string>() { "Default", "Mythical", "Arch", "Arch (Legacy)" };

                    return Temp.PetConf("YoungWyvern", tooltips);
                },
                onUIStart: delegate
                {
                    return youngWyvernType;
                },
                onUIEnd: delegate
                {
                    youngWyvernType = (byte)CircleUI.returned;
                },
                savedName: "youngWyvernType"
            ));
            CircleUIList.Add(new Temp(
                triggerItem: AssUtils.Instance.ItemType<VanitySelector>(),
                condition: delegate
                {
                    return PetFishron;
                },
                uiConf: delegate
                {
                    List<string> tooltips = new List<string>() { "Default", "Sharkron", "Sharknado" };

                    return Temp.PetConf("PetFishronProj", tooltips);
                },
                onUIStart: delegate
                {
                    return petFishronType;
                },
                onUIEnd: delegate
                {
                    petFishronType = (byte)CircleUI.returned;
                },
                savedName: "petFishronType"
            ));
            CircleUIList.Add(new Temp(
                triggerItem: AssUtils.Instance.ItemType<VanitySelector>(),
                condition: delegate
                {
                    return PetMoon;
                },
                uiConf: delegate
                {
                    List<string> tooltips = new List<string>() { "Default", "Orange", "Green" }; //only 0, 1, 2 registered, 3 and 4 are event related

                    return Temp.PetConf("PetMoonProj", tooltips);
                },
                onUIStart: delegate
                {
                    return petMoonType;
                },
                onUIEnd: delegate
                {
                    petMoonType = (byte)CircleUI.returned;
                },
                triggerLeft: false,
                savedName: "petMoonType"
            ));
            CircleUIList.Add(new Temp(
                triggerItem: AssUtils.Instance.ItemType<VanitySelector>(),
                condition: delegate
                {
                    return YoungHarpy;
                },
                uiConf: delegate
                {
                    List<string> tooltips = new List<string>() { "Default", "Eagle", "Raven", "Dove" };

                    return Temp.PetConf("YoungHarpy", tooltips);
                },
                onUIStart: delegate
                {
                    return youngHarpyType;
                },
                onUIEnd: delegate
                {
                    youngHarpyType = (byte)CircleUI.returned;
                },
                savedName: "youngHarpyType"
            ));
            CircleUIList.Add(new Temp(
                triggerItem: AssUtils.Instance.ItemType<VanitySelector>(),
                condition: delegate
                {
                    return Abeemination;
                },
                uiConf: delegate
                {
                    List<string> tooltips = new List<string>() { "Default", "Snow Bee", "Oil Spill", "Missing Ingredients" };

                    return Temp.PetConf("AbeeminationProj", tooltips);
                },
                onUIStart: delegate
                {
                    return abeeminationType;
                },
                onUIEnd: delegate
                {
                    abeeminationType = (byte)CircleUI.returned;
                },
                savedName: "abeeminationType"
            ));
            CircleUIList.Add(new Temp(
                triggerItem: AssUtils.Instance.ItemType<VanitySelector>(),
                condition: delegate
                {
                    return LilWraps;
                },
                uiConf: delegate
                {
                    List<string> tooltips = new List<string>() { "Default", "Dark", "Light", "Shadow", "Spectral" };

                    return Temp.PetConf("LilWrapsProj", tooltips);
                },
                onUIStart: delegate
                {
                    return lilWrapsType;
                },
                onUIEnd: delegate
                {
                    lilWrapsType = (byte)CircleUI.returned;
                },
                savedName: "lilWrapsType"
            ));
            CircleUIList.Add(new Temp(
                triggerItem: AssUtils.Instance.ItemType<VanitySelector>(),
                condition: delegate
                {
                    return VampireBat;
                },
                uiConf: delegate
                {
                    List<string> tooltips = new List<string>() { "Default", "Werebat" };

                    return Temp.PetConf("VampireBat", tooltips);
                },
                onUIStart: delegate
                {
                    return vampireBatType;
                },
                onUIEnd: delegate
                {
                    vampireBatType = (byte)CircleUI.returned;
                },
                savedName: "vampireBatType"
            ));
            CircleUIList.Add(new Temp(
                triggerItem: AssUtils.Instance.ItemType<VanitySelector>(),
                condition: delegate
                {
                    return Pigronata;
                },
                uiConf: delegate
                {
                    List<string> tooltips = new List<string>() { "Default", "Winter", "Autumn", "Spring", "Summer", "Halloween", "Christmas" };

                    return Temp.PetConf("Pigronata", tooltips);
                },
                onUIStart: delegate
                {
                    return pigronataType;
                },
                onUIEnd: delegate
                {
                    pigronataType = (byte)CircleUI.returned;
                },
                savedName: "pigronataType"
            ));
            CircleUIList.Add(new Temp(
                triggerItem: AssUtils.Instance.ItemType<VanitySelector>(),
                condition: delegate
                {
                    return QueenLarva;
                },
                uiConf: delegate
                {
                    List<string> tooltips = new List<string>() { "Default", "Prawn Larva", "Unexpected Seed", "Big Kid Larva", "Where's The Baby?" };

                    return Temp.PetConf("QueenLarvaProj", tooltips);
                },
                onUIStart: delegate
                {
                    return queenLarvaType;
                },
                onUIEnd: delegate
                {
                    queenLarvaType = (byte)CircleUI.returned;
                },
                savedName: "queenLarvaType"
            ));
            CircleUIList.Add(new Temp(
                triggerItem: AssUtils.Instance.ItemType<VanitySelector>(),
                condition: delegate
                {
                    return OceanSlime;
                },
                uiConf: delegate
                {
                    List<string> tooltips = new List<string>() { "Default", "Stupid Hat", "Gnarly Grin", "Flipped Jelly" };

                    return Temp.PetConf("OceanSlimeProj", tooltips);
                },
                onUIStart: delegate
                {
                    return oceanSlimeType;
                },
                onUIEnd: delegate
                {
                    oceanSlimeType = (byte)CircleUI.returned;
                },
                savedName: "oceanSlimeType"
            ));
            CircleUIList.Add(new Temp(
                triggerItem: AssUtils.Instance.ItemType<VanitySelector>(),
                condition: delegate
                {
                    return MiniAntlion;
                },
                uiConf: delegate
                {
                    List<string> tooltips = new List<string>() { "Default", "Albino" };

                    return Temp.PetConf("MiniAntlionProj", tooltips);
                },
                onUIStart: delegate
                {
                    return miniAntlionType;
                },
                onUIEnd: delegate
                {
                    miniAntlionType = (byte)CircleUI.returned;
                },
                savedName: "miniAntlionType"
            ));
            CircleUIList.Add(new Temp(
                triggerItem: AssUtils.Instance.ItemType<VanitySelector>(),
                condition: delegate
                {
                    return PetGoldfish;
                },
                uiConf: delegate
                {
                    List<string> tooltips = new List<string>() { "Default", "Crimson", "Corruption", "Bunny" };

                    return Temp.PetConf("PetGoldfishProj", tooltips);
                },
                onUIStart: delegate
                {
                    return petGoldfishType;
                },
                onUIEnd: delegate
                {
                    petGoldfishType = (byte)CircleUI.returned;
                },
                savedName: "petGoldfishType"
            ));
            CircleUIList.Add(new Temp(
                triggerItem: AssUtils.Instance.ItemType<VanitySelector>(),
                condition: delegate
                {
                    return SkeletronHand;
                },
                uiConf: delegate
                {
                    List<string> tooltips = new List<string>() { "Default", "OK-Hand", "Peace", "Rock It", "Fist" };

                    return Temp.PetConf("SkeletronHandProj", tooltips);
                },
                onUIStart: delegate
                {
                    return skeletronHandType;
                },
                onUIEnd: delegate
                {
                    skeletronHandType = (byte)CircleUI.returned;
                },
                savedName: "skeletronHandType"
            ));
            CircleUIList.Add(new Temp(
                triggerItem: AssUtils.Instance.ItemType<VanitySelector>(),
                condition: delegate
                {
                    return SkeletronPrimeHand;
                },
                uiConf: delegate
                {
                    List<string> tooltips = new List<string>() { "Cannon", "Saw", "Vice", "Laser" };

                    return Temp.PetConf("SkeletronPrimeHandProj", tooltips);
                },
                onUIStart: delegate
                {
                    return skeletronPrimeHandType;
                },
                onUIEnd: delegate
                {
                    skeletronPrimeHandType = (byte)CircleUI.returned;
                },
                savedName: "skeletronPrimeHandType"
            ));
            CircleUIList.Add(new Temp(
                triggerItem: AssUtils.Instance.ItemType<VanitySelector>(),
                condition: delegate
                {
                    return PetCultist;
                },
                uiConf: delegate
                {
                    List<string> tooltips = new List<string>() { "Lunar", "Solar" };

                    return Temp.PetConf("PetCultistProj", tooltips);
                },
                onUIStart: delegate
                {
                    return petCultistType;
                },
                onUIEnd: delegate
                {
                    petCultistType = (byte)CircleUI.returned;
                },
                triggerLeft: false,
                savedName: "petCultistType"
            ));

            //ALTERNATE
            //CircleUIList.Add(new Temp(
            //    triggerItem: AssUtils.Instance.ItemType<VanitySelector>(),
            //    condition: delegate
            //    {
            //        return ClassName;
            //    },
            //    uiConf: delegate
            //    {
            //        List<string> tooltips = new List<string>() { "Default", "AltName1", "AltName2" };

            //        return Temp.PetConf("ClassNameProj", tooltips);
            //    },
            //    onUIStart: delegate
            //    {
            //        return classNameType;
            //    },
            //    onUIEnd: delegate
            //    {
            //        classNameType = (byte)CircleUI.returned;
            //    },
            //    savedName: "classNameType"
            //));

            // after filling the list, set the trigger list
            for (int i = 0; i < CircleUIList.Count; i++)
            {
                CircleUIConf.AddItemAsTrigger(CircleUIList[i].TriggerItem, CircleUIList[i].TriggerLeft);
            }
        }
        #endregion
    }
}