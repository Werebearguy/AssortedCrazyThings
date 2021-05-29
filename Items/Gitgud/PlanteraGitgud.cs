using Terraria;

namespace AssortedCrazyThings.Items.Gitgud
{
    public class GreenThumb : GitgudItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Green Thumb");
        }

        public override void MoreSetDefaults()
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
