using AssortedCrazyThings.Buffs;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
    public class CompanionDungeonSoulPetItem : CaughtDungeonSoulBase
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Companion Soul");
            Tooltip.SetDefault("Summons a friendly Soul to give you light.");
            ItemID.Sets.ItemNoGravity[item.type] = true;
        }

        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.ZephyrFish);
            frame2CounterCount = 8.0;
            animatedTextureSelect = 0;

            item.width = 14;
            item.height = 24;
            item.shoot = mod.ProjectileType<CompanionDungeonSoulPetProj>();
            item.buffType = mod.BuffType<CompanionDungeonSoulPetBuff>();
            item.rare = -11;

            item.value = Item.sellPrice(copper: 10);
        }

        public override void UseStyle(Player player)
        {
            if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
            {
                player.AddBuff(item.buffType, 3600, true);
            }
        }

        //hardmode recipe
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType<CaughtDungeonSoulAwakened>(), 1);
            recipe.AddTile(TileID.CrystalBall);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
