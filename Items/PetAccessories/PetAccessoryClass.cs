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
     * finally, go into Load(Mod mod), and follow the instructions there
     * 
     * 
     * suggestion for names : prefixed with "Cute", so its easy to find in recipe browser 
     * 
     * 
     */

    //this is the class that holds all the properties about the accessories, like texture, offset etc
    public class PetAccessories
    {
        //internal fields
        private static Mod InternalMod;
        internal static int[] Items;
        internal static byte addCounter = 0; //if you ever add more than 255 accessories, make that a short
        internal static string[] namesOfAccessories;

        //public fields
        public static int[] ItemsIndexed; //used in ToggleAccessory
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
             * - Game will throw you an error if the namesOfAccessories length and the added number of accessories is different
             * 
             * - if you want to remove certain accessories from being useable for the system, comment the line out in namesOfAccessories
             * (for example <slash star> "PetAccessoryAmethystStaff", <star slash>), and comment the corresponding Add line (with //)
             * 
             * - if you want to disable the whole system, comment the PetAccessories.Load() and PetAccessories.Unload() in AssortedCrazyThings.cs
             */

            //this is needed before you call Add() (it needs to know the total number of accessories)
            //order doesn't matter
            namesOfAccessories = new string[]
            {
                "PetAccessoryBow",
                "PetAccessoryBowGreen",
                "PetAccessoryBowBlack",
                "PetAccessoryBowBlue",
                "PetAccessoryXmasHat",
                "PetAccessoryAmethystStaff",
                "PetAccessoryTopazStaff",
                "PetAccessorySlimeHead",
                "PetAccessoryBlueMittens",
                "PetAccessoryRedMittens",
                "PetAccessoryGrayMittens",
            };

            Init(mod, namesOfAccessories);

            //signature looks like this: Add(string name, float offsetX = 0f, float offsetY = 0f, bool preDraw = false, byte alpha = 0)
            //the "= something" is a default, if you dont specify that parameter it will assume it is that "something"

            //name is the string name of the class you specify above
            //offsetX/Y is self explanatory, remember, negative X is left, negative Y is up
            //preDraw decides if that accessory should be drawn "behind" the actual slime (false means it will draw infront)
            //alpha says by how much it should be transparent (0 is fully opaque, 255 fully transparent)
            //order of the Add() doesn't matter

            Add(name: "PetAccessoryBow");
            Add(name: "PetAccessoryBowGreen");
            Add(name: "PetAccessoryBowBlack");
            Add(name: "PetAccessoryBowBlue");
            Add(name: "PetAccessoryXmasHat", offsetY: -13f);
            Add(name: "PetAccessoryAmethystStaff", offsetX: -14f, preDraw: true);
            Add(name: "PetAccessoryTopazStaff", offsetX: -14f, preDraw: true);
            Add(name: "PetAccessorySlimeHead", offsetY: -18f, alpha: 56);
            Add(name: "PetAccessoryBlueMittens", allowLegacy: false);
            Add(name: "PetAccessoryRedMittens", allowLegacy: false);
            Add(name: "PetAccessoryGrayMittens", allowLegacy: false);

            Check();
        }

        public static void Unload()
        {
            Texture = null;
            Offset = null;
            InternalMod = null;
            addCounter = 0;
        }


        private static void Check(bool duringAdd = false)
        {
            if (duringAdd && namesOfAccessories.Length < addCounter)
            {
                throw new Exception("Assigned number of Pet Accessories (" + namesOfAccessories.Length + ") is less than number of added Pet Accessories (" + addCounter + ").");
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
            TryAdd(InternalMod.ItemType(name), InternalMod.GetTexture("Items/PetAccessories/" + name + "_Draw"), new Vector2(offsetX, offsetY), preDraw, alpha, allowLegacy);
        }

        private static void TryAdd(int type, Texture2D texture, Vector2 offset, bool preDraw, byte alpha, bool allowLegacy)
        {
            addCounter++;

            Check(true);

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

    public class PetAccessoryBow : PetAccessoryBase
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Red Bow");
            Tooltip.SetDefault("Something to decorate your cute slime with.");
        }

        protected override void MoreSetDefaults()
        {
            item.value = (int)SlotType.Body;
        }
    }

    public class PetAccessoryBowBlue : PetAccessoryBase
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Blue Bow");
            Tooltip.SetDefault("Something to decorate your cute slime with.");
        }

        protected override void MoreSetDefaults()
        {
            item.value = (int)SlotType.Body;
        }
    }

    public class PetAccessoryBowGreen : PetAccessoryBase
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Green Bow");
            Tooltip.SetDefault("Something to decorate your cute slime with.");
        }

        protected override void MoreSetDefaults()
        {
            item.value = (int)SlotType.Body;
        }
    }

    public class PetAccessoryBowBlack : PetAccessoryBase
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Black Bow");
            Tooltip.SetDefault("Something to decorate your cute slime with.");
        }

        protected override void MoreSetDefaults()
        {
            item.value = (int)SlotType.Body;
        }
    }

    public class PetAccessoryXmasHat : PetAccessoryBase
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

    public class PetAccessoryAmethystStaff : PetAccessoryBase
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

    public class PetAccessoryTopazStaff : PetAccessoryBase
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

    public class PetAccessorySlimeHead : PetAccessoryBase
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

    public class PetAccessoryBlueMittens : PetAccessoryBase
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

    public class PetAccessoryRedMittens : PetAccessoryBase
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

    public class PetAccessoryGrayMittens : PetAccessoryBase
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

    public enum SlotType : byte
    {
        None, //reserved
        Hat,
        Body,
        Carried,
        Misc
        //Please settle on (max) four groups for now (ignoring None), those I listed are suggestions.
        //Also, concider that there cant be more than one accessory active in each slot, so decide on proper
        //categories that make sense.

        //for Carried, its actually only the front facing hand. For something like gloves or dual wielding, use Misc instead

        //also, keep the sprite dimensions the same as the slime girls
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
            if(e == 1)
            {
                return "Worn on the head.";
            }
            if (e == 2)
            {
                return "Worn on the body.";
            }
            if (e == 3)
            {
                return "Carried.";
            }
            if (e == 4)
            {
                return "Worn somewhere else (misc).";
            }
            return "REDACTED";
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            AssPlayer mPlayer = Main.LocalPlayer.GetModPlayer<AssPlayer>(mod);

            tooltips.Add(new TooltipLine(mod, "slot", Enum2string(item.value)));

            if (!PetAccessories.AllowLegacy[PetAccessories.ItemsIndexed[item.type]])
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
                    ErrorLogger.Log("had to change index of slime pet of " + player.name + " because it was -1");
                    //find first occurence of a player owned cute slime
                    for (int i = 0; i < 1000; i++)
                    {
                        if (Main.projectile[i].active)
                        {
                            if (Main.projectile[i].modProjectile != null)
                            {
                                if (Main.projectile[i].owner == Main.myPlayer && typeof(CuteSlimeBasePet).IsInstanceOfType(Main.projectile[i].modProjectile))
                                {
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

                if (Main.projectile[mPlayer.petIndex].active && Main.projectile[mPlayer.petIndex].owner == Main.myPlayer && typeof(CuteSlimeBasePet).IsInstanceOfType(Main.projectile[mPlayer.petIndex].modProjectile))
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
                            Main.NewText(!PetAccessories.AllowLegacy[PetAccessories.ItemsIndexed[item.type]] + " " + (Array.IndexOf(AssortedCrazyThings.slimePetLegacy, Main.projectile[mPlayer.petIndex].type) != -1));
                            if (!PetAccessories.AllowLegacy[PetAccessories.ItemsIndexed[item.type]] && Array.IndexOf(AssortedCrazyThings.slimePetLegacy, Main.projectile[mPlayer.petIndex].type) != -1)
                            {
                                return true;
                            }

                            gProjectile.ToggleAccessory((byte)item.value, (uint)PetAccessories.ItemsIndexed[item.type]);

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
