using AssortedCrazyThings.Projectiles;
using AssortedCrazyThings.Projectiles.Pets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.PetAccessories
{
    /*
     * read the readme.txt
     * For every new Texture you add, copypaste a new class in this namespace (below PetAccessories), and adjust its DisplayName and item.value.
     * item.value is the "SlotType" in our case.
     * (yes, this means that the accessories are worth almost nothing when sold, who cares lmao)
     * 
     * example:
     * item.value = (int)SlotType.Body;
     * 
     * finally, go into Load(Mod mod) (further down below), and follow the instructions there
     * 
     * 
     * suggestion for names : prefixed with "Cute ", so its easy to find in recipe browser 
     * inside CuteSlimeGlobalTooltip the "Cute " prefix will be cut off in the tooltip to save space
     * 
     */

    public class PetAccessoryBowtieBlack : PetAccessoryBase
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Black Bowtie");
            Tooltip.SetDefault("Something to decorate your cute slime with.");
        }

        protected override void MoreSetDefaults()
        {
            item.value = (int)SlotType.Body;
        }
    }

    public class PetAccessoryBowtieBlue : PetAccessoryBase
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Blue Bowtie");
            Tooltip.SetDefault("Something to decorate your cute slime with.");
        }

        protected override void MoreSetDefaults()
        {
            item.value = (int)SlotType.Body;
        }
    }

    public class PetAccessoryBowtieGray : PetAccessoryBase
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Gray Bowtie");
            Tooltip.SetDefault("Something to decorate your cute slime with.");
        }

        protected override void MoreSetDefaults()
        {
            item.value = (int)SlotType.Body;
        }
    }

    public class PetAccessoryBowtieGreen : PetAccessoryBase
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Green Bowtie");
            Tooltip.SetDefault("Something to decorate your cute slime with.");
        }

        protected override void MoreSetDefaults()
        {
            item.value = (int)SlotType.Body;
        }
    }

    public class PetAccessoryBowtieOrange : PetAccessoryBase
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Orange Bowtie");
            Tooltip.SetDefault("Something to decorate your cute slime with.");
        }

        protected override void MoreSetDefaults()
        {
            item.value = (int)SlotType.Body;
        }
    }

    public class PetAccessoryBowtiePink : PetAccessoryBase
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Pink Bowtie");
            Tooltip.SetDefault("Something to decorate your cute slime with.");
        }

        protected override void MoreSetDefaults()
        {
            item.value = (int)SlotType.Body;
        }
    }

    public class PetAccessoryBowtiePurple : PetAccessoryBase
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Purple Bowtie");
            Tooltip.SetDefault("Something to decorate your cute slime with.");
        }

        protected override void MoreSetDefaults()
        {
            item.value = (int)SlotType.Body;
        }
    }

    public class PetAccessoryBowtieRed : PetAccessoryBase
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Red Bowtie");
            Tooltip.SetDefault("Something to decorate your cute slime with.");
        }

        protected override void MoreSetDefaults()
        {
            item.value = (int)SlotType.Body;
        }
    }

    public class PetAccessoryBowtieWhite : PetAccessoryBase
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cute White Bowtie");
            Tooltip.SetDefault("Something to decorate your cute slime with.");
        }

        protected override void MoreSetDefaults()
        {
            item.value = (int)SlotType.Body;
        }
    }

    public class PetAccessoryBowtieYellow : PetAccessoryBase
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Yellow Bowtie");
            Tooltip.SetDefault("Something to decorate your cute slime with.");
        }

        protected override void MoreSetDefaults()
        {
            item.value = (int)SlotType.Body;
        }
    }

    public class PetAccessoryCrownGold : PetAccessoryBase
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Gold Crown");
            Tooltip.SetDefault("Something to decorate your cute slime with.");
        }

        protected override void MoreSetDefaults()
        {
            item.value = (int)SlotType.Hat;
        }
    }

    public class PetAccessoryCrownPlatinum : PetAccessoryBase
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Platinum Crown");
            Tooltip.SetDefault("Something to decorate your cute slime with.");
        }

        protected override void MoreSetDefaults()
        {
            item.value = (int)SlotType.Hat;
        }
    }

    public class PetAccessoryHairBowBlack : PetAccessoryBase
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Black Hair Bow");
            Tooltip.SetDefault("Something to decorate your cute slime with.");
        }

        protected override void MoreSetDefaults()
        {
            item.value = (int)SlotType.Hat;
        }
    }

    public class PetAccessoryHairBowBlue : PetAccessoryBase
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Blue Hair Bow");
            Tooltip.SetDefault("Something to decorate your cute slime with.");
        }

        protected override void MoreSetDefaults()
        {
            item.value = (int)SlotType.Hat;
        }
    }

    public class PetAccessoryHairBowGray : PetAccessoryBase
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Gray Hair Bow");
            Tooltip.SetDefault("Something to decorate your cute slime with.");
        }

        protected override void MoreSetDefaults()
        {
            item.value = (int)SlotType.Hat;
        }
    }

    public class PetAccessoryHairBowGreen : PetAccessoryBase
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Green Hair Bow");
            Tooltip.SetDefault("Something to decorate your cute slime with.");
        }

        protected override void MoreSetDefaults()
        {
            item.value = (int)SlotType.Hat;
        }
    }

    public class PetAccessoryHairBowOrange : PetAccessoryBase
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Orange Hair Bow");
            Tooltip.SetDefault("Something to decorate your cute slime with.");
        }

        protected override void MoreSetDefaults()
        {
            item.value = (int)SlotType.Hat;
        }
    }

    public class PetAccessoryHairBowPink : PetAccessoryBase
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Pink Hair Bow");
            Tooltip.SetDefault("Something to decorate your cute slime with.");
        }

        protected override void MoreSetDefaults()
        {
            item.value = (int)SlotType.Hat;
        }
    }

    public class PetAccessoryHairBowPurple : PetAccessoryBase
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Purple Hair Bow");
            Tooltip.SetDefault("Something to decorate your cute slime with.");
        }

        protected override void MoreSetDefaults()
        {
            item.value = (int)SlotType.Hat;
        }
    }

    public class PetAccessoryHairBowRed : PetAccessoryBase
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Red Hair Bow");
            Tooltip.SetDefault("Something to decorate your cute slime with.");
        }

        protected override void MoreSetDefaults()
        {
            item.value = (int)SlotType.Hat;
        }
    }

    public class PetAccessoryHairBowWhite : PetAccessoryBase
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cute White Hair Bow");
            Tooltip.SetDefault("Something to decorate your cute slime with.");
        }

        protected override void MoreSetDefaults()
        {
            item.value = (int)SlotType.Hat;
        }
    }

    public class PetAccessoryHairBowYellow : PetAccessoryBase
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Yellow Hair Bow");
            Tooltip.SetDefault("Something to decorate your cute slime with.");
        }

        protected override void MoreSetDefaults()
        {
            item.value = (int)SlotType.Hat;
        }
    }

    public class PetAccessoryKitchenKnife : PetAccessoryBase
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Kitchen Knife");
            Tooltip.SetDefault("Something to decorate your cute slime with.");
        }

        protected override void MoreSetDefaults()
        {
            item.value = (int)SlotType.Carried;
        }
    }

    public class PetAccessoryMittensBlack : PetAccessoryBase
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Blue Mittens");
            Tooltip.SetDefault("Something to decorate your cute slime with.");
        }

        protected override void MoreSetDefaults()
        {
            item.value = (int)SlotType.Misc;
        }
    }

    public class PetAccessoryMittensBlue : PetAccessoryBase
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Blue Mittens");
            Tooltip.SetDefault("Something to decorate your cute slime with.");
        }

        protected override void MoreSetDefaults()
        {
            item.value = (int)SlotType.Misc;
        }
    }

    public class PetAccessoryMittensGray : PetAccessoryBase
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Gray Mittens");
            Tooltip.SetDefault("Something to decorate your cute slime with.");
        }

        protected override void MoreSetDefaults()
        {
            item.value = (int)SlotType.Misc;
        }
    }

    public class PetAccessoryMittensGreen : PetAccessoryBase
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Green Mittens");
            Tooltip.SetDefault("Something to decorate your cute slime with.");
        }

        protected override void MoreSetDefaults()
        {
            item.value = (int)SlotType.Misc;
        }
    }

    public class PetAccessoryMittensOrange : PetAccessoryBase
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Orange Mittens");
            Tooltip.SetDefault("Something to decorate your cute slime with.");
        }

        protected override void MoreSetDefaults()
        {
            item.value = (int)SlotType.Misc;
        }
    }

    public class PetAccessoryMittensPink : PetAccessoryBase
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Pink Mittens");
            Tooltip.SetDefault("Something to decorate your cute slime with.");
        }

        protected override void MoreSetDefaults()
        {
            item.value = (int)SlotType.Misc;
        }
    }

    public class PetAccessoryMittensPurple : PetAccessoryBase
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Purple Mittens");
            Tooltip.SetDefault("Something to decorate your cute slime with.");
        }

        protected override void MoreSetDefaults()
        {
            item.value = (int)SlotType.Misc;
        }
    }

    public class PetAccessoryMittensRed : PetAccessoryBase
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Red Mittens");
            Tooltip.SetDefault("Something to decorate your cute slime with.");
        }

        protected override void MoreSetDefaults()
        {
            item.value = (int)SlotType.Misc;
        }
    }

    public class PetAccessoryMittensWhite : PetAccessoryBase
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cute White Mittens");
            Tooltip.SetDefault("Something to decorate your cute slime with.");
        }

        protected override void MoreSetDefaults()
        {
            item.value = (int)SlotType.Misc;
        }
    }

    public class PetAccessoryMittensYellow : PetAccessoryBase
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Yellow Mittens");
            Tooltip.SetDefault("Something to decorate your cute slime with.");
        }

        protected override void MoreSetDefaults()
        {
            item.value = (int)SlotType.Misc;
        }
    }

    public class PetAccessorySlimeHeadBlack : PetAccessoryBase
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Black Head Slime");
            Tooltip.SetDefault("Something to decorate your cute slime with.");
        }

        protected override void MoreSetDefaults()
        {
            item.value = (int)SlotType.Hat;
        }
    }

    public class PetAccessorySlimeHeadBlue : PetAccessoryBase
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Blue Head Slime");
            Tooltip.SetDefault("Something to decorate your cute slime with.");
        }

        protected override void MoreSetDefaults()
        {
            item.value = (int)SlotType.Hat;
        }
    }

    public class PetAccessorySlimeHeadGreen : PetAccessoryBase
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Green Head Slime");
            Tooltip.SetDefault("Something to decorate your cute slime with.");
        }

        protected override void MoreSetDefaults()
        {
            item.value = (int)SlotType.Hat;
        }
    }

    public class PetAccessorySlimeHeadPink : PetAccessoryBase
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Pink Head Slime");
            Tooltip.SetDefault("Something to decorate your cute slime with.");
        }

        protected override void MoreSetDefaults()
        {
            item.value = (int)SlotType.Hat;
        }
    }

    public class PetAccessorySlimeHeadPinky : PetAccessoryBase
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Pinky Head Slime");
            Tooltip.SetDefault("Something to decorate your cute slime with.");
        }

        protected override void MoreSetDefaults()
        {
            item.value = (int)SlotType.Hat;
        }
    }

    public class PetAccessorySlimeHeadPurple : PetAccessoryBase
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Purple Head Slime");
            Tooltip.SetDefault("Something to decorate your cute slime with.");
        }

        protected override void MoreSetDefaults()
        {
            item.value = (int)SlotType.Hat;
        }
    }

    public class PetAccessorySlimeHeadRed : PetAccessoryBase
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Red Head Slime");
            Tooltip.SetDefault("Something to decorate your cute slime with.");
        }

        protected override void MoreSetDefaults()
        {
            item.value = (int)SlotType.Hat;
        }
    }

    public class PetAccessorySlimeHeadYellow : PetAccessoryBase
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Yellow Head Slime");
            Tooltip.SetDefault("Something to decorate your cute slime with.");
        }

        protected override void MoreSetDefaults()
        {
            item.value = (int)SlotType.Hat;
        }
    }

    public class PetAccessoryStaffAmber : PetAccessoryBase
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Amber Staff");
            Tooltip.SetDefault("Something to decorate your cute slime with.");
        }

        protected override void MoreSetDefaults()
        {
            item.value = (int)SlotType.Carried;
        }
    }

    public class PetAccessoryStaffAmethyst : PetAccessoryBase
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Amethyst Staff");
            Tooltip.SetDefault("Something to decorate your cute slime with.");
        }

        protected override void MoreSetDefaults()
        {
            item.value = (int)SlotType.Carried;
        }
    }

    public class PetAccessoryStaffDiamond : PetAccessoryBase
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Diamond Staff");
            Tooltip.SetDefault("Something to decorate your cute slime with.");
        }

        protected override void MoreSetDefaults()
        {
            item.value = (int)SlotType.Carried;
        }
    }

    public class PetAccessoryStaffEmerald : PetAccessoryBase
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Emerald Staff");
            Tooltip.SetDefault("Something to decorate your cute slime with.");
        }

        protected override void MoreSetDefaults()
        {
            item.value = (int)SlotType.Carried;
        }
    }

    public class PetAccessoryStaffRuby : PetAccessoryBase
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Ruby Staff");
            Tooltip.SetDefault("Something to decorate your cute slime with.");
        }

        protected override void MoreSetDefaults()
        {
            item.value = (int)SlotType.Carried;
        }
    }

    public class PetAccessoryStaffSapphire : PetAccessoryBase
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Sapphire Staff");
            Tooltip.SetDefault("Something to decorate your cute slime with.");
        }

        protected override void MoreSetDefaults()
        {
            item.value = (int)SlotType.Carried;
        }
    }

    public class PetAccessoryStaffTopaz : PetAccessoryBase
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Topaz Staff");
            Tooltip.SetDefault("Something to decorate your cute slime with.");
        }

        protected override void MoreSetDefaults()
        {
            item.value = (int)SlotType.Carried;
        }
    }

    public class PetAccessoryXmasHatGreen : PetAccessoryBase
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Green Santa Hat");
            Tooltip.SetDefault("Something to decorate your cute slime with.");
        }

        protected override void MoreSetDefaults()
        {
            item.value = (int)SlotType.Hat;
        }
    }

    public class PetAccessoryXmasHatRed : PetAccessoryBase
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Santa Hat");
            Tooltip.SetDefault("Something to decorate your cute slime with.");
        }

        protected override void MoreSetDefaults()
        {
            item.value = (int)SlotType.Hat;
        }
    }

    public enum SlotType : byte
    {
        None, //reserved
        Body,
        Hat,
        Carried,
        Misc
        //Please settle on (max) four groups for now (ignoring None), those I listed are suggestions.
        //Also, concider that there cant be more than one accessory active in each slot, so decide on proper
        //categories that make sense.

        //for Carried, it's actually only the front facing hand. For something like gloves or dual wielding, use Misc instead

        //also, keep the sprite dimensions the same as the slime girls

        //also they will be rendered in this order aswell (means that Carried can overlap with Body)
    }

    //this is the class that holds all the properties about the accessories, like texture, offset etc
    public class PetAccessory
    {
        //internal fields
        internal static Mod InternalMod;
        internal static byte addCounter = 0; //if you ever add more than 255 accessories, make that a short
        internal static string[] namesOfAccessories;

        //public fields
        public static int[] Items;
        public static int[] ItemsIndexed; //used in ToggleAccessory
        public static int[] ReverseIndexed; //CuteSlimeGlobalTooltip
        public static Texture2D[] Texture;
        public static Vector2[] Offset;
        public static bool[] PreDraw;
        public static byte[] Alpha;
        public static bool[] AllowLegacy;

        public static void Load(Mod mod)
        {
            //-------------------------------------------------------------------
            //------------ADD PET ACCESSORY PROPERTIES HERE----------------------
            //-------------------------------------------------------------------
            /*
             * How to:
             * - put the name of the class you added in ^ to namesOfAccessories
             * - call Add() with the appropriate parameters
             * - Game will throw you an error if the namesOfAccessories length and the added number of accessories is different,
             * or if one of the class names is misspelt
             * 
             * - if you want to remove certain accessories from being usable for the system, comment the line out in namesOfAccessories
             * and comment the corresponding Add line (with //)
             * 
             * - if you want to disable the whole system, comment the PetAccessories.Load() and PetAccessories.Unload() in AssortedCrazyThings.cs
             */

            //this is needed before you call Add() (it needs to know the total number of accessories)
            //order doesn't matter
            namesOfAccessories = new string[]
            {
                "PetAccessoryBowtieBlack",
                "PetAccessoryBowtieBlue",
                "PetAccessoryBowtieGray",
                "PetAccessoryBowtieGreen",
                "PetAccessoryBowtieOrange",
                "PetAccessoryBowtiePink",
                "PetAccessoryBowtiePurple",
                "PetAccessoryBowtieRed",
                "PetAccessoryBowtieWhite",
                "PetAccessoryBowtieYellow",

                "PetAccessoryCrownGold",
                "PetAccessoryCrownPlatinum",

                "PetAccessoryHairBowBlack",
                "PetAccessoryHairBowBlue",
                "PetAccessoryHairBowGray",
                "PetAccessoryHairBowGreen",
                "PetAccessoryHairBowOrange",
                "PetAccessoryHairBowPink",
                "PetAccessoryHairBowPurple",
                "PetAccessoryHairBowRed",
                "PetAccessoryHairBowWhite",
                "PetAccessoryHairBowYellow",

                "PetAccessoryKitchenKnife",

                "PetAccessoryMittensBlack",
                "PetAccessoryMittensBlue",
                "PetAccessoryMittensGray",
                "PetAccessoryMittensGreen",
                "PetAccessoryMittensOrange",
                "PetAccessoryMittensPink",
                "PetAccessoryMittensPurple",
                "PetAccessoryMittensRed",
                "PetAccessoryMittensWhite",
                "PetAccessoryMittensYellow",

                "PetAccessorySlimeHeadBlack",
                "PetAccessorySlimeHeadBlue",
                "PetAccessorySlimeHeadGreen",
                "PetAccessorySlimeHeadPink",
                "PetAccessorySlimeHeadPinky",
                "PetAccessorySlimeHeadPurple",
                "PetAccessorySlimeHeadRed",
                "PetAccessorySlimeHeadYellow",

                "PetAccessoryStaffAmber",
                "PetAccessoryStaffAmethyst",
                "PetAccessoryStaffDiamond",
                "PetAccessoryStaffEmerald",
                "PetAccessoryStaffRuby",
                "PetAccessoryStaffSapphire",
                "PetAccessoryStaffTopaz",

                "PetAccessoryXmasHatRed",
                "PetAccessoryXmasHatGreen",
            };

            Init(mod, namesOfAccessories);

            //signature looks like this: Add(string name, float offsetX = 0f, float offsetY = 0f, bool preDraw = false, byte alpha = 0)
            //the "= something" is a default, if you dont specify that parameter it will assume it is that "something"

            //name is the string name of the class you specify above
            //offsetX/Y is self explanatory, remember, negative X is left, negative Y is up
            //preDraw decides if that accessory should be drawn "behind" the actual slime (false means it will draw infront)
            //alpha says by how much it should be transparent (0 is fully opaque, 255 fully transparent)
            //order of the Add() doesn't matter

            Add(name: "PetAccessoryBowtieBlack");
            Add(name: "PetAccessoryBowtieBlue");
            Add(name: "PetAccessoryBowtieGray");
            Add(name: "PetAccessoryBowtieGreen");
            Add(name: "PetAccessoryBowtieOrange");
            Add(name: "PetAccessoryBowtiePink");
            Add(name: "PetAccessoryBowtiePurple");
            Add(name: "PetAccessoryBowtieRed");
            Add(name: "PetAccessoryBowtieWhite");
            Add(name: "PetAccessoryBowtieYellow");

            Add(name: "PetAccessoryCrownGold");
            Add(name: "PetAccessoryCrownPlatinum");

            Add(name: "PetAccessoryHairBowBlack");
            Add(name: "PetAccessoryHairBowBlue");
            Add(name: "PetAccessoryHairBowGray");
            Add(name: "PetAccessoryHairBowGreen");
            Add(name: "PetAccessoryHairBowOrange");
            Add(name: "PetAccessoryHairBowPink");
            Add(name: "PetAccessoryHairBowPurple");
            Add(name: "PetAccessoryHairBowRed");
            Add(name: "PetAccessoryHairBowWhite");
            Add(name: "PetAccessoryHairBowYellow");

            Add(name: "PetAccessoryKitchenKnife", offsetX: -6f, preDraw: true);

            Add(name: "PetAccessoryMittensBlack", allowLegacy: false);
            Add(name: "PetAccessoryMittensBlue", allowLegacy: false);
            Add(name: "PetAccessoryMittensGray", allowLegacy: false);
            Add(name: "PetAccessoryMittensGreen", allowLegacy: false);
            Add(name: "PetAccessoryMittensOrange", allowLegacy: false);
            Add(name: "PetAccessoryMittensPink", allowLegacy: false);
            Add(name: "PetAccessoryMittensPurple", allowLegacy: false);
            Add(name: "PetAccessoryMittensRed", allowLegacy: false);
            Add(name: "PetAccessoryMittensWhite", allowLegacy: false);
            Add(name: "PetAccessoryMittensYellow", allowLegacy: false);

            Add(name: "PetAccessorySlimeHeadBlack", offsetY: -18f, alpha: 100);
            Add(name: "PetAccessorySlimeHeadBlue", offsetY: -18f, alpha: 100);
            Add(name: "PetAccessorySlimeHeadGreen", offsetY: -18f, alpha: 100);
            Add(name: "PetAccessorySlimeHeadPink", offsetY: -18f, alpha: 100);
            Add(name: "PetAccessorySlimeHeadPinky", offsetY: -18f, alpha: 100);
            Add(name: "PetAccessorySlimeHeadPurple", offsetY: -18f, alpha: 100);
            Add(name: "PetAccessorySlimeHeadRed", offsetY: -18f, alpha: 100);
            Add(name: "PetAccessorySlimeHeadYellow", offsetY: -18f, alpha: 100);

            Add(name: "PetAccessoryStaffAmber", offsetX: -14f, preDraw: true);
            Add(name: "PetAccessoryStaffAmethyst", offsetX: -14f, preDraw: true);
            Add(name: "PetAccessoryStaffDiamond", offsetX: -14f, preDraw: true);
            Add(name: "PetAccessoryStaffEmerald", offsetX: -14f, preDraw: true);
            Add(name: "PetAccessoryStaffRuby", offsetX: -14f, preDraw: true);
            Add(name: "PetAccessoryStaffSapphire", offsetX: -14f, preDraw: true);
            Add(name: "PetAccessoryStaffTopaz", offsetX: -14f, preDraw: true);

            Add(name: "PetAccessoryXmasHatGreen", offsetY: -13f);
            Add(name: "PetAccessoryXmasHatRed", offsetY: -13f);

            Check();

        }

        public static void Unload()
        {
            Texture = null;
            Offset = null;
            InternalMod = null;
            addCounter = 0;
        }


        private static void Check(bool duringAdd = false, string addedClassName = "fv4zuuuuuuuuuuo728fzehoxg")
        {
            if (duringAdd && namesOfAccessories.Length < addCounter)
            {
                throw new Exception("Assigned number of Pet Accessories (" + namesOfAccessories.Length + ") is less than number of added Pet Accessories (" + addCounter + ").");
            }
            else if (duringAdd && addedClassName != "fv4zuuuuuuuuuuo728fzehoxg" && Array.IndexOf(namesOfAccessories, addedClassName) == -1)
            {
                throw new Exception("Tried to add '" + addedClassName + "', but it wasn't registed in 'namesOfAccessories'");
            }

            if (!duringAdd && namesOfAccessories.Length > addCounter)
            {
                throw new Exception("Assigned number of Pet Accessories (" + namesOfAccessories.Length + ") is bigger than number of added Pet Accessories (" + addCounter + ").");
            }
        }

        private static void Init(Mod mod, string[] typeList)
        {
            InternalMod = mod;
            Items = new int[typeList.Length];
            int itemIndex = 0;

            do
            {
                if (mod.ItemType(typeList[itemIndex]) == 0)
                {
                    throw new Exception("Pet Accessory named '" + typeList[itemIndex] + "' is not found. Is it spelt correctly?");
                }
                Items[itemIndex] = mod.ItemType(typeList[itemIndex]);
                itemIndex++;
            }
            while (itemIndex < typeList.Length);

            //+ 1 on all because the 0th index is actually never written (ItemsIndexed starts at 1)
            Texture = new Texture2D[itemIndex + 1];
            Offset = new Vector2[itemIndex + 1];
            PreDraw = new bool[itemIndex + 1];
            Alpha = new byte[itemIndex + 1];
            AllowLegacy = new bool[itemIndex + 1];

            int[] parameters = new int[Items.Length * 2];
            for (int i = 0; i < Items.Length; i++)
            {
                parameters[2 * i] = Items[i];
                parameters[2 * i + 1] = i + 1;
            }
            ItemsIndexed = IntSet(parameters);
        }

        private static void Add(string name, float offsetX = 0f, float offsetY = 0f, bool preDraw = false, byte alpha = 0, bool allowLegacy = true)
        {
            addCounter++;

            Check(true, name);

            TryAdd(InternalMod.ItemType(name), InternalMod.GetTexture("Items/PetAccessories/" + name + "_Draw"), new Vector2(offsetX, offsetY), preDraw, alpha, allowLegacy);
        }

        private static void TryAdd(int type, Texture2D texture, Vector2 offset, bool preDraw, byte alpha, bool allowLegacy)
        {
            Texture[ItemsIndexed[type]] = texture;

            Offset[ItemsIndexed[type]] = offset;

            PreDraw[ItemsIndexed[type]] = preDraw;

            Alpha[ItemsIndexed[type]] = alpha;

            AllowLegacy[ItemsIndexed[type]] = allowLegacy;
        }

        private static int[] IntSet(int[] inputs)
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

    }

    public abstract class PetAccessoryBase : ModItem
    {
        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.Silk);
            item.width = 28;
            item.height = 30;
            item.maxStack = 1;
            item.rare = -11;
            item.useAnimation = 16;
            item.useTime = 16;
            item.useStyle = 4;
            item.UseSound = SoundID.Item1;
            item.consumable = false;
            item.value = (int)SlotType.Body;
            MoreSetDefaults();
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Silk, 15);
            recipe.AddTile(TileID.Loom);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        private string Enum2string(int e)
        {
            if(e == (byte)SlotType.Hat)
            {
                return "Worn on the head.";
            }
            if (e == (byte)SlotType.Body)
            {
                return "Worn on the body.";
            }
            if (e == (byte)SlotType.Carried)
            {
                return "Carried.";
            }
            if (e == (byte)SlotType.Misc)
            {
                return "Worn somewhere else (misc).";
            }
            return "UNINTENDED BEHAVIOR, REPORT TO DEV";
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            AssPlayer mPlayer = Main.LocalPlayer.GetModPlayer<AssPlayer>(mod);

            tooltips.Add(new TooltipLine(mod, "slot", Enum2string(item.value)));

            if (!PetAccessory.AllowLegacy[PetAccessory.ItemsIndexed[item.type]])
            {
                tooltips.Add(new TooltipLine(mod, "AllowLegacy", "Does not work on 'Legacy Appearance' pets."));
            }
        }

        protected virtual void MoreSetDefaults()
        {

        }

        public override bool UseItem(Player player)
        {
            //IS ACTUALLY CALLED EVERY TICK WHENEVER YOU USE THE ITEM ON THE SERVER; BUT ONLY ONCE ON THE CLIENT
            AssPlayer mPlayer = player.GetModPlayer<AssPlayer>(mod);

            if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
            {
                if (mPlayer.petIndex == -1)
                {
                    //find first occurence of a player owned cute slime
                    for (int i = 0; i < 1000; i++)
                    {
                        if (Main.projectile[i].active)
                        {
                            if (Main.projectile[i].modProjectile != null)
                            {
                                if (Main.projectile[i].owner == Main.myPlayer && typeof(CuteSlimeBasePet).IsInstanceOfType(Main.projectile[i].modProjectile))
                                {
                                    ErrorLogger.Log("had to change index of slime pet of " + player.name + " because it was -1");
                                    mPlayer.petIndex = i;
                                    return true;
                                }
                            }
                        }
                    }
                }

                bool shouldReset = false;
                if (player.altFunctionUse == 2) //right click use
                {
                    if (/*mPlayer.slotsPlayer != 0 &&*/ mPlayer.ThreeTimesUseTime(Main.time)) //true after three right clicks in 60 ticks
                    {
                        shouldReset = true;
                    }
                }
                //else normal left click use

                if (mPlayer.petIndex != -1 && Main.projectile[mPlayer.petIndex].active && Main.projectile[mPlayer.petIndex].owner == Main.myPlayer && typeof(CuteSlimeBasePet).IsInstanceOfType(Main.projectile[mPlayer.petIndex].modProjectile))
                {
                    PetAccessoryProj gProjectile = Main.projectile[mPlayer.petIndex].GetGlobalProjectile<PetAccessoryProj>(mod);

                    //only client side
                    if (Main.netMode != NetmodeID.Server)
                    {
                        if (shouldReset && player.altFunctionUse == 2)
                        {
                            //CombatText.NewText(new Rectangle((int)player.position.X, (int)player.position.Y, player.width, player.height), CombatText.HealLife, "reverted all accessories");

                            gProjectile.SetAccessoryAll(mPlayer.slotsPlayer != 0 ? 0 : mPlayer.slotsPlayerLast);

                            //"dust" originating from the center, forming a circle and going outwards
                            Dust dust;
                            for (double angle = 0; angle < Math.PI * 2; angle += Math.PI / 6)
                            {
                                dust = Dust.NewDustPerfect(Main.projectile[mPlayer.petIndex].Center - new Vector2(0f, Main.projectile[mPlayer.petIndex].height / 4), 16, new Vector2((float)-Math.Cos(angle), (float)Math.Sin(angle)) * 1.2f, 0, new Color(255, 255, 255), 1.6f);
                            }

                            //save it for next time shouldReset is true
                            mPlayer.slotsPlayerLast = mPlayer.slotsPlayer;

                            //sync with player, for when he respawns, it gets reapplied
                            mPlayer.slotsPlayer = gProjectile.GetAccessoryAll(); //triggers SyncPlayer

                            if (Main.netMode == NetmodeID.MultiplayerClient)
                            {
                                mPlayer.SendRedrawPetAccessories();
                            }
                            //mPlayer.SendSlotData();
                        }
                        else if (player.altFunctionUse != 2)
                        {
                            //check if selected item is valid on the pet, if it is a legacy version
                            if (!PetAccessory.AllowLegacy[PetAccessory.ItemsIndexed[item.type]] && Array.IndexOf(AssortedCrazyThings.slimePetLegacy, Main.projectile[mPlayer.petIndex].type) != -1)
                            {
                                return true;
                            }

                            gProjectile.ToggleAccessory((byte)item.value, (uint)PetAccessory.ItemsIndexed[item.type]);

                            //sync with player, for when he respawns, it gets reapplied
                            mPlayer.slotsPlayer = gProjectile.GetAccessoryAll();
                            //mPlayer.SendSlotData();
                        }
                    }
                }
            }
            return true;
        }
    }
}
