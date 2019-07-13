using AssortedCrazyThings.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.PetAccessories
{
    //Add new classes here in alphabetic order

    public class PetAccessoryBowtie : PetAccessoryItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Bowtie");
            Tooltip.SetDefault("'A soft bowtie for your cute slime to wear on her chest'");
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

    public class PetAccessoryBunnyEars : PetAccessoryItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Bunny Ears");
            Tooltip.SetDefault("'A pair of Easter bunny ears for your cute slime to wear on her head'");
        }
    }

    public class PetAccessoryCrown : PetAccessoryItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Crown");
            Tooltip.SetDefault("'A regal crown for your cute slime to wear on her head'");
        }
    }

    public class PetAccessoryHairBow : PetAccessoryItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Hair Bow");
            Tooltip.SetDefault("'A large bow for your cute slime to wear on her head'");
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
    }

    public class PetAccessorySwallowedKey : PetAccessoryItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Swallowed Key");
            Tooltip.SetDefault("'A plush key for your cute slime to...carry?'");
        }

        protected override bool UseDefaultRecipe { get { return false; } }

        protected override void MoreAddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType<PetAccessorySwallowedKey>());
            recipe.SetResult(ItemID.GoldenKey);
            recipe.AddRecipe();
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

    /// <summary>
    /// The type of pet vanity it is. Used for rendering, has separate lists
    /// </summary>
    public enum SlotType : byte
    {
        None = 0, //reserved
        Body = 1,
        Hat = 2,
        Carried = 3,
        Accessory = 4

        //for Carried, it's actually only the front facing hand. For something like gloves or dual wielding, use Accessory instead

        //also, keep the sprite dimensions the same as the cute slimes

        //also they will be rendered in this order aswell (means that Carried can overlap with Body)

        //Each slot can have up to 255 accessories (total of 1020)
    }

    /// <summary>
    /// Contains data about all pet vanity accessories
    /// </summary>
    public class PetAccessory
    {
        /// <summary>
        /// Contains all pet vanity accessories
        /// </summary>
        public static List<PetAccessory> petAccessoryListGlobal;
        /// <summary>
        /// Contains all pet vanity accessories of SlotType.Body
        /// </summary>
        public static List<PetAccessory> petAccessoryListB;
        /// <summary>
        /// Contains all pet vanity accessories of SlotType.Head
        /// </summary>
        public static List<PetAccessory> petAccessoryListH;
        /// <summary>
        /// Contains all pet vanity accessories of SlotType.Carried
        /// </summary>
        public static List<PetAccessory> petAccessoryListC;
        /// <summary>
        /// Contains all pet vanity accessories of SlotType.Accessory
        /// </summary>
        public static List<PetAccessory> petAccessoryListA;
        /// <summary>
        /// Contains IDs of pet vanity accessories of SlotType.Body
        /// </summary>
        public static List<int> petAccessoryIdsB;
        /// <summary>
        /// Contains IDs of pet vanity accessories of SlotType.Head
        /// </summary>
        public static List<int> petAccessoryIdsH;
        /// <summary>
        /// Contains IDs of pet vanity accessories of SlotType.Carried
        /// </summary>
        public static List<int> petAccessoryIdsC;
        /// <summary>
        /// Contains IDs of pet vanity accessories of SlotType.Accessory
        /// </summary>
        public static List<int> petAccessoryIdsA;
        /// <summary>
        /// Look-up table where the index is the ID and it returns the corresponding item type
        /// </summary>
        public static List<int> petAccessoryTypesGlobal;

        /// <summary>
        /// Unique ID for this accessory (unique in the scope of a single SlotType)
        /// </summary>
        public byte ID { private set; get; }
        public string Name { private set; get; }
        public int Type { private set; get; }
        public SlotType Slot { private set; get; }
        private byte _color;
        /// <summary>
        /// Index for AltTextureSuffixes. No private setter, since it is set directly when used
        /// </summary>
        public byte Color
        {
            set
            {
                _color = value;
            }
            get
            {
                return Utils.Clamp(_color, (byte)0, (byte)(AltTextureSuffixes.Count - 1));
            }
        }
        public Vector2 Offset { private set; get; }
        public bool PreDraw { private set; get; }
        public byte Alpha { private set; get; }
        public bool UseNoHair { private set; get; }

        /// <summary>
        /// For deciding if it has a UI or not
        /// </summary>
        public bool HasAlts { private set; get; }
        /// <summary>
        /// For UI tooltips
        /// </summary>
        public List<string> AltTextureSuffixes { private set; get; }
        /// <summary>
        /// For UI only, the _Draw{number} stuff is done manually
        /// </summary>
        public List<Texture2D> AltTextures { private set; get; }
        /// <summary>
        /// The number in _Draw{number}
        /// </summary>
        public List<sbyte> PetVariations { private set; get; }

        public PetAccessory(byte id, string name, float offsetX = 0f, float offsetY = 0f, bool preDraw = false, byte alpha = 0, bool useNoHair = false, List<string> altTextures = null)
        {
            if (id == 0) throw new Exception("Invalid ID '0', start with 1");
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

        /// <summary>
        /// Shorter overload of AddPetVariation that takes a list
        /// </summary>
        public PetAccessory AddPetVariation(string[] petNames, sbyte number)
        {
            for (int i = 0; i < petNames.Length; i++)
            {
                AddPetVariation(petNames[i], number);
            }
            return this;
        }

        /// <summary>
        /// Adds a color variation for the current pet vanity accessory.
        /// -1: Accessory won't be drawn.
        /// 0 (default): Use default texture.
        /// 1..127: Use texture specified by _Draw{number}
        /// </summary>
        public PetAccessory AddPetVariation(string petName, sbyte number)
        {
            int type = AssUtils.Instance.ProjectileType("CuteSlime" + petName + "NewProj");
            if (SlimePets.slimePets.IndexOf(type) < 0) throw new Exception("Slime pet of type 'CuteSlime" + petName + "NewProj' not registered in SlimePets.Load()");
            PetVariations[SlimePets.slimePets.IndexOf(type)] = number;
            return this;
        }

        /// <summary>
        /// Called in Mod.Load
        /// </summary>
        public static void Load()
        {
            petAccessoryListGlobal = new List<PetAccessory>();
            petAccessoryListB = new List<PetAccessory>();
            petAccessoryListH = new List<PetAccessory>();
            petAccessoryListC = new List<PetAccessory>();
            petAccessoryListA = new List<PetAccessory>();
            //-------------------------------------------------------------------
            //------------ADD PET ACCESSORY PROPERTIES HERE----------------------
            //-------------------------------------------------------------------
            /*
             * How to:
             * - call Add() with the appropriate parameters in its appropriate place (by slots)
             * - watch out for brackets, they're finnicky here
             * - the first parameter of Add() is always the SlotType, the second one is a 'PetAccessory' object, which has the following parameters:
             *   * id: set that to the next highest ID thats specified in the other Add() calls for that particular SlotType
             *   * name: the name of the class without the 'PetAccessory' infront, to save space
             *   * offsetX/Y: self explanatory, remember, negative X is left, negative Y is up
             *   * preDraw: decides if that accessory should be drawn "behind" the actual slime (false means it will draw infront)
             *   * alpha: says by how much it should be transparent (0 is fully opaque, 255 fully transparent)
             *   * useNoHair: used for SlotType.Hat, if the accessory should cover the hair and should use a NoHair texture of the slime if available
             *   * altTextures: a List of names that denotes the selection options for the UI
             *   * For each `altTexture` thing you specify in the list, you need to add a texture (for the UI) suffixed with that thing,
             *     and a `_Draw` texture (there will be a duplicate icon for the default item)
             *     
             * - after you've done that, add a `_Draw` texture with the same name as the item you add, now the item should work
             *
             * 
             * - if you want to add alternative textures based on the pet they are on (Suffixed with _Draw<identifyingNumber>), call AddPetVariation()
             * on the PetAccessory object (watch the brackets) and assign each pet a texture to use
             * (-1 is "not rendered", 0 is "default, > 0 is "use _Draw<identifyingNumber> texture")
             * you can leave the other pet types out if you only need to adjust the texture of one pet
             * 
             * - if you want to remove certain accessories from being usable for the system, comment the Add() call out with //
             */
            //START WITH ID: 1

            //DONT CHANGE UP THE ORDER OF THE COLORS, IT'LL MESS THINGS UP (but not badly)
            //BODY SLOT ACCESSORIES GO HERE, SEPARATE IDs
            //------------------------------------------------
            Add(SlotType.Body, new PetAccessory(id: 1, name: "Bowtie", altTextures: new List<string>() { "Red", "Orange", "Gold", "Yellow", "Green", "Blue", "Purple", "Pink", "White", "Gray", "Black" }));
            Add(SlotType.Body, new PetAccessory(id: 2, name: "ToyBreastplate", altTextures: new List<string>() { "Iron", "Gold" })
                 .AddPetVariation("Xmas", 1)
                 );

            //HAT SLOT ACCESSORIES GO HERE, SEPARATE IDs
            //------------------------------------------------
            Add(SlotType.Hat, new PetAccessory(id: 1, name: "Crown", altTextures: new List<string>() { "Gold", "Platinum" })
                .AddPetVariation("Pink", 1)
                .AddPetVariation("Yellow", 2)
                .AddPetVariation("Dungeon", 3)
                );
            Add(SlotType.Hat, new PetAccessory(id: 2, name: "HairBow", altTextures: new List<string>() { "Red", "Orange", "Gold", "Yellow", "Green", "Blue", "Purple", "Pink", "White", "Gray", "Black" }));
            Add(SlotType.Hat, new PetAccessory(id: 3, name: "MetalHelmet", offsetY: -2f, useNoHair: true, altTextures: new List<string>() { "Iron", "Gold" })
                .AddPetVariation("Green", 1)
                .AddPetVariation("Jungle", 2)
                .AddPetVariation("Xmas", 3)
                );
            Add(SlotType.Hat, new PetAccessory(id: 4, name: "SlimeHead", offsetY: -14f, alpha: 56, altTextures: new List<string>() { "Blue", "Purple", "Pink", "Pinky", "Red", "Yellow", "Green", "Black" }));
            Add(SlotType.Hat, new PetAccessory(id: 5, name: "WizardHat", offsetY: -10f, useNoHair: true)
                .AddPetVariation(new string[] { "Black", "Corrupt", "Dungeon", "Purple", "Toxic" }, 1)
                .AddPetVariation("Pink", 2)
                );
            Add(SlotType.Hat, new PetAccessory(id: 6, name: "XmasHat", offsetY: -4f, useNoHair: true, altTextures: new List<string>() { "Red", "Green" }));
            Add(SlotType.Hat, new PetAccessory(id: 7, name: "BunnyEars", preDraw: true, offsetY: -12f));

            //CARRIED SLOT ACCESSORIES GO HERE, SEPARATE IDs
            //------------------------------------------------
            Add(SlotType.Carried, new PetAccessory(id: 1, name: "KitchenKnife", preDraw: true, altTextures: new List<string>() { "Iron", "Gold" }));
            Add(SlotType.Carried, new PetAccessory(id: 2, name: "Staff", offsetX: -8f, preDraw: true, altTextures: new List<string>() { "Amethyst", "Sapphire", "Emerald", "Ruby", "Amber", "Topaz", "Diamond" }));
            Add(SlotType.Carried, new PetAccessory(id: 3, name: "ToyMace", offsetX: -4f, preDraw: true, altTextures: new List<string>() { "Iron", "Gold" }));
            Add(SlotType.Carried, new PetAccessory(id: 4, name: "ToySpear", offsetX: -8f, preDraw: true, altTextures: new List<string>() { "Iron", "Gold" }));
            Add(SlotType.Carried, new PetAccessory(id: 5, name: "ToySword", preDraw: true, altTextures: new List<string>() { "Iron", "Gold" }));

            //ACCESSORY SLOT ACCESSORIES GO HERE, SEPARATE IDs
            //------------------------------------------------
            Add(SlotType.Accessory, new PetAccessory(id: 1, name: "Mittens", altTextures: new List<string>() { "Red", "Orange", "Gold", "Yellow", "Green", "Blue", "Purple", "Pink", "White", "Gray", "Black" }));
            Add(SlotType.Accessory, new PetAccessory(id: 2, name: "SwallowedKey", preDraw: true));
            Add(SlotType.Accessory, new PetAccessory(id: 3, name: "ToyShield", altTextures: new List<string>() { "Iron", "Gold" }));

            CreateMaps();
        }

        /// <summary>
        /// Called in Mod.Unload
        /// </summary>
        public static void Unload()
        {
            petAccessoryListGlobal = null;
            petAccessoryListB = null;
            petAccessoryListH = null;
            petAccessoryListC = null;
            petAccessoryListA = null;
            petAccessoryIdsB = null;
            petAccessoryIdsH = null;
            petAccessoryIdsC = null;
            petAccessoryIdsA = null;
            petAccessoryTypesGlobal = null;
        }

        /// <summary>
        /// Called in Load, creates a map of pet vanity accessory -> ID for each SlotType
        /// </summary>
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
                    List<PetAccessory> tempAccessoryList = GetAccessoryListFromType(slotType);
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

        /// <summary>
        /// Returns the specific list that only contains pet vanity accessories of the specified SlotType
        /// </summary>
        private static List<PetAccessory> GetAccessoryListFromType(SlotType slotType)
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
                case SlotType.None:
                default:
                    //unused
                    return petAccessoryListGlobal;
            }
        }

        /// <summary>
        /// Returns a list that only contains IDs of pet vanity accessories of the specified SlotType
        /// </summary>
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
                case SlotType.None:
                default:
                    throw new Exception("Invalid slottype " + slotType);
            }
        }

        /// <summary>
        /// Add a new pet vanity accessory to the list
        /// </summary>
        public static void Add(SlotType slotType, PetAccessory aPetAccessory)
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
            GetAccessoryListFromType(slotType).Add(aPetAccessory);

            petAccessoryListGlobal.Add(aPetAccessory);
        }

        /// <summary>
        /// Called in PetPlayer.GetAccessoryInSlot.
        /// Used to retreive the pet vanity accessory in the current slot of that current ID
        /// </summary>
        public static PetAccessory GetAccessoryFromID(SlotType slotType, byte id) //if something has the id, it always has the slottype available
        {
            return GetAccessoryListFromType(slotType)[GetIdListFromType(slotType).IndexOf(id)];
        }

        /// <summary>
        /// Returns the pet vanity accessory corresponding to the item type
        /// </summary>
        public static PetAccessory GetAccessoryFromType(int type) //since item types are unique, just look up in the global list
        {
            return petAccessoryListGlobal[petAccessoryTypesGlobal.IndexOf(type)];
        }

        /// <summary>
        /// Checks if the item is a registered PetVanity, and if it has multiple variants when attempting to open it as a UI
        /// </summary>
        public static bool IsItemAPetVanity(int type, bool forUI = false)
        {
            for (int i = 0; i < petAccessoryListGlobal.Count; i++)
            {
                if (petAccessoryListGlobal[i].Type == type && (forUI ? petAccessoryListGlobal[i].HasAlts : true)) return true;
            }
            return false;
        }

        public override string ToString()
        {
            return "ID: " + ID
                + "; Col: " + Color
                + "; Slo: " + Slot
                + "; Nam: " + Name
                + "; Typ: " + Type
                + "; Off: " + Offset
                + "; Pre: " + (PreDraw ? "y" : "n")
                + "; Alp: " + Alpha
                + "; NoH: " + (UseNoHair ? "y" : "n")
                + "; Alt: " + (HasAlts ? "y" : "n");
        }
    }

    /// <summary>
    /// Class that all vanity accessories inherit from. Provides the functionality.
    /// Has a default recipe which can be changed
    /// </summary>
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
            item.value = Item.sellPrice(silver: 30);
        }

        private string Enum2string(SlotType e)
        {
            switch (e)
            {
                case SlotType.Body:
                    return "Worn on the body";
                case SlotType.Hat:
                    return "Worn on the head";
                case SlotType.Carried:
                    return "Carried";
                case SlotType.Accessory:
                    return "Worn as an accessory";
                case SlotType.None:
                default:
                    return "UNINTENDED BEHAVIOR, REPORT TO DEV! (" + e + ")";
            }
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            if (PetAccessory.IsItemAPetVanity(item.type))
            {
                tooltips.Add(new TooltipLine(mod, "slot", Enum2string(PetAccessory.GetAccessoryFromType(item.type).Slot)));

                PetPlayer mPlayer = Main.LocalPlayer.GetModPlayer<PetPlayer>(mod);

                if (mPlayer.slimePetIndex != -1 &&
                    Main.projectile[mPlayer.slimePetIndex].active &&
                    Main.projectile[mPlayer.slimePetIndex].owner == Main.myPlayer)
                {
                    if (SlimePets.slimePets.Contains(Main.projectile[mPlayer.slimePetIndex].type))
                    {
                        if (SlimePets.GetPet(Main.projectile[mPlayer.slimePetIndex].type).IsSlotTypeBlacklisted[(byte)PetAccessory.GetAccessoryFromType(item.type).Slot])
                        {
                            tooltips.Add(new TooltipLine(mod, "Blacklisted", "This accessory type is disabled for your particular slime"));
                        }
                    }
                }
            }
            else
            {
                tooltips.Add(new TooltipLine(mod, "Disabled", "This accessory type is not registered for use by the devs"));
            }
        }

        protected virtual bool UseDefaultRecipe { get { return true; } }

        public sealed override void AddRecipes()
        {
            if (UseDefaultRecipe)
            {
                ModRecipe recipe = new ModRecipe(mod);
                recipe.AddIngredient(mod.ItemType<KnittingSet>());
                recipe.AddTile(TileID.Loom);
                recipe.SetResult(this);
                recipe.AddRecipe();
            }

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
            //item not registered
            if (!PetAccessory.IsItemAPetVanity(item.type)) return false;

            PetPlayer pPlayer = player.GetModPlayer<PetPlayer>(mod);
            //no valid slime pet found
            if (!(pPlayer.slimePetIndex != -1 &&
                Main.projectile[pPlayer.slimePetIndex].active &&
                Main.projectile[pPlayer.slimePetIndex].owner == Main.myPlayer &&
                SlimePets.slimePets.Contains(Main.projectile[pPlayer.slimePetIndex].type))) return false;

            //if a right click, enable usage
            if (player.altFunctionUse == 2) return true;
            //if a left click and no alts, enable usage
            else if (!PetAccessory.GetAccessoryFromType(item.type).HasAlts) return true;
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
                                    Main.projectile[i].owner == Main.myPlayer)
                                {
                                    mod.Logger.Debug("Had to change index of slime pet of " + player.name + " because it was -1");
                                    pPlayer.slimePetIndex = i;
                                    return true;
                                }
                            }
                        }
                    }
                }

                PetAccessory petAccessory = PetAccessory.GetAccessoryFromType(item.type);

                bool shouldReset = false;
                if (player.altFunctionUse == 2) //right click use
                {
                    if (pPlayer.ThreeTimesUseTime()) //true after three right clicks in 60 ticks
                    {
                        shouldReset = true;
                    }
                }
                //else normal left click use

                if (pPlayer.slimePetIndex != -1 &&
                    Main.projectile[pPlayer.slimePetIndex].active &&
                    Main.projectile[pPlayer.slimePetIndex].owner == Main.myPlayer &&
                    SlimePets.slimePets.Contains(Main.projectile[pPlayer.slimePetIndex].type))
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
}
