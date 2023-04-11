using AssortedCrazyThings.NPCs.Harvester;
using AssortedCrazyThings.Projectiles.NPCs.Bosses.Harvester;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items
{
	[Content(ContentType.Bosses)]
	public class CaughtDungeonSoul : CaughtDungeonSoulBase
	{
		public override void SetStaticDefaults()
		{
			// ticksperframe, frameCount
			//Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 4));
			//ItemID.Sets.AnimatesAsSoul[Item.type] = true;
			
			Item.ResearchUnlockCount = 15;

			ItemID.Sets.ItemNoGravity[Item.type] = true;
		}

		public override void SafeSetDefaults()
		{
			frame2CounterCount = 6;
			animatedTextureSelect = 0;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.autoReuse = true;
			Item.useTurn = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.consumable = true;
			Item.noUseGraphic = true;
			Item.makeNPC = (short)ModContent.NPCType<DungeonSoul>();
		}

		public static bool CanUseCondition()
		{
			return BabyHarvesterHandler.TryFindBabyHarvester(out _, out _);
		}

		public override bool CanUseItem(Player player)
		{
			return CanUseCondition();
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			if (CanUseCondition())
			{
				// Can use item
				tooltips.Add(new TooltipLine(Mod, "MakeNPC", "Use it to spawn a soul for the Soul Harvester to eat")
				{
					OverrideColor = new Color(35, 200, 254)
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
