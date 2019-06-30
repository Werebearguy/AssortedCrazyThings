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
        /// <summary>
        /// Deprecated
        /// </summary>
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
        public bool AnimatedTome = false;
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
            AnimatedTome = false;
            //ALTERNATE
            //ClassName = false;
        }

        /// <summary>
        /// Returns true if this has been called the third time after two successful calls within 36 ticks
        /// </summary>
        public bool ThreeTimesUseTime()
        {
            if (Math.Abs(lastTime - Main.time) > 35.0) //(usetime + 1) x 3 + 1
            {
                resetSlots = false;
                lastTime = Main.time;
                return false; //step one
            }

            //step two and three have to be done in 35 ticks
            if (Math.Abs(lastTime - Main.time) <= 35.0)
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
            GetFromClonedTypes("in mp");
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
                    mod.Logger.Debug("Recieved unspecified PetPlayerChanges Packet " + changes);
                    break;
            }
            GetFromClonedTypes("in mp");
        }

        /// <summary>
        /// Sends the player fields either to clients or to the server. Called in OnEnterWorld (to the server), and in Mod.HandlePacket()
        /// (forwarding data from the server to other players)
        /// </summary>
        public void SendClientChangesPacketSub(byte changes, int index, int toClient = -1, int ignoreClient = -1)
        {
            //AssUtils.Print("SendClientChangesPacketSub " + changes + " index " + index + " from p " + player.whoAmI + ((Main.netMode == NetmodeID.MultiplayerClient)? " client":" server"));
            ModPacket packet = mod.GetPacket();
            packet.Write((byte)AssMessageType.ClientChangesVanity);
            packet.Write((byte)player.whoAmI);
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
                    mod.Logger.Debug("Sending unspecified PetPlayerChanges " + changes);
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
                mod.Logger.Debug("Reset pet vanity slots during update from 1.2.3 to " + mod.Version);
                slots = 0;
            }
            if (!petVanityRework)
            {
                petVanityRework = true;
            }
            else
            {
                //AssUtils.Print("onenterworld p " + player.whoAmI);
                GetFromClonedTypes("in sp");
            }
            SendClientChangesPacket(PetPlayerChanges.All);
        }

        public override void PreUpdate()
        {
            if (Main.myPlayer == player.whoAmI) SetClonedTypes();
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

        public override void Initialize()
        {
            //called before Load()
            //needs to call new List() since Initialize() is called per player in the player select screen
            CircleUIList = new List<CircleUIHandler>
            {
                new CircleUIHandler(
                triggerItem: AssUtils.Instance.ItemType<VanitySelector>(),
                condition: delegate
                {
                    return LifelikeMechanicalFrog;
                },
                uiConf: delegate
                {
                    List<Texture2D> textures = new List<Texture2D>() {
                        AssUtils.Instance.GetTexture("Projectiles/Pets/LifelikeMechanicalFrog"),
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
                needsSaving: true
            ),
                new CircleUIHandler(
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

                    return CircleUIHandler.PetConf("DocileDemonEyeProj", tooltips);
                },
                onUIStart: delegate
                {
                    return petEyeType;
                },
                onUIEnd: delegate
                {
                    petEyeType = (byte)CircleUI.returned;
                },
                needsSaving: true
            ),
                new CircleUIHandler(
                triggerItem: AssUtils.Instance.ItemType<VanitySelector>(),
                condition: delegate
                {
                    return CursedSkull;
                },
                uiConf: delegate
                {
                    List<string> tooltips = new List<string>() { "Default", "Dragon" };

                    return CircleUIHandler.PetConf("CursedSkull", tooltips);
                },
                onUIStart: delegate
                {
                    return cursedSkullType;
                },
                onUIEnd: delegate
                {
                    cursedSkullType = (byte)CircleUI.returned;
                },
                needsSaving: true
            ),
                new CircleUIHandler(
                triggerItem: AssUtils.Instance.ItemType<VanitySelector>(),
                condition: delegate
                {
                    return YoungWyvern;
                },
                uiConf: delegate
                {
                    List<string> tooltips = new List<string>() { "Default", "Mythical", "Arch", "Arch (Legacy)" };

                    return CircleUIHandler.PetConf("YoungWyvern", tooltips);
                },
                onUIStart: delegate
                {
                    return youngWyvernType;
                },
                onUIEnd: delegate
                {
                    youngWyvernType = (byte)CircleUI.returned;
                },
                needsSaving: true
            ),
                new CircleUIHandler(
                triggerItem: AssUtils.Instance.ItemType<VanitySelector>(),
                condition: delegate
                {
                    return PetFishron;
                },
                uiConf: delegate
                {
                    List<string> tooltips = new List<string>() { "Default", "Sharkron", "Sharknado" };

                    return CircleUIHandler.PetConf("PetFishronProj", tooltips);
                },
                onUIStart: delegate
                {
                    return petFishronType;
                },
                onUIEnd: delegate
                {
                    petFishronType = (byte)CircleUI.returned;
                },
                needsSaving: true
            ),
                new CircleUIHandler(
                triggerItem: AssUtils.Instance.ItemType<VanitySelector>(),
                condition: delegate
                {
                    return PetMoon;
                },
                uiConf: delegate
                {
                    List<string> tooltips = new List<string>() { "Default", "Orange", "Green" }; //only 0, 1, 2 registered, 3 and 4 are event related

                    return CircleUIHandler.PetConf("PetMoonProj", tooltips);
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
                needsSaving: true
            ),
                new CircleUIHandler(
                triggerItem: AssUtils.Instance.ItemType<VanitySelector>(),
                condition: delegate
                {
                    return YoungHarpy;
                },
                uiConf: delegate
                {
                    List<string> tooltips = new List<string>() { "Default", "Eagle", "Raven", "Dove" };

                    return CircleUIHandler.PetConf("YoungHarpy", tooltips);
                },
                onUIStart: delegate
                {
                    return youngHarpyType;
                },
                onUIEnd: delegate
                {
                    youngHarpyType = (byte)CircleUI.returned;
                },
                needsSaving: true
            ),
                new CircleUIHandler(
                triggerItem: AssUtils.Instance.ItemType<VanitySelector>(),
                condition: delegate
                {
                    return Abeemination;
                },
                uiConf: delegate
                {
                    List<string> tooltips = new List<string>() { "Default", "Snow Bee", "Oil Spill", "Missing Ingredients" };

                    return CircleUIHandler.PetConf("AbeeminationProj", tooltips);
                },
                onUIStart: delegate
                {
                    return abeeminationType;
                },
                onUIEnd: delegate
                {
                    abeeminationType = (byte)CircleUI.returned;
                },
                needsSaving: true
            ),
                new CircleUIHandler(
                triggerItem: AssUtils.Instance.ItemType<VanitySelector>(),
                condition: delegate
                {
                    return LilWraps;
                },
                uiConf: delegate
                {
                    List<string> tooltips = new List<string>() { "Default", "Dark", "Light", "Shadow", "Spectral" };

                    return CircleUIHandler.PetConf("LilWrapsProj", tooltips);
                },
                onUIStart: delegate
                {
                    return lilWrapsType;
                },
                onUIEnd: delegate
                {
                    lilWrapsType = (byte)CircleUI.returned;
                },
                needsSaving: true
            ),
                new CircleUIHandler(
                triggerItem: AssUtils.Instance.ItemType<VanitySelector>(),
                condition: delegate
                {
                    return VampireBat;
                },
                uiConf: delegate
                {
                    List<string> tooltips = new List<string>() { "Default", "Werebat" };

                    return CircleUIHandler.PetConf("VampireBat", tooltips);
                },
                onUIStart: delegate
                {
                    return vampireBatType;
                },
                onUIEnd: delegate
                {
                    vampireBatType = (byte)CircleUI.returned;
                },
                needsSaving: true
            ),
                new CircleUIHandler(
                triggerItem: AssUtils.Instance.ItemType<VanitySelector>(),
                condition: delegate
                {
                    return Pigronata;
                },
                uiConf: delegate
                {
                    List<string> tooltips = new List<string>() { "Default", "Winter", "Autumn", "Spring", "Summer", "Halloween", "Christmas" };

                    return CircleUIHandler.PetConf("Pigronata", tooltips);
                },
                onUIStart: delegate
                {
                    return pigronataType;
                },
                onUIEnd: delegate
                {
                    pigronataType = (byte)CircleUI.returned;
                },
                needsSaving: true
            ),
                new CircleUIHandler(
                triggerItem: AssUtils.Instance.ItemType<VanitySelector>(),
                condition: delegate
                {
                    return QueenLarva;
                },
                uiConf: delegate
                {
                    List<string> tooltips = new List<string>() { "Default", "Prawn Larva", "Unexpected Seed", "Big Kid Larva", "Where's The Baby?" };

                    return CircleUIHandler.PetConf("QueenLarvaProj", tooltips);
                },
                onUIStart: delegate
                {
                    return queenLarvaType;
                },
                onUIEnd: delegate
                {
                    queenLarvaType = (byte)CircleUI.returned;
                },
                needsSaving: true
            ),
                new CircleUIHandler(
                triggerItem: AssUtils.Instance.ItemType<VanitySelector>(),
                condition: delegate
                {
                    return OceanSlime;
                },
                uiConf: delegate
                {
                    List<string> tooltips = new List<string>() { "Default", "Stupid Hat", "Gnarly Grin", "Flipped Jelly" };

                    return CircleUIHandler.PetConf("OceanSlimeProj", tooltips);
                },
                onUIStart: delegate
                {
                    return oceanSlimeType;
                },
                onUIEnd: delegate
                {
                    oceanSlimeType = (byte)CircleUI.returned;
                },
                needsSaving: true
            ),
                new CircleUIHandler(
                triggerItem: AssUtils.Instance.ItemType<VanitySelector>(),
                condition: delegate
                {
                    return MiniAntlion;
                },
                uiConf: delegate
                {
                    List<string> tooltips = new List<string>() { "Default", "Albino" };

                    return CircleUIHandler.PetConf("MiniAntlionProj", tooltips);
                },
                onUIStart: delegate
                {
                    return miniAntlionType;
                },
                onUIEnd: delegate
                {
                    miniAntlionType = (byte)CircleUI.returned;
                },
                needsSaving: true
            ),
                new CircleUIHandler(
                triggerItem: AssUtils.Instance.ItemType<VanitySelector>(),
                condition: delegate
                {
                    return PetGoldfish;
                },
                uiConf: delegate
                {
                    List<string> tooltips = new List<string>() { "Default", "Crimson", "Corruption", "Bunny" };

                    return CircleUIHandler.PetConf("PetGoldfishProj", tooltips);
                },
                onUIStart: delegate
                {
                    return petGoldfishType;
                },
                onUIEnd: delegate
                {
                    petGoldfishType = (byte)CircleUI.returned;
                },
                needsSaving: true
            ),
                new CircleUIHandler(
                triggerItem: AssUtils.Instance.ItemType<VanitySelector>(),
                condition: delegate
                {
                    return SkeletronHand;
                },
                uiConf: delegate
                {
                    List<string> tooltips = new List<string>() { "Default", "OK-Hand", "Peace", "Rock It", "Fist" };

                    return CircleUIHandler.PetConf("SkeletronHandProj", tooltips);
                },
                onUIStart: delegate
                {
                    return skeletronHandType;
                },
                onUIEnd: delegate
                {
                    skeletronHandType = (byte)CircleUI.returned;
                },
                needsSaving: true
            ),
                new CircleUIHandler(
                triggerItem: AssUtils.Instance.ItemType<VanitySelector>(),
                condition: delegate
                {
                    return SkeletronPrimeHand;
                },
                uiConf: delegate
                {
                    List<string> tooltips = new List<string>() { "Cannon", "Saw", "Vice", "Laser" };

                    return CircleUIHandler.PetConf("SkeletronPrimeHandProj", tooltips);
                },
                onUIStart: delegate
                {
                    return skeletronPrimeHandType;
                },
                onUIEnd: delegate
                {
                    skeletronPrimeHandType = (byte)CircleUI.returned;
                },
                needsSaving: true
            ),
                new CircleUIHandler(
                triggerItem: AssUtils.Instance.ItemType<VanitySelector>(),
                condition: delegate
                {
                    return PetCultist;
                },
                uiConf: delegate
                {
                    List<string> tooltips = new List<string>() { "Lunar", "Solar" };

                    return CircleUIHandler.PetConf("PetCultistProj", tooltips);
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
                needsSaving: true
            ),
                new CircleUIHandler(
                triggerItem: AssUtils.Instance.ItemType<VanitySelector>(),
                condition: delegate
                {
                    return AnimatedTome;
                },
                uiConf: delegate
                {
                    List<string> tooltips = new List<string>() { "Green", "Blue", "Purple", "Pink", "Yellow" };

                    return CircleUIHandler.PetConf("AnimatedTomeProj", tooltips);
                },
                onUIStart: delegate
                {
                    return animatedTomeType;
                },
                onUIEnd: delegate
                {
                    animatedTomeType = (byte)CircleUI.returned;
                },
                needsSaving: true
            ),
                new CircleUIHandler(
                triggerItem: AssUtils.Instance.ItemType<VanitySelector>(),
                condition: delegate
                {
                    return AnimatedTome;
                },
                uiConf: delegate
                {
                    List<string> tooltips = new List<string>() { "Green", "Blue", "Purple", "Pink", "Yellow" };

                    return CircleUIHandler.PetConf("AnimatedTomeProj", tooltips);
                },
                onUIStart: delegate
                {
                    return animatedTomeType;
                },
                onUIEnd: delegate
                {
                    animatedTomeType = (byte)CircleUI.returned;
                },
                needsSaving: true
            ),
            //ALTERNATE
            //    new CircleUIHandler(
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
            //    triggerLeft: true
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
        public void GetFromClonedTypes(string mp = "")
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
                //ALTERNATE
                //ClonedTypes[++index] = classNameType;
            }
        }

        #endregion
    }
}