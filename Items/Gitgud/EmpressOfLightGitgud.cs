using Terraria;

namespace AssortedCrazyThings.Items.Gitgud
{
	public class EmpressOfLightGitgud : GitgudItem
	{
		public override void SafeSetDefaults()
		{
			Item.width = 30;
			Item.height = 24;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetModPlayer<GitGudPlayer>().empressOfLightGitgud = true;
		}
	}
}
