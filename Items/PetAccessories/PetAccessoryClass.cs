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
            Tooltip.SetDefault("'A soft black bowtie for your cute slime to wear on her chest'");
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
            Tooltip.SetDefault("'A soft blue bowtie for your cute slime to wear on her chest'");
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
            Tooltip.SetDefault("'A soft gray bowtie for your cute slime to wear on her chest'");
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
            Tooltip.SetDefault("'A soft green bowtie for your cute slime to wear on her chest'");
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
            Tooltip.SetDefault("'A soft orange bowtie for your cute slime to wear on her chest'");
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
            Tooltip.SetDefault("'A soft pink bowtie for your cute slime to wear on her chest'");
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
            Tooltip.SetDefault("'A soft purple bowtie for your cute slime to wear on her chest'");
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
            Tooltip.SetDefault("'A soft red bowtie for your cute slime to wear on her chest'");
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
            Tooltip.SetDefault("'A soft white bowtie for your cute slime to wear on her chest'");
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
            Tooltip.SetDefault("'A soft yellow bowtie for your cute slime to wear on her chest'");
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
            Tooltip.SetDefault("'A regal golden crown for your cute slime to wear on her head'");
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
            Tooltip.SetDefault("'A regal platinum crown for your cute slime to wear on her head'");
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
            Tooltip.SetDefault("'A large black bow for your cute slime to wear on her head'");
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
            Tooltip.SetDefault("'A large blue bow for your cute slime to wear on her head'");
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
            Tooltip.SetDefault("'A large gray bow for your cute slime to wear on her head'");
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
            Tooltip.SetDefault("'A large green bow for your cute slime to wear on her head'");
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
            Tooltip.SetDefault("'A large orange bow for your cute slime to wear on her head'");
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
            Tooltip.SetDefault("'A large pink bow for your cute slime to wear on her head'");
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
            Tooltip.SetDefault("'A large purple bow for your cute slime to wear on her head'");
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
            Tooltip.SetDefault("'A large red bow for your cute slime to wear on her head'");
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
            Tooltip.SetDefault("'A large white bow for your cute slime to wear on her head'");
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
            Tooltip.SetDefault("'A large yellow bow for your cute slime to wear on her head'");
        }

        protected override void MoreSetDefaults()
        {
            item.value = (int)SlotType.Hat;
        }
    }

    //public class PetAccessoryKitchenKnife : PetAccessoryBase
    //{
    //    public override void SetStaticDefaults()
    //    {
    //        DisplayName.SetDefault("Cute Kitchen Knife");
    //        Tooltip.SetDefault("'A plush kitchen knife for your cute slime to carry'");
    //    }

    //    protected override void MoreSetDefaults()
    //    {
    //        item.value = (int)SlotType.Carried;
    //    }
    //}

    //public class PetAccessoryMetalHelmet : PetAccessoryBase
    //{
    //    public override void SetStaticDefaults()
    //    {
    //        DisplayName.SetDefault("Cute Knight Helmet");
    //        Tooltip.SetDefault("'A plush knight's helmet for your cute slime to wear on her head'");
    //    }

    //    protected override void MoreSetDefaults()
    //    {
    //        item.value = (int)SlotType.Hat;
    //    }
    //}

    public class PetAccessoryMittensBlack : PetAccessoryBase
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Black Mittens");
            Tooltip.SetDefault("'Warm black mittens for your cute slime's hands'");
        }

        protected override void MoreSetDefaults()
        {
            item.value = (int)SlotType.Accessory;
        }
    }

    public class PetAccessoryMittensBlue : PetAccessoryBase
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Blue Mittens");
            Tooltip.SetDefault("'Warm blue mittens for your cute slime's hands'");
        }

        protected override void MoreSetDefaults()
        {
            item.value = (int)SlotType.Accessory;
        }
    }

    public class PetAccessoryMittensGray : PetAccessoryBase
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Gray Mittens");
            Tooltip.SetDefault("'Warm gray mittens for your cute slime's hands'");
        }

        protected override void MoreSetDefaults()
        {
            item.value = (int)SlotType.Accessory;
        }
    }

    public class PetAccessoryMittensGreen : PetAccessoryBase
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Green Mittens");
            Tooltip.SetDefault("'Warm green mittens for your cute slime's hands'");
        }

        protected override void MoreSetDefaults()
        {
            item.value = (int)SlotType.Accessory;
        }
    }

    public class PetAccessoryMittensOrange : PetAccessoryBase
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Orange Mittens");
            Tooltip.SetDefault("'Warm orange mittens for your cute slime's hands'");
        }

        protected override void MoreSetDefaults()
        {
            item.value = (int)SlotType.Accessory;
        }
    }

    public class PetAccessoryMittensPink : PetAccessoryBase
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Pink Mittens");
            Tooltip.SetDefault("'Warm pink mittens for your cute slime's hands'");
        }

        protected override void MoreSetDefaults()
        {
            item.value = (int)SlotType.Accessory;
        }
    }

    public class PetAccessoryMittensPurple : PetAccessoryBase
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Purple Mittens");
            Tooltip.SetDefault("'Warm purple mittens for your cute slime's hands'");
        }

        protected override void MoreSetDefaults()
        {
            item.value = (int)SlotType.Accessory;
        }
    }

    public class PetAccessoryMittensRed : PetAccessoryBase
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Red Mittens");
            Tooltip.SetDefault("'Warm red mittens for your cute slime's hands'");
        }

        protected override void MoreSetDefaults()
        {
            item.value = (int)SlotType.Accessory;
        }
    }

    public class PetAccessoryMittensWhite : PetAccessoryBase
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cute White Mittens");
            Tooltip.SetDefault("'Warm white mittens for your cute slime's hands'");
        }

        protected override void MoreSetDefaults()
        {
            item.value = (int)SlotType.Accessory;
        }
    }

    public class PetAccessoryMittensYellow : PetAccessoryBase
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Yellow Mittens");
            Tooltip.SetDefault("'Warm yellow mittens for your cute slime's hands'");
        }

        protected override void MoreSetDefaults()
        {
            item.value = (int)SlotType.Accessory;
        }
    }

    public class PetAccessorySlimeHeadBlack : PetAccessoryBase
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Black Head Slime");
            Tooltip.SetDefault("'A black slime plush that sits on your cute slime's head'");
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
            Tooltip.SetDefault("'A blue slime plush that sits on your cute slime's head'");
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
            Tooltip.SetDefault("'A green slime plush that sits on your cute slime's head'");
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
            Tooltip.SetDefault("'A pink slime plush that sits on your cute slime's head'");
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
            Tooltip.SetDefault("'A Pinky plush that sits on your cute slime's head'");
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
            Tooltip.SetDefault("'A purple slime plush that sits on your cute slime's head'");
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
            Tooltip.SetDefault("'A red slime plush that sits on your cute slime's head'");
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
            Tooltip.SetDefault("'A yellow slime plush that sits on your cute slime's head'");
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
            Tooltip.SetDefault("'A plush amber staff for your cute slime to carry'");
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
            Tooltip.SetDefault("'A plush amethyst staff for your cute slime to carry'");
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
            Tooltip.SetDefault("'A plush diamond staff for your cute slime to carry'");
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
            Tooltip.SetDefault("'A plush emerald staff for your cute slime to carry'");
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
            Tooltip.SetDefault("'A plush ruby staff for your cute slime to carry'");
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
            Tooltip.SetDefault("'A plush sapphire staff for your cute slime to carry'");
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
            Tooltip.SetDefault("'A plush topaz staff for your cute slime to carry'");
        }

        protected override void MoreSetDefaults()
        {
            item.value = (int)SlotType.Carried;
        }
    }

    //public class PetAccessorySwallowedKey : PetAccessoryBase
    //{
    //    public override void SetStaticDefaults()
    //    {
    //        DisplayName.SetDefault("Cute Swallowed Key");
    //        Tooltip.SetDefault("'A plush key for your cute slime to...carry?'");
    //    }

    //    protected override void MoreSetDefaults()
    //    {
    //        item.value = (int)SlotType.Accessory;
    //    }
    //}

    //public class PetAccessoryToyBreastplate : PetAccessoryBase
    //{
    //    public override void SetStaticDefaults()
    //    {
    //        DisplayName.SetDefault("Cute Knight Breastplate");
    //        Tooltip.SetDefault("'A plush knight's breastplate for your cute slime to wear on her body'");
    //    }

    //    protected override void MoreSetDefaults()
    //    {
    //        item.value = (int)SlotType.Body;
    //    }
    //}

    //public class PetAccessoryToyMace : PetAccessoryBase
    //{
    //    public override void SetStaticDefaults()
    //    {
    //        DisplayName.SetDefault("Cute Paladin's Mace");
    //        Tooltip.SetDefault("'A plush paladin's mace for your cute slime to carry'");
    //    }

    //    protected override void MoreSetDefaults()
    //    {
    //        item.value = (int)SlotType.Carried;
    //    }
    //}

    //public class PetAccessoryToyShield : PetAccessoryBase
    //{
    //    public override void SetStaticDefaults()
    //    {
    //        DisplayName.SetDefault("Cute Knight Shield");
    //        Tooltip.SetDefault("'A plush knight's shield for your cute slime to carry'");
    //    }

    //    protected override void MoreSetDefaults()
    //    {
    //        item.value = (int)SlotType.Accessory;
    //    }
    //}

    //public class PetAccessoryToySpear : PetAccessoryBase
    //{
    //    public override void SetStaticDefaults()
    //    {
    //        DisplayName.SetDefault("Cute Spartan's Spear");
    //        Tooltip.SetDefault("'A plush warrior's spear for your cute slime to carry'");
    //    }

    //    protected override void MoreSetDefaults()
    //    {
    //        item.value = (int)SlotType.Carried;
    //    }
    //}

    //public class PetAccessoryToySword : PetAccessoryBase
    //{
    //    public override void SetStaticDefaults()
    //    {
    //        DisplayName.SetDefault("Cute Knight Sword");
    //        Tooltip.SetDefault("'A plush knight's sword for your cute slime to carry'");
    //    }

    //    protected override void MoreSetDefaults()
    //    {
    //        item.value = (int)SlotType.Carried;
    //    }
    //}

    //public class PetAccessoryWizardHat : PetAccessoryBase
    //{
    //    public override void SetStaticDefaults()
    //    {
    //        DisplayName.SetDefault("Cute Wizard Hat");
    //        Tooltip.SetDefault("'A brown wizard's hat for your cute slime to wear on her head'");
    //    }

    //    protected override void MoreSetDefaults()
    //    {
    //        item.value = (int)SlotType.Hat;
    //    }
    //}

    public class PetAccessoryXmasHatGreen : PetAccessoryBase
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Green Santa Hat");
            Tooltip.SetDefault("'A festive green hat for your cute slime to wear'");
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
            Tooltip.SetDefault("'A festive red hat for your cute slime to wear'");
        }

        protected override void MoreSetDefaults()
        {
            item.value = (int)SlotType.Hat;
        }
    }

    public enum SlotType : byte
    {
        None, //reserved
        Body = 1,
        Hat = 2,
        Carried = 3,
        Accessory = 4

        //for Carried, it's actually only the front facing hand. For something like gloves or dual wielding, use Accessory instead

        //also, keep the sprite dimensions the same as the slime girls

        //also they will be rendered in this order aswell (means that Carried can overlap with Body)
    }

    //this is the class that holds all the properties about the accessories, like texture, offset etc
    public class PetAccessory
    {
        //internal fields
        internal static byte addCounter = 0; //if you ever add more than 255 accessories, make that a short
        internal static string[] namesOfAccessories;

        //public fields
        public static int[] Items;
        public static int[] ItemsIndexed; //used in ToggleAccessory
        public static Texture2D[] Texture;
        public static Vector2[] Offset;
        public static bool[] PreDraw;
        public static byte[] Alpha;
        public static bool[] UseNoHair;
        public static int[,] AltTexture; //accessory -> alt tex array for cuteslime<color>pet

        public static void Load()
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
             * - if you want to add alternative textures (Suffixed with _Draw<identifyingNumber>), call AddAltTextures and assign each 
             * pet a texture to use (-1 is "not rendered", 0 is "default, > 0 is "use _Draw<identifyingNumber> texture")
             * you can leave the other pet types out if you only need to adjust the texture of one pet
             * 
             * - if you want to remove certain accessories from being usable for the system, comment the line out in namesOfAccessories
             * and comment the corresponding Add line (with //)
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

                //"PetAccessoryKitchenKnife",

                //"PetAccessoryMetalHelmet",

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

                //"PetAccessorySwallowedKey",

                //"PetAccessoryToyBreastplate",
                //"PetAccessoryToyMace",
                //"PetAccessoryToyShield",
                //"PetAccessoryToySpear",
                //"PetAccessoryToySword",

                //"PetAccessoryWizardHat",

                "PetAccessoryXmasHatRed",
                "PetAccessoryXmasHatGreen",
            };

            Init(namesOfAccessories);

            //signature looks like this: Add(string name, float offsetX = 0f, float offsetY = 0f, bool preDraw = false, byte alpha = 0)
            //the "= something" is a default, if you dont specify that parameter it will assume it is that "something"

            //name is the string name of the class you specify above
            //offsetX/Y is self explanatory, remember, negative X is left, negative Y is up
            //preDraw decides if that accessory should be drawn "behind" the actual slime (false means it will draw infront)
            //alpha says by how much it should be transparent (0 is fully opaque, 255 fully transparent)
            //order of the Add() doesn't matter, though it is alphabetically here for organization

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
            AddAltTextures(name: "PetAccessoryCrownGold",
            pink: 1,
            dungeon: 1,
            yellow: 2);
            Add(name: "PetAccessoryCrownPlatinum");
            AddAltTextures(name: "PetAccessoryCrownPlatinum",
            pink: 1,
            dungeon: 1,
            yellow: 2);

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

            //Add(name: "PetAccessoryKitchenKnife", preDraw: true);

            //Add(name: "PetAccessoryMetalHelmet", offsetY: -2f, useNoHair: true);
            //AddAltTextures(name: "PetAccessoryMetalHelmet",
            //green: 1);

            Add(name: "PetAccessoryMittensBlack");
            Add(name: "PetAccessoryMittensBlue");
            Add(name: "PetAccessoryMittensGray");
            Add(name: "PetAccessoryMittensGreen");
            Add(name: "PetAccessoryMittensOrange");
            Add(name: "PetAccessoryMittensPink");
            Add(name: "PetAccessoryMittensPurple");
            Add(name: "PetAccessoryMittensRed");
            Add(name: "PetAccessoryMittensWhite");
            Add(name: "PetAccessoryMittensYellow");

            Add(name: "PetAccessorySlimeHeadBlack", offsetY: -12f, alpha: 56);
            Add(name: "PetAccessorySlimeHeadBlue", offsetY: -12f, alpha: 56);
            Add(name: "PetAccessorySlimeHeadGreen", offsetY: -12f, alpha: 56);
            Add(name: "PetAccessorySlimeHeadPink", offsetY: -12f, alpha: 56);
            Add(name: "PetAccessorySlimeHeadPinky", alpha: 39);
            Add(name: "PetAccessorySlimeHeadPurple", offsetY: -12f, alpha: 56);
            Add(name: "PetAccessorySlimeHeadRed", offsetY: -12f, alpha: 56);
            Add(name: "PetAccessorySlimeHeadYellow", offsetY: -12f, alpha: 56);

            Add(name: "PetAccessoryStaffAmber", offsetX: -8f, preDraw: true);
            Add(name: "PetAccessoryStaffAmethyst", offsetX: -8f, preDraw: true);
            Add(name: "PetAccessoryStaffDiamond", offsetX: -8f, preDraw: true);
            Add(name: "PetAccessoryStaffEmerald", offsetX: -8f, preDraw: true);
            Add(name: "PetAccessoryStaffRuby", offsetX: -8f, preDraw: true);
            Add(name: "PetAccessoryStaffSapphire", offsetX: -8f, preDraw: true);
            Add(name: "PetAccessoryStaffTopaz", offsetX: -8f, preDraw: true);
			
            //Add(name: "PetAccessorySwallowedKey", preDraw: true);
            
            //Add(name: "PetAccessoryToyBreastplate");
            //AddAltTextures(name: "PetAccessoryToyBreastplate",
            //xmas: 1);
            //Add(name: "PetAccessoryToyMace", offsetX: -4f, preDraw: true);
            //Add(name: "PetAccessoryToyShield");
            //Add(name: "PetAccessoryToySpear", offsetX: -8f, preDraw: true);
            //Add(name: "PetAccessoryToySword", preDraw: true);

            //Add(name: "PetAccessoryWizardHat", offsetY: -10f, useNoHair: true);
            //AddAltTextures(name: "PetAccessoryWizardHat",
            //black: 1,
            //corrupt: 1,
            //dungeon: 1,
            //pink: 2,
            //purple: 1,
            //toxic: 1);
			
            Add(name: "PetAccessoryXmasHatGreen", offsetY: -4f, useNoHair: true);
            Add(name: "PetAccessoryXmasHatRed", offsetY: -4f, useNoHair: true);

            Check();

        }

        public static void Unload()
        {
            if (!Main.dedServ && Main.netMode != 2)
            {
                Texture = null;
                addCounter = 0;
            }
        }

        private static void Check(bool duringAdd = false, string addedClassName = "fv4zruuu")
        {
            if (duringAdd && namesOfAccessories.Length < addCounter)
            {
                throw new Exception("Assigned number of Pet Accessories (" + namesOfAccessories.Length + ") is less than number of added Pet Accessories (" + addCounter + ").");
            }
            else if (duringAdd && addedClassName != "fv4zruuu" && Array.IndexOf(namesOfAccessories, addedClassName) == -1)
            {
                throw new Exception("Tried to add '" + addedClassName + "', but it wasn't registed in 'namesOfAccessories'");
            }

            if (!duringAdd && namesOfAccessories.Length > addCounter)
            {
                throw new Exception("Assigned number of Pet Accessories (" + namesOfAccessories.Length + ") is bigger than number of added Pet Accessories (" + addCounter + ").");
            }
        }

        private static void Init(string[] typeList)
        {
            Items = new int[typeList.Length];
            int itemIndex = 0;

            do
            {
                if (AssUtils.Instance.ItemType(typeList[itemIndex]) == 0)
                {
                    throw new Exception("Pet Accessory named '" + typeList[itemIndex] + "' is not found. Is it spelt correctly?");
                }
                Items[itemIndex] = AssUtils.Instance.ItemType(typeList[itemIndex]);
                itemIndex++;
            }
            while (itemIndex < typeList.Length);

            //+ 1 on all because the 0th index is actually never written (ItemsIndexed starts at 1)
            Texture = new Texture2D[itemIndex + 1];
            Offset = new Vector2[itemIndex + 1];
            PreDraw = new bool[itemIndex + 1];
            Alpha = new byte[itemIndex + 1];
            UseNoHair = new bool[itemIndex + 1];
            AltTexture = new int[itemIndex + 1, SlimePets.slimePets.Count];

            int[] parameters = new int[Items.Length * 2];
            for (int i = 0; i < Items.Length; i++)
            {
                parameters[2 * i] = Items[i];
                parameters[2 * i + 1] = i + 1;
            }
            ItemsIndexed = IntSet(parameters);
        }

        private static void Add(string name, float offsetX = 0f, float offsetY = 0f, bool preDraw = false, byte alpha = 0, bool useNoHair = false)
        {
            addCounter++;

            Check(true, name);

            TryAdd(AssUtils.Instance.ItemType(name), AssUtils.Instance.GetTexture("Items/PetAccessories/" + name + "_Draw"), new Vector2(offsetX, offsetY), preDraw, alpha, useNoHair);
        }

        private static void TryAdd(int type, Texture2D texture, Vector2 offset, bool preDraw, byte alpha, bool useNoHair)
        {
            Texture[ItemsIndexed[type]] = texture;

            Offset[ItemsIndexed[type]] = offset;

            PreDraw[ItemsIndexed[type]] = preDraw;

            Alpha[ItemsIndexed[type]] = alpha;

            UseNoHair[ItemsIndexed[type]] = useNoHair;
        }

        private static void AddAltTextures(string name,
            int black = 0,
            int blue = 0,
            int corrupt = 0,
            int crimson = 0,
            int dungeon = 0,
            int green = 0,
            int ice = 0,
            int pink = 0,
            int purple = 0,
            int rainbow = 0,
            int red = 0,
            int toxic = 0,
            int xmas = 0,
            int yellow = 0)
        {
            //not specifying anything (or 0) means it only takes the default texture (_Draw)
            //setting it to -1 makes it so the accessory won't render (excluding certain accessories for a slime)
            //setting it to anything other than 0 makes it use the proper texture (_Draw<number>)

            //order matters here:
            /* public enum PetColor : byte
                {
                    Black,
                    Blue,
                    Corrupt,
                    Crimson,
                    Green,
                    Pink,
                    Purple,
                    Rainbow,
                    Red,
                    Xmas,
                    Yellow
                }
             */
            int[] intArray = new int[] {black, blue, corrupt, crimson, dungeon, green, ice, pink, purple, rainbow, red, toxic, xmas, yellow};

            //i is the color (PetColor)
            for (int i = 0; i < intArray.Length; i++)
            {
                AltTexture[ItemsIndexed[AssUtils.Instance.ItemType(name)], i] = intArray[i];
            }
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

    public class APetAccessory
    {
        public static List<APetAccessory> petAccessoryListGlobal = new List<APetAccessory>();
        public static List<APetAccessory> petAccessoryListB = new List<APetAccessory>();
        public static List<APetAccessory> petAccessoryListH = new List<APetAccessory>();
        public static List<APetAccessory> petAccessoryListC = new List<APetAccessory>();
        public static List<APetAccessory> petAccessoryListA = new List<APetAccessory>();
        public static List<int> petAccessoryIdsB;
        public static List<int> petAccessoryIdsH;
        public static List<int> petAccessoryIdsC;
        public static List<int> petAccessoryIdsA;
        public static List<int> petAccessoryTypesGlobal;

        public byte ID { private set; get; }
        public string Name { private set; get; }
        public int Type { private set; get; }
        public SlotType Slot { private set; get; }
        public byte Color { set; get; } //index for AltTextureSuffixes
        public Vector2 Offset { private set; get; }
        public bool PreDraw { private set; get; }
        public byte Alpha { private set; get; }
        public bool UseNoHair { private set; get; }

        public bool HasAlts { private set; get; } //for deciding if it has a UI or not
        public List<string> AltTextureSuffixes { private set; get; } //for UI tooltips
        public List<Texture2D> AltTextures { private set; get; } //for UI only, the _Draw<number> stuff is done manually
        public List<sbyte> PetVariations { private set; get; } //the number in _Draw<number>

        //private static void Add(string name, float offsetX = 0f, float offsetY = 0f, bool preDraw = false, byte alpha = 0, bool useNoHair = false)
        public APetAccessory(byte id, string name, float offsetX = 0f, float offsetY = 0f, bool preDraw = false, byte alpha = 0, bool useNoHair = false, List<string> altTextures = null)
        {
            ID = id;
            Name = "PetAccessory" + name;
            Type = AssUtils.Instance.ItemType(Name);
            if (Type == 0) throw new Exception("Item called '" + Name + "' doesn't exist, are you sure you spelled it correctly?");
            Color = 0;

            Offset = new Vector2(offsetX, offsetY);
            PreDraw = preDraw;
            Alpha = alpha;
            UseNoHair = useNoHair;

            AltTextureSuffixes = new List<string>();
            if (altTextures != null)
            {
                AltTextureSuffixes.AddRange(altTextures);
                HasAlts = true;
            }
            else
            {
                HasAlts = false;
            }
            //add icons for UI
            AltTextures = new List<Texture2D>(AltTextureSuffixes.Count);
            for (int i = 0; i < AltTextureSuffixes.Count; i++)
            {
                AltTextures.Add(AssUtils.Instance.GetTexture("Items/PetAccessories/" + Name + AltTextureSuffixes[i]));
            }

            //fill list with zeroes
            PetVariations = new List<sbyte>(SlimePets.slimePets.Count);
            for (int i = 0; i < SlimePets.slimePets.Count; i++) PetVariations.Add(0);
        }

        public APetAccessory AddPetVariation(string petName, sbyte number)
        {
            //(byte)-1, 0 (default), 1..254 alt texture number
            int type = AssUtils.Instance.ProjectileType("CuteSlime" + petName + "NewProj");
            if (SlimePets.slimePets.IndexOf(type) < 0) throw new Exception("slime pet of type 'CuteSlime" + petName + "NewProj' not registered in SlimePets.Load()");
            PetVariations[SlimePets.slimePets.IndexOf(type)] = number;
            return this;
        }

        public static void Load()
        {
            //START WITH ID: 1
            //Add(new APetAccessory(id: 1, name: "PetAccessoryCrownGold")
            //    .AddPetVariation("CuteSlimePinkNewProj", 2));
            //Add(new APetAccessory(id: 2, name: "PetAccessoryToyBreastplate")
            //    .AddPetVariation("CuteSlimeXmasNewProj", 1));
            //Add(new APetAccessory(id: 3, name: "PetAccessoryBowtieRed")
            //    .AddPetVariation("CuteSlimeXmasNewProj", 5));

            //Bowtie: Red, Crown: Gold, Hair Bow: Red, Mittens: Red, Head Slime: Blue, Staff: Amethyst, Xmas Hat: Red

            //DONT CHANGE UP THE ORDER OF THE COLORS, IT'LL MESS THINGS UP (but not badly)
            //BODY SLOT ACCESSORIES GO HERE, SEPARATE IDs
            Add(SlotType.Body, new APetAccessory(id: 1, name: "Bowtie", altTextures: new List<string>() { "Red", "Orange", "Yellow", "Green", "Blue", "Purple", "Pink", "White", "Gray", "Black"}));
            Add(SlotType.Body, new APetAccessory(id: 2, name: "ToyBreastplate")
                 .AddPetVariation("Xmas", 1)
                 );

            //HAT SLOT ACCESSORIES GO HERE, SEPARATE IDs
            Add(SlotType.Hat, new APetAccessory(id: 1, name: "Crown", altTextures: new List<string>() { "Gold", "Platinum" })
                .AddPetVariation("Pink", 1)
                .AddPetVariation("Dungeon", 1)
                .AddPetVariation("Yellow", 2)
                );
            Add(SlotType.Hat, new APetAccessory(id: 2, name: "HairBow", altTextures: new List<string>() { "Red", "Orange", "Yellow", "Green", "Blue", "Purple", "Pink", "White", "Gray", "Black" }));
            Add(SlotType.Hat, new APetAccessory(id: 3, name: "MetalHelmet", offsetY: -2f, useNoHair: true)
                .AddPetVariation("Green", 1)
                );
            Add(SlotType.Hat, new APetAccessory(id: 4, name: "SlimeHead", offsetY: -12f, alpha: 56, altTextures: new List<string>() { "Blue", "Purple", "Pink", "Pinky", "Red", "Yellow", "Green", "Black" }));
            Add(SlotType.Hat, new APetAccessory(id: 5, name: "WizardHat", offsetY: -10f, useNoHair: true)
                .AddPetVariation("Black", 1)
                .AddPetVariation("Corrupt", 1)
                .AddPetVariation("Dungeon", 1)
                .AddPetVariation("Pink", 2)
                .AddPetVariation("Purple", 1)
                .AddPetVariation("Toxic", 1)
                );
            Add(SlotType.Hat, new APetAccessory(id: 6, name: "XmasHat", offsetY: -4f, useNoHair: true, altTextures: new List<string>() { "Red", "Green" }));

            //CARRIED SLOT ACCESSORIES GO HERE, SEPARATE IDs
            Add(SlotType.Carried, new APetAccessory(id: 1, name: "KitchenKnife"));
            Add(SlotType.Carried, new APetAccessory(id: 2, name: "Staff", offsetX: -8f, preDraw: true, altTextures: new List<string>() { "Amethyst", "Sapphire", "Emerald", "Ruby", "Amber", "Topaz", "Diamond"}));
            Add(SlotType.Carried, new APetAccessory(id: 3, name: "ToyMace", offsetX: -4f, preDraw: true));
            Add(SlotType.Carried, new APetAccessory(id: 4, name: "ToySpear", offsetX: -8f, preDraw: true));
            Add(SlotType.Carried, new APetAccessory(id: 5, name: "ToySword", preDraw: true));

            //ACCESSORY SLOT ACCESSORIES GO HERE, SEPARATE IDs
            Add(SlotType.Accessory, new APetAccessory(id: 1, name: "Mittens", altTextures: new List<string>() { "Red", "Orange", "Yellow", "Green", "Blue", "Purple", "Pink", "White", "Gray", "Black" }));
            Add(SlotType.Accessory, new APetAccessory(id: 2, name: "SwallowedKey", preDraw: true));
            Add(SlotType.Accessory, new APetAccessory(id: 3, name: "ToyShield"));

            CreateMaps();
        }

        public static void CreateMaps()
        {
            petAccessoryIdsB = new List<int>(petAccessoryListB.Count);
            petAccessoryIdsH = new List<int>(petAccessoryListH.Count);
            petAccessoryIdsC = new List<int>(petAccessoryListC.Count);
            petAccessoryIdsA = new List<int>(petAccessoryListA.Count);

            foreach (SlotType slotType in Enum.GetValues(typeof(SlotType)))
            {
                if (slotType != SlotType.None)
                {
                    List<APetAccessory> tempAccessoryList = GetAccessoryListFromType(slotType);
                    List<int> tempIdList = GetIdListFromType(slotType);
                    for (int i = 0; i < tempAccessoryList.Count; i++)
                    {
                        tempIdList.Add(tempAccessoryList[i].ID);
                    }
                }
            }

            petAccessoryTypesGlobal = new List<int>(petAccessoryListGlobal.Count); //because types are unique we use only one list
            for (int i = 0; i < petAccessoryListGlobal.Count; i++)
            {
                petAccessoryTypesGlobal.Add(petAccessoryListGlobal[i].Type);
            }
        }

        public static void Unload()
        {
            petAccessoryListGlobal.Clear();
            petAccessoryListB.Clear();
            petAccessoryListH.Clear();
            petAccessoryListC.Clear();
            petAccessoryListA.Clear();
            petAccessoryIdsB.Clear();
            petAccessoryIdsH.Clear();
            petAccessoryIdsC.Clear();
            petAccessoryIdsA.Clear();
            petAccessoryTypesGlobal.Clear();
        }

        private static List<APetAccessory> GetAccessoryListFromType(SlotType slotType)
        {
            switch (slotType)
            {
                case SlotType.Body:
                    return petAccessoryListB;
                case SlotType.Hat:
                    return petAccessoryListH;
                case SlotType.Carried:
                    return petAccessoryListC;
                case SlotType.Accessory:
                    return petAccessoryListA;
                default:
                    return petAccessoryListGlobal;
            }
        }

        private static List<int> GetIdListFromType(SlotType slotType)
        {
            switch (slotType)
            {
                case SlotType.Body:
                    return petAccessoryIdsB;
                case SlotType.Hat:
                    return petAccessoryIdsH;
                case SlotType.Carried:
                    return petAccessoryIdsC;
                case SlotType.Accessory:
                    return petAccessoryIdsA;
                default:
                    throw new Exception("invalid slottype");
            }
        }

        public static void Add(SlotType slotType, APetAccessory aPetAccessory)
        {
            for (int i = 0; i < petAccessoryListGlobal.Count; i++)
            {
                if (petAccessoryListGlobal[i].Name == aPetAccessory.Name) throw new Exception("Added Accessory '" + aPetAccessory.Name + "' already exists");
                if (petAccessoryListGlobal[i].Slot == slotType && petAccessoryListGlobal[i].ID == aPetAccessory.ID)
                    throw new Exception("ID '" + aPetAccessory.ID + "' in Slot '" + aPetAccessory.Slot.ToString() + "' for '" + aPetAccessory.Name + "' already registered for '" + petAccessoryListGlobal[i].Name + "'");
            }

            aPetAccessory.Slot = slotType;
            if (slotType == SlotType.None) throw new Exception("There has to be a slot specified as the first argument in 'Add()'");

            //everything fine
            List<APetAccessory> tempAccessoryList = GetAccessoryListFromType(slotType);
            tempAccessoryList.Add(aPetAccessory);

            petAccessoryListGlobal.Add(aPetAccessory);
        }

        public static APetAccessory GetAccessoryFromID(SlotType slotType, byte id) //if something has the id, it always has the slottype available
        {
            return GetAccessoryListFromType(slotType)[GetIdListFromType(slotType).IndexOf(id)];
            //return petAccessoryListGlobal[petAccessoryIdsB.IndexOf(id)];
        }

        public static APetAccessory GetAccessoryFromType(int type) //since types are unique, just look up in the global list
        {
            return petAccessoryListGlobal[petAccessoryTypesGlobal.IndexOf(type)];
        }

        public static bool IsItemAPetVanity(int type)
        {
            for (int i = 0; i < petAccessoryListGlobal.Count; i++)
            {
                if (petAccessoryListGlobal[i].Type == type && petAccessoryListGlobal[i].HasAlts) return true;
            }
            return false;
        }

        public override string ToString()
        {
            return "ID: " + ID
                + "; Col: " + Color
                + "; Slo: " + Slot.ToString()
                + "; Nam: " + Name
                + "; Typ: " + Type
                + "; Off: " + Offset
                + "; Pre: " + (PreDraw ? "y" : "n")
                + "; Alp: " + Alpha
                + "; NoH: " + (UseNoHair ? "y" : "n")
                + " ; Alt: " + (HasAlts? "y":"n");
        }
    }

    public class PetAccessoryBowtie : PetAccessoryItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Bowtie");
            Tooltip.SetDefault("'A soft bowtie for your cute slime to wear on her chest'");
        }

        protected override void MoreAddRecipes()
        {
            int[] types = new int[] {
                mod.ItemType<PetAccessoryBowtieBlack>(),
                mod.ItemType<PetAccessoryBowtieBlue>(),
                mod.ItemType<PetAccessoryBowtieGray>(),
                mod.ItemType<PetAccessoryBowtieGreen>(),
                mod.ItemType<PetAccessoryBowtieOrange>(),
                mod.ItemType<PetAccessoryBowtiePink>(),
                mod.ItemType<PetAccessoryBowtiePurple>(),
                mod.ItemType<PetAccessoryBowtieRed>(),
                mod.ItemType<PetAccessoryBowtieWhite>(),
                mod.ItemType<PetAccessoryBowtieYellow>(),
            };

            ModRecipe recipe;
            for (int i = 0; i < types.Length; i++)
            {
                recipe = new ModRecipe(mod);
                recipe.AddIngredient(types[i]);
                recipe.SetResult(this);
                recipe.AddRecipe();
            }
        }
    }

    public class PetAccessoryToyBreastplate : PetAccessoryItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Knight's Breastplate");
            Tooltip.SetDefault("'A plush knight's breastplate for your cute slime to wear on her body'");
        }
    }

    public class PetAccessoryCrown : PetAccessoryItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Crown");
            Tooltip.SetDefault("'A regal crown for your cute slime to wear on her head'");
        }

        protected override void MoreAddRecipes()
        {
            int[] types = new int[] {
                mod.ItemType<PetAccessoryCrownGold>(),
                mod.ItemType<PetAccessoryCrownPlatinum>(),
            };

            ModRecipe recipe;
            for (int i = 0; i < types.Length; i++)
            {
                recipe = new ModRecipe(mod);
                recipe.AddIngredient(types[i]);
                recipe.SetResult(this);
                recipe.AddRecipe();
            }
        }
    }

    public class PetAccessoryHairBow : PetAccessoryItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Hair Bow");
            Tooltip.SetDefault("'A large bow for your cute slime to wear on her head'");
        }

        protected override void MoreAddRecipes()
        {
            int[] types = new int[] {
                mod.ItemType<PetAccessoryHairBowBlack>(),
                mod.ItemType<PetAccessoryHairBowBlue>(),
                mod.ItemType<PetAccessoryHairBowGray>(),
                mod.ItemType<PetAccessoryHairBowGreen>(),
                mod.ItemType<PetAccessoryHairBowOrange>(),
                mod.ItemType<PetAccessoryHairBowPink>(),
                mod.ItemType<PetAccessoryHairBowPurple>(),
                mod.ItemType<PetAccessoryHairBowRed>(),
                mod.ItemType<PetAccessoryHairBowWhite>(),
                mod.ItemType<PetAccessoryHairBowYellow>(),
            };

            ModRecipe recipe;
            for (int i = 0; i < types.Length; i++)
            {
                recipe = new ModRecipe(mod);
                recipe.AddIngredient(types[i]);
                recipe.SetResult(this);
                recipe.AddRecipe();
            }
        }
    }

    public class PetAccessoryMetalHelmet : PetAccessoryItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Knight's Helmet");
            Tooltip.SetDefault("'A plush knight's helmet for your cute slime to wear on her head'");
        }
    }

    public class PetAccessorySlimeHead : PetAccessoryItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Head Slime");
            Tooltip.SetDefault("'A slime plush that sits on your cute slime's head'");
        }

        protected override void MoreAddRecipes()
        {
            int[] types = new int[] {
                mod.ItemType<PetAccessorySlimeHeadBlack>(),
                mod.ItemType<PetAccessorySlimeHeadBlue>(),
                mod.ItemType<PetAccessorySlimeHeadGreen>(),
                mod.ItemType<PetAccessorySlimeHeadPink>(),
                mod.ItemType<PetAccessorySlimeHeadPinky>(),
                mod.ItemType<PetAccessorySlimeHeadPurple>(),
                mod.ItemType<PetAccessorySlimeHeadRed>(),
                mod.ItemType<PetAccessorySlimeHeadYellow>(),
            };

            ModRecipe recipe;
            for (int i = 0; i < types.Length; i++)
            {
                recipe = new ModRecipe(mod);
                recipe.AddIngredient(types[i]);
                recipe.SetResult(this);
                recipe.AddRecipe();
            }
        }
    }

    public class PetAccessoryWizardHat : PetAccessoryItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Wizard Hat");
            Tooltip.SetDefault("'A brown wizard's hat for your cute slime to wear on her head'");
        }
    }

    public class PetAccessoryXmasHat : PetAccessoryItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Santa Hat");
            Tooltip.SetDefault("'A festive hat for your cute slime to wear'");
        }

        protected override void MoreAddRecipes()
        {
            int[] types = new int[] {
                mod.ItemType<PetAccessoryXmasHatGreen>(),
                mod.ItemType<PetAccessoryXmasHatRed>(),
            };

            ModRecipe recipe;
            for (int i = 0; i < types.Length; i++)
            {
                recipe = new ModRecipe(mod);
                recipe.AddIngredient(types[i]);
                recipe.SetResult(this);
                recipe.AddRecipe();
            }
        }
    }

    public class PetAccessoryKitchenKnife : PetAccessoryItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Kitchen Knife");
            Tooltip.SetDefault("'A plush kitchen knife for your cute slime to carry'");
        }
    }

    public class PetAccessoryStaff : PetAccessoryItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Staff");
            Tooltip.SetDefault("'A plush staff for your cute slime to carry'");
        }

        protected override void MoreAddRecipes()
        {
            int[] types = new int[] {
                mod.ItemType<PetAccessoryStaffAmber>(),
                mod.ItemType<PetAccessoryStaffAmethyst>(),
                mod.ItemType<PetAccessoryStaffDiamond>(),
                mod.ItemType<PetAccessoryStaffEmerald>(),
                mod.ItemType<PetAccessoryStaffRuby>(),
                mod.ItemType<PetAccessoryStaffSapphire>(),
                mod.ItemType<PetAccessoryStaffTopaz>(),
            };

            ModRecipe recipe;
            for (int i = 0; i < types.Length; i++)
            {
                recipe = new ModRecipe(mod);
                recipe.AddIngredient(types[i]);
                recipe.SetResult(this);
                recipe.AddRecipe();
            }
        }
    }

    public class PetAccessoryToyMace : PetAccessoryItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Paladin's Mace");
            Tooltip.SetDefault("'A plush paladin's mace for your cute slime to carry'");
        }
    }

    public class PetAccessoryToySpear : PetAccessoryItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Spartan's Spear");
            Tooltip.SetDefault("'A plush warrior's spear for your cute slime to carry'");
        }
    }

    public class PetAccessoryToySword : PetAccessoryItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Knight's Sword");
            Tooltip.SetDefault("'A plush knight's sword for your cute slime to carry'");
        }
    }

    public class PetAccessoryMittens : PetAccessoryItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Mittens");
            Tooltip.SetDefault("'Warm mittens for your cute slime's hands'");
        }

        protected override void MoreAddRecipes()
        {
            int[] types = new int[] {
                mod.ItemType<PetAccessoryMittensBlack>(),
                mod.ItemType<PetAccessoryMittensBlue>(),
                mod.ItemType<PetAccessoryMittensGray>(),
                mod.ItemType<PetAccessoryMittensGreen>(),
                mod.ItemType<PetAccessoryMittensOrange>(),
                mod.ItemType<PetAccessoryMittensPink>(),
                mod.ItemType<PetAccessoryMittensPurple>(),
                mod.ItemType<PetAccessoryMittensRed>(),
                mod.ItemType<PetAccessoryMittensWhite>(),
                mod.ItemType<PetAccessoryMittensYellow>(),
            };

            ModRecipe recipe;
            for (int i = 0; i < types.Length; i++)
            {
                recipe = new ModRecipe(mod);
                recipe.AddIngredient(types[i]);
                recipe.SetResult(this);
                recipe.AddRecipe();
            }
        }
    }

    public class PetAccessorySwallowedKey : PetAccessoryItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Swallowed Key");
            Tooltip.SetDefault("'A plush key for your cute slime to...carry?'");
        }
    }

    public class PetAccessoryToyShield : PetAccessoryItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Knight's Shield");
            Tooltip.SetDefault("'A plush knight's shield for your cute slime to carry'");
        }
    }

    public abstract class PetAccessoryItem : ModItem
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
            item.value = Item.sellPrice(silver:30);
        }

        private string Enum2string(int e)
        {
            if (e == (byte)SlotType.Hat)
            {
                return "Worn on the head";
            }
            if (e == (byte)SlotType.Body)
            {
                return "Worn on the body";
            }
            if (e == (byte)SlotType.Carried)
            {
                return "Carried";
            }
            if (e == (byte)SlotType.Accessory)
            {
                return "Worn as an accessory";
            }
            return "UNINTENDED BEHAVIOR, REPORT TO DEV! (" + e + ")";
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            tooltips.Add(new TooltipLine(mod, "slot", Enum2string((byte)APetAccessory.GetAccessoryFromType(item.type).Slot)));

            PetPlayer mPlayer = Main.LocalPlayer.GetModPlayer<PetPlayer>(mod);

            if (mPlayer.slimePetIndex != -1 &&
                Main.projectile[mPlayer.slimePetIndex].active &&
                Main.projectile[mPlayer.slimePetIndex].owner == Main.myPlayer)
            {
                if (SlimePets.slimePets.Contains(Main.projectile[mPlayer.slimePetIndex].type))
                {
                    if (SlimePets.GetPet(Main.projectile[mPlayer.slimePetIndex].type).IsSlotTypeBlacklisted[(byte)APetAccessory.GetAccessoryFromType(item.type).Slot])
                    {
                        tooltips.Add(new TooltipLine(mod, "Blacklisted", "This accessory type is disabled for your particular slime"));
                    }
                }
                else if (SlimePets.slimePetLegacy.Contains(Main.projectile[mPlayer.slimePetIndex].type))
                {
                    tooltips.Add(new TooltipLine(mod, "AllowLegacy", "Does not work on 'Legacy Appearance' pets"));
                }
            }
        }

        public sealed override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType<KnittingSet>());
            recipe.AddTile(TileID.Loom);
            recipe.SetResult(this);
            recipe.AddRecipe();

            MoreAddRecipes();
        }

        protected virtual void MoreAddRecipes()
        {

        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            PetPlayer pPlayer = player.GetModPlayer<PetPlayer>(mod);
            //no valid slime pet found
            if (!(pPlayer.slimePetIndex != -1 &&
                Main.projectile[pPlayer.slimePetIndex].active &&
                Main.projectile[pPlayer.slimePetIndex].owner == Main.myPlayer &&
                SlimePets.slimePets.Contains(Main.projectile[pPlayer.slimePetIndex].type) &&
                !SlimePets.slimePetLegacy.Contains(Main.projectile[pPlayer.slimePetIndex].type)))
            {
                return false;
            }

            //if a right click, enable usage
            if (player.altFunctionUse == 2) return true;
            //if a left click and no alts, enable usage
            else if (!APetAccessory.GetAccessoryFromType(item.type).HasAlts) return true;
            //else disable (if it has alts when left clicked)
            return false;
        }

        public override bool UseItem(Player player)
        {
            PetPlayer pPlayer = player.GetModPlayer<PetPlayer>(mod);

            if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
            {
                if (pPlayer.slimePetIndex == -1)
                {
                    //find first occurence of a player owned cute slime
                    for (int i = 0; i < 1000; i++)
                    {
                        if (Main.projectile[i].active)
                        {
                            if (Main.projectile[i].modProjectile != null)
                            {
                                if (SlimePets.slimePets.Contains(Main.projectile[i].type) &&
                                    Main.projectile[i].owner == Main.myPlayer &&
                                    !SlimePets.slimePetLegacy.Contains(Main.projectile[i].type))
                                {
                                    ErrorLogger.Log("had to change index of slime pet of " + player.name + " because it was -1");
                                    pPlayer.slimePetIndex = i;
                                    return true;
                                }
                            }
                        }
                    }
                }

                APetAccessory petAccessory = APetAccessory.GetAccessoryFromType(item.type);

                bool shouldReset = false;
                if (player.altFunctionUse == 2) //right click use
                {
                    if (pPlayer.ThreeTimesUseTime(Main.time)) //true after three right clicks in 60 ticks
                    {
                        shouldReset = true;
                    }
                }
                //else normal left click use

                if (pPlayer.slimePetIndex != -1 &&
                    Main.projectile[pPlayer.slimePetIndex].active &&
                    Main.projectile[pPlayer.slimePetIndex].owner == Main.myPlayer &&
                    SlimePets.slimePets.Contains(Main.projectile[pPlayer.slimePetIndex].type) &&
                    !SlimePets.slimePetLegacy.Contains(Main.projectile[pPlayer.slimePetIndex].type))
                {
                    //only client side
                    if (Main.netMode != NetmodeID.Server)
                    {
                        if (shouldReset && player.altFunctionUse == 2)
                        {
                            if (pPlayer.slots != 0)
                            {
                                pPlayer.slotsLast = pPlayer.slots;
                                pPlayer.slots = 0;
                            }
                            else
                            {
                                pPlayer.slots = pPlayer.slotsLast;
                                pPlayer.slotsLast = 0;
                            }

                            //"dust" originating from the center, forming a circle and going outwards
                            Dust dust;
                            for (double angle = 0; angle < Math.PI * 2; angle += Math.PI / 6)
                            {
                                dust = Dust.NewDustPerfect(Main.projectile[pPlayer.slimePetIndex].Center - new Vector2(0f, Main.projectile[pPlayer.slimePetIndex].height / 4), 16, new Vector2((float)-Math.Cos(angle), (float)Math.Sin(angle)) * 1.2f, 0, new Color(255, 255, 255), 1.6f);
                            }
                        }
                        else if (player.altFunctionUse != 2)
                        {
                            if (!SlimePets.GetPet(Main.projectile[pPlayer.slimePetIndex].type).IsSlotTypeBlacklisted[(int)petAccessory.Slot])
                            {
                                pPlayer.ToggleAccessory(petAccessory);
                            }
                        }
                    }
                }
            }
            return true;
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
            //item.useAnimation = 16;
            //item.useTime = 16;
            //item.useStyle = 4;
            //item.UseSound = SoundID.Item1;
            item.consumable = false;
            item.value = (int)SlotType.Body;
            MoreSetDefaults();
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            tooltips.Add(new TooltipLine(mod, "Discontinued", "Discontinued, craft it into the new version"));
        }

        protected virtual void MoreSetDefaults()
        {

        }
    }
}
