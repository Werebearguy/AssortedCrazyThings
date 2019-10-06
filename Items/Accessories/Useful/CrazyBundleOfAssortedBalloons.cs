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
            item.width = 46;
            item.height = 42;
            item.value = 0;
            item.rare = -11;
            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.AddBuff(ModContent.BuffType<BipolarBuff>(), 60);
            player.noFallDmg = true;
            player.jumpBoost = true;
            player.doubleJumpCloud = true;
            player.doubleJumpSandstorm = true;
            player.doubleJumpBlizzard = true;
            player.doubleJumpFart = true;
            player.doubleJumpSail = true;
            player.doubleJumpUnicorn = true;
            player.bee = true;
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
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<MassiveBundleOfBalloons>());
            recipe.AddIngredient(ModContent.ItemType<BipolarCandleInABalloon>());
            //recipe.AddIngredient(ModContent.ItemType<StarInABalloon>());
            recipe.AddIngredient(ModContent.ItemType<StarWispBalloon>());
            recipe.AddIngredient(ModContent.ItemType<Cobballoon>());
            recipe.AddIngredient(ModContent.ItemType<EyelloonRetinazer>());
            recipe.AddIngredient(ModContent.ItemType<SpazmatismEyelloon>());
            recipe.AddIngredient(ItemID.PartyBundleOfBalloonsAccessory);
            recipe.AddIngredient(ItemID.PartyBalloonAnimal);
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
