using AssortedCrazyThings.Items.PetAccessories;
using AssortedCrazyThings.NPCs;
using AssortedCrazyThings.Projectiles.Pets;
using System;
using System.Collections.Generic;
using Terraria;

namespace AssortedCrazyThings
{
    public static class SlimePets
    {
        internal static List<SlimePet> slimePetList = new List<SlimePet>();

        public static List<int> slimePets = new List<int>(); //slimePets.IndexOf(type) returns the indexed type
        public static List<int> slimePetLegacy = new List<int>();
        public static List<int> slimePetNPCs = new List<int>();

        public static void Load()
        {
            //in all these lists, insert stuff in alphabetic order please

            //legacy, no need to adjust
            slimePetLegacy.AddRange(new List<int>
            {
                AssUtils.Instance.ProjectileType<CuteSlimeBlackProj>(),
                AssUtils.Instance.ProjectileType<CuteSlimeBlueProj>(),
                AssUtils.Instance.ProjectileType<CuteSlimeGreenProj>(),
                AssUtils.Instance.ProjectileType<CuteSlimePinkProj>(),
                AssUtils.Instance.ProjectileType<CuteSlimePurpleProj>(),
                AssUtils.Instance.ProjectileType<CuteSlimeRainbowProj>(),
                AssUtils.Instance.ProjectileType<CuteSlimeRedProj>(),
                AssUtils.Instance.ProjectileType<CuteSlimeXmasProj>(),
                AssUtils.Instance.ProjectileType<CuteSlimeYellowProj>(),
            });

            //for adding new slime NPCs
            slimePetNPCs.AddRange(new List<int>
            {
                AssUtils.Instance.NPCType<CuteSlimeBlack>(),
                AssUtils.Instance.NPCType<CuteSlimeBlue>(),
                AssUtils.Instance.NPCType<CuteSlimeCrimson>(),
                AssUtils.Instance.NPCType<CuteSlimeCorrupt>(),
                AssUtils.Instance.NPCType<CuteSlimeDungeon>(),
                AssUtils.Instance.NPCType<CuteSlimeGreen>(),
                AssUtils.Instance.NPCType<CuteSlimeIce>(),
                AssUtils.Instance.NPCType<CuteSlimePink>(),
                AssUtils.Instance.NPCType<CuteSlimePurple>(),
                AssUtils.Instance.NPCType<CuteSlimeRainbow>(),
                AssUtils.Instance.NPCType<CuteSlimeRed>(),
                AssUtils.Instance.NPCType<CuteSlimeToxic>(),
                AssUtils.Instance.NPCType<CuteSlimeXmas>(),
                AssUtils.Instance.NPCType<CuteSlimeYellow>()
            });

            //start list
            slimePetList.Add(SlimePet.NewSlimePet
            (
                name: "CuteSlimeBlackNewProj",
                color: PetColor.Black,
                hasNoHair: true
            ));
            slimePetList.Add(SlimePet.NewSlimePet
            (
                name: "CuteSlimeBlueNewProj",
                color: PetColor.Blue,
                hasNoHair: true
            ));
            slimePetList.Add(SlimePet.NewSlimePet
            (
                name: "CuteSlimeCorruptNewProj",
                color: PetColor.Corrupt,
                hasNoHair: true
            ));
            slimePetList.Add(SlimePet.NewSlimePet
            (
                name: "CuteSlimeCrimsonNewProj",
                color: PetColor.Crimson,
                hasNoHair: true
            ));
            slimePetList.Add(SlimePet.NewSlimePet
            (
                name: "CuteSlimeDungeonNewProj",
                color: PetColor.Dungeon,
                hasNoHair: true
            ));
            slimePetList.Add(SlimePet.NewSlimePet
            (
                name: "CuteSlimeGreenNewProj",
                color: PetColor.Green
            ));
            slimePetList.Add(SlimePet.NewSlimePet
            (
                name: "CuteSlimeIceNewProj",
                color: PetColor.Ice
            ));
            slimePetList.Add(SlimePet.NewSlimePet
            (
                name: "CuteSlimePurpleNewProj",
                color: PetColor.Purple,
                hasNoHair: true
            ));
            slimePetList.Add(SlimePet.NewSlimePet
            (
                name: "CuteSlimePinkNewProj",
                color: PetColor.Pink,
                hasNoHair: true
            ));
            slimePetList.Add(SlimePet.NewSlimePet
            (
                name: "CuteSlimeRainbowNewProj",
                color: PetColor.Rainbow
            ));
            slimePetList.Add(SlimePet.NewSlimePet
            (
                name: "CuteSlimeRedNewProj",
                color: PetColor.Red,
                hasNoHair: true
            ));
            slimePetList.Add(SlimePet.NewSlimePet
            (
                name: "CuteSlimeToxicNewProj",
                color: PetColor.Toxic
            ));
            slimePetList.Add(SlimePet.NewSlimePet
            (
                name: "CuteSlimeXmasNewProj",
                color: PetColor.Xmas,
                hasNoHair: true,
                postAdditionSlot: (byte)SlotType.Body,
                carried: true,
                accessory: true
            ));
            slimePetList.Add(SlimePet.NewSlimePet
            (
                name: "CuteSlimeYellowNewProj",
                color: PetColor.Yellow,
                hasNoHair: true
            ));

            /*
            slimePetList.Add(SlimePet.NewSlimePet
            (
                name: "CuteSlimeColorNewProj",
                color: PetColor.Color,
                hasNoHair: false,
                preAdditionSlot: (byte)SlotType.None,
                postAdditionSlot: (byte)SlotType.None,
                body: false,
                hat: false,
                carried: false,
                accessory: false
                //last entry here needs to have no comma ","
            ));
            
             * TEMPLATE EXPLANATION:
             * name: name of the projectile class
             * color: enum PetColor, specified at the end of this file
             * hasNoHair: if true, makes it so it uses the NoHair texture for a Hat accessory if specified
             * preAdditionSlot: SlotType that causes the Addition texture to not be drawn when equipped (used for drawing behind the pet)
             * postAdditionSlot: SlotType that causes the Addition texture to not be drawn when equipped (used for drawing infront of the pet) (Example: Xmas)
             * //Note: don't bother including those two, ask me instead if there is an "Addition" that needs to be drawn
             * body, hat etc: if true, makes it so said accessory type can't be equipped and won't be drawn on the pet
             *
             * The things listed there are defaults (besides the name and color), so you can omit those if the pet is basic
             */

            //end list, don't write anything new after this line

            CreateMap();
        }

