using AssortedCrazyThings.Projectiles.Tools;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Tools
{
    public class ExtendoNetRegular : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Extendo Net");
            Tooltip.SetDefault("Catches those hard to reach critters.");
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
            item.value = Item.sellPrice(silver: 10);

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
    }
}
