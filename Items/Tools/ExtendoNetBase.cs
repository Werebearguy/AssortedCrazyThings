using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TerrariaOverhaul;

namespace AssortedCrazyThings.Items.Tools
{
    public abstract class ExtendoNetBase : ModItem
    {
        public override void SetDefaults()
        {
            item.useStyle = 5;
            item.shootSpeed = 3.7f;
            item.width = 40;
            item.height = 40;
            item.melee = true;
            item.noMelee = true;
            item.noUseGraphic = true;
            item.autoReuse = true;
            item.UseSound = SoundID.Item1;
        }

        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[item.shoot] < 1;
        }

        public void OverhaulInit()
        {
            this.SetTag(ItemTags.AllowQuickUse);
        }
    }
}
