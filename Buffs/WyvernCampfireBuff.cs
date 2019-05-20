using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs
{
    public class WyvernCampfireBuff : ModBuff
    {
        //Just to have visual feedback
        //Logic handled in WyvernCampfireTile.NearbyEffects and AssWorld.ResetNearbyTileEffects
        //Applied in AssPlayer.PreUpdate
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Wyvern Campfire");
            Description.SetDefault("You are protected from wyverns");
            Main.buffNoTimeDisplay[Type] = true;
        }
    }
}