using Terraria.ID;

namespace AssortedCrazyThings.Projectiles.Minions.CompanionDungeonSouls
{
    public class CompanionDungeonSoulMightMinion : CompanionDungeonSoulMinionBase
    {
        public override void MoreSetDefaults()
        {
            projectile.minionSlots = 1f;
            defdistanceFromTarget = 700f;
            defdistancePlayerFarAway = 800f;
            defdistancePlayerFarAwayWhenHasTarget = 1200f;
            defdistanceToEnemyBeforeCanDash = 200f; //20f
            defplayerFloatHeight = -60f; //-60f
            defplayerCatchUpIdle = 300f; //300f
            defbackToIdleFromNoclipping = 150f; //150f
            defdashDelay = 40f; //time it stays in the "dashing" state after a dash, he dashes when he is in state 0 aswell
            defdistanceAttackNoclip = defdistanceToEnemyBeforeCanDash + 100f;
            defstartDashRange = defdistanceToEnemyBeforeCanDash + 400f; //30f
            defdashIntensity = 8f; //4f

            veloFactorToEnemy = 8f; //8f
            accFactorToEnemy = 10f; //41f

            veloFactorAfterDash = 4f; //4f
            accFactorAfterDash = 41f; //41f

            defveloIdle = 4f;
            defveloCatchUpIdle = 7f;
            defveloNoclip = 13f;

            dustColor = ItemID.BlueDye;
        }
    }
}
