using Terraria.ModLoader;
using log4net;

namespace AssortedCrazyThings.Base.ModSupport
{
	[Content(ConfigurationSystem.AllFlags)]
	public class ModCallHandler : AssSystem
	{
		internal static object Call(params object[] args)
		{
			Mod mod = AssUtils.Instance;
			ILog logger = mod.Logger;

			if (args.Length <= 0 || args[0] is not string method)
			{
				logger.Error("CALL ERROR: NO METHOD NAME! First param MUST be a method name!");
				return null;
			}

			if (method == "GetDownedBoss")
			{
				if (args.Length <= 1 || args[1] is not string bossName)
				{
					logger.Error("CALL ERROR: NOT A STRING! Second param MUST be a valid boss name!");
					return false;
				}

				bool downed;
				switch (bossName)
				{
					//Bosses
					case "SoulHarvester":
						downed = AssWorld.downedHarvester;
						break;
					default:
						logger.Error("CALL ERROR: NOT A MATCHING STRING! Second param MUST be a valid boss name! Possible names found on the wiki.");
						return false;
				}

				return downed;
			}

			return null;
		}
	}
}
