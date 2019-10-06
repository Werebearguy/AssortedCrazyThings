using Terraria;

namespace AssortedCrazyThings.Items.Gitgud
{
    public class MoonLordGitgud : GitgudItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bright Side of the Moon");
        }

        public override void MoreSetDefaults()
        {
            item.width = 30;
            item.height = 30;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<GitGudPlayer>().moonLordGitgud = true;
        }
    }
}
