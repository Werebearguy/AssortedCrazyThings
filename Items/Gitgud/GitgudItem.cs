using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Gitgud
{
	/// <summary>
	/// Serves as a base for all gitgud items
	/// </summary>
	[Content(ContentType.BossConsolation)]
	public abstract class GitgudItem : AssItem
	{
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			if (GitgudData.GetDataFromItemType(Item.type, out GitgudData data) &&
				tooltips.FindIndex(l => l.Name == "Social") == -1)
			{
				int insertIndex = tooltips.FindLastIndex(l => l.Name.StartsWith("Tooltip"));
				if (insertIndex == -1) insertIndex = tooltips.Count;
				tooltips.Insert(insertIndex++, new TooltipLine(Mod, "Desc", "Consolation Prize"));
				string reduced = "" + Math.Round(data.Reduction * 100) + "% reduced damage taken " + (data.Invasion != "" ? "during " + data.Invasion : "from " + data.BossName);
				tooltips.Insert(insertIndex++, new TooltipLine(Mod, "Reduced", reduced));
				if (data.BuffTypeList.Length > 0)
				{
					tooltips.Insert(insertIndex++, new TooltipLine(Mod, "BuffImmune", "Immunity to '" + data.BuffNameFunc() + "' while " + data.BossName + (data.BossName.Contains(" or ") ? " are" : " is") + " alive"));
				}

				if (!(data.Accessory[Main.myPlayer] || Main.LocalPlayer.HasItem(Item.type) || Main.LocalPlayer.trashItem.type == Item.type))
				{
					tooltips.Insert(insertIndex++, new TooltipLine(Mod, "Count", "Times died: " + data.Counter[Main.myPlayer] + "/" + data.CounterMax));
				}
				tooltips.Insert(insertIndex++, new TooltipLine(Mod, "Gitgud", "[c/E180CE:'git gud']"));
			}
		}

		public sealed override void SetStaticDefaults()
		{
			Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

			SafeSetStaticDefaults();
		}

		public virtual void SafeSetStaticDefaults()
		{

		}

		public sealed override void SetDefaults()
		{
			Item.value = Item.sellPrice(copper: 1);
			Item.rare = -1;
			Item.maxStack = 1;
			Item.accessory = true;

			//item.width = 32;
			//item.height = 32;

			SafeSetDefaults();
		}

		public virtual void SafeSetDefaults()
		{

		}
	}
}
