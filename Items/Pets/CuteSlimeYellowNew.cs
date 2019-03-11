using AssortedCrazyThings.Buffs;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
    public class CuteSlimeYellowNew : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bottled Cute Yellow Slime");
            Tooltip.SetDefault("Summons a friendly Cute Yellow Slime to follow you");
        }

        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.LizardEgg);
            item.shoot = mod.ProjectileType<CuteSlimeYellowNewProj>();
            item.buffType = mod.BuffType<CuteSlimeYellowNewBuff>();
            item.rare = -11;
            item.value = Item.sellPrice(copper: 10);
        }
		
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod, "CuteSlimeYellow");
			recipe.AddTile(TileID.Solidifier);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

        public override void UseStyle(Player player)
        {
            if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
            {
                player.AddBuff(item.buffType, 3600, true);
            }
        }
    }
}
