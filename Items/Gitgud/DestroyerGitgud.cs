using Terraria;

namespace AssortedCrazyThings.Items.Gitgud
{
	public class DestroyerGitgud : GitgudItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Metal Dreamcatcher");
		}

		public override void SafeSetDefaults()
		{
			Item.width = 32;
			Item.height = 32;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetModPlayer<GitGudPlayer>().destroyerGitgud = true;
		}
	}
}
