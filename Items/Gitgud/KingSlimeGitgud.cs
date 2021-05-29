using Terraria;

namespace AssortedCrazyThings.Items.Gitgud
{
    public class KingSlimeGitgud : GitgudItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Slime Inquisition Notice");
        }

        public override void MoreSetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<GitGudPlayer>().kingSlimeGitgud = true;
        }
    }
}
