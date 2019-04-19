using AssortedCrazyThings.Projectiles.Tools;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TerrariaOverhaul;

namespace AssortedCrazyThings.Items.Tools
{
    public class ExtendoNetRegular : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Extendo-Net");
            Tooltip.SetDefault("'Catches those hard to reach critters'");
        }

        public override void SetDefaults()
        {
            //item.damage = 40;
            item.useStyle = 5;
            item.useAnimation = 24; //18
            item.useTime = 32; //24
            item.shootSpeed = 3.7f; //3.7f
            item.knockBack = 6.5f;
            item.width = 40;
            item.height = 40;
            item.scale = 1f;
            item.rare = -11;
            item.value = Item.sellPrice(silver: 45);

            item.melee = true;
            item.noMelee = true;
            item.noUseGraphic = true;
            item.autoReuse = true;

            item.UseSound = SoundID.Item1;
            item.shoot = mod.ProjectileType<ExtendoNetRegularProj>();
        }

        public void OverhaulInit()
        {
            this.SetTag(ItemTags.AllowQuickUse);
        }

        public override bool CanUseItem(Player player)
        {
            // Ensures no more than one spear can be thrown out, use this when using autoReuse
            return player.ownedProjectileCounts[item.shoot] < 1;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Wire, 10);
            recipe.AddRecipeGroup("IronBar", 10);
            recipe.AddIngredient(ItemID.BugNet, 1);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
