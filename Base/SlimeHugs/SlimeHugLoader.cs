using System.Collections.Generic;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Base.SlimeHugs
{
    public static class SlimeHugLoader /*: Loader<SlimeHug>*/
    {
		private static List<SlimeHug> list;

		internal static List<SlimeHug> List
        {
            get
            {
				if (list == null)
                {
					list = new List<SlimeHug>();
                }
				return list;
            }
        }

		public static int Register(SlimeHug obj)
		{
			int type = List.Count;

			ModTypeLookup<SlimeHug>.Register(obj);
			List.Add(obj);

			return type;
		}

		public static SlimeHug Get(int id)
		{
			if (id < 0 || id >= List.Count)
			{
				return default;
			}

			return List[id];
		}

		internal static void Unload()
		{
			List.Clear();
		}
    }
}
