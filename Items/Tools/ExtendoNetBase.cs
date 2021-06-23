using System;
using System.Reflection;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Tools
{
    [Content(ContentType.Tools)]
    public abstract class ExtendoNetBase : AssItem
    {
        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.shootSpeed = 3.7f;
            Item.width = 40;
            Item.height = 40;
            Item.DamageType = DamageClass.Melee;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.autoReuse = true;
            Item.UseSound = SoundID.Item1;
        }

        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[Item.shoot] < 1;
        }

        public void OverhaulInit()
        {
            //TODO remove this/change/whatever
            //Mod oMod = ModLoader.GetMod("TerrariaOverhaul");
            //if (oMod != null)
            //{
            //    try
            //    {
            //        Assembly TerrariaOverhaul = oMod.Code;
            //        Type Extensions = TerrariaOverhaul.GetType(oMod.Name + ".Extensions");
            //        MethodInfo SetTag = Extensions.GetMethod("SetTag", new Type[] { typeof(ModItem), typeof(int), typeof(bool) });
            //        Type ItemTags = TerrariaOverhaul.GetType(oMod.Name + ".ItemTags");
            //        FieldInfo AllowQuickUse = ItemTags.GetField("AllowQuickUse", BindingFlags.Static | BindingFlags.Public);
            //        object AllowQuickUseValue = AllowQuickUse.GetValue(null);
            //        SetTag.Invoke(null, new object[] { this, AllowQuickUseValue, true });
            //    }
            //    catch
            //    {
            //        mod.Logger.Warn("Failed to register Overhaul Quick Use feature to Extendo Nets");
            //    }
            //}
            //this.SetTag(ItemTags.AllowQuickUse);
        }
    }
}
