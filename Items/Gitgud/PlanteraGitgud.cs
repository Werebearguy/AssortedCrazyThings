using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Gitgud
{
	[LegacyName("GreenThumb")]
	public class PlanteraGitgud : GitgudItem
	{
		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Green Thumb");
		}

		public override void SafeSetDefaults()
		{
			Item.width = 32;
			Item.height = 32;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetModPlayer<GitGudPlayer>().planteraGitgud = true;
		}
	}
}
