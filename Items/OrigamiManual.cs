using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items
{
    public class OrigamiManual : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("'This book can be folded a thousand times...'");
        }

        public override void SetDefaults()
        {
            item.width = 28;
            item.height = 30;
            item.maxStack = 1;
            item.rare = -11;
            item.useAnimation = 45;
            item.useTime = 45;
            item.useStyle = 4;
            item.UseSound = SoundID.Item44;
            item.consumable = true;
            item.value = Item.sellPrice(silver: 10);
        }

        public override bool CanUseItem(Player player)
        {
            return !NPC.AnyNPCs(mod.NPCType("FoldfishBoss"));
        }

        public override bool UseItem(Player player)
        {
            NPC.SpawnOnPlayer(player.whoAmI, mod.NPCType("FoldfishBoss"));
            Main.PlaySound(SoundID.Roar, player.position, 0);
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                for (int k = 0; k < 6; k++)
                {
                    int type = mod.NPCType("FoldfishBaby");
                    int index = NPC.NewNPC((int)player.position.X, (int)player.position.Y, type);
                    NPC npc = Main.npc[index];
                    npc.SetDefaults(type);
                    npc.velocity.X = (float)Main.rand.Next(-15, 16) * 0.1f;
                    npc.velocity.Y = (float)Main.rand.Next(-30, 1) * 0.1f;
                    if (Main.netMode == NetmodeID.Server && index < Main.maxNPCs)
                    {
                        NetMessage.SendData(23, -1, -1, null, index);
                    }
                }
            }
            return true;
        }
    }
}
