using AssortedCrazyThings.Projectiles.Weapons;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Fun
{
    public class GuideVoodoorang : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Guide Voodoorang");
            Tooltip.SetDefault("'Why are you like this?'");
        }

        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.WoodenBoomerang);
            item.width = 22;
            item.height = 30;
            item.rare = -11;

            item.value = Item.sellPrice(silver: 2);
            item.shoot = ModContent.ProjectileType<GuideVoodoorangProj>();
        }

        public override bool CanUseItem(Player player)
        {
            // Ensures no more than one boomerang can be thrown out
            return player.ownedProjectileCounts[item.shoot] < 1;
        }

        public override void PostUpdate()
        {
            if (item.lavaWet)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient && !NPC.AnyNPCs(NPCID.WallofFlesh))
                {
                    for (int i = 0; i < Main.maxNPCs; i++)
                    {
                        NPC npc = Main.npc[i];
                        if (npc.active && npc.type == NPCID.Guide)
                        {
                            if (Main.netMode == NetmodeID.Server)
                            {
                                NetMessage.SendData(MessageID.StrikeNPC, -1, -1, null, i, 9999f, 10f, -npc.direction);
                            }
                            npc.StrikeNPCNoInteraction(9999, 10f, -npc.direction);
                            NPC.SpawnWOF(item.position);

                            byte plr = Player.FindClosest(item.position, item.width, item.height);
                            Item.NewItem(Main.player[plr].getRect(), ModContent.ItemType<GuideVoodoorang>());

                            //despawns upon wof spawn
                            item.TurnToAir();
                            NetMessage.SendData(MessageID.SyncItem, -1, -1, null, item.whoAmI);
                            return;
                        }
                    }
                }
            }
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.GuideVoodooDoll);
            recipe.AddTile(TileID.DemonAltar);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
