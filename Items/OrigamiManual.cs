using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Harblesnargits_Mod_01.Items
{
    //imported from my tAPI mod because I'm lazy
    public class OrigamiManual : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("This book can be folded a thousand times...");
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
            return !NPC.AnyNPCs(mod.NPCType("FoldfishBoss"));
        }

        public override bool UseItem(Player player)
        {
            NPC.SpawnOnPlayer(player.whoAmI, mod.NPCType("FoldfishBoss"));
            Main.PlaySound(SoundID.Roar, player.position, 0);
            return true;
        }
    }
}
