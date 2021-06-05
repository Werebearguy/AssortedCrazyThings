using Terraria;

namespace AssortedCrazyThings.Items.Gitgud
{
    public class GolemGitgud : GitgudItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rechargeable Solar Battery");
        }

        public override void SafeSetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<GitGudPlayer>().golemGitgud = true;
        }
    }
}
