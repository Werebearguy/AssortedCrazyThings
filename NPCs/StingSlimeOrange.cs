using AssortedCrazyThings.Items.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs
{
    public class StingSlimeOrange : StingSlimeBlack
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            NPC.catchItem = (short)ModContent.ItemType<StingSlimeOrangeItem>();
        }
    }
}
