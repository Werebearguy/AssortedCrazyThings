using Terraria;

namespace AssortedCrazyThings.Items.Gitgud
{
    public class SkeletronPrimeGitgud : GitgudItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Clock Set Ten Years Ahead");
        }

        public override void SafeSetDefaults()
        {
            Item.width = 30;
            Item.height = 30;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<GitGudPlayer>().skeletronPrimeGitgud = true;
        }
    }
}
