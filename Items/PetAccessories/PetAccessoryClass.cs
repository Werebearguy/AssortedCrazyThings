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

    public class PetAccessoryBunnyEars : PetAccessoryItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Bunny Ears");
            Tooltip.SetDefault("'A pair of Easter bunny ears for your cute slime to wear on her head'");
        }
    }

    public class PetAccessoryBunnySuit : PetAccessoryItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Bunny Suit");
            Tooltip.SetDefault("'A bunny(?) costume for your cute slime to wear on her chest'");
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

    public class PetAccessoryMetalHelmet : PetAccessoryItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Knight's Helmet");
            Tooltip.SetDefault("'A plush knight's helmet for your cute slime to wear on her head'");
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

    public class PetAccessoryHairBow : PetAccessoryItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Hair Bow");
            Tooltip.SetDefault("'A large bow for your cute slime to wear on her head'");
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

    public class PetAccessorySlimeHead : PetAccessoryItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Head Slime");
            Tooltip.SetDefault("'A slime plush that sits on your cute slime's head'");
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

    public class PetAccessorySwallowedKey : PetAccessoryItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Swallowed Key");
            Tooltip.SetDefault("'A plush key for your cute slime to...carry?'");
        }

        protected override bool UseDefaultRecipe => false;

        protected override void SafeAddRecipes()
        {
            Recipe recipe = Mod.CreateRecipe(ItemID.GoldenKey);
            recipe.AddIngredient(ModContent.ItemType<PetAccessorySwallowedKey>());
            recipe.Register();
        }
    }

    public class PetAccessoryTophat : PetAccessoryItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Top Hat");
            Tooltip.SetDefault("'A classy top hat for your cute slime to wear on her head'");
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

    public class PetAccessoryToyShield : PetAccessoryItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Knight's Shield");
            Tooltip.SetDefault("'A plush knight's shield for your cute slime to carry'");
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
            Tooltip.SetDefault("'A festive hat for your cute slime to wear on hear head'");
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
        private static Dictionary<SlotType, List<PetAccessory>> petAccessoriesByType;
        private static Dictionary<SlotType, List<int>> petAccessoryIdsByType;

        /// <summary>
        /// Look-up table where the key is item ID and it returns the corresponding PetAccessory
        /// </summary>
        private static Dictionary<int, PetAccessory> petAccessoriesByItem;

        /// <summary>
        /// Contains all pet vanity accessories
        /// </summary>
        private static List<PetAccessory> petAccessoryList;

        /// <summary>
        /// Unique ID for this accessory (unique in the scope of a single SlotType)
        /// </summary>
        public byte ID { private set; get; } //The internal ID of the accessory
        public string Name { private set; get; }
        public int Type { private set; get; } //The item type associated with the accessory
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

        public PetAccessory(byte id, string name, float offsetX = 0f, float offsetY = 0f, bool preDraw = false, byte alpha = 0, bool useNoHair = false, List<string> altTextures = null)
        {
            if (id == 0) throw new Exception("Invalid ID '0', start with 1");
            ID = id;
            Name = "PetAccessory" + name;
            Type = AssUtils.Instance.Find<ModItem>(Name).Type;
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
                AltTextures.Add(Main.dedServ ? null : AssUtils.Instance.GetTexture("Items/PetAccessories/" + Name + AltTextureSuffixes[i]).Value);
            }
        }

        /// <summary>
        /// Called in Mod.Load
        /// </summary>
        public static void Load()
        {
            petAccessoriesByType = new Dictionary<SlotType, List<PetAccessory>>();
            petAccessoryIdsByType = new Dictionary<SlotType, List<int>>();
            petAccessoriesByItem = new Dictionary<int, PetAccessory>();

            petAccessoryList = new List<PetAccessory>();

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
            Add(SlotType.Body, new PetAccessory(id: 2, name: "ToyBreastplate", altTextures: new List<string>() { "Iron", "Gold" }));
            Add(SlotType.Body, new PetAccessory(id: 3, name: "BunnySuit", altTextures: new List<string>() { "Black", "Orange", "Gold", "Yellow", "Green", "Blue", "Purple", "Pink", "White", "Gray", "Red"}));

            //HAT SLOT ACCESSORIES GO HERE, SEPARATE IDs
            //------------------------------------------------
            Add(SlotType.Hat, new PetAccessory(id: 1, name: "Crown", altTextures: new List<string>() { "Gold", "Platinum" }));
            Add(SlotType.Hat, new PetAccessory(id: 2, name: "HairBow", altTextures: new List<string>() { "Red", "Orange", "Gold", "Yellow", "Green", "Blue", "Purple", "Pink", "White", "Gray", "Black" }));
            Add(SlotType.Hat, new PetAccessory(id: 3, name: "MetalHelmet", useNoHair: true, altTextures: new List<string>() { "Iron", "Gold" }));
            Add(SlotType.Hat, new PetAccessory(id: 4, name: "SlimeHead", alpha: 56, altTextures: new List<string>() { "Blue", "Purple", "Pink", "Pinky", "Red", "Yellow", "Green", "Black" }));
            Add(SlotType.Hat, new PetAccessory(id: 5, name: "WizardHat", useNoHair: true));
            Add(SlotType.Hat, new PetAccessory(id: 6, name: "XmasHat", useNoHair: true, altTextures: new List<string>() { "Red", "Green" }));
            Add(SlotType.Hat, new PetAccessory(id: 7, name: "BunnyEars", preDraw: true));
            Add(SlotType.Hat, new PetAccessory(id: 8, name: "Tophat"));


            //CARRIED SLOT ACCESSORIES GO HERE, SEPARATE IDs
            //------------------------------------------------
            Add(SlotType.Carried, new PetAccessory(id: 1, name: "KitchenKnife", preDraw: true, altTextures: new List<string>() { "Iron", "Gold" }));
            Add(SlotType.Carried, new PetAccessory(id: 2, name: "Staff", preDraw: true, altTextures: new List<string>() { "Amethyst", "Sapphire", "Emerald", "Ruby", "Amber", "Topaz", "Diamond", "Magic" }));
            Add(SlotType.Carried, new PetAccessory(id: 3, name: "ToyMace", preDraw: true, altTextures: new List<string>() { "Iron", "Gold" }));
            Add(SlotType.Carried, new PetAccessory(id: 4, name: "ToySpear", preDraw: true, altTextures: new List<string>() { "Iron", "Gold" }));
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
            petAccessoriesByType = null;
            petAccessoryIdsByType = null;
            petAccessoriesByItem = null;

            petAccessoryList = null;
        }

        /// <summary>
        /// Called in Load, creates a map of pet vanity accessory -> ID for each SlotType
        /// </summary>
        public static void CreateMaps()
        {
            foreach (SlotType slotType in Enum.GetValues(typeof(SlotType)))
            {
                if (slotType != SlotType.None)
                {
                    List<PetAccessory> tempAccessoryList = petAccessoriesByType[slotType];

                    if (!petAccessoryIdsByType.ContainsKey(slotType))
                    {
                        petAccessoryIdsByType[slotType] = new List<int>();
                    }

                    List<int> tempIdList = petAccessoryIdsByType[slotType];
                    for (int i = 0; i < tempAccessoryList.Count; i++)
                    {
                        tempIdList.Add(tempAccessoryList[i].ID);
                    }
                }
            }

            for (int i = 0; i < petAccessoryList.Count; i++)
            {
                PetAccessory petAccessory = petAccessoryList[i];
                petAccessoriesByItem[petAccessory.Type] = petAccessory;
            }
        }

        /// <summary>
        /// Add a new pet vanity accessory to the list
        /// </summary>
        public static void Add(SlotType slotType, PetAccessory aPetAccessory)
        {
            for (int i = 0; i < petAccessoryList.Count; i++)
            {
                PetAccessory petAccessory = petAccessoryList[i];
                if (petAccessory.Name == aPetAccessory.Name)
                    throw new Exception("Added Accessory '" + aPetAccessory.Name + "' already exists");

                if (petAccessory.Slot == slotType && petAccessory.ID == aPetAccessory.ID)
                    throw new Exception("ID '" + aPetAccessory.ID + "' in Slot '" + aPetAccessory.Slot.ToString() + "' for '" + aPetAccessory.Name + "' already registered for '" + petAccessory.Name + "'");
            }

            aPetAccessory.Slot = slotType;
            if (slotType == SlotType.None) throw new Exception("There has to be a slot specified as the first argument in 'Add()'");

            if (!petAccessoriesByType.ContainsKey(slotType))
            {
                petAccessoriesByType[slotType] = new List<PetAccessory>();
            }

            //everything fine
            petAccessoriesByType[slotType].Add(aPetAccessory);

            petAccessoryList.Add(aPetAccessory);
        }

        /// <summary>
        /// Called in PetPlayer.GetAccessoryInSlot.
        /// Used to retreive the pet vanity accessory in the current slot of that current ID
        /// </summary>
        public static PetAccessory GetAccessoryFromID(SlotType slotType, byte id) //if something has the id, it always has the slottype available
        {
            return petAccessoriesByType[slotType][petAccessoryIdsByType[slotType].IndexOf(id)];
        }

        /// <summary>
        /// Returns the pet vanity accessory corresponding to the item type.
        /// </summary>
        public static bool TryGetAccessoryFromItem(int type, out PetAccessory petAccessory) //since item types are unique, just look up in the global list
        {
            return petAccessoriesByItem.TryGetValue(type, out petAccessory);
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
    [Autoload]
    public abstract class PetAccessoryItem : AssItem
    {
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 30;
            Item.maxStack = 1;
            Item.rare = -11;
            Item.useAnimation = 16;
            Item.useTime = 16;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.UseSound = SoundID.Item1;
            Item.consumable = false;
            Item.value = Item.sellPrice(silver: 30);
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
            if (PetAccessory.TryGetAccessoryFromItem(Item.type, out PetAccessory petAccessory))
            {
                tooltips.Add(new TooltipLine(Mod, "Slot", Enum2string(petAccessory.Slot)));

                Player player = Main.LocalPlayer;
                PetPlayer pPlayer = player.GetModPlayer<PetPlayer>();

                if (pPlayer.HasValidSlimePet(out SlimePet slimePet))
                {
                    if (slimePet.IsSlotTypeBlacklisted[(byte)petAccessory.Slot])
                    {
                        tooltips.Add(new TooltipLine(Mod, "Blacklisted", "This accessory type is disabled for your particular slime"));
                    }
                }
                else if (player.HasItem(Item.type))
                {
                    tooltips.Add(new TooltipLine(Mod, "NoUse", "You have no summoned slime to equip this on")
                    {
                        overrideColor = Color.OrangeRed
                    });
                }
            }
            else
            {
                tooltips.Add(new TooltipLine(Mod, "Disabled", "This accessory type is not registered for use by the devs"));
            }
        }

        protected virtual bool UseDefaultRecipe => true;

        public sealed override void AddRecipes()
        {
            if (UseDefaultRecipe)
            {
                Recipe recipe = CreateRecipe();
                recipe.AddIngredient(ModContent.ItemType<KnittingSet>());
                recipe.AddTile(TileID.Loom);
                recipe.Register();
            }

            SafeAddRecipes();
        }

        protected virtual void SafeAddRecipes()
        {

        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            //item not registered
            if (!PetAccessory.TryGetAccessoryFromItem(Item.type, out PetAccessory petAccessory)) return false;

            PetPlayer pPlayer = player.GetModPlayer<PetPlayer>();
            //no valid slime pet found
            if (!pPlayer.HasValidSlimePet(out _)) return false;

            //if a right click, enable usage
            if (player.altFunctionUse == 2) return true;
            //if a left click and no alts, enable usage
            else if (!petAccessory.HasAlts) return true;
            //else disable (if it has alts when left clicked)
            return false;
        }

        public override bool UseItem(Player player)
        {
            PetPlayer pPlayer = player.GetModPlayer<PetPlayer>();

            if (pPlayer.slimePetIndex < 0)
            {
                return true;
            }

            if (player.whoAmI == Main.myPlayer && player.itemTime == 0 && PetAccessory.TryGetAccessoryFromItem(Item.type, out PetAccessory petAccessory))
            {
                bool shouldReset = false;
                if (player.altFunctionUse == 2) //right click use
                {
                    if (pPlayer.ThreeTimesUseTime()) //true after three right clicks in 60 ticks
                    {
                        shouldReset = true;
                    }
                }
                //else normal left click use

                if (pPlayer.HasValidSlimePet(out SlimePet slimePet))
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

                            Projectile projectile = Main.projectile[pPlayer.slimePetIndex];
                            //"dust" originating from the center, forming a circle and going outwards
                            for (double angle = 0; angle < MathHelper.TwoPi; angle += Math.PI / 6)
                            {
                                Dust.NewDustPerfect(projectile.Center - new Vector2(0f, projectile.height / 4), 16, new Vector2((float)-Math.Cos(angle), (float)Math.Sin(angle)) * 1.2f, 0, new Color(255, 255, 255), 1.6f);
                            }
                        }
                        else if (player.altFunctionUse != 2)
                        {
                            if (!slimePet.IsSlotTypeBlacklisted[(int)petAccessory.Slot])
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
