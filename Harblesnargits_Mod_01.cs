using Terraria.ModLoader;

namespace Harblesnargits_Mod_01
{
	class Harblesnargits_Mod_01 : Mod
	{
		public Harblesnargits_Mod_01()
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
