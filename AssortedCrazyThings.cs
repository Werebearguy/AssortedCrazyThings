using AssortedCrazyThings.Items.PetAccessories;
using AssortedCrazyThings.NPCs.DungeonBird;
using AssortedCrazyThings.Projectiles.Pets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
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

        //Slime textures n shiet
        public static int[] slimeAccessoryItems = new int[30];
        public static int[] slimePetLegacy = new int[9];
        //these two are only accessed locally (not on a server)
        public static int[] slimeAccessoryItemsIndexed;
        public static Texture2D[] slimeAccessoryTextures;
        public static Vector2[] slimeAccessoryOffsets;
        public static bool[] slimeAccessoryPreDraw;
        public static byte[] slimeAccessoryAlpha;

        //Soul item animated textures
        public static Texture2D[] animatedTextureArray;

        private void InitPetAccessories()
        {
            //ignore this
            slimePetLegacy[0] = ProjectileType<CuteSlimeBlackPet>();
            slimePetLegacy[1] = ProjectileType<CuteSlimeBluePet>();
            slimePetLegacy[2] = ProjectileType<CuteSlimeGreenPet>();
            slimePetLegacy[3] = ProjectileType<CuteSlimePinkPet>();
            slimePetLegacy[4] = ProjectileType<CuteSlimePurplePet>();
            slimePetLegacy[5] = ProjectileType<CuteSlimeRainbowPet>();
            slimePetLegacy[6] = ProjectileType<CuteSlimeRedPet>();
            slimePetLegacy[7] = ProjectileType<CuteSlimeXmasPet>();
            slimePetLegacy[8] = ProjectileType<CuteSlimeYellowPet>();
            //



            /* Here you add the items from PetAccessories in two arrays,
            * one is the slimeAccessoryItems one (mainly for searching when applying the accessories)
            * the other one is the texture array, follow the same pattern (this is for taking the texture in each draw call)
            * the last one is the offset array, you can leave it as 0,0 if there is none
            */


            //------------------------------------------------------------------------------------------------------
            //------------------slimeAccessoryItems-----------------------------------------------------------------
            //------------------------------------------------------------------------------------------------------
            //ive set the limit to 30 different accessories for now, we can expand that later
            //(check definition of slimeAccessoryItems)
            int itemIndex = 0;
            slimeAccessoryItems[itemIndex++] = ItemType<PetAccessoryBow>();
            slimeAccessoryItems[itemIndex++] = ItemType<PetAccessoryXmasHat>();
            slimeAccessoryItems[itemIndex++] = ItemType<PetAccessoryBowGreen>();
            slimeAccessoryItems[itemIndex++] = ItemType<PetAccessoryBowBlack>();
            slimeAccessoryItems[itemIndex++] = ItemType<PetAccessoryBowBlue>();
            slimeAccessoryItems[itemIndex++] = ItemType<PetAccessoryAmethystStaff>();
            slimeAccessoryItems[itemIndex++] = ItemType<PetAccessoryTopazStaff>();
            slimeAccessoryItems[itemIndex++] = ItemType<PetAccessorySlimeHead>();

            //add more here, for example like this:
            //slimeAccessoryItems[itemIndex++] = mod.ItemType<PetAccessoryStrapOn>();

            slimeAccessoryTextures = new Texture2D[itemIndex + 1];
            slimeAccessoryOffsets = new Vector2[itemIndex + 1];
            slimeAccessoryPreDraw = new bool[itemIndex + 1];
            slimeAccessoryAlpha = new byte[itemIndex + 1];

            //Array.Resize(ref slimeAccessoryItems, itemIndex);

            if (!Main.dedServ && Main.netMode != 2)
            {
                int[] parameters = new int[slimeAccessoryItems.Length * 2];
                for (int i = 0; i < slimeAccessoryItems.Length; i++)
                {
                    parameters[2 * i] = slimeAccessoryItems[i];
                    parameters[2 * i + 1] = i + 1;
                }
                slimeAccessoryItemsIndexed = IntSet(parameters);
                //-> slimeAccessoryItemsIndexed[mod.ItemType<PetAccessoryXmasHat>()] returns 2

                //------------------------------------------------------------------------------------------------------
                //------------slimeAccessoryTextures--------------------------------------------------------------------
                //------------------------------------------------------------------------------------------------------
                //for every new line, just add the new items class name in the <> and then the class name into the middle string

                slimeAccessoryTextures[slimeAccessoryItemsIndexed[ItemType<PetAccessoryBow>()]] = GetTexture("Items/PetAccessories/" + "PetAccessoryBow" + "_Draw");
                slimeAccessoryTextures[slimeAccessoryItemsIndexed[ItemType<PetAccessoryXmasHat>()]] = GetTexture("Items/PetAccessories/" + "PetAccessoryXmasHat" + "_Draw");
                slimeAccessoryTextures[slimeAccessoryItemsIndexed[ItemType<PetAccessoryBowGreen>()]] = GetTexture("Items/PetAccessories/" + "PetAccessoryBowGreen" + "_Draw");
                slimeAccessoryTextures[slimeAccessoryItemsIndexed[ItemType<PetAccessoryBowBlack>()]] = GetTexture("Items/PetAccessories/" + "PetAccessoryBowBlack" + "_Draw");
                slimeAccessoryTextures[slimeAccessoryItemsIndexed[ItemType<PetAccessoryBowBlue>()]] = GetTexture("Items/PetAccessories/" + "PetAccessoryBowBlue" + "_Draw");
                slimeAccessoryTextures[slimeAccessoryItemsIndexed[ItemType<PetAccessoryAmethystStaff>()]] = GetTexture("Items/PetAccessories/" + "PetAccessoryAmethystStaff" + "_Draw");
                slimeAccessoryTextures[slimeAccessoryItemsIndexed[ItemType<PetAccessoryTopazStaff>()]] = GetTexture("Items/PetAccessories/" + "PetAccessoryTopazStaff" + "_Draw");
                slimeAccessoryTextures[slimeAccessoryItemsIndexed[ItemType<PetAccessorySlimeHead>()]] = GetTexture("Items/PetAccessories/" + "PetAccessorySlimeHead" + "_Draw");


                //------------------------------------------------------------------------------------------------------
                //-----------slimeAccessoryOffsets----------------------------------------------------------------------
                //------------------------------------------------------------------------------------------------------
                //the offset is mainly only for the first frame of the spritesheet, everything else is adjusted automatically
                slimeAccessoryOffsets[slimeAccessoryItemsIndexed[ItemType<PetAccessoryBow>()]] = new Vector2(0f, 0f);
                slimeAccessoryOffsets[slimeAccessoryItemsIndexed[ItemType<PetAccessoryXmasHat>()]] = new Vector2(0f, -13f);
                slimeAccessoryOffsets[slimeAccessoryItemsIndexed[ItemType<PetAccessoryBowGreen>()]] = new Vector2(0f, 0f);
                slimeAccessoryOffsets[slimeAccessoryItemsIndexed[ItemType<PetAccessoryBowBlack>()]] = new Vector2(0f, 0f);
                slimeAccessoryOffsets[slimeAccessoryItemsIndexed[ItemType<PetAccessoryBowBlue>()]] = new Vector2(0f, 0f);
                slimeAccessoryOffsets[slimeAccessoryItemsIndexed[ItemType<PetAccessoryAmethystStaff>()]] = new Vector2(-14f, 0f);
                slimeAccessoryOffsets[slimeAccessoryItemsIndexed[ItemType<PetAccessoryTopazStaff>()]] = new Vector2(-24f, 0f);
                slimeAccessoryOffsets[slimeAccessoryItemsIndexed[ItemType<PetAccessorySlimeHead>()]] = new Vector2(0f, -18f);

                slimeAccessoryPreDraw[slimeAccessoryItemsIndexed[ItemType<PetAccessoryBow>()]] = false;
                slimeAccessoryPreDraw[slimeAccessoryItemsIndexed[ItemType<PetAccessoryXmasHat>()]] = false;
                slimeAccessoryPreDraw[slimeAccessoryItemsIndexed[ItemType<PetAccessoryBowGreen>()]] = false;
                slimeAccessoryPreDraw[slimeAccessoryItemsIndexed[ItemType<PetAccessoryBowBlack>()]] = false;
                slimeAccessoryPreDraw[slimeAccessoryItemsIndexed[ItemType<PetAccessoryBowBlue>()]] = false;
                slimeAccessoryPreDraw[slimeAccessoryItemsIndexed[ItemType<PetAccessoryAmethystStaff>()]] = true;
                slimeAccessoryPreDraw[slimeAccessoryItemsIndexed[ItemType<PetAccessoryTopazStaff>()]] = true;
                slimeAccessoryPreDraw[slimeAccessoryItemsIndexed[ItemType<PetAccessorySlimeHead>()]] = false;

                slimeAccessoryAlpha[slimeAccessoryItemsIndexed[ItemType<PetAccessoryBow>()]] = 0; //255 fully opaque, 0 fully transparent
                slimeAccessoryAlpha[slimeAccessoryItemsIndexed[ItemType<PetAccessoryXmasHat>()]] = 0;
                slimeAccessoryAlpha[slimeAccessoryItemsIndexed[ItemType<PetAccessoryBowGreen>()]] = 0;
                slimeAccessoryAlpha[slimeAccessoryItemsIndexed[ItemType<PetAccessoryBowBlack>()]] = 0;
                slimeAccessoryAlpha[slimeAccessoryItemsIndexed[ItemType<PetAccessoryBowBlue>()]] = 0;
                slimeAccessoryAlpha[slimeAccessoryItemsIndexed[ItemType<PetAccessoryAmethystStaff>()]] = 0;
                slimeAccessoryAlpha[slimeAccessoryItemsIndexed[ItemType<PetAccessoryTopazStaff>()]] = 0;
                slimeAccessoryAlpha[slimeAccessoryItemsIndexed[ItemType<PetAccessorySlimeHead>()]] = 56;


                //finishing up, ignore
                //Array.Resize(ref slimeAccessoryTextures, slimeAccessoryItems.Length + 1); //since index starts at 1
                //Array.Resize(ref slimeAccessoryOffsets, slimeAccessoryItems.Length + 1); //since index starts at 1
            }
        }

        private int[] IntSet(int[] inputs)
        {
            //inputs.Length % 2 == 0
            int[] temp = new int[inputs.Length];
            Array.Copy(inputs, temp, inputs.Length);
            Array.Sort(temp); //highest index should hold the max value of inputs
            int[] ret = new int[temp[temp.Length - 1] + 1];//length == max value of inputs
            for (int i = 0; i < ret.Length; i++)
            {
                ret[i] = 0; //fill array with 0
            }
            for (int j = 0; j < inputs.Length; j += 2)
            {
                ret[inputs[j]] = inputs[j + 1]; //fill array with pair of key:value
            }
            return ret;
        }

        public override void Load()
        {
            InitPetAccessories();


            if (!Main.dedServ && Main.netMode != 2)
            {
                animatedTextureArray = new Texture2D[2];

                animatedTextureArray[0] = GetTexture("Items/CaughtDungeonSoulAnimated");
                animatedTextureArray[1] = GetTexture("Items/CaughtDungeonSoulAwakenedAnimated");
            }
        }

        public override void Unload()
        {
            if (!Main.dedServ && Main.netMode != 2)
            {
                slimeAccessoryTextures = null;
                slimeAccessoryOffsets = null;
                animatedTextureArray = null;
            }
        }

        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {

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
        }
    }

    enum AssMessageType : byte
    {
        RedrawPetAccessories,
        SendClientChanges,
        SyncPlayer
    }
}