        public static void CreateMap()
        {
            slimePets = new List<int>(slimePetList.Count);
            for (int i = 0; i < slimePetList.Count; i++)
            {
                slimePets.Add(slimePetList[i].Type);
            }
        }

        public static SlimePet GetPet(int type)
        {
            return slimePetList[slimePets.IndexOf(type)];
        }
    }

    public class SlimePet
    {
        public string Name { private set; get; }
        public int Type { private set; get; }
        public PetColor Color { private set; get; }
        public bool HasNoHair { private set; get; }
        public byte PreAdditionSlot { private set; get; }
        public byte PostAdditionSlot { private set; get; }
        public bool[] IsSlotTypeBlacklisted { private set; get; }

        public SlimePet(string name, PetColor color, bool hasNoHair = false, byte preAdditionSlot = 0, byte postAdditionSlot = 0, List<bool> isSlotTypeBlacklisted = null)
        {
            Name = name;
            Type = AssUtils.Instance.ProjectileType(name);
            Color = color;
            HasNoHair = hasNoHair;
            PreAdditionSlot = preAdditionSlot;
            PostAdditionSlot = postAdditionSlot;

            if (isSlotTypeBlacklisted != null)
            {
                if (isSlotTypeBlacklisted.Count != Enum.GetValues(typeof(SlotType)).Length - 1)
                    throw new Exception("isSlotTypeBlacklisted is longer than the amount of accessory slot types");
                else
                {
                    IsSlotTypeBlacklisted = new bool[5];
                    IsSlotTypeBlacklisted[0] = false; //maybe for later
                    IsSlotTypeBlacklisted[1] = isSlotTypeBlacklisted[0];
                    IsSlotTypeBlacklisted[2] = isSlotTypeBlacklisted[1];
                    IsSlotTypeBlacklisted[3] = isSlotTypeBlacklisted[2];
                    IsSlotTypeBlacklisted[4] = isSlotTypeBlacklisted[3];
                }
            }
            else
            {
                IsSlotTypeBlacklisted = new bool[5]; //all false
            }
        }

        public static SlimePet NewSlimePet(string name, PetColor color, bool hasNoHair = false, byte preAdditionSlot = 0, byte postAdditionSlot = 0,
            bool body = false, bool hat = false, bool carried = false, bool accessory = false)
        {
            List<bool> isSlotTypeBlacklisted = new List<bool>(){ body, hat, carried, accessory };

            return new SlimePet(name, color, hasNoHair, preAdditionSlot, postAdditionSlot, isSlotTypeBlacklisted);
        }

        public override string ToString()
        {
            return "Name: " + Name
                +"; Type: " + Type
                + "; HasNoHair: " + (HasNoHair? "y":"n")
                + "; PreAdditionSlot: " + ((SlotType)PreAdditionSlot).ToString()
                + "; PostAdditionSlot: " + ((SlotType)PostAdditionSlot).ToString();
        }
    }

    //add a new color in alphabetic order (same in PetAccessoryClass.AddAltTextures, in the signature and in the intArray inside
    public enum PetColor : byte
    {
        // I REPEAT: ALPHABETIC ORDER
        Black,
        Blue,
        Corrupt,
        Crimson,
        Dungeon,
        Green,
        Ice,
        Pink,
        Purple,
        Rainbow,
        Red,
        Toxic,
        Xmas,
        Yellow
    }
}
