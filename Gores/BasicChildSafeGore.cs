using Terraria.GameContent;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Gores
{
	//Base class for all gores that would otherwise not require a file at all because they should appear regardless of the "Blood and Gore" setting
	public abstract class BasicChildSafeGore : ModGore
	{
		public sealed override void SetStaticDefaults()
		{
			ChildSafety.SafeGore[Type] = true;
		}
	}
}
