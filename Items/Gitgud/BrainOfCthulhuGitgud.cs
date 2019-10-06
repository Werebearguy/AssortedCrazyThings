using Terraria;

namespace AssortedCrazyThings.Items.Gitgud
{
    public class BrainOfCthulhuGitgud : GitgudItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Insanity-B-Gone");
        }

        public override void MoreSetDefaults()
        {
            item.width = 32;
            item.height = 32;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<GitGudPlayer>().brainOfCthulhuGitgud = true;
        }
    }
}
