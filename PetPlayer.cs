using AssortedCrazyThings.Projectiles.Pets;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace AssortedCrazyThings
{
    public class PetPlayer : ModPlayer
    {
        //docile demon eye stuff
        public int eyePetIndex = -1;
        public byte petEyeType = 0; //texture type, not ID

        //mech frog stuff
        public bool mechFrogCrown = false;

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
        public bool DocileMechanicalLaserEyeGreen = false;
        public bool DocileMechanicalLaserEyePurple = false;
        public bool DocileMechanicalLaserEyeRed = false;
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
        public bool DocileDemonEyeRed = false;
        public bool DocileFracturedEyeRed = false;
        public bool DocileDemonEyeGreen = false;
        public bool DocileFracturedEyeGreen = false;
        public bool DocileDemonEyePurple = false;
        public bool DocileFracturedEyePurple = false;
        public bool DocileMechanicalEyeRed = false;
        public bool DocileMechanicalEyeGreen = false;
        public bool DocileMechanicalEyePurple = false;
        public bool DetachedHungry = false;
        public bool BabyOcram = false;
        public bool CursedSkull = false;
        public bool BabyCrimera = false;
        public bool VampireBat = false;
        public bool TorturedSoul = false;
        public bool EnchantedSword = false;
        public bool GobletPet = false;
        public bool SoulLightPet = false;
        public bool SoulLightPet2 = false;
        public bool DocileDemonEye = false;
        public bool QueenLarva = false;
        public bool HealingDrone = false;
        public bool SunPet = false;
        public bool MoonPet = false;

        public override void ResetEffects()
        {
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
            DocileMechanicalLaserEyeGreen = false;
            DocileMechanicalLaserEyePurple = false;
            DocileMechanicalLaserEyeRed = false;
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
            DocileDemonEyeRed = false;
            DocileFracturedEyeRed = false;
            DocileDemonEyeGreen = false;
            DocileFracturedEyeGreen = false;
            DocileDemonEyePurple = false;
            DocileFracturedEyePurple = false;
            DocileMechanicalEyeRed = false;
            DocileMechanicalEyeGreen = false;
            DocileMechanicalEyePurple = false;
            DetachedHungry = false;
            BabyOcram = false;
            CursedSkull = false;
            BabyCrimera = false;
            VampireBat = false;
            TorturedSoul = false;
            EnchantedSword = false;
            GobletPet = false;
            SoulLightPet = false;
            SoulLightPet2 = false;
            DocileDemonEye = false;
            QueenLarva = false;
            HealingDrone = false;
            SunPet = false;
            MoonPet = false;
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
                {"petEyeType", (byte)petEyeType},
                {"mechFrogCrown", (bool)mechFrogCrown}
            };
        }

        public override void Load(TagCompound tag)
        {
            slots = (uint)tag.GetInt("slots");
            petEyeType = tag.GetByte("petEyeType");
            mechFrogCrown = tag.GetBool("mechFrogCrown");
        }

        public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
        {
            ModPacket packet = mod.GetPacket();
            packet.Write((byte)AssMessageType.OnEnterWorldVanity);
            packet.Write((byte)player.whoAmI);
            packet.Write((uint)slots);
            packet.Write((byte)petEyeType);
            packet.Write((bool)mechFrogCrown);
            packet.Send(toWho, fromWho);
        }

        private void SendClientChangesPacket()
        {
            if(Main.netMode == NetmodeID.MultiplayerClient)
            {
                //Main.NewText("Send: " + slots + " " + slotsLast);
                ModPacket packet = mod.GetPacket();
                packet.Write((byte)AssMessageType.SendClientChangesVanity);
                packet.Write((byte)player.whoAmI);
                packet.Write((uint)slots);
                packet.Write((byte)petEyeType);
                packet.Write((bool)mechFrogCrown);
                packet.Send();
            }
        }

        public override void clientClone(ModPlayer clientClone)
        {
            PetPlayer clone = clientClone as PetPlayer;
            clone.slots = slots;
            clone.mechFrogCrown = mechFrogCrown;
        }

        public override void SendClientChanges(ModPlayer clientPlayer)
        {
            PetPlayer clone = clientPlayer as PetPlayer;
            if (clone.slots != slots || clone.mechFrogCrown != mechFrogCrown)
            {
                SendClientChangesPacket();
            }
        }

        public override void OnEnterWorld(Player player)
        {
            SendClientChangesPacket();
        }

        public byte CyclePetEyeType()
        {
            petEyeType++;
            if (petEyeType >= DocileDemonEyeProj.TotalNumberOfThese) petEyeType = 0;
            return petEyeType;
        }

        public int slimePetIndex = -1;
        public byte petColor = 0;
        public uint slotsLast = 0;
        private bool resetSlots = false;
        private double lastTime = 0.0;

        private const uint mask = 255;//0000 0000|0000 0000|0000 0000|1111 1111 
        public uint slots = 0;        //0000 0000|0000 0000|0000 0000|0000 0000 
                                      //slt3     |slt2     |slt1     |slt0     

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