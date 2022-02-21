using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Base.Handlers.UnreplaceableMinionWith0SlotsHandler
{
	/// <summary>
	/// This class, along with <see cref="UnreplaceableMinionWith0SlotsModPlayer"/>, is used to conditionally
	/// unset the minion flag from projectiles added to the <see cref="UnreplaceableMinionWith0SlotsSystem"/> cache in order to prevent them from
	/// being sacrificed when the player uses a summon item. 
	/// </summary>
	[Content(ConfigurationSystem.AllFlags, needsAllToFilter: true)]
	public class UnreplaceableMinionWith0SlotsGlobalItem : AssGlobalItem
	{
		public override bool CanUseItem(Item item, Player player)
		{
			if (item.DamageType == DamageClass.Summon && ProjectileID.Sets.MinionSacrificable[item.shoot])
			{
				bool atleastOne = false;
				for (int i = 0; i < Main.maxProjectiles; i++)
				{
					Projectile proj = Main.projectile[i];
					if (proj.active && proj.owner == player.whoAmI && proj.minion && UnreplaceableMinionWith0SlotsSystem.Exists(proj.type))
					{
						if (!atleastOne)
						{
							player.GetModPlayer<UnreplaceableMinionWith0SlotsModPlayer>().shouldResetMinionStatus = true;
						}
						atleastOne = true;
						proj.minion = false; //Temporarily de-minion it so that it doesn't get sacrificed
					}
				}
			}

			return base.CanUseItem(item, player);
		}
	}
}
