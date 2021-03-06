﻿using AssortedCrazyThings.Base;
using AssortedCrazyThings.NPCs.DungeonBird;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items
{
    public class IdolOfDecay : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Idol Of Decay");
            Tooltip.SetDefault("Summons " + Harvester.name + "'s first form in the dungeon");
        }

        public override void SetDefaults()
        {
            item.width = 38;
            item.height = 30;
            item.maxStack = 30;
            item.rare = -11;
            item.useAnimation = 45;
            item.useTime = 45;
            item.useStyle = ItemUseStyleID.HoldingUp;
            item.value = Item.sellPrice(silver: 5);
            item.UseSound = SoundID.Item44;
            item.consumable = true;
        }

        public override bool CanUseItem(Player player)
        {
            return !AssUtils.AnyNPCs(AssWorld.harvesterTypes.Take(3).ToArray()) && player.ZoneDungeon;
        }

        public override bool UseItem(Player player)
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                int i = NPC.NewNPC((int)player.Center.X, (int)player.Center.Y, AssWorld.harvesterTypes[0]);
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
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.WaterCandle, 1);
            recipe.AddIngredient(ItemID.Bone, 50);
            recipe.AddTile(TileID.DemonAltar);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
