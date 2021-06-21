using AssortedCrazyThings.Items.VanityArmor;
using AssortedCrazyThings.NPCs.DungeonBird;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Consumables
{
    //[Content(ContentType.Boss)]
    [Autoload]
    public class HarvesterTreasureBag : AssItem
    {
        //Sets the associated NPC this treasure bag is dropped from
        public override int BossBagNPC => ModContent.NPCType<Harvester>();

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Treasure Bag");
            Tooltip.SetDefault("{$CommonItemTooltip.RightClickToOpen}"); //References a language key that says "Right Click To Open" in the language of the game
        }

        public override void SetDefaults()
        {
            Item.maxStack = 999;
            Item.consumable = true;
            Item.width = 24;
            Item.height = 24;
            Item.rare = ItemRarityID.Purple;
            Item.expert = true;
        }

        public override bool CanRightClick()
        {
            return true;
        }

        public override void OpenBossBag(Player player)
        {
            if (Main.tenthAnniversaryWorld) //Because this is a pre-HM boss, we have to include this check
            {
                //Using a particular secret world grants doubled chance on dev sets (handled inside TryGettingDevArmor) even for pre-HM bosses
                player.TryGettingDevArmor();
            }

            //We have to replicate the expert drops from Harvester here via QuickSpawnItem
            player.QuickSpawnItem(ItemID.Bone, Main.rand.Next(40, 61));
            player.QuickSpawnItem(ModContent.ItemType<DesiccatedLeather>());

            if (Main.rand.NextBool(7))
            {
                player.QuickSpawnItem(ModContent.ItemType<SoulHarvesterMask>());
            }
        }
    }
}
