using Terraria;

namespace AssortedCrazyThings.Projectiles.NPCs.Bosses.DungeonBird
{
    [Content(ContentType.Bosses)]
    public class BabyHarvesterPlayer : AssPlayerBase
    {
        public const int TimeOutsideConditionMax = 300;
        private int timeOutsideCondition = TimeOutsideConditionMax; //Set Valid to false by default

        public bool Valid => timeOutsideCondition < TimeOutsideConditionMax;

        /// <summary>
        /// Returns true if outside vanilla condition, with timeLeft being assigned the time left until it is fully counted as invalid (ending at 0)
        /// </summary>
        public bool IsTurningInvalid(out int timeLeft)
        {
            timeLeft = TimeOutsideConditionMax - timeOutsideCondition;

            return timeOutsideCondition > 0;
        }

        private bool VanillaCondition()
        {
            return Player.ZoneDungeon;
        }

        public override void PostUpdateEquips()
        {
            if (!VanillaCondition())
            {
                if (timeOutsideCondition < TimeOutsideConditionMax)
                {
                    timeOutsideCondition++;
                }
            }
            else
            {
                timeOutsideCondition = 0;
            }
        }
    }
}
