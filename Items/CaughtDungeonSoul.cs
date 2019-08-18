using System.Collections.Generic;
using System.Linq;
using AssortedCrazyThings.Base;
using AssortedCrazyThings.NPCs.DungeonBird;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items
{
    public class CaughtDungeonSoul : CaughtDungeonSoulBase
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Loose Dungeon Soul");
            Tooltip.SetDefault("'An inert soul caught by a net'"
                + "\nAwakened in your inventory when " + Harvester.name + " is defeated");
            // ticksperframe, frameCount
            //Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(5, 4));
            //ItemID.Sets.AnimatesAsSoul[item.type] = true;

            ItemID.Sets.ItemNoGravity[item.type] = true;
        }

        public override void MoreSetDefaults()
        {
            frame2CounterCount = 8.0;
            animatedTextureSelect = 0;
            item.useStyle = 1;
            item.autoReuse = true;
            item.useTurn = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.consumable = true;
            item.noUseGraphic = true;
            item.makeNPC = (short)mod.NPCType<DungeonSoul>();
        }

        public override bool CanUseItem(Player player)
        {
            return AssUtils.AnyNPCs(AssWorld.harvesterTypes.Take(3).ToArray());
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            if (AssUtils.AnyNPCs(AssWorld.harvesterTypes.Take(3).ToArray()))
            {
                // Can use item
                tooltips.Add(new TooltipLine(mod, "MakeNPC", "Use it to spawn a soul for the Soul Harvester to eat")
                {
                    overrideColor = new Microsoft.Xna.Framework.Color(35, 200, 254)
                });
            }
            else
            {
                // Can not use item
                TooltipLine consumable = tooltips.Find(line => line.Name == "Consumable");
                if (consumable != null) tooltips.Remove(consumable);
            }
        }
    }
}
