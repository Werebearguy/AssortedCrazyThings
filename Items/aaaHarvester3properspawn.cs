using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items
{
    public class aaaHarvester3properspawn : ModItem
    {
        public override string Texture
        {
            get
            {
                return "AssortedCrazyThings/Items/OrigamiManual"; //temp
            }
        }

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("IF YOU SEE THIS YOU ARE MOST LIKELY HARBLESNARGITS OR DIREWOLF420");
        }

        public override void SetDefaults()
        {
            item.width = 28;
            item.height = 30;
            item.maxStack = 1;
            item.rare = 9;
            item.useAnimation = 45;
            item.useTime = 45;
            item.useStyle = 4;
            item.UseSound = SoundID.Item44;
            item.consumable = true;
        }
        
        public override bool CanUseItem(Player player)
        {
            return !NPC.AnyNPCs(AssWorld.harvesterTypes[2]);
        }

        public override bool UseItem(Player player)
        {
            NPC.NewNPC((int)player.Center.X, (int)player.Center.Y, AssWorld.harvesterTypes[2], 150);
            return true;
        }
    }
}
