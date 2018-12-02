using Terraria.ModLoader;

namespace AssortedCrazyThings
{
	class AssortedCrazyThings : Mod
	{
		public AssortedCrazyThings()
		{
			Properties = new ModProperties()
			{
				Autoload = true,
				AutoloadGores = true,
				AutoloadSounds = true
			};
		}
	}
}
