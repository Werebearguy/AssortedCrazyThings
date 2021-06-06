using AssortedCrazyThings.Projectiles.Pets.CuteSlimes;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.UI;

namespace AssortedCrazyThings.Base.SlimeHugs
{
    public class SlimeHugThunder : SlimeHug
    {
        protected override int Cooldown => 60 * 60 * 2;

        public override int HugEmote => EmoteID.EmoteSadness;

        public override int PreHugEmoteDuration => 40;

        public override int PreHugEmote => Graveyard ? EmoteID.ItemTombstone : Storm ? EmoteID.WeatherStorming : -1;

        public override bool IsAvailable(CuteSlimeBaseProj slime, PetPlayer petPlayer)
        {
            return petPlayer.Player.position.Y < Main.rockLayer * 16.0 && Main.atmo == 1f &&
                (Storm || Graveyard);
        }

        public override Vector2 GetHugOffset(CuteSlimeBaseProj slime, PetPlayer petPlayer)
        {
            //Shaking left/right
            return new Vector2((petPlayer.slimeHugTimer / 3 % 2 == 0).ToDirectionInt() * 0.8f, 0);
        }

        //TODO Untested
        public static bool Storm => Main.IsItStorming;

        public static bool Graveyard => Main.GraveyardVisualIntensity >= 0.9f;
    }
}
