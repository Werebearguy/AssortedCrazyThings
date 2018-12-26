using AssortedCrazyThings.Items.PetAccessories;
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
        public static int[] slimeAccessoryItems = new int[100];
        public static Texture2D[] slimeAccessoryTextures = new Texture2D[100];
        public static int[] slimeAccessoryItemsIndexed;

        private void InitPetAccessories()
        {
            /* Here you add the items from PetAccessories in two arrays,
             * one is the slimeAccessoryItems one (mainly for searching when applying the accessories)
             * the other one is the texture array, follow the same pattern (this is for taking the texture in each draw call)
             * 
             */


            //------------------------------------------------------------------------------------------------------
            //------------------------------------------------------------------------------------------------------
            //------------------------------------------------------------------------------------------------------
            //ive set the limit to 100 different accessories for now, we can expand that later
            //(check definition of slimeAccessoryItems)
            int itemIndex = 0;
            slimeAccessoryItems[itemIndex++] = ItemType<PetAccessoryBow>();
            slimeAccessoryItems[itemIndex++] = ItemType<PetAccessoryXmasHat>();
            slimeAccessoryItems[itemIndex++] = ItemType<PetAccessoryBowGreen>();
            slimeAccessoryItems[itemIndex++] = ItemType<PetAccessoryBowYellow>();
            slimeAccessoryItems[itemIndex++] = ItemType<PetAccessoryBowBlue>();
            //add more here, for example like this:
            //slimeAccessoryItems[itemIndex++] = mod.ItemType<PetAccessoryStrapOn>();

            Array.Resize(ref slimeAccessoryItems, itemIndex);

            int[] parameters = new int[slimeAccessoryItems.Length * 2];
            for (int i = 0; i < slimeAccessoryItems.Length; i++)
            {
                parameters[2 * i] = slimeAccessoryItems[i];
                parameters[2 * i + 1] = i + 1;
            }
            slimeAccessoryItemsIndexed = IntSet(parameters);
            //-> slimeAccessoryItemsIndexed[mod.ItemType<PetAccessoryXmasHat>()] returns 2

            //------------------------------------------------------------------------------------------------------
            //------------------------------------------------------------------------------------------------------
            //------------------------------------------------------------------------------------------------------
            slimeAccessoryTextures[slimeAccessoryItemsIndexed[ItemType<PetAccessoryBow>()]] = GetTexture("Items/PetAccessories/PetAccessoryBow_Draw");
            slimeAccessoryTextures[slimeAccessoryItemsIndexed[ItemType<PetAccessoryXmasHat>()]] = GetTexture("Items/PetAccessories/PetAccessoryXmasHat_Draw");
            slimeAccessoryTextures[slimeAccessoryItemsIndexed[ItemType<PetAccessoryBowGreen>()]] = GetTexture("Items/PetAccessories/PetAccessoryBowGreen_Draw");
            slimeAccessoryTextures[slimeAccessoryItemsIndexed[ItemType<PetAccessoryBowYellow>()]] = GetTexture("Items/PetAccessories/PetAccessoryBowYellow_Draw");
            slimeAccessoryTextures[slimeAccessoryItemsIndexed[ItemType<PetAccessoryBowBlue>()]] = GetTexture("Items/PetAccessories/PetAccessoryBowBlue_Draw");

            //for every new line, just add the new items class name in the <> and then the texture with _Draw in the ""

            //finishing up, ignore
            Array.Resize(ref slimeAccessoryTextures, itemIndex + 1); //since index starts at 1
        }

        public int[] IntSet(int[] inputs)
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
            if (!Main.dedServ)
            {
                InitPetAccessories();
            }
        }

        public override void Unload()
        {
            if (!Main.dedServ)
            {
                slimeAccessoryTextures = null;
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
