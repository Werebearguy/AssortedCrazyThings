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
     * finally, go into AssWorld.cs, then into InitPetAccessories(), and add the item's class name to the array lists
     * 
     * 
     * 
     * 
     */


    public class PetAccessoryBow : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("aaaRed Bow");
            Tooltip.SetDefault("Something to decorate your cute slime with.");
        }
        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.Silk);
            item.width = 28;
            item.height = 30;
            item.maxStack = 1;
            item.rare = -11;
            item.useAnimation = 45;
            item.useTime = 45;
            item.useStyle = 4;
            item.UseSound = SoundID.Item1;
            item.consumable = false;
            item.value = (int)SlotType.Body;
        }
    }

    public class PetAccessoryXmasHat : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("aaaSanta Hat");
            Tooltip.SetDefault("Something to decorate your cute slime with.");
        }
        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.Silk);
            item.width = 28;
            item.height = 30;
            item.maxStack = 1;
            item.rare = -11;
            item.useAnimation = 45;
            item.useTime = 45;
            item.useStyle = 4;
            item.UseSound = SoundID.Item1;
            item.consumable = false;
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
        //please settle on (max) four groups for now (ignoring None), those I listed are suggestions
    }
}