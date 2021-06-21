using AssortedCrazyThings.Base;
using AssortedCrazyThings.NPCs.DungeonBird;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items
{
    [Autoload]
    public class IdolOfDecay : AssItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Idol Of Decay");
            Tooltip.SetDefault("Summons " + Harvester.name + "'s first form in the dungeon");
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 28;
            Item.maxStack = 30;
            Item.rare = -11;
            Item.useAnimation = 45;
            Item.useTime = 45;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.value = Item.sellPrice(silver: 5);
            Item.UseSound = SoundID.Item44;
            Item.consumable = true;
        }

        public override bool CanUseItem(Player player)
        {
            return !AssUtils.AnyNPCs(AssortedCrazyThings.harvesterTypes.Take(3).ToArray()) && player.ZoneDungeon;
        }

        public override bool UseItem(Player player)
        {
            if (!AConfigurationConfig.Instance.Bosses)
            {
                return true;
            }

            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                int i = NPC.NewNPC((int)player.Center.X, (int)player.Center.Y, AssortedCrazyThings.harvesterTypes[0]);
                AssWorld.AwakeningMessage("Soul Harvester has been Awakened!");
                if (Main.netMode == NetmodeID.Server && i < Main.maxNPCs)
                {
                    NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, i);
                }
            }
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.WaterCandle, 1).AddIngredient(ItemID.Bone, 50).AddTile(TileID.DemonAltar).Register();
        }
    }
}
