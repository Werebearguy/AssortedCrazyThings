using AssortedCrazyThings.Projectiles.Tools;
using System;
using System.Reflection;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

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

        public override bool CanUseItem(Player player)
        {
            // Ensures no more than one spear can be thrown out, use this when using autoReuse
            return player.ownedProjectileCounts[item.shoot] < 1;
        }
        public void OverhaulInit()
        {
            Mod oMod = ModLoader.GetMod("TerrariaOverhaul");
            if (oMod != null)
            {
                try
                {
                    Assembly TerrariaOverhaul = oMod.Code;
                    Type Extensions = TerrariaOverhaul.GetType(oMod.Name + ".Extensions");
                    MethodInfo SetTag = Extensions.GetMethod("SetTag", new Type[] { typeof(ModItem), typeof(int), typeof(bool) });
                    Type ItemTags = TerrariaOverhaul.GetType(oMod.Name + ".ItemTags");
                    FieldInfo AllowQuickUse = ItemTags.GetField("AllowQuickUse", BindingFlags.Static | BindingFlags.Public);
                    object AllowQuickUseValue = AllowQuickUse.GetValue(null);
                    SetTag.Invoke(null, new object[] { this, AllowQuickUseValue, true });
                }
                catch
                {
                    ErrorLogger.Log("Failed to register Overhaul Quick Use feature to Extendo Nets");
                }
            }
            //this.SetTag(ItemTags.AllowQuickUse);
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
