using System;
using System.Collections.Generic;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Base.Chatter
{
	public static class ChatterHandlerLoader
	{
		private static Dictionary<ChatterType, ChatterHandler> dict;

		internal static Dictionary<ChatterType, ChatterHandler> Dict
		{
			get
			{
				if (dict == null)
				{
					dict = new Dictionary<ChatterType, ChatterHandler>();
				}
				return dict;
			}
		}

		public static void Register(ChatterHandler obj)
		{
			var chatterType = obj.ChatterType;
			if (Dict.ContainsKey(chatterType))
			{
				throw new Exception($"Cannot add a {nameof(ChatterGenerator)} of type {obj.GetType()}, {nameof(chatterType)} already registered!");
			}

			if (obj.Generators is not IEnumerable<ChatterGenerator> gens)
			{
				throw new Exception($"{nameof(obj.Generators)} of type {obj.GetType()} cannot be null!");
			}

			foreach (var gen in gens)
			{
				if (gen.Chatters == null)
				{
					throw new Exception($"{nameof(gen.Chatters)} of type {obj.GetType()} is not initialized!");
				}
			}

			ModTypeLookup<ChatterHandler>.Register(obj);
			Dict[chatterType] = obj;
		}

		public static ChatterHandler Get(ChatterType chatterType)
		{
			return Dict[chatterType];
		}

		internal static void Unload()
		{
			Dict.Clear();
		}
	}
}
