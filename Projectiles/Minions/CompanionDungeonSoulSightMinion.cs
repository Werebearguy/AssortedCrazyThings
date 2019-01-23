namespace AssortedCrazyThings.Projectiles.Minions
{
    public class CompanionDungeonSoulSightMinion : CompanionDungeonSoulMinionBase
    {
        public override void MoreSetDefaults()
        {
            projectile.minionSlots = 1f;
            defdistanceFromTarget = 700f;
            defdistancePlayerFarAway = 800f;
            defdistancePlayerFarAwayWhenHasTarget = 1200f;
            defdistanceToEnemyBeforeCanDash = 20f; //20f
            defplayerFloatHeight = -60f; //-60f
            defplayerCatchUpIdle = 300f; //300f
            defbackToIdleFromNoclipping = 150f; //150f
            defdashDelay = 40f; //time it stays in the "dashing" state after a dash, he dashes when he is in state 0 aswell
            defstartDashRange = defdistanceToEnemyBeforeCanDash + 10f; //30f
            defdashIntensity = 4f; //4f

            defveloIdle = 1f;
            defveloCatchUpIdle = 8f;
            defveloNoclip = 12f;
        }
    }
}
