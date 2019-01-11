using AssortedCrazyThings.NPCs.DungeonBird;
using Terraria.ID;

namespace AssortedCrazyThings.Items
{
    public class CaughtDungeonSoul : CaughtDungeonSoulBase
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Loose Dungeon Soul");
            Tooltip.SetDefault("An inert soul caught by a net.\nAwakened in your inventory when " + aaaHarvester3.name + " is defeated.");
            // ticksperframe, frameCount
            //Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(5, 4));
            //ItemID.Sets.AnimatesAsSoul[item.type] = true;

            ItemID.Sets.ItemNoGravity[item.type] = true;
        }

        public override void MoreSetDefaults()
        {
            frame2CounterCount = 8.0;
            animatedTextureSelect = 0;
        }
    }
}
