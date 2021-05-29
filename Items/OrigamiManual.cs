using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;

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
            Item.width = 28;
            Item.height = 30;
            Item.maxStack = 1;
            Item.rare = -11;
            Item.useAnimation = 45;
            Item.useTime = 45;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.UseSound = SoundID.Item44;
            Item.consumable = true;
            Item.value = Item.sellPrice(silver: 10);
        }

        public override bool CanUseItem(Player player)
        {
            return !NPC.AnyNPCs(Mod.Find<ModNPC>("FoldfishBoss").Type);
        }

        public override bool UseItem(Player player)
        {
            NPC.SpawnOnPlayer(player.whoAmI, Mod.Find<ModNPC>("FoldfishBoss").Type);
            SoundEngine.PlaySound(SoundID.Roar, player.position, 0);
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                for (int k = 0; k < 6; k++)
                {
                    int type = Mod.Find<ModNPC>("FoldfishBaby").Type;
                    int index = NPC.NewNPC((int)player.position.X, (int)player.position.Y, type);
                    NPC npc = Main.npc[index];
                    npc.SetDefaults(type);
                    npc.velocity.X = (float)Main.rand.Next(-15, 16) * 0.1f;
                    npc.velocity.Y = (float)Main.rand.Next(-30, 1) * 0.1f;
                    if (Main.netMode == NetmodeID.Server && index < Main.maxNPCs)
                    {
                        NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, index);
                    }
                }
            }
            return true;
        }
    }
}
