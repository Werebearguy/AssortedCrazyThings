using AssortedCrazyThings.Buffs.CuteSlimes;
using AssortedCrazyThings.Projectiles.Pets.CuteSlimes;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets.CuteSlimes
{
    public class CuteSlimePrincessNew : CuteSlimeItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bottled Cute Princess Slime");
            Tooltip.SetDefault("Summons a friendly Cute Princess Slime to follow you");
        }

        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.LizardEgg);
            item.shoot = mod.ProjectileType<CuteSlimePrincessNewProj>();
            item.buffType = mod.BuffType<CuteSlimePrincessNewBuff>();
            item.rare = -11;
            item.value = Item.sellPrice(copper: 20);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType<PrinceSlimeItem>());
            recipe.AddIngredient(mod.ItemType<CuteSlimeBlueNew>());
            recipe.AddTile(TileID.Solidifier);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}