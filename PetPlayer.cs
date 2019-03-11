using AssortedCrazyThings.Projectiles.Pets;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace AssortedCrazyThings
{
    public class PetPlayer : ModPlayer
    {
        //docile demon eye texture
        public byte petEyeType = 0; //texture type, not ID

        //mech frog texture
        public bool mechFrogCrown = false;

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

        //queen larva texture
        public byte oceanSlimeType = 0;

        //ALTERNATE
        ////name pet texture
        //public byte classNameType = 0;

        public bool CuteSlimeDungeonNew = false;
        public bool CuteSlimeCrimsonNew = false;
        public bool CuteSlimeCorruptNew = false;
        public bool LilWraps = false;
        public bool PetFishron = false;
        public bool RainbowSlimePet = false;
        public bool PrinceSlimePet = false;
        public bool IlluminantSlimePet = false;
        public bool ChunkySlimePet = false;
        public bool FairySlimePet = false;
        public bool HornedSlimePet = false;
        public bool JoyousSlimePet = false;
        public bool MeatballSlimePet = false;
        public bool OceanSlimePet = false;
        public bool StingSlimeBlackPet = false;
        public bool StingSlimeOrangePet = false;
        public bool TurtleSlimePet = false;
        public bool Pigronata = false;
        public bool Abeemination = false;
        public bool CuteSlimeYellowNew = false;
        public bool CuteSlimeXmasNew = false;
        public bool CuteSlimeRedNew = false;
        public bool CuteSlimeRainbowNew = false;
        public bool CuteSlimePurpleNew = false;
        public bool CuteSlimePinkNew = false;
        public bool CuteSlimeGreenNew = false;
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
        //ALTERNATE
        //public bool ClassName = false;

        public override void ResetEffects()
        {
            CuteSlimeDungeonNew = false;
            CuteSlimeCrimsonNew = false;
            CuteSlimeCorruptNew = false;
            LilWraps = false;
            PetFishron = false;
            RainbowSlimePet = false;
            PrinceSlimePet = false;
            IlluminantSlimePet = false;
            ChunkySlimePet = false;
            FairySlimePet = false;
            HornedSlimePet = false;
            JoyousSlimePet = false;
            MeatballSlimePet = false;
            OceanSlimePet = false;
            StingSlimeBlackPet = false;
            StingSlimeOrangePet = false;
            TurtleSlimePet = false;
            Pigronata = false;
            Abeemination = false;
            CuteSlimeYellowNew = false;
            CuteSlimeXmasNew = false;
            CuteSlimeRedNew = false;
            CuteSlimeRainbowNew = false;
            CuteSlimePurpleNew = false;
            CuteSlimePinkNew = false;
            CuteSlimeGreenNew = false;
            CuteSlimeBlueNew = false;
            CuteSlimeBlackNew = false;
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
                {"mechFrogCrown", (bool)mechFrogCrown},
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
                //ALTERNATE
                //{"classNameType", (byte)classNameType},
            };
        }

        public override void Load(TagCompound tag)
        {
            slots = (uint)tag.GetInt("slots");
            mechFrogCrown = tag.GetBool("mechFrogCrown");
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
            //ALTERNATE
            //classNameType = tag.GetByte("classNameType");
        }

        public override void clientClone(ModPlayer clientClone)
        {
            PetPlayer clone = clientClone as PetPlayer;
            clone.slots = slots;
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
            //ALTERNATE
            //clone.classNameType = classNameType;
        }

        public override void SendClientChanges(ModPlayer clientPlayer)
        {
            PetPlayer clone = clientPlayer as PetPlayer;
            PetPlayerChanges changes = PetPlayerChanges.none;
            if (clone.slots != slots) changes = PetPlayerChanges.slots;
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
            packet.Write((bool)mechFrogCrown);
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
            //ALTERNATE
            //packet.Write((byte)classNameType);
        }

        public void RecvSyncPlayerVanitySub(BinaryReader reader)
        {
            slots = reader.ReadUInt32();
            mechFrogCrown = reader.ReadBoolean();
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
                    break;
                case (byte)PetPlayerChanges.mechFrogCrown:
                    mechFrogCrown = reader.ReadBoolean();
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
                    break;
                case (byte)PetPlayerChanges.mechFrogCrown:
                    packet.Write((bool)mechFrogCrown);
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
            SendClientChangesPacket(PetPlayerChanges.all);
        }

        //----------------------------------Slime Pet Vanity here---------------------------------------------

        public int slimePetIndex = -1;
        public byte petColor = 0;
        public uint slotsLast = 0;
        private bool resetSlots = false;
        private double lastTime = 0.0;

        private const uint mask = 255;//0000 0000|0000 0000|0000 0000|1111 1111 
        public uint slots = 0;        //0000 0000|0000 0000|0000 0000|0000 0000 
                                      //slot3    |slto2    |slto1    |slto0     

        private bool AddAccessory(byte slotNumber, uint type)
        {
            //type is between 0 and 255

            //returns false if accessory was already equipped   //for slotNumber = 1:
            uint setmask = mask << (slotNumber * 8);            //0000 0000|0000 0000|1111 1111|0000 0000
            uint clearmask = ~setmask; //setmask but inverted   //1111 1111|1111 1111|0000 0000|1111 1111
            
            type = type << (slotNumber * 8);
            uint tempslots = slots & setmask;
            if (type == tempslots) return false;

            //if accessory not the same as the applied one: override/set
            slots &= clearmask; //delete only current slot
            slots |= type; //set current slot

            return true;
        }

        private void DelAccessory(byte slotNumber)
        {
            uint setmask = mask << (slotNumber * 8);
            uint clearmask = ~setmask; //setmask but inverted
            slots &= clearmask; //delete only current slot
        }

        public uint ToggleAccessory(byte slotNumber, uint type)
        {
            if (slotNumber == 0) return slots;
            slotNumber -= 1;
            if (!AddAccessory(slotNumber, type)) DelAccessory(slotNumber);
            return slots;
        }

        public uint GetAccessory(byte slotNumber)
        {
            slotNumber -= 1;
            return (slots >> (slotNumber * 8)) & mask; //shift the selected 8 bits of the slot into the rightmost position
        }
	}
}