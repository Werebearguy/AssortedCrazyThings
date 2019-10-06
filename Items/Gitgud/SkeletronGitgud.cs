using Terraria;

namespace AssortedCrazyThings.Items.Gitgud
{
    public class SkeletronGitgud : GitgudItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Carton of Soy Milk");
        }

        public override void MoreSetDefaults()
        {
            item.width = 32;
            item.height = 32;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<GitGudPlayer>().skeletronGitgud = true;
        }
    }
}
