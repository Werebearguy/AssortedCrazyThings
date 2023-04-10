using Terraria;

namespace AssortedCrazyThings.Items.Gitgud
{
	public class LunaticCultistGitgud : GitgudItem
	{
		public override void SafeSetStaticDefaults()
		{
			// DisplayName.SetDefault("Notice of Occupational Termination");
		}

		public override void SafeSetDefaults()
		{
			Item.width = 32;
			Item.height = 32;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetModPlayer<GitGudPlayer>().lunaticCultistGitgud = true;
		}
	}
}
