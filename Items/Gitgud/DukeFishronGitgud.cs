using Terraria;

namespace AssortedCrazyThings.Items.Gitgud
{
	public class DukeFishronGitgud : GitgudItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("White Hook");
		}

		public override void SafeSetDefaults()
		{
			Item.width = 12;
			Item.height = 22;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetModPlayer<GitGudPlayer>().dukeFishronGitgud = true;
		}
	}
}
