using AssortedCrazyThings.Buffs;
using AssortedCrazyThings.Items.Accessories.Vanity;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Accessories.Useful
{
    [AutoloadEquip(EquipType.Balloon)]
    public class CrazyBundleOfAssortedBalloons : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Crazy Bundle of Assorted Balloons");
            Tooltip.SetDefault("It's got all kinds of effects");
        }

        public override void SetDefaults()
        {
            Item.width = 46;
            Item.height = 42;
            Item.value = 0;
            Item.rare = -11;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.noFallDmg = true;
            player.jumpBoost = true;
            player.hasJumpOption_Cloud = true;
            player.hasJumpOption_Sandstorm = true;
            player.hasJumpOption_Blizzard = true;
            player.hasJumpOption_Fart = true;
            player.hasJumpOption_Sail = true;
            player.hasJumpOption_Unicorn = true;
            player.honeyCombItem = Item;
            if (!player.HasBuff(BuffID.StarInBottle))
            {
                player.manaRegenBonus += 2;
            }
            Lighting.AddLight(player.Center, 0.7f, 1.3f, 1.6f);
        }

        /*
         * Massive bundle + Bipolar (+ Star) + Star Wisp + Cobballoon + Retinazer + Spaz + Bundled Party + Balloon Animal
         */
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<MassiveBundleOfBalloons>()).AddIngredient(ModContent.ItemType<StarWispBalloon>()).AddIngredient(ModContent.ItemType<Cobballoon>()).AddIngredient(ModContent.ItemType<EyelloonRetinazer>()).AddIngredient(ModContent.ItemType<SpazmatismEyelloon>()).AddIngredient(ItemID.PartyBundleOfBalloonsAccessory).AddIngredient(ItemID.PartyBalloonAnimal).AddTile(TileID.TinkerersWorkbench).Register();
        }
    }
}
