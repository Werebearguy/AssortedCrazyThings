using AssortedCrazyThings.Items.Consumables;
using AssortedCrazyThings.Projectiles.Pets.CuteSlimes;
using Terraria.GameContent.UI;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Base.SlimeHugs
{
    public class SlimeHugAle : SlimeHug
    {
        protected override int Cooldown => 60 * 60 * 2;
        
        public override int HugEmote => EmoteID.EmotionLove;

        public override int PreHugEmoteDuration => 40;

        public override int PreHugEmote => EmoteID.ItemAle;

        public override bool IsAvailable(CuteSlimeBaseProj slime, PetPlayer petPlayer)
        {
            return petPlayer.Player.HeldItem.type == ModContent.ItemType<CuteSlimeSpawnEnableFlask>();
        }
    }
}
