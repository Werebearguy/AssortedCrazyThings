using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs
{
	[Content(ContentType.PlaceablesFunctional)]
	public class WyvernCampfireGlobalBuff : AssGlobalBuff
	{
		public static LocalizedText WyvernCampfireNearbyText { get; private set; }

		public override void SetStaticDefaults()
		{
			string category = $"Buffs.Campfire.";
			WyvernCampfireNearbyText ??= Mod.GetLocalization($"{category}WyvernCampfireNearby");
		}

		//Tells you its effects in the regular campfire buff (which the wyvern campfire applies)
		//Logic handled in WyvernCampfireTile.NearbyEffects and AssWorld.ResetNearbyTileEffects
		public override void ModifyBuffText(int type, ref string buffName, ref string tip, ref int rare)
		{
			if (type == BuffID.Campfire && Main.LocalPlayer.GetModPlayer<AssPlayer>().wyvernCampfire)
			{
				tip += "\n" + WyvernCampfireNearbyText.ToString();
			}
		}
	}
}
