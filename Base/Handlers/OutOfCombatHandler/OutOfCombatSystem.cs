using System.Collections.Generic;

namespace AssortedCrazyThings.Base.Handlers.OutOfCombatHandler
{
	[Content(ConfigurationSystem.AllFlags)]
	internal class OutOfCombatSystem : AssSystem
	{
		/// <summary>
		/// Projectile types in this set won't reset the "out of combat" timer. Used to prevent chaining behavior if something depends on out of combat state
		/// </summary>
		public static HashSet<int> IgnoredFriendlyProj { get; private set; }

		public override void OnModLoad()
		{
			IgnoredFriendlyProj = new HashSet<int>();
		}

		public override void Unload()
		{
			IgnoredFriendlyProj = null;
		}
	}
}
