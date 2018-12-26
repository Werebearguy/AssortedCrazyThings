using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.PetAccessories
{
    /*
     * For every new Texture you add, copypaste a new class in this namespace, and adjust its DisplayName and item.value.
     * item.value is the "SlotType" in our case.
     * (yes, this means that the accessories are worth almost nothing when sold, who cares lmao)
     * 
     * example:
     * item.value = (int)SlotType.Body;
     * 
     * finally, go into AssortedCrazyThings.cs, then into InitPetAccessories(), and add the item's class name to the array lists
     * 
     * 
     * 
     * 
     */


    public abstract class PetAccessory: ModItem
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

        protected virtual void MoreSetDefaults()
        {

        }
    }

    public class PetAccessoryBow : PetAccessory
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("aaaRed Bow");
            Tooltip.SetDefault("Something to decorate your cute slime with.");
        }

        protected override void MoreSetDefaults()
        {
            item.value = (int)SlotType.Body;
        }
    }

    public class PetAccessoryBowBlue : PetAccessory
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("aaaBlue Bow");
            Tooltip.SetDefault("Something to decorate your cute slime with.");
        }

        protected override void MoreSetDefaults()
        {
            item.value = (int)SlotType.Body;
        }
    }

    public class PetAccessoryBowGreen : PetAccessory
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("aaaGreen Bow");
            Tooltip.SetDefault("Something to decorate your cute slime with.");
        }

        protected override void MoreSetDefaults()
        {
            item.value = (int)SlotType.Body;
        }
    }

    public class PetAccessoryBowYellow : PetAccessory
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("aaaYellow Bow");
            Tooltip.SetDefault("Something to decorate your cute slime with.");
        }

        protected override void MoreSetDefaults()
        {
            item.value = (int)SlotType.Body;
        }
    }

    public class PetAccessoryXmasHat : PetAccessory
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("aaaSanta Hat");
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
        Hat,
        Body,
        Hands,
        Tail
        //Please settle on (max) four groups for now (ignoring None), those I listed are suggestions.
        //Also, concider that there cant be more than one accessory active in each slot, so decide on proper
        //categories that make sense.
        //what I plan to do next is a fixed offset for each accessory (so not all hats have the same one, but different,
        //if you for example decide to add a hat that is very tall, and then one that goes down to the feet, you can adjust that
        //based on the sprite)

        //also, keep the sprite dimensions the same as the slime girls
    }
}
