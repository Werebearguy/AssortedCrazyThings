using Terraria;

namespace AssortedCrazyThings.Base.Handlers.UnreplaceableMinionWith0SlotsHandler
{
	/// <summary>
	/// This class, along with <see cref="UnreplaceableMinionWith0SlotsGlobalItem"/>, is used to conditionally
	/// unset the minion flag from projectiles added to the <see cref="UnreplaceableMinionWith0SlotsSystem"/> cache in order to prevent them from
	/// being sacrificed when the player uses a summon item. 
	/// </summary>
	[Content(ConfigurationSystem.AllFlags, needsAllToFilter: true)]
	public class UnreplaceableMinionWith0SlotsModPlayer : AssPlayerBase
	{
		internal bool shouldResetMinionStatus = false;

		public override void PreUpdate()
		{
			// re-minionify all necessary minions
			if (shouldResetMinionStatus)
			{
				shouldResetMinionStatus = true;
				for (int i = 0; i < Main.maxProjectiles; i++)
				{
					Projectile proj = Main.projectile[i];
					if (proj.active && proj.owner == Player.whoAmI && !proj.minion && UnreplaceableMinionWith0SlotsSystem.Exists(proj.type))
					{
						proj.minion = true;
					}
				}
			}
		}
	}
}
