using Terraria;

namespace AssortedCrazyThings.Items.Gitgud
{
    public class WallOfFleshGitgud : GitgudItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Wall of Flesh Voodoo Doll");
        }

        public override void MoreSetDefaults()
        {
            item.width = 32;
            item.height = 32;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<GitGudPlayer>().wallOfFleshGitgud = true;
        }
    }
}
