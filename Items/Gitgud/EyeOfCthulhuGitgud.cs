using Terraria;

namespace AssortedCrazyThings.Items.Gitgud
{
    public class EyeOfCthulhuGitgud : GitgudItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Broken Lens");
        }

        public override void MoreSetDefaults()
        {
            item.width = 16;
            item.height = 20;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<GitGudPlayer>(mod).eyeOfCthulhuGitgud = true;
        }
    }
}
