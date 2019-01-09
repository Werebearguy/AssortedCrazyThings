using AssortedCrazyThings.NPCs.DungeonBird;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items
{
    //the one actually used in recipes
    public class CaughtDungeonSoulAwakened : CaughtDungeonSoulBase
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Caught Dungeon Soul");
            Tooltip.SetDefault("Awakened by defeating the " + aaaHarvester3.name + ".");
            ItemID.Sets.ItemIconPulse[item.type] = true;
            ItemID.Sets.ItemNoGravity[item.type] = true;
        }

        public override void MoreSetDefaults()
        {
            frame2CounterCount = 4.0;
            animatedTextureSelect = 1;
        }

        //hardmode recipe
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(this, 15);
            recipe.AddTile(TileID.CrystalBall);
            recipe.SetResult(ItemID.WaterCandle);
            recipe.AddRecipe();
        }
    }
}

