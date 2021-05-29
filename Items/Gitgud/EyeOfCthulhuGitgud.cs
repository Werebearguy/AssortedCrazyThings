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
            Item.width = 16;
            Item.height = 20;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<GitGudPlayer>().eyeOfCthulhuGitgud = true;
        }
    }
}
