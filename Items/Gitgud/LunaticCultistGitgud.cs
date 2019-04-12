using Terraria;

namespace AssortedCrazyThings.Items.Gitgud
{
    public class LunaticCultistGitgud : GitgudItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Notice of Occupational Termination");
        }

        public override void MoreSetDefaults()
        {
            item.width = 32;
            item.height = 32;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<GitGudPlayer>(mod).lunaticCultistGitgud = true;
        }
    }
}
