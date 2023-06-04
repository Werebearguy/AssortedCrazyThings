using System.Collections.Generic;

namespace AssortedCrazyThings.Base.Handlers.UnreplaceableMinionWith0SlotsHandler
{
	//Taken with permission from billybobmcbo
	[Content(ConfigurationSystem.AllFlags)]
	public class UnreplaceableMinionWith0SlotsSystem : AssSystem
	{
		private static HashSet<int> Minions;

		public static bool Add(int type)
		{
			return Minions.Add(type);
		}

		public static bool Exists(int type)
		{
			return Minions.Contains(type);
		}

		public override void OnModLoad()
		{
			Minions = new HashSet<int>();
		}

		public override void Unload()
		{
			Minions = null;
		}
	}
}
