using AssortedCrazyThings.Base;
using AssortedCrazyThings.Items;
using AssortedCrazyThings.Items.PetAccessories;
using AssortedCrazyThings.Projectiles.Pets;
using AssortedCrazyThings.UI;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
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
        /// <summary>
        /// transition from 1.2.3 to 1.3.0 (reset cute slime vanity slots)
        /// </summary>
        private bool petAccessoryRework = false;

        /// <summary>
        /// transition from 1.3.0 to "CURRENT VERSION" (simplified pet type handling)
        /// </summary>
        private bool petVanityRework = false;

        private bool enteredWorld = false;

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

        //pet cultist texture
        public byte animatedTomeType = 0;

        //pet wall of flesh texture
        public byte wallFragmentType = 0;

        //ALTERNATE
        ////name pet texture
        //public byte classNameType = 0;

        public bool CuteSlimeYellow = false;
        public bool CuteSlimeXmas = false;
        public bool CuteSlimeToxic = false;
        public bool CuteSlimeSand = false;
        public bool CuteSlimeRed = false;
        public bool CuteSlimeRainbow = false;
        public bool CuteSlimePurple = false;
        public bool CuteSlimePrincess = false;
        public bool CuteSlimePink = false;
        public bool CuteSlimeLava = false;
        public bool CuteSlimeJungle = false;
        public bool CuteSlimeIlluminant = false;
        public bool CuteSlimeIce = false;
        public bool CuteSlimeGreen = false;
        public bool CuteSlimeDungeon = false;
        public bool CuteSlimeCrimson = false;
        public bool CuteSlimeCorrupt = false;
        public bool CuteSlimeBlue = false;
        public bool CuteSlimeBlack = false;

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
        public bool MiniMegalodon = false;
        public bool YoungHarpy = false;
        public bool CuteGastropod = false;
        public bool YoungWyvern = false;
        public bool BabyIchorSticker = false;
        public bool LifelikeMechanicalFrog = false;
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
        public bool AnimatedTome = false;
        //ALTERNATE
        //public bool ClassName = false;

        public override void ResetEffects()
        {
            CuteSlimeYellow = false;
            CuteSlimeXmas = false;
            CuteSlimeToxic = false;
            CuteSlimeSand = false;
            CuteSlimeRed = false;
            CuteSlimeRainbow = false;
            CuteSlimePrincess = false;
            CuteSlimePurple = false;
            CuteSlimePink = false;
            CuteSlimeLava = false;
            CuteSlimeJungle = false;
            CuteSlimeIce = false;
            CuteSlimeIlluminant = false;
            CuteSlimeGreen = false;
            CuteSlimeBlue = false;
            CuteSlimeBlack = false;
            CuteSlimeCrimson = false;
            CuteSlimeCorrupt = false;
            CuteSlimeDungeon = false;
            CuteSlimeXmas = false;

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
            MiniMegalodon = false;
            YoungHarpy = false;
            CuteGastropod = false;
            YoungWyvern = false;
            BabyIchorSticker = false;
            LifelikeMechanicalFrog = false;
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
            AnimatedTome = false;
            //ALTERNATE
            //ClassName = false;
        }

        /// <summary>
        /// Returns true if this has been called the third time after two successful calls within 80 ticks
        /// </summary>
        public bool ThreeTimesUseTime()
        {
            uint diff = (uint)Math.Abs(Main.GameUpdateCount - lastTime);
            if (diff > 40.0)
            {
                //19
                resetSlots = false;
                lastTime = Main.GameUpdateCount;
                return false; //step one
            }

            //step two and three have to be done in 40 ticks each
            if (diff <= 40.0)
            {
                if (!resetSlots)
                {
                    lastTime = Main.GameUpdateCount;
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
            TagCompound tag = new TagCompound {
                {"slots", (int)slots},
                {"color", (int)color},
                {"petAccessoryRework", (bool)petAccessoryRework},
                {"petVanityRework", (bool)petVanityRework}
            };
            var petTypes = new List<byte>(ClonedTypes);
            tag.Add("petTypes", petTypes);
            return tag;
        }

        public override void Load(TagCompound tag)
        {
            slots = (uint)tag.GetInt("slots");
            color = (uint)tag.GetInt("color");
            petAccessoryRework = tag.GetBool("petAccessoryRework");
            petVanityRework = tag.GetBool("petVanityRework");
            if (!petVanityRework) //transfer to new system
            {
                //DON'T TOUCH ANYTHING IN THIS SECTION
                int index = -1;
                ClonedTypes[++index] = mechFrogCrown = tag.GetByte("mechFrogCrown");
                ClonedTypes[++index] = petEyeType = tag.GetByte("petEyeType");
                ClonedTypes[++index] = cursedSkullType = tag.GetByte("cursedSkullType");
                ClonedTypes[++index] = youngWyvernType = tag.GetByte("youngWyvernType");
                ClonedTypes[++index] = petFishronType = tag.GetByte("petFishronType");
                ClonedTypes[++index] = petMoonType = tag.GetByte("petMoonType");
                ClonedTypes[++index] = youngHarpyType = tag.GetByte("youngHarpyType");
                ClonedTypes[++index] = abeeminationType = tag.GetByte("abeeminationType");
                ClonedTypes[++index] = lilWrapsType = tag.GetByte("lilWrapsType");
                ClonedTypes[++index] = vampireBatType = tag.GetByte("vampireBatType");
                ClonedTypes[++index] = pigronataType = tag.GetByte("pigronataType");
                ClonedTypes[++index] = queenLarvaType = tag.GetByte("queenLarvaType");
                ClonedTypes[++index] = oceanSlimeType = tag.GetByte("oceanSlimeType");
                ClonedTypes[++index] = miniAntlionType = tag.GetByte("miniAntlionType");
                ClonedTypes[++index] = petGoldfishType = tag.GetByte("petGoldfishType");
                ClonedTypes[++index] = skeletronHandType = tag.GetByte("skeletronHandType");
                ClonedTypes[++index] = skeletronPrimeHandType = tag.GetByte("skeletronPrimeHandType");
                ClonedTypes[++index] = petCultistType = tag.GetByte("petCultistType");
                //every other new type will be 0 (makes sense since this is when the player first updates to the new version)
                //DON'T TOUCH ANYTHING IN THIS SECTION
            }
            else
            {
                var petTypeList = tag.GetList<byte>("petTypes");
                int clonedTypesLength = ClonedTypes.Length;
                ClonedTypes = new List<byte>(petTypeList).ToArray();
                //in case new types got added (since this assignment overrides the old ClonedTypes length)
                Array.Resize(ref ClonedTypes, clonedTypesLength);
            }
        }

        public override void clientClone(ModPlayer clientClone)
        {
            PetPlayer clone = clientClone as PetPlayer;
            clone.slots = slots;
            clone.color = color;
            Array.Copy(ClonedTypes, clone.ClonedTypes, ClonedTypes.Length);
        }

        public override void SendClientChanges(ModPlayer clientPlayer)
        {
            PetPlayer clone = clientPlayer as PetPlayer;
            PetPlayerChanges changes = PetPlayerChanges.None;
            int index = 255;
            if (clone.slots != slots || clone.color != color)
            {
                changes = PetPlayerChanges.Slots;
            }
            else
            {
                for (int i = 0; i < ClonedTypes.Length; i++)
                {
                    if (clone.ClonedTypes[i] != ClonedTypes[i])
                    {
                        changes = PetPlayerChanges.PetTypes;
                        index = i;
                        break;
                    }
                }
            }

            if (changes != PetPlayerChanges.None) SendClientChangesPacket(changes, index);
        }

        public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
        {
            if (Main.netMode != NetmodeID.Server) return;
            //from server to clients
            ModPacket packet = Mod.GetPacket();
            packet.Write((byte)AssMessageType.SyncPlayerVanity);
            packet.Write((byte)Player.whoAmI);
            //no "changes" packet
            SendFieldValues(packet);
            packet.Send(toWho, fromWho);
        }

        private void SendFieldValues(ModPacket packet)
        {
            packet.Write((uint)slots);
            packet.Write((uint)color);

            for (int i = 0; i < ClonedTypes.Length; i++)
            {
                packet.Write((byte)ClonedTypes[i]);
            }
        }

        public void RecvSyncPlayerVanitySub(BinaryReader reader)
        {
            slots = reader.ReadUInt32();
            color = reader.ReadUInt32();
            for (int i = 0; i < ClonedTypes.Length; i++)
            {
                ClonedTypes[i] = reader.ReadByte();
            }
            GetFromClonedTypes();
        }

        /// <summary>
        /// Reads from reader to assign the player fields (Called in Mod.HandlePacket())
        /// </summary>
        public void RecvClientChangesPacketSub(BinaryReader reader, byte changes, int index)
        {
            //AssUtils.Print("RecvClientChangesPacketSub " + changes + " index " + index + " from p " + player.whoAmI);
            switch (changes)
            {
                case (byte)PetPlayerChanges.All:
                    RecvSyncPlayerVanitySub(reader);
                    break;
                case (byte)PetPlayerChanges.Slots:
                    slots = reader.ReadUInt32();
                    color = reader.ReadUInt32();
                    break;
                case (byte)PetPlayerChanges.PetTypes:
                    if (index >= 0 && index < ClonedTypes.Length) ClonedTypes[index] = reader.ReadByte();
                    break;
                default: //shouldn't get there hopefully
                    Mod.Logger.Debug("Received unspecified PetPlayerChanges Packet " + changes);
                    break;
            }
            GetFromClonedTypes();
        }

        /// <summary>
        /// Sends the player fields either to clients or to the server. Called in OnEnterWorld (to the server), and in Mod.HandlePacket()
        /// (forwarding data from the server to other players)
        /// </summary>
        public void SendClientChangesPacketSub(byte changes, int index, int toClient = -1, int ignoreClient = -1)
        {
            //AssUtils.Print("SendClientChangesPacketSub " + changes + " index " + index + " from p " + player.whoAmI + ((Main.netMode == NetmodeID.MultiplayerClient)? " client":" server"));
            ModPacket packet = Mod.GetPacket();
            packet.Write((byte)AssMessageType.ClientChangesVanity);
            packet.Write((byte)Player.whoAmI);
            packet.Write((byte)changes);
            packet.Write((byte)index);

            switch (changes)
            {
                case (byte)PetPlayerChanges.All:
                    SendFieldValues(packet);
                    break;
                case (byte)PetPlayerChanges.Slots:
                    packet.Write((uint)slots);
                    packet.Write((uint)color);
                    break;
                case (byte)PetPlayerChanges.PetTypes:
                    packet.Write((byte)ClonedTypes[index]);
                    break;
                default: //shouldn't get there hopefully
                    Mod.Logger.Debug("Sending unspecified PetPlayerChanges " + changes);
                    break;
            }

            packet.Send(toClient, ignoreClient);
        }

        /// <summary>
        /// Cliendside method to send the chances specified to the server.
        /// Called in OnEnterWorld() and in SendClientChanges()
        /// </summary>
        private void SendClientChangesPacket(PetPlayerChanges changes, int index = 255)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                SendClientChangesPacketSub((byte)changes, index);
            }
        }

        public override void OnEnterWorld(Player player)
        {
            enteredWorld = true;
            if (!petAccessoryRework)
            {
                petAccessoryRework = true;
                Mod.Logger.Debug("Reset pet vanity slots during update from 1.2.3 to " + Mod.Version);
                slots = 0;
            }
            if (!petVanityRework)
            {
                petVanityRework = true;
            }
            else
            {
                //AssUtils.Print("onenterworld p " + player.whoAmI);
                GetFromClonedTypes();
            }
            SendClientChangesPacket(PetPlayerChanges.All);
        }

        public override void PreUpdate()
        {
            if (Main.myPlayer == Player.whoAmI) SetClonedTypes();
        }

        #region Slime Pet Vanity

        public int slimePetIndex = -1;
        public uint slotsLast = 0;
        private bool resetSlots = false;
        private uint lastTime = 0;

        private const uint mask = 255;//0000 0000|0000 0000|0000 0000|1111 1111
        public uint slots = 0;        //0000 0000|0000 0000|0000 0000|0000 0000
        public uint color = 0;        //0000 0000|0000 0000|0000 0000|0000 0000
                                      //slot4    |slot3    |slot2    |slot1     

        /// <summary>
        /// Adds the pet vanity accessory to the current pet
        /// </summary>
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

        /// <summary>
        /// Deletes the pet vanity accessory on the current pet
        /// </summary>
        public void DelAccessory(PetAccessory petAccessory)
        {
            byte slotNumber = (byte)(petAccessory.Slot - 1);
            uint setmask = mask << (slotNumber * 8);
            uint clearmask = ~setmask; //setmask but inverted
            slots &= clearmask; //delete only current slot
            color &= clearmask; //delete only current slot color
        }

        /// <summary>
        /// Toggles the pet vanity acessory on the current pet
        /// </summary>
        public void ToggleAccessory(PetAccessory petAccessory)
        {
            if (petAccessory.Slot == SlotType.None) throw new Exception("Can't toggle accessory on reserved slot");
            if (!AddAccessory(petAccessory)) DelAccessory(petAccessory);
        }

        /// <summary>
        /// Returns the pet vanity accessory equipped in the specified SlotType of the current pet
        /// </summary>
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
        /// <summary>
        /// Contains a list of CircleUIHandlers that are used in CircleUIStart/End in Mod
        /// </summary>
        public List<CircleUIHandler> CircleUIList;

        /// <summary>
        /// Contains a list of pet type fields (assigned during Load() and every tick in PreUpdate(),
        /// and read from in OnEnterWorld(), and in Multiplayer whenever data is received).
        /// Simplifies the saving/loading of tags
        /// </summary>
        public byte[] ClonedTypes;

        public static CircleUIConf GetLifelikeMechanicalFrogConf()
        {
            List<Asset<Texture2D>> assets = new List<Asset<Texture2D>>() {
                        AssUtils.Instance.GetTexture("Projectiles/Pets/LifelikeMechanicalFrog"),
                        AssUtils.Instance.GetTexture("Projectiles/Pets/LifelikeMechanicalFrogCrown") };

            List<string> tooltips = new List<string>() { "Default", "Crowned" };

            //no need for unlocked + toUnlock
            return new CircleUIConf(
                Main.projFrames[ModContent.ProjectileType<LifelikeMechanicalFrogProj>()],
                ModContent.ProjectileType<LifelikeMechanicalFrogProj>(),
                assets, null, tooltips, null);
        }

        public static CircleUIConf GetDocileDemonEyeConf()
        {
            List<string> tooltips = new List<string>() { "Red", "Green", "Purple",
                "Red Fractured", "Green Fractured", "Purple Fractured",
                "Red Mechanical", "Green Mechanical", "Purple Mechanical",
                "Red Laser", "Green Laser", "Purple Laser" };

            return CircleUIHandler.PetConf("DocileDemonEyeProj", tooltips);
        }

        public static CircleUIConf GetCursedSkullConf()
        {
            List<string> tooltips = new List<string>() { "Default", "Dragon" };

            return CircleUIHandler.PetConf("CursedSkullProj", tooltips);
        }

        public static CircleUIConf GetYoungWyvernConf()
        {
            List<string> tooltips = new List<string>() { "Default", "Mythical", "Arch", "Arch (Legacy)" };

            return CircleUIHandler.PetConf("YoungWyvernProj", tooltips);
        }

        public static CircleUIConf GetPetFishronConf()
        {
            List<string> tooltips = new List<string>() { "Default", "Sharkron", "Sharknado" };

            return CircleUIHandler.PetConf("PetFishronProj", tooltips);
        }

        public static CircleUIConf GetPetMoonConf()
        {
            List<string> tooltips = new List<string>() { "Default", "Brown", "Ring", "Green", "White", "Green 2", "Pink", "Orange", "Purple", }; //9 10 11 are contextual

            return CircleUIHandler.PetConf("PetMoonProj", tooltips);
        }

        public static CircleUIConf GetYoungHarpyConf()
        {
            List<string> tooltips = new List<string>() { "Default", "Eagle", "Raven", "Dove", "Default (Legacy)", "Eagle (Legacy)", "Raven (Legacy)", "Dove (Legacy)" };

            return CircleUIHandler.PetConf("YoungHarpyProj", tooltips);
        }

        public static CircleUIConf GetAbeeminationConf()
        {
            List<string> tooltips = new List<string>() { "Default", "Snow Bee", "Oil Spill", "Missing Ingredients" };

            return CircleUIHandler.PetConf("AbeeminationProj", tooltips);
        }

        public static CircleUIConf GetLilWrapsConf()
        {
            List<string> tooltips = new List<string>() { "Default", "Dark", "Light", "Shadow", "Spectral" };

            return CircleUIHandler.PetConf("LilWrapsProj", tooltips);
        }

        public static CircleUIConf GetVampireBatConf()
        {
            List<string> tooltips = new List<string>() { "Default", "Werebat" };

            return CircleUIHandler.PetConf("VampireBatProj", tooltips);
        }

        public static CircleUIConf GetPigronataConf()
        {
            List<string> tooltips = new List<string>() { "Default", "Winter", "Autumn", "Spring", "Summer", "Halloween", "Christmas" };

            return CircleUIHandler.PetConf("PigronataProj", tooltips);
        }

        public static CircleUIConf GetQueenLarvaConf()
        {
            List<string> tooltips = new List<string>() { "Default", "Prawn Larva", "Unexpected Seed", "Big Kid Larva", "Where's The Baby?" };

            return CircleUIHandler.PetConf("QueenLarvaProj", tooltips);
        }

        public static CircleUIConf GetOceanSlimeConf()
        {
            List<string> tooltips = new List<string>() { "Default", "Stupid Hat", "Gnarly Grin", "Flipped Jelly" };

            return CircleUIHandler.PetConf("OceanSlimeProj", tooltips);
        }

        public static CircleUIConf GetMiniAntlionConf()
        {
            List<string> tooltips = new List<string>() { "Default", "Albino" };

            return CircleUIHandler.PetConf("MiniAntlionProj", tooltips);
        }

        public static CircleUIConf PetGoldfishConf()
        {
            List<string> tooltips = new List<string>() { "Default", "Crimson", "Corruption", "Bunny" };

            return CircleUIHandler.PetConf("PetGoldfishProj", tooltips);
        }

        public static CircleUIConf GetSkeletronHandConf()
        {
            List<string> tooltips = new List<string>() { "Default", "OK-Hand", "Peace", "Rock It", "Fist" };

            return CircleUIHandler.PetConf("SkeletronHandProj", tooltips);
        }

        public static CircleUIConf GetSkeletronPrimeHandConf()
        {
            List<string> tooltips = new List<string>() { "Cannon", "Saw", "Vice", "Laser" };

            return CircleUIHandler.PetConf("SkeletronPrimeHandProj", tooltips);
        }

        public static CircleUIConf GetPetCultistConf()
        {
            List<string> tooltips = new List<string>() { "Lunar", "Solar" };

            return CircleUIHandler.PetConf("PetCultistProj", tooltips);
        }

        public static CircleUIConf GetAnimatedTomeConf()
        {
            List<string> tooltips = new List<string>() { "Green", "Blue", "Purple", "Pink", "Yellow", "Spell" };

            return CircleUIHandler.PetConf("AnimatedTomeProj", tooltips);
        }

        public static CircleUIConf GetWallFragmentConf()
        {
            List<string> tooltips = new List<string>() { "Default", "Chinese" };

            return CircleUIHandler.PetConf("WallFragmentMouth", tooltips);
        }

        //ALTERNATE
        //public static CircleUIConf GetClassNameConf()
        //{
        //    List<string> tooltips = new List<string>() { "Default", "AltName1", "AltName2" };

        //    return CircleUIHandler.PetConf("ClassNameProj", tooltips);
        //}

        public override void Initialize()
        {
            //called before Load()
            //needs to call new List() since Initialize() is called per player in the player select screen
            CircleUIList = new List<CircleUIHandler>
            {
                new CircleUIHandler(
                triggerItem: ModContent.ItemType<VanitySelector>(),
                condition: () => LifelikeMechanicalFrog,
                uiConf: GetLifelikeMechanicalFrogConf,
                onUIStart: () => mechFrogCrown,
                onUIEnd: () => mechFrogCrown = (byte)CircleUI.returned,
                needsSaving: true
            ),
                new CircleUIHandler(
                triggerItem: ModContent.ItemType<VanitySelector>(),
                condition: () => DocileDemonEye,
                uiConf: GetDocileDemonEyeConf,
                onUIStart: () => petEyeType,
                onUIEnd: () => petEyeType = (byte)CircleUI.returned,
                needsSaving: true
            ),
                new CircleUIHandler(
                triggerItem: ModContent.ItemType<VanitySelector>(),
                condition: () => CursedSkull,
                uiConf: GetCursedSkullConf,
                onUIStart: () => cursedSkullType,
                onUIEnd: () => cursedSkullType = (byte)CircleUI.returned,
                needsSaving: true
            ),
                new CircleUIHandler(
                triggerItem: ModContent.ItemType<VanitySelector>(),
                condition: () => YoungWyvern,
                uiConf: GetYoungWyvernConf,
                onUIStart: () => youngWyvernType,
                onUIEnd: () => youngWyvernType = (byte)CircleUI.returned,
                needsSaving: true
            ),
                new CircleUIHandler(
                triggerItem: ModContent.ItemType<VanitySelector>(),
                condition: () => PetFishron,
                uiConf: GetPetFishronConf,
                onUIStart: () => petFishronType,
                onUIEnd: () => petFishronType = (byte)CircleUI.returned,
                needsSaving: true
            ),
                new CircleUIHandler(
                triggerItem: ModContent.ItemType<VanitySelector>(),
                condition: () => PetMoon,
                uiConf: GetPetMoonConf,
                onUIStart: () => petMoonType,
                onUIEnd: () => petMoonType = (byte)CircleUI.returned,
                triggerLeft: false,
                needsSaving: true
            ),
                new CircleUIHandler(
                triggerItem: ModContent.ItemType<VanitySelector>(),
                condition: () => YoungHarpy,
                uiConf: GetYoungHarpyConf,
                onUIStart: () => youngHarpyType,
                onUIEnd: () => youngHarpyType = (byte)CircleUI.returned,
                needsSaving: true
            ),
                new CircleUIHandler(
                triggerItem: ModContent.ItemType<VanitySelector>(),
                condition: () => Abeemination,
                uiConf: GetAbeeminationConf,
                onUIStart: () => abeeminationType,
                onUIEnd: () => abeeminationType = (byte)CircleUI.returned,
                needsSaving: true
            ),
                new CircleUIHandler(
                triggerItem: ModContent.ItemType<VanitySelector>(),
                condition: () => LilWraps,
                uiConf: GetLilWrapsConf,
                onUIStart: () => lilWrapsType,
                onUIEnd: () => lilWrapsType = (byte)CircleUI.returned,
                needsSaving: true
            ),
                new CircleUIHandler(
                triggerItem: ModContent.ItemType<VanitySelector>(),
                condition: () => VampireBat,
                uiConf: GetVampireBatConf,
                onUIStart: () => vampireBatType,
                onUIEnd: () => vampireBatType = (byte)CircleUI.returned,
                needsSaving: true
            ),
                new CircleUIHandler(
                triggerItem: ModContent.ItemType<VanitySelector>(),
                condition: () => Pigronata,
                uiConf: GetPigronataConf,
                onUIStart: () => pigronataType,
                onUIEnd: () => pigronataType = (byte)CircleUI.returned,
                needsSaving: true
            ),
                new CircleUIHandler(
                triggerItem: ModContent.ItemType<VanitySelector>(),
                condition: () => QueenLarva,
                uiConf: GetQueenLarvaConf,
                onUIStart: () => queenLarvaType,
                onUIEnd: () => queenLarvaType = (byte)CircleUI.returned,
                needsSaving: true
            ),
                new CircleUIHandler(
                triggerItem: ModContent.ItemType<VanitySelector>(),
                condition: () => OceanSlime,
                uiConf: GetOceanSlimeConf,
                onUIStart: () => oceanSlimeType,
                onUIEnd: () => oceanSlimeType = (byte)CircleUI.returned,
                needsSaving: true
            ),
                new CircleUIHandler(
                triggerItem: ModContent.ItemType<VanitySelector>(),
                condition: () => MiniAntlion,
                uiConf: GetMiniAntlionConf,
                onUIStart: () => miniAntlionType,
                onUIEnd: () => miniAntlionType = (byte)CircleUI.returned,
                needsSaving: true
            ),
                new CircleUIHandler(
                triggerItem: ModContent.ItemType<VanitySelector>(),
                condition: () => PetGoldfish,
                uiConf: PetGoldfishConf,
                onUIStart: () => petGoldfishType,
                onUIEnd: () => petGoldfishType = (byte)CircleUI.returned,
                needsSaving: true
            ),
                new CircleUIHandler(
                triggerItem: ModContent.ItemType<VanitySelector>(),
                condition: () => SkeletronHand,
                uiConf: GetSkeletronHandConf,
                onUIStart: () => skeletronHandType,
                onUIEnd: () => skeletronHandType = (byte)CircleUI.returned,
                needsSaving: true
            ),
                new CircleUIHandler(
                triggerItem: ModContent.ItemType<VanitySelector>(),
                condition: () => SkeletronPrimeHand,
                uiConf: GetSkeletronPrimeHandConf,
                onUIStart: () => skeletronPrimeHandType,
                onUIEnd: () => skeletronPrimeHandType = (byte)CircleUI.returned,
                needsSaving: true
            ),
                new CircleUIHandler(
                triggerItem: ModContent.ItemType<VanitySelector>(),
                condition: () => PetCultist,
                uiConf: GetPetCultistConf,
                onUIStart: () => petCultistType,
                onUIEnd: () => petCultistType = (byte)CircleUI.returned,
                triggerLeft: false,
                needsSaving: true
            ),
                new CircleUIHandler(
                triggerItem: ModContent.ItemType<VanitySelector>(),
                condition: () => AnimatedTome,
                uiConf: GetAnimatedTomeConf,
                onUIStart: () => animatedTomeType,
                onUIEnd: () => animatedTomeType = (byte)CircleUI.returned,
                needsSaving: true
            ),
                new CircleUIHandler(
                triggerItem: ModContent.ItemType<VanitySelector>(),
                condition: () => WallFragment,
                uiConf: GetWallFragmentConf,
                onUIStart: () => wallFragmentType,
                onUIEnd: () => wallFragmentType = (byte)CircleUI.returned,
                needsSaving: true
            ),
            //ALTERNATE
            //    new CircleUIHandler(
            //    triggerItem: ModContent.ItemType<VanitySelector>(),
            //    condition: () => ClassName,
            //    uiConf: GetClassNameConf,
            //    onUIStart: () => classNameType,
            //    onUIEnd: () => classNameType = (byte)CircleUI.returned,
            //    needsSaving: true
            //),
            };

            // after filling the list, set the trigger list
            for (int i = 0; i < CircleUIList.Count; i++)
            {
                CircleUIHandler.AddItemAsTrigger(CircleUIList[i].TriggerItem, CircleUIList[i].TriggerLeft);
            }

            //after filling the list, initialize the cloned list
            int length = 0;
            for (int i = 0; i < CircleUIList.Count; i++)
            {
                if (CircleUIList[i].NeedsSaving) length++;
            }

            ClonedTypes = new byte[length];
        }

        /// <summary>
        /// Called whenever something is received (MP on reveive, or in Singleplayer/Local client in OnEnterWorld).
        /// Sets the pet type of the corresponding entry of ClonedTypes
        /// </summary>
        public void GetFromClonedTypes()
        {
            //AssUtils.Print("set getfromclonedtypes p " + player.whoAmI + " " + mp);
            int index = 0;
            mechFrogCrown = ClonedTypes[index++];
            petEyeType = ClonedTypes[index++];
            cursedSkullType = ClonedTypes[index++];
            youngWyvernType = ClonedTypes[index++];
            petFishronType = ClonedTypes[index++];
            petMoonType = ClonedTypes[index++];
            youngHarpyType = ClonedTypes[index++];
            abeeminationType = ClonedTypes[index++];
            lilWrapsType = ClonedTypes[index++];
            vampireBatType = ClonedTypes[index++];
            pigronataType = ClonedTypes[index++];
            queenLarvaType = ClonedTypes[index++];
            oceanSlimeType = ClonedTypes[index++];
            miniAntlionType = ClonedTypes[index++];
            petGoldfishType = ClonedTypes[index++];
            skeletronHandType = ClonedTypes[index++];
            skeletronPrimeHandType = ClonedTypes[index++];
            petCultistType = ClonedTypes[index++];
            animatedTomeType = ClonedTypes[index++];
            wallFragmentType = ClonedTypes[index++];
            //ALTERNATE
            //classNameType = ClonedTypes[index++];
        }

        /// <summary>
        /// Called in PreUpdate (which runs before OnEnterWorld, hence the check).
        /// Sets each entry of ClonedTypes to the corresponding pet type
        /// </summary>
        public void SetClonedTypes()
        {
            if (enteredWorld)
            {
                int index = -1;
                ClonedTypes[++index] = mechFrogCrown;
                ClonedTypes[++index] = petEyeType;
                ClonedTypes[++index] = cursedSkullType;
                ClonedTypes[++index] = youngWyvernType;
                ClonedTypes[++index] = petFishronType;
                ClonedTypes[++index] = petMoonType;
                ClonedTypes[++index] = youngHarpyType;
                ClonedTypes[++index] = abeeminationType;
                ClonedTypes[++index] = lilWrapsType;
                ClonedTypes[++index] = vampireBatType;
                ClonedTypes[++index] = pigronataType;
                ClonedTypes[++index] = queenLarvaType;
                ClonedTypes[++index] = oceanSlimeType;
                ClonedTypes[++index] = miniAntlionType;
                ClonedTypes[++index] = petGoldfishType;
                ClonedTypes[++index] = skeletronHandType;
                ClonedTypes[++index] = skeletronPrimeHandType;
                ClonedTypes[++index] = petCultistType;
                ClonedTypes[++index] = animatedTomeType;
                ClonedTypes[++index] = wallFragmentType;
                //ALTERNATE
                //ClonedTypes[++index] = classNameType;
            }
        }

        #endregion
    }
}
