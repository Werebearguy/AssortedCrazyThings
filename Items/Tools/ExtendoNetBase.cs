using System;
using System.Reflection;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

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
                    mod.Logger.Warn("Failed to register Overhaul Quick Use feature to Extendo Nets");
                }
            }
            //this.SetTag(ItemTags.AllowQuickUse);
        }
    }
}
