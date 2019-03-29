using Terraria;

namespace AssortedCrazyThings.Items.Gitgud
{
    public class TwinsGitgud : GitgudItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Metal Contact Lens");
        }

        public override void MoreSetDefaults()
        {
            item.width = 32;
            item.height = 32;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<GitGudPlayer>(mod).twinsGitgud = true;
        }
    }
}
