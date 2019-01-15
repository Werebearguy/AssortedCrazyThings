using AssortedCrazyThings.Items.PetAccessories;
using AssortedCrazyThings.Projectiles.Pets;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Terraria;
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

        //Soul item animated textures
        public static Texture2D[] animatedTextureArray;

        private void InitPets()
        {
            slimePetLegacy[0] = ProjectileType<CuteSlimeBlackPet>();
            slimePetLegacy[1] = ProjectileType<CuteSlimeBluePet>();
            slimePetLegacy[2] = ProjectileType<CuteSlimeGreenPet>();
            slimePetLegacy[3] = ProjectileType<CuteSlimePinkPet>();
            slimePetLegacy[4] = ProjectileType<CuteSlimePurplePet>();
            slimePetLegacy[5] = ProjectileType<CuteSlimeRainbowPet>();
            slimePetLegacy[6] = ProjectileType<CuteSlimeRedPet>();
            slimePetLegacy[7] = ProjectileType<CuteSlimeXmasPet>();
            slimePetLegacy[8] = ProjectileType<CuteSlimeYellowPet>();

            if (!Main.dedServ && Main.netMode != 2)
            {
                PetAccessory.Load(this);
            }
        }

        public override void Load()
        {
            InitPets();

            if (!Main.dedServ && Main.netMode != 2)
            {
                animatedTextureArray = new Texture2D[2];

                animatedTextureArray[0] = GetTexture("Items/CaughtDungeonSoulAnimated");
                animatedTextureArray[1] = GetTexture("Items/CaughtDungeonSoulFreedAnimated");
            }
        }

        public override void Unload()
        {
            if (!Main.dedServ && Main.netMode != 2)
            {
                PetAccessory.Unload();
                animatedTextureArray = null;
            }
        }

        public override void HandlePacket(BinaryReader reader, int whoAmI)
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

    enum AssMessageType : byte
    {
        RedrawPetAccessories,
        SendClientChanges,
        SyncPlayer
    }
}
