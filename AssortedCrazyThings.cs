using AssortedCrazyThings.Items.PetAccessories;
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



            slimeAccessoryTextures = new Texture2D[30];
            slimeAccessoryOffsets = new Vector2[30];
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
            //add more here, for example like this:
            //slimeAccessoryItems[itemIndex++] = mod.ItemType<PetAccessoryStrapOn>();

            Array.Resize(ref slimeAccessoryItems, itemIndex);

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
                //for every new line, just add the new items class name in the <> and then the texture with _Draw in the ""

                //ErrorLogger.Log(slimeAccessoryItemsIndexed.Length + " " + ItemType<PetAccessoryXmasHat>() + " " + slimeAccessoryItemsIndexed[ItemType<PetAccessoryXmasHat>()]);
                //ErrorLogger.Log(slimeAccessoryTextures.Length);
                //ErrorLogger.Log(GetTexture("Items/PetAccessories/PetAccessoryXmasHat_Draw"));
                slimeAccessoryTextures[slimeAccessoryItemsIndexed[ItemType<PetAccessoryBow>()]] = GetTexture("Items/PetAccessories/PetAccessoryBow_Draw");
                slimeAccessoryTextures[slimeAccessoryItemsIndexed[ItemType<PetAccessoryXmasHat>()]] = GetTexture("Items/PetAccessories/PetAccessoryXmasHat_Draw");
                slimeAccessoryTextures[slimeAccessoryItemsIndexed[ItemType<PetAccessoryBowGreen>()]] = GetTexture("Items/PetAccessories/PetAccessoryBowGreen_Draw");
                slimeAccessoryTextures[slimeAccessoryItemsIndexed[ItemType<PetAccessoryBowBlack>()]] = GetTexture("Items/PetAccessories/PetAccessoryBowBlack_Draw");
                slimeAccessoryTextures[slimeAccessoryItemsIndexed[ItemType<PetAccessoryBowBlue>()]] = GetTexture("Items/PetAccessories/PetAccessoryBowBlue_Draw");
                //ErrorLogger.Log(slimeAccessoryTextures[slimeAccessoryItemsIndexed[ItemType<PetAccessoryXmasHat>()]]);


                //------------------------------------------------------------------------------------------------------
                //-----------slimeAccessoryOffsets----------------------------------------------------------------------
                //------------------------------------------------------------------------------------------------------
                slimeAccessoryOffsets[slimeAccessoryItemsIndexed[ItemType<PetAccessoryBow>()]] = new Vector2(0f, 0f);
                slimeAccessoryOffsets[slimeAccessoryItemsIndexed[ItemType<PetAccessoryXmasHat>()]] = new Vector2(0f, -13f);
                slimeAccessoryOffsets[slimeAccessoryItemsIndexed[ItemType<PetAccessoryBowGreen>()]] = new Vector2(0f, 0f);
                slimeAccessoryOffsets[slimeAccessoryItemsIndexed[ItemType<PetAccessoryBowBlack>()]] = new Vector2(0f, 0f);
                slimeAccessoryOffsets[slimeAccessoryItemsIndexed[ItemType<PetAccessoryBowBlue>()]] = new Vector2(0f, 0f);


                //finishing up, ignore
                Array.Resize(ref slimeAccessoryTextures, slimeAccessoryItems.Length + 1); //since index starts at 1
                Array.Resize(ref slimeAccessoryOffsets, slimeAccessoryItems.Length + 1); //since index starts at 1
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
        }

        public override void Unload()
        {
            if (!Main.dedServ && Main.netMode != 2)
            {
                slimeAccessoryTextures = null;
                slimeAccessoryOffsets = null;
            }
        }

        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {

            AssMessageType msgType = (AssMessageType)reader.ReadByte();
            byte playernumber;
            switch (msgType)
            {
                case AssMessageType.PetAccessorySlots:
                    if (Main.netMode == NetmodeID.Server)
                    {
                        //Console.WriteLine("recieved a PetAccessorySlots packet");
                    }
                    playernumber = reader.ReadByte();
                    Player tempPlayer = Main.player[playernumber];
                    uint slotsPlayer = (uint)reader.ReadInt32();
                    uint slotsPlayerLast = (uint)reader.ReadInt32();
                    AssPlayer mPlayer = tempPlayer.GetModPlayer<AssPlayer>();
                    if (Main.netMode == NetmodeID.Server)
                    {
                        //Console.WriteLine(" " + tempPlayer.name + " slots " + slotsPlayer);
                    }
                    mPlayer.slotsPlayer = slotsPlayer;
                    break;
                default:
                    ErrorLogger.Log("AssortedCrazyThings: Unknown Message type: " + msgType);
                    break;
            }
        }
    }

    enum AssMessageType : byte
    {
        PetAccessorySlots,
    }
}
