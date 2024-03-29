using Terraria;

namespace AssortedCrazyThings.Items.Gitgud
{
	public class TwinsGitgud : GitgudItem
	{
		public override void SafeSetDefaults()
		{
			Item.width = 16;
			Item.height = 20;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetModPlayer<GitGudPlayer>().twinsGitgud = true;
		}
	}
}
