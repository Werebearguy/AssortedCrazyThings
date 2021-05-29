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
            Item.width = 32;
            Item.height = 32;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<GitGudPlayer>().wallOfFleshGitgud = true;
        }
    }
}
